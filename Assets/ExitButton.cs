using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public void OnClick()
    {
        Application.Quit();
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}
