using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace SpeedBus.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public static UnityEvent GameTickEvent = new UnityEvent();
        public static System.Random Randomiser;

        private float[] _stopCumulativeWeightings;
        private BusStop[] _stops;

        private void Awake()
        {
            Randomiser = new System.Random();
        }

        private void Start()
        {
            BalanceBusStops();
        }

        private void BalanceBusStops()
        {
            BusStop[] stops = FindObjectsOfType<BusStop>();
            _stops = stops;
            float totalTargetWeighting = 0f;
            foreach(BusStop stop in stops)
            {
                totalTargetWeighting += stop.TargetWeighting;
            }
            float cumulativeValue = 0f;
            _stopCumulativeWeightings = new float[stops.Length];
            for(int i = 0; i < stops.Length; i++)
            {
                _stopCumulativeWeightings[i] = cumulativeValue + (stops[i].TargetWeighting / totalTargetWeighting);
                cumulativeValue = _stopCumulativeWeightings[i];
            }
        }

        private float tickTimer = 0f;
        private const float TickFrequency = 1f; // every x seconds
        private void Update()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer > TickFrequency)
            {
                tickTimer = 0f;
                GameTickEvent.Invoke();
            }
        }

        public BusStop SelectRandomBusStop()
        {
            BusStop selection = null;

            double roll = Randomiser.NextDouble();

            for (int i = 0; i < _stopCumulativeWeightings.Length; i++)
            {
                if (roll < _stopCumulativeWeightings[i])
                {
                    selection = _stops[i];
                }
            }

            return selection;
        }

        public BusStop SelectRandomBusStop(BusStop ignoredStop)
        {
            while (true)
            {
                BusStop selected = SelectRandomBusStop();
                if (selected != ignoredStop) return selected;
            }
        }
    }
}