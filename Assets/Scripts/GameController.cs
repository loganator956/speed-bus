using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public BusStop[] busStops { get; private set; }
    
    private float _totalWeighting = 0;

    private void Awake()
    {
        // find all the bus stops
        busStops = FindObjectsOfType<BusStop>();
        
        foreach(BusStop stop in busStops)
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
        // TODO: Change this to be dynamically set
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
        }
    }
}
