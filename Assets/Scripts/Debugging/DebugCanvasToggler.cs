using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvasToggler : MonoBehaviour
{
    private Canvas _canvas;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        if (Debug.isDebugBuild)
        {
            _canvas.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // TODO: Create keybindings for toggling this menu
    }
}
