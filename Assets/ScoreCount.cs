using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeedBus.Gameplay;
using TMPro;

namespace SpeedBus.GUI
{
    public class ScoreCount : MonoBehaviour
    {
        private PlayerScoreController _scoreController;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _scoreController = FindObjectOfType<PlayerScoreController>();
            _text = GetComponent<TextMeshProUGUI>();

            _scoreController.ScoreChangedEvent.AddListener(OnPlayerScoreChanged);
        }

        void OnPlayerScoreChanged(int delta)
        {
            _text.text = _scoreController.Score.ToString();
        }
    }
}