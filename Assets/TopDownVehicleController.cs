using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SpeedBus.Gameplay;

public class TopDownVehicleController : MonoBehaviour
{
    public float DriftFactor = 0.55f;
    public float AccelerationSpeed = 40f;
    private float _decelerationInput = 0.995f;
    public float MinDecelerateFactor = 0.995f;
    public float MaxDecelerateFactor = 15f;
    public AnimationCurve DecelerateCurve;
    public float MaxTurnSpeed = 5.0f;

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

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnControlsChanged;

        _moveAction = _playerInput.actions["Movement"];

        _rigidbody2D = GetComponent<Rigidbody2D>();
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

    // Update is called once per frame
    void Update()
    {
        if (EnableMovement)
        {
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

        if (IsInStop && _rigidbody2D.velocity.magnitude < 0.1f)
        {
            IsStoppedAtStop = true;
        }
    }

    private void OnStopAtStopChanged(bool val)
    {
        if (val)
        {
            Debug.Log("Player stopped at stop");
            EnableMovement = false;
        }
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
        if (collision.GetComponentInParent<BusStop>() != null)
        {
            IsInStop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<BusStop>() != null)
        {
            IsInStop = false;
        }
    }
}
