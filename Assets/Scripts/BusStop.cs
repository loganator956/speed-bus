using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedBus.Gameplay
{
    public class BusStop : MonoBehaviour
    {
        public string DisplayName = "Untitled Bus Stop";
        [Range(0f, 1f)]
        public float PassengerSpawnChance = 0.1f;
        public int SoftCapacity = 8;
        public float TargetWeighting = 1f;
        public List<Passenger> WaitingPassengers = new List<Passenger>();

        private GameController _gameController;

        private void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
            GameController.GameTickEvent.AddListener(OnGameTickEvent);
        }

        private void OnGameTickEvent()
        {
            double roll = GameController.Randomiser.NextDouble();
            float chance = Mathf.Max(PassengerSpawnChance * ((SoftCapacity - WaitingPassengers.Count) / (float)SoftCapacity), 0.02f); // makes the number of passengers spawning per bus stop reduce as they fill up
            if (roll < chance)
            {
                Debug.Log($"Spawning passenger at {DisplayName} stop with a roll of {roll} and chance of {chance}");
                Passenger passenger = new Passenger();
                // TODO: Implement passengers choosing locations

                passenger.TargetStop = _gameController.SelectRandomBusStop(this);
                WaitingPassengers.Add(passenger);
            }
        }
    }
}