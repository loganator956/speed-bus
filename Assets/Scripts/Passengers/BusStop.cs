using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedBus.Gameplay.Passengers
{
    [RequireComponent(typeof(BoxCollider))]
    public class BusStop : MonoBehaviour
    {
        public int PassengerCount;
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
                PassengerCount++;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    } 
}
