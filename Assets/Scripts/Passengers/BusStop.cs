using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedBus.Gameplay.Passengers
{
    [RequireComponent(typeof(BoxCollider))]
    public class BusStop : MonoBehaviour
    {
        public List<Passenger> Passengers = new List<Passenger>();
        private void Awake()
        {
            GameController.GameTickEvent.AddListener(OnGameTickEvent_Invoked);
        }

        // TODO: Replace this counter with chances
        int t_SpawnCounter = 0;

        private void OnGameTickEvent_Invoked()
        {
            t_SpawnCounter++;
            if (t_SpawnCounter > 10)
            {
                t_SpawnCounter = 0;
                Passengers.Add(new Passenger(this));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    } 
}
