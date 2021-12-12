using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private static int _riderCount = 0;
    public static int RiderCount
    {
        get { return _riderCount; }
        set
        {
            _riderCount = value;
            RiderCountChangedEvent.Invoke();
        }
    }

    public static UnityEvent RiderCountChangedEvent = new UnityEvent();

    public int AttemptOffloadPassengers(int request)
    {
        /*
        try offload passengers
        do RiderCount - the amount of passengers the bus stop needs. Clamp it above 0
        Subtract that amount from the number of riders and add to the received variable of the bus stop
        */
        // int amountToOffload = RiderCount - request;
        // if (amountToOffload < 0) { amountToOffload = request; }; 
        int amountToOffload = request;
        if (RiderCount - request < 0) { amountToOffload = RiderCount; };
        RiderCount -= amountToOffload;
        return amountToOffload;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
