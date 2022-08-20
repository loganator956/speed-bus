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
        public RadialMenu AverageAngryRadial;
        public RadialMenu AngryRadial;

        public BusStop RelatedBusStop;

        //({stops[i].WaitingPassengers.Count}) ({controller.GetNumberOfPassengersForTarget(stops[i])})

        public TopDownVehicleController controller;

        private void Awake()
        {
            controller = FindObjectOfType<TopDownVehicleController>();
        }

        public const float AngryThresholdMin = -5;
        public const float AngryThresholdMax = -25;

        // Update is called once per frame
        void Update()
        {
            PickupText.text = RelatedBusStop.WaitingPassengers.Count.ToString();
            DropOffText.text = controller.GetNumberOfPassengersForTarget(RelatedBusStop).ToString();
            // work out inverse lerp in threshold of average of the angriest passengers? (-5 -> -25?)
            // find angriest passenger
            Passenger angriestPassenger = null;
            float averageHappiness = 0f;
            foreach (Passenger passenger in RelatedBusStop.WaitingPassengers)
            {
                averageHappiness += passenger.Happiness;
                if (angriestPassenger == null) { angriestPassenger = passenger; }
                else if (passenger.Happiness < angriestPassenger.Happiness) { angriestPassenger = passenger; };
            }
            averageHappiness /= (float)RelatedBusStop.WaitingPassengers.Count;
            Debug.Log(averageHappiness);
            float averageT = Mathf.InverseLerp(AngryThresholdMin, AngryThresholdMax, averageHappiness);
            if (averageT <= 0)
            {
                // average passenger anger is not very angry so don't show average
                AverageAngryRadial.gameObject.SetActive(false);
            }
            else
            {
                AverageAngryRadial.gameObject.SetActive(true);
                AverageAngryRadial.Value = averageT;
            }
            if (angriestPassenger!= null)
            {
                float val = Mathf.InverseLerp(AngryThresholdMin, AngryThresholdMax, angriestPassenger.Happiness);
                if (val <= 0)
                {
                    // angriest passenger is not very angry so don't show radials
                    AngryRadial.gameObject.SetActive(false);
                }
                else
                {
                    // angriest passenger is quite angry so show radials
                    AngryRadial.gameObject.SetActive(true);
                    AngryRadial.Value = val;
                }
            }
        }
    }

}