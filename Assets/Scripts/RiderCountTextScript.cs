using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiderCountTextScript : MonoBehaviour
{
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        OnRiderCount_Changed();
        /*PlayerController.RiderCountChangedEvent.AddListener(OnRiderCount_Changed);*/
    }

    void OnRiderCount_Changed()
    {
        /*text.text = $"RIDERS: {PlayerController.RiderCount}";*/
    }
}
