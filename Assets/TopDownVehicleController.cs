using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SpeedBus.Gameplay;
using SpeedBus.GUI;
using UnityEngine.Events;

public class TopDownVehicleController : MonoBehaviour
{
    public float DriftFactor = 0.55f;
    public float AccelerationSpeed = 40f;
    private float _decelerationInput = 0.995f;
    public float MinDecelerateFactor = 0.995f;
    public float MaxDecelerateFactor = 15f;
    public AnimationCurve DecelerateCurve;
    public float MaxTurnSpeed = 5.0f;

    public AnimationCurve HappinessInfluenceCurve;

    private PlayerInput _playerInput;
    public PlayerInput PlayerInput
    {
        get { return _playerInput; }
    }
    private InputAction _moveAction;

    private Rigidbody2D _rigidbody2D;

    private float accelerationInput = 0f;

    private const float SteerAngleDeadzone = 0.05f;

    public List<Passenger> Passengers = new List<Passenger>();

    public UnityEvent<Passenger> OnPassengerUnloaded, OnPassengerLoaded;
    public UnityEvent<int> OnChanceBarResult; // -1 bad, 0 okay, +1 excellent

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnControlsChanged;

        _moveAction = _playerInput.actions["Movement"];

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _attractionManager = FindObjectOfType<AttractionManager>();
    }

    float decelerateT = 0f;

    private const float DecelerationTRate = 2f;

    private bool _enableMovement = true;
    public bool EnableMovement
    {
        get { return _enableMovement; }
        set
        {
            _enableMovement = value;
            // TODO: Create an event
        }
    }

    private bool _isInStop = false;
    public bool IsInStop
    {
        get { return _isInStop; }
        set
        {
            _isInStop = value;
            // TODO: Create an event
        }
    }

    private BusStop _currentStop = null;
    private Attraction _currentAttraction = null;

    AttractionManager _attractionManager;

    private bool _isStoppedAtStop = false;
    public bool IsStoppedAtStop
    {
        get { return _isStoppedAtStop; }
        set
        {
            if (value != _isStoppedAtStop)
            {
                _isStoppedAtStop = value;
                OnStopAtStopChanged(_isStoppedAtStop);
            }
        }
    }
    public GameObject ChanceBarPrefab;

    private float _sittingStillT = 0f;
    public float t_SittingStillT { get { return _sittingStillT; } }
    public const float SittingStillPenaltyThreshold = 6f;
    private const float SittingStillDetectionThreshold = 0.5f;

    public UnityEvent OnSittingStillPenalty = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        if (_stopCooldown > 0)
        {
            _stopCooldown -= Time.deltaTime;
        }
        if (EnableMovement)
        {
            if (_rigidbody2D.velocity.magnitude < SittingStillDetectionThreshold)
            {
                _sittingStillT += Time.deltaTime;
                if (_sittingStillT > SittingStillPenaltyThreshold)
                {
                    _sittingStillT = 0f;
                    OnSittingStillPenalty.Invoke();
                }
            }
            else if (_sittingStillT > 0f)
            {
                _sittingStillT -= Time.deltaTime;
            }
            Vector2 dirInput = _moveAction.ReadValue<Vector2>();

            if (dirInput.magnitude > SteerAngleDeadzone)
            {
                Quaternion rotation = GetRotationFromInput(dirInput);
                float angle = Quaternion.Angle(rotation, transform.rotation);
                _rigidbody2D.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, MaxTurnSpeed / angle/*0.2f*/));
            }

            accelerationInput = GetAccelerationFromInput(dirInput);

            if (accelerationInput < 0.05f)
            {
                // no inputs (slowdown)
                decelerateT += Time.deltaTime;
                decelerateT = Mathf.Clamp01(decelerateT);
            }
            else
            {
                decelerateT -= Time.deltaTime * DecelerationTRate;
                decelerateT = Mathf.Clamp01(decelerateT);
            }
            _decelerationInput = Mathf.Lerp(MinDecelerateFactor, MaxDecelerateFactor * (IsInStop ? 0.5f : 1f), decelerateT);
        }
        else
        {
            _decelerationInput = 0.0f; // lock vehicle
        }

        if (IsInStop && _rigidbody2D.velocity.magnitude < 0.1f && _stopCooldown <= 0)
        {
            IsStoppedAtStop = true;
        }
    }

    private GameObject _chanceBarInstance;

    public RectTransform ChanceBarParentReference;

    private void OnStopAtStopChanged(bool val)
    {
        if (val && _stopCooldown <= 0)
        {
            Debug.Log("Player stopped at stop");
            EnableMovement = false;
            _chanceBarInstance = Instantiate(ChanceBarPrefab, ChanceBarParentReference);
            ChanceBar bar = _chanceBarInstance.GetComponent<ChanceBar>();
            InputAction actionAction = _playerInput.actions["Action"];
            actionAction.Enable();
            actionAction.started += ctx => bar.ButtonPressed(ChanceBarResult.ResultType.Normal);
            InputAction altActionAction = _playerInput.actions["AltAction"];
            altActionAction.Enable();
            altActionAction.started += ctx => bar.ButtonPressed(ChanceBarResult.ResultType.UnloadOnly);
            bar.BadRangeSize = 35;
            bar.GoodRangeSize = 20;
            bar.BarSpeed = 1f;
            bar.UpdateBar();
            bar.OnBarSubmission.AddListener(OnBarSubmitted);
        }
        else if (!val)
        {
            _stopCooldown = 5f;
        }
    }

    private float _stopCooldown = 0f;
    public float StopCooldown { get { return _stopCooldown; } }

    private void OnBarSubmitted(ChanceBarResult result)
    {
        double t = result.T;
        // TODO: Allow unloading only
        ChanceBar bar = _chanceBarInstance.GetComponent<ChanceBar>();
        float b1 = (bar.BadRangeSize / 2f) / 100f; // lower than this value is in the bad range
        float b2 = 1 - b1; // higher than this value is in the bad range

        float g1 = (0.5f - (bar.GoodRangeSize / 2f) / 100f); // if between the g1 and g2 then that is best
        float g2 = (0.5f + (bar.GoodRangeSize / 2f) / 100f);

        float val = 0f;

        if (t < b1 || t > b2)
        {
            Debug.Log("Bad");
            val = 0;
            OnChanceBarResult.Invoke(-1);
        }
        else if (t > g1 && t < g2)
        {
            Debug.Log("Excellent");
            val = 1;
            OnChanceBarResult.Invoke(1);
        }
        else
        {
            Debug.Log("Okay");
            val = 1 - (Mathf.Abs((float)t - 0.5f) * 2);
            OnChanceBarResult.Invoke(0);
        }
        val = Mathf.Clamp01(val); // ensure that value is between 0 and 1
        // val will be 0->1 (where 0 is bad and 1 is good)

        if (_currentStop != null)
        {
            if (result.Type == ChanceBarResult.ResultType.Normal)
            {
                int passengerCount = _currentStop.WaitingPassengers.Count;
                int passengersToPickup = Mathf.CeilToInt((float)passengerCount * val);
                Debug.Log($"Picking up {passengersToPickup} passengers from {_currentStop.DisplayName} stop");
                for (int i = 0; i < passengersToPickup; i++)
                {
                    Passenger passenger = _currentStop.WaitingPassengers[0];
                    passenger.IsOnBus = true;
                    Passengers.Add(passenger);
                    _currentStop.WaitingPassengers.RemoveAt(0);
                    OnPassengerLoaded.Invoke(passenger);
                }
            }

            // only unload passengers if you don't get a BAD
            if (val != 0)
            {
                // iterate through all passengers and unload the ones that have this stop as the target stop
                for (int i = Passengers.Count - 1; i >= 0; i--)
                {
                    Passenger passenger = Passengers[i];
                    if (passenger.TargetStop == _currentStop)
                    {
                        passenger.IsOnBus = false;
                        Passengers.RemoveAt(i);
                        OnPassengerUnloaded.Invoke(passenger);
                    }
                }
            }
        }
        else if (_currentAttraction != null)
        {
            _attractionManager.UseAttraction(val);
        }
        Destroy(_chanceBarInstance);
        IsStoppedAtStop = false;
        EnableMovement = true;
    }

    public int GetNumberOfPassengersForTarget(BusStop target)
    {
        int count = 0;
        foreach(Passenger passenger in Passengers)
        {
            if (passenger.TargetStop == target)
            {
                count++;
            }
        }
        return count;
    }

    public float GetAveragePassengerHappiness()
    {
        float a = 0;
        foreach(Passenger passenger in Passengers)
        {
            a += passenger.Happiness;
        }
        a /= (float)Passengers.Count;
        return a;
    }

    private void OnControlsChanged(PlayerInput obj)
    {
        Debug.Log($"Changed Controls: Now using {_playerInput.currentControlScheme}");
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ReduceOrthogonalVelocity();
    }

    private void ApplyEngineForce()
    {
        Vector2 engineForceVector = transform.up * accelerationInput * AccelerationSpeed;

        _rigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ReduceOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = (forwardVelocity * _decelerationInput) + (rightVelocity * DriftFactor);
    }

    private Quaternion GetRotationFromInput(Vector2 input)
    {
        float angle = Vector2.SignedAngle(Vector3.up, input);
        return Quaternion.Euler(0, 0, angle);
    }

    private float GetAccelerationFromInput(Vector2 input)
    {
        return Mathf.Clamp01(input.magnitude);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BusStop stop = collision.GetComponentInParent<BusStop>();
        Attraction attraction = collision.GetComponentInParent<Attraction>();
        if (stop != null)
        {
            IsInStop = true;
            _currentStop = stop;
        }
        else if (attraction != null && _attractionManager.CurrentAttraction == attraction)
        {
            IsInStop = true;
            _currentAttraction = attraction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<BusStop>() != null)
        {
            IsInStop = false;
            _currentStop = null;
        }
        else if (collision.GetComponentInParent<Attraction>() != null)
        {
            IsInStop = false;
            _currentAttraction = null;
        }
    }
}
