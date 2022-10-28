using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedBus.Gameplay.Passengers
{
    [RequireComponent(typeof(BoxCollider))]
    public class DropOffPoint : MonoBehaviour
    {
        public float CoolDownTimer = 0f;

        private BoxCollider _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if (CoolDownTimer> 0f) { CoolDownTimer -= Time.deltaTime; };
        }

        private void OnDisable()
        {
            _boxCollider.enabled = false;
        }

        private void OnEnable()
        {
            _boxCollider.enabled = true;
        }

        private void OnDrawGizmos()
        {
            if (this.enabled) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; };
            Gizmos.DrawSphere(transform.position, 1f);
        }
    } 
}
