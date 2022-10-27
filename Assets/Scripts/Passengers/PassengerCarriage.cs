using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpeedBus.Gameplay.Passengers
{
    [RequireComponent(typeof(TopDownVehicleController), typeof(PlayerInput))]
    public class PassengerCarriage : MonoBehaviour
    {
        private List<Passenger> _passengersList = new List<Passenger>();

        private PlayerInput _playerInput;
        private InputAction _transferAction;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _transferAction = _playerInput.actions["Transfer"];
            _transferAction.Disable();
            _transferAction.performed += TransferAction_Performed;
        }

        private BusStop _currentBusStop = null;

        private void TransferAction_Performed(InputAction.CallbackContext obj)
        {
            PickupPassengers(_currentBusStop);
            DropOffPassengers(_currentBusStop);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void PickupPassengers(BusStop stop)
        {
            // TODO: Here will be where the chance bar appears
            // for now just pickup all of em

            for (int i = stop.Passengers.Count - 1; i >= 0; i--)
            {
                Passenger selectedPassenger = stop.Passengers[i];
                _passengersList.Add(selectedPassenger);
                stop.Passengers.Remove(selectedPassenger);
            }
        }

        void DropOffPassengers(BusStop stop)
        {
            for (int i = _passengersList.Count - 1; i >= 0; i--)
            { 
                if (_passengersList[i].TryCompleteJourney(stop))
                {
                    _passengersList.RemoveAt(i);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            BusStop busStop = other.GetComponent<BusStop>();
            if (busStop != null)
            {
                _currentBusStop = busStop;
                _transferAction.Enable();
                Debug.Log("Enabled transfer acton");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            BusStop busStop = other.GetComponent<BusStop>();
            if (busStop != null)
            {
                // left bus stop
                _transferAction.Disable();
                _currentBusStop = null;
                Debug.Log("Disabled transfer acton");
            }
        }
    }
}