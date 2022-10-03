using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpeedBus.Gameplay
{
    public class Passenger
    {
        // Event listeners are added where Passengers are created (BusStop.cs)
        public Passenger()
        {
            OnPassengerLongWait.AddListener(OnPassengerLongWait_Invoked);
        }

        public PlayerScoreController ScoreController;

        public BusStop TargetStop { get; set; }
        public float TravelTime { get; private set; }
        private float _waitTime = 0;
        public float WaitTime
        {
            get { return _waitTime; }
            set
            {
                float delta = value - _waitTime;
                _waitTime = value;
                _WaitTimePenaltyCounter(delta);
            }
        }
        public float Happiness { get; private set; }
        public void AddHappiness(float delta)
        {
            Happiness += delta;
        }
        private int _crashCount = 0;

        private float _waitTimePenaltyTimer = 0f;

        public const float WaitTimePenaltyThreshold = 30;


        private void _WaitTimePenaltyCounter(float delta)
        {
            _waitTimePenaltyTimer += delta;
            if (_waitTimePenaltyTimer > WaitTimePenaltyThreshold)
            {
                _waitTimePenaltyTimer = 0f;
                OnPassengerLongWait.Invoke();
            }
        }

        public bool IsOnBus = false;

        public void GameTick()
        {
            if (IsOnBus)
            {
                TravelTime++;
            }
            else
            {
                WaitTime++;
            }
        }
        public UnityEvent OnPassengerLongWait = new UnityEvent();

        /*public UnityEvent<float> OnPassengerHappinessChanged = new UnityEvent<float>();*/
        public void OnBusCrash_Invoked(float force)
        {
            _crashCount++;
            // TODO: Reduce happiness by x * force?
        }
        public void OnBusIdle_Invoked()
        {
            if (IsOnBus)
            {
                Debug.Log("Bus Idling");
                int amount = ScoreAndHappinessChanges.BusIdlePenalty;
                AddHappiness(amount);
                ScoreController.ChangeScore(amount);
            }
        }
        public void OnPassengerLongWait_Invoked()
        {
            Debug.Log("passenger long wait");
            int amount = ScoreAndHappinessChanges.PassengerWaitTooLongPenalty;
            AddHappiness(amount);
            ScoreController.ChangeScore(amount);
        }
        public void OnBusReachAttraction_Invoked(float t)
        {
            if (IsOnBus)
            {
                int amount = Mathf.CeilToInt(ScoreAndHappinessChanges.AttractionBonus * t);
                AddHappiness(amount);
                ScoreController.ChangeScore(Mathf.CeilToInt(amount * 0.3f));
            }
        }
        public void OnBusThrillNearMiss_Invoked()
        {

        }
        public void OnBusThrillHighSpeed_Invoked()
        {

        }
    }
}