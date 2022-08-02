using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpeedBus.Gameplay
{
    public class PlayerScoreController : MonoBehaviour
    {
        private int _score;
        public int Score
        {
            get { return _score; }
            private set
            {
                if (_score != value)
                {
                    int oldScore = _score;
                    _score = value;
                    ScoreChangedEvent.Invoke(value - oldScore);
                }
            }
        }

        /// <summary>
        /// Invoked when the score property has been changed. (Runs after change). Passes the score delta as an int
        /// </summary>
        public UnityEvent<int> ScoreChangedEvent;

        private TopDownVehicleController _controller;

        private void Awake()
        {
            ScoreChangedEvent.AddListener(OnScoreChangedEvent);

            _controller = GetComponent<TopDownVehicleController>();

            _controller.OnPassengerLoaded.AddListener(OnPassengerLoaded);
            _controller.OnPassengerUnloaded.AddListener(OnPassengerUnloaded);
        }

        private void OnPassengerLoaded(Passenger passenger)
        {
            
        }

        private void OnPassengerUnloaded(Passenger passenger)
        {
            Score += ScoreAndHappinessChanges.AwardPassengerUnload;
        }

        private void OnScoreChangedEvent(int delta)
        {
            Debug.Log($"Score has changed by {delta} to become {Score}");
        }

        public void ChangeScore(int amount)
        {
            Score += amount;
        }
    }
}