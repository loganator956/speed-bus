using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedBus.Gameplay.Passengers
{
    [System.Serializable]
    public class Passenger
    {
        public Passenger(BusStop boardedStop)
        {
            BoardedStop = boardedStop;
            BusStop[] availableStops = Object.FindObjectsOfType<BusStop>();
            while(true)
            {
                int i = Random.Range(0, availableStops.Length);
                if (availableStops[i] != BoardedStop)
                {
                    TargetStop = availableStops[i];
                    break;
                }
            }
        }

        public Passenger(BusStop boardedStop, BusStop targetStop)
        {
            BoardedStop = boardedStop;
            TargetStop = targetStop;
        }

        private BusStop _boardedStop;
        public BusStop BoardedStop
        {
            get { return _boardedStop; }
            private set { _boardedStop = value; }
        }

        private BusStop _targetStop;
        public BusStop TargetStop
        {
            get { return _targetStop; }
            private set { _targetStop = value; }
        }


        public bool TryCompleteJourney(BusStop stop)
        {
            bool success = (stop == TargetStop);

            // TODO: Alert the gameplay scoreboard about it

            return success;
        }
    } 
}
