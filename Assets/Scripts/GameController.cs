using System.Collections;
using System.Collections.Generic;
using SpeedBus.Gameplay.Passengers;
using UnityEngine;
using UnityEngine.Events;

namespace SpeedBus.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public float TickTimerInterval = 1f;
        public static UnityEvent GameTickEvent = new UnityEvent();

        private float _tickT = 0f;

        private void Awake()
        {
            dropOffPoints = FindObjectsOfType<DropOffPoint>();
        }

        private void Start()
        {
            PickNewDropOffPoint();
        }

        void Update()
        {
            _tickT += Time.deltaTime;
            if (_tickT > TickTimerInterval)
            {
                _tickT = 0f;
                GameTickEvent.Invoke();
            }
        }

        DropOffPoint[] dropOffPoints;

        public void PickNewDropOffPoint()
        {
            foreach (DropOffPoint dropOffPoint in dropOffPoints)
            {
                dropOffPoint.enabled = false;
            }
            CurrentDropOffPoint = dropOffPoints[Random.Range(0, dropOffPoints.Length)];
            CurrentDropOffPoint.enabled = true;
            DropOffPoint_Changed.Invoke(CurrentDropOffPoint);
        }

        public DropOffPoint CurrentDropOffPoint { get; private set; }
        public UnityEvent<DropOffPoint> DropOffPoint_Changed = new UnityEvent<DropOffPoint>();
    } 
}
