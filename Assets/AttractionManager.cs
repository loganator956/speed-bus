using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpeedBus.Gameplay
{
    public class AttractionManager : MonoBehaviour
    {
        GameController _gameController;
        private void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
            GameController.GameTickEvent.AddListener(OnGameTick);

            attractionsList.AddRange(FindObjectsOfType<Attraction>());
        }

        public const float AttractionCooldownTMax = 60;
        private float _attractionCooldownT = 0f;
        public float AttractionCooldownT { get { return _attractionCooldownT; } private set { _attractionCooldownT = value; } }

        private List<Attraction> attractionsList = new List<Attraction>();

        public UnityEvent<Attraction> OnAttractionBecomeAvailable;
        public Attraction CurrentAttraction { get; private set; }

        public UnityEvent<float> OnAttractionUsed;

        void OnGameTick()
        {

        }

        private void Update()
        {
            if (AttractionCooldownT > 0)
            {
                AttractionCooldownT -= Time.deltaTime;
            }
            if (AttractionCooldownT <= 0 && CurrentAttraction == null)
            {
                AttractionCooldownT = 0;
                CurrentAttraction = attractionsList[GameController.Randomiser.Next(0, attractionsList.Count)];
                OnAttractionBecomeAvailable.Invoke(CurrentAttraction);
            }
        }

        public void UseAttraction(float amount)
        {
            CurrentAttraction = null;
            OnAttractionUsed.Invoke(amount);
            AttractionCooldownT = AttractionCooldownTMax;
        }
    }
}