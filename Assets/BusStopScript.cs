using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BusStopScript : MonoBehaviour
{
    public int NumberOfPassengersDefault = 5;
    public int NumberOfPassengersWaiting;
    public int PassengerRequestDefault = 5;
    public AnimationCurve PassengerRequestCurve, PassengerSupplyCurve;

    // [HideInInspector()]
    public int PassengerRequest = 5;
    public int PassengersReceived = 0;

    private bool _isEntered = false;
    private bool _hasShownPopup = false;
    private Rigidbody2D playerRigidbody2D;
    private Vector2 playerPreviousVelocity;

    private Text _slowDownText;
    private BusStopManager _busStopManager;

    private float _cooldown = 0f;

    private const float _passengerRegenRate = 1f;

    private void Awake()
    {
        _slowDownText = GameObject.Find("SlowDownText").GetComponent<Text>();
        _busStopManager = GameObject.FindObjectOfType<BusStopManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool CheckSatisfication()
    {
        if (PassengerRequest == 0) { return true; };
        return (PassengersReceived >= PassengerRequest);
    }

    float regenTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (_isEntered && !_hasShownPopup && _cooldown <= 0f)
        {
            if (NumberOfPassengersDefault == 0)
            {
                // this stop is not needed for the current stage
                _slowDownText.enabled = true;
                _slowDownText.text = "GO TO DIFFERENT STOP";
            }
            else
            {
                if (playerRigidbody2D.velocity.magnitude < maxSpeed)
                {
                    Debug.Log("Showing skill bar");
                    _hasShownPopup = true;
                    playerPreviousVelocity = playerRigidbody2D.velocity;
                    playerRigidbody2D.velocity = Vector2.zero;
                    TopDownCarController.EnableDriving = false;
                    _slowDownText.enabled = false;
                    _busStopManager.ShowSliderPopup().AddListener(OnTimingPopupFinish);
                }
                else
                {
                    _slowDownText.text = "SLOW DOWN";
                    _slowDownText.enabled = true;
                }
            }
        }

        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
        }

        regenTimer += Time.deltaTime;
        if (regenTimer > _passengerRegenRate)
        {
            regenTimer = 0f;
            NumberOfPassengersWaiting++;
            if (NumberOfPassengersWaiting > NumberOfPassengersDefault)
            {
                NumberOfPassengersWaiting = NumberOfPassengersDefault;
            }
        }
    }

    private const float maxSpeed = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            playerRigidbody2D = other.attachedRigidbody;
            _isEntered = true;
            PassengersReceived += other.GetComponent<PlayerController>().AttemptOffloadPassengers(PassengerRequest - PassengersReceived);
            FindObjectOfType<GameController>().CheckStopsSatisfiedEvent.Invoke(); // invoking a separate event instead of when the player RiderCount value changes due to the order of which things are executed
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            _slowDownText.enabled = false;
            _isEntered = false;
        }
    }

    private void OnTimingPopupFinish(float percentageOfPassengers)
    {
        Debug.Log($"Loading {NumberOfPassengersDefault * percentageOfPassengers}");
        playerRigidbody2D.velocity = playerPreviousVelocity; // reapply previous velocity
        _hasShownPopup = false;
        _cooldown = 10f;
        TopDownCarController.EnableDriving = true;

        // TODO: Do the actual loading up of passengers
        int numberOfPassengers = Mathf.FloorToInt(NumberOfPassengersWaiting * percentageOfPassengers);
        NumberOfPassengersWaiting -= numberOfPassengers;
        PlayerController.RiderCount += numberOfPassengers;
    }
}

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float>
{

}