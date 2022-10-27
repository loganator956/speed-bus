using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpeedBus.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public float TickTimerInterval = 1f;
        public static UnityEvent GameTickEvent = new UnityEvent();

        private float _tickT = 0f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _tickT += Time.deltaTime;
            if (_tickT > TickTimerInterval)
            {
                _tickT = 0f;
                GameTickEvent.Invoke();
            }
        }
    } 
}
