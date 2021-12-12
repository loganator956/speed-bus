using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingStopsGUIScript : MonoBehaviour
{
    Text text;
    GameController gameController;

    private void Awake()
    {
        text = GetComponent<Text>();
        gameController = FindObjectOfType<GameController>();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = $"THERE ARE STILL {gameController.GetRemainingStops()} STOPS REMAINING";
    }
}
