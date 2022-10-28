using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreGUIText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        _text.text = string.Format("{0:n0}", _scoreManager.PlayerScore);
    }
}
