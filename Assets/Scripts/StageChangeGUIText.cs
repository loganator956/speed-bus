using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageChangeGUIText : MonoBehaviour
{
    Text text;
    Animator animator;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStageChanged()
    {
        int currentStage = FindObjectOfType<GameController>().CurrentStage;
        animator.SetTrigger("Animate");
        text.text = $"STAGE CHANGE: {currentStage}";
    }
}
