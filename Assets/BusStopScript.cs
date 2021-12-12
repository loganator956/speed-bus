using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BusStopScript : MonoBehaviour
{
    public int NumberOfPassengersDefault = 5;
    public float PassengerRequestDefault = 5f;

    private bool _isEntered = false;
    private bool _hasShownPopup = false;
    private Rigidbody2D playerRigidbody2D;
    private Vector2 playerPreviousVelocity;

    private Text _slowDownText;
    private BusStopManager _busStopManager;

    private float _cooldown = 0f;

    private void Awake()
    {
        _slowDownText = GameObject.Find("SlowDownText").GetComponent<Text>();
        _busStopManager = GameObject.FindObjectOfType<BusStopManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_isEntered && !_hasShownPopup && _cooldown <= 0f)
        {
            if (playerRigidbody2D.velocity.magnitude < maxSpeed)
            {
                Debug.Log("Showing skill bar");
                _hasShownPopup = true;
                playerPreviousVelocity = playerRigidbody2D.velocity;
                playerRigidbody2D.velocity = Vector2.zero;
                PlayerController.EnableInputs = false;
                // TODO: Fix the enable inputs not working for some reason
                _slowDownText.enabled = false;
                _busStopManager.ShowSliderPopup().AddListener(OnTimingPopupFinish);
            }
            else
            {
                Debug.Log("Slow down");
                _slowDownText.enabled = true;
            }
        }

        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    private const float maxSpeed = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            playerRigidbody2D = other.attachedRigidbody;
            _isEntered = true;
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
        // TODO: introduce a stage multiplier (so as the player progresses through the stages, the number of passengers change)
        Debug.Log($"Loading {NumberOfPassengersDefault * percentageOfPassengers}");
        playerRigidbody2D.velocity = playerPreviousVelocity; // reapply previous velocity
        _hasShownPopup = false;
        _cooldown = 10f;

        // TODO: Do the actual loading up of passengers
    }
}

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float>
{

}