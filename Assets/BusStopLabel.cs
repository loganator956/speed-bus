using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SpeedBus.Gameplay;

namespace SpeedBus.GUI
{
    public class BusStopLabel : MonoBehaviour
    {
        public TextMeshProUGUI PickupText;
        public TextMeshProUGUI DropOffText;

        public BusStop RelatedBusStop;

        //({stops[i].WaitingPassengers.Count}) ({controller.GetNumberOfPassengersForTarget(stops[i])})

        public TopDownVehicleController controller;

        private void Awake()
        {
            controller = FindObjectOfType<TopDownVehicleController>();
        }

        // Update is called once per frame
        void Update()
        {
            PickupText.text = RelatedBusStop.WaitingPassengers.Count.ToString();
            DropOffText.text = controller.GetNumberOfPassengersForTarget(RelatedBusStop).ToString();
        }
    }

}