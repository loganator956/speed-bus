using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private int _currentStage = 0;
    public int CurrentStage
    {
        get { return _currentStage; }
        set
        {
            if (value > 10)
            {
                GameFinishedEvent.Invoke();
            }
            else
            {
                _currentStage = value;
                PlayerStageChangedEvent.Invoke();
            }
        }
    }
    public const int MaxStage = 10; // limit to 10 just for the game jam build

    [Header("Events")]
    public UnityEvent PlayerStageChangedEvent, GameFinishedEvent, CheckStopsSatisfiedEvent;
    private void Awake()
    {
        // register listeners
        PlayerController.RiderCountChangedEvent.AddListener(CheckStopsSatisfied);
        PlayerStageChangedEvent.AddListener(GameStage_Changed);
        CheckStopsSatisfiedEvent.AddListener(CheckStopsSatisfied);
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentStage++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckStopsSatisfied()
    {
        bool success = true;
        BusStopScript[] stops = GameObject.FindObjectsOfType<BusStopScript>();
        foreach (BusStopScript stop in stops)
        {
            if (!stop.CheckSatisfication())
            {
                success = false;
            }
        }

        if (success)
        {
            // all stops are satisfied
            CurrentStage++;
        }
        if (!success)
        {
            Debug.Log("Not completed yet");
        }
        if (success)
        {
            Debug.Log("Completed a stage. Now moving onto stage: " + CurrentStage);
        }
    }

    void GameStage_Changed()
    {
        /*
        Iterate through each bus stop and initialize their correct values for the stage
        */

        foreach(BusStopScript stop in GameObject.FindObjectsOfType<BusStopScript>())
        {
            stop.PassengerRequest = Convert.ToInt32(stop.PassengerRequestCurve.Evaluate(CurrentStage));
            stop.NumberOfPassengersDefault = stop.NumberOfPassengersWaiting = Convert.ToInt32(stop.PassengerSupplyCurve.Evaluate(CurrentStage));
        }
    }
}
