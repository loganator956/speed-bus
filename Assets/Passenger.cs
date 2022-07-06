using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedBus.Gameplay
{
    public class Passenger
    {
        public BusStop TargetStop { get; set; }
        public float TravelTime { get; private set; }
        public float WaitTime { get; private set; }
    }
}