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
        public BusStop[] busStops { get; private set; }

        private float _totalWeighting = 0;
        public static System.Random Randomiser;

        private void Awake()
        {
            Randomiser = new System.Random();
            // find all the bus stops
            busStops = FindObjectsOfType<BusStop>();

            foreach (BusStop stop in busStops)
            {
                _totalWeighting += stop.Weighting;
            }
        }

        private void Start()
        {
            StartGame();
        }

        List<Person> people = new List<Person>();

        public int GetTotalPeople()
        {
            return people.Count;
        }

        private void StartGame()
        {
            /*// TODO: Change this to be dynamically set
            float NumberOfPassengersThisRound = 10;
            foreach(BusStop busStop in busStops)
            {
                people.AddRange(busStop.GeneratePeople(Mathf.CeilToInt((busStop.Weighting / _totalWeighting) * NumberOfPassengersThisRound)));
            }
            System.Random random = new System.Random();
            foreach(Person person in people)
            {
                while(person.WantedStop == null)
                {
                    BusStop randomStop = busStops[random.Next(busStops.Length)];
                    if (randomStop != person.CurrentStop)
                    {
                        person.WantedStop = randomStop;
                    }
                }
            }*/
            SelectRandomStop();
        }

        /// <summary>
        /// Select a random bus stop, taking into consideration the weighting the stop has been given
        /// </summary>
        /// <returns>A random bus stop</returns>
        /// <exception cref="BusStopNotFoundException">Was unable to find a bus stop</exception>
        private BusStop SelectRandomStop()
        {
            float[] chances = new float[busStops.Length];
            float totalWeighting = 0;
            for (int i = 0; i < chances.Length; i++)
            {
                chances[i] = busStops[i].Weighting;
                totalWeighting += chances[i];
            }
            for (int i = 0; i < chances.Length; i++)
            {
                chances[i] /= totalWeighting;
                if (i > 0) { chances[i] += chances[i - 1]; }; // makes chances cumulative
            }

            double random = Randomiser.NextDouble();

            for (int i = chances.Length - 1; i >= 0; i--)
            {
                if (chances[i] > random) { return busStops[i]; };
            }
            throw new BusStopNotFoundException("Cannot find bus stop :(");
        }
    }
}