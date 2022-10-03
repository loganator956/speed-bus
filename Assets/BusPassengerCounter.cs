using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SpeedBus.Gameplay;

namespace SpeedBus.GUI
{
    public class BusPassengerCounter : MonoBehaviour
    {
        TopDownVehicleController controller;
        TextMeshProUGUI text;

        public Color EmptyBusColour = Color.gray;
        public Color RegularColour = Color.white;

        private void Awake()
        {
            controller = FindObjectOfType<TopDownVehicleController>();
            text = GetComponent<TextMeshProUGUI>();

            controller.OnPassengerLoaded.AddListener(OnPassengerLoaded);
            controller.OnPassengerUnloaded.AddListener(OnPassengerUnloaded);
            
            UpdateText();
        }

        void OnPassengerLoaded(Passenger passenger)
        {
            UpdateText();
            // TODO: Create a lil popup making it clear that you loaded/unloaded passengers (Like one that animates slightly sliding upwards and fading away)
            // Make sure have delay
        }

        void OnPassengerUnloaded(Passenger passenger)
        {
            UpdateText();
            // TODO: Create a lil popup making it clear that you loaded/unloaded passengers (Like one that animates slightly sliding upwards and fading away)
            // Make sure have delay

            // TODO: Could create a struct that contains the information required (eg: happiness, etc.) 
        }

        void UpdateText()
        {
            text.text = controller.Passengers.Count.ToString();
            if (controller.Passengers.Count == 0)
            {
                text.color = EmptyBusColour;
            }
            else
            {
                text.color = RegularColour;
            }
        }

        private int _positiveQueue = 0;
        private int _negativeQueue = 0;

        private float _cooldownT = 0f;
        private const float PopupCooldown = 0.1f; // 0.1 seconds

        private void Update()
        {
            if (_positiveQueue > 0 || _negativeQueue > 0)
            {
                _cooldownT -= Time.deltaTime;
                if (_cooldownT < 0f)
                {
                    _cooldownT = PopupCooldown;
                    // TODO: Spawn the popup for postiive/negative
                }
            }
        }
    }
}