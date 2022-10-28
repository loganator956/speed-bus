using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpeedBus.Gameplay.Passengers
{
    [RequireComponent(typeof(TopDownVehicleController), typeof(PlayerInput))]
    public class PassengerCarriage : MonoBehaviour
    {
        public int PassengerCount { get; private set; }

        private PlayerInput _playerInput;
        private InputAction _loadPassengersAction;
        private InputAction _unloadPassengersAction;

        private ScoreManager _scoreManager;
        private GameController _gameController;

        public int PassengerMaxCapacity = 10;

        private void Awake()
        {
            _scoreManager = FindObjectOfType<ScoreManager>();
            _gameController = FindObjectOfType<GameController>();
            _playerInput = GetComponent<PlayerInput>();
            _loadPassengersAction = _playerInput.actions["Load"];
            _loadPassengersAction.Disable();
            _loadPassengersAction.performed += LoadPassengersAction_Performed;
            _unloadPassengersAction = _playerInput.actions["Unload"];
            _unloadPassengersAction.Disable();
            _unloadPassengersAction.performed += UnloadPassengersAction_Performed;
        }

        private void UnloadPassengersAction_Performed(InputAction.CallbackContext obj)
        {
            for (int i = 0; i < PassengerCount; i++)
            {
                _scoreManager.AwardPlayer(1, "Drop Off", transform.position);
            }
            PassengerCount = 0;
            _gameController.CurrentDropOffPoint.CoolDownTimer = 5f;
            _gameController.PickNewDropOffPoint();
        }

        private BusStop _currentBusStop = null;

        private void LoadPassengersAction_Performed(InputAction.CallbackContext obj)
        {
            // TODO: Here will be where the chance bar appears
            // for now just pickup all of em

            int maxLoadable = PassengerMaxCapacity - PassengerCount;
            int toLoad = Mathf.Min(maxLoadable, _currentBusStop.PassengerCount);
            _currentBusStop.PassengerCount -= toLoad;
            PassengerCount += toLoad;
        }

        private void OnTriggerEnter(Collider other)
        {
            BusStop busStop = other.GetComponent<BusStop>();
            if (busStop != null)
            {
                _currentBusStop = busStop;
                _loadPassengersAction.Enable(); 
                return;
            }
            DropOffPoint dropOff = other.GetComponent<DropOffPoint>();
            if (dropOff != null)
            {
                // left drop off
                _unloadPassengersAction.Enable();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            BusStop busStop = other.GetComponent<BusStop>();
            if (busStop != null)
            {
                // left bus stop
                _loadPassengersAction.Disable();
                _currentBusStop = null;
                return;
            }
            DropOffPoint dropOff = other.GetComponent<DropOffPoint>();
            if (dropOff != null)
            {
                // left drop off
                _unloadPassengersAction.Disable();
            }
        }
    }
}