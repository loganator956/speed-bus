using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(1);
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}
