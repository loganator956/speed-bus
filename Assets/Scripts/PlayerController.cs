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
        int amountToOffload = request;
        if (RiderCount - request < 0) { amountToOffload = RiderCount; };
        RiderCount -= amountToOffload;
        return amountToOffload;
    }
}
