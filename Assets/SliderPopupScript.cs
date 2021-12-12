using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPopupScript : MonoBehaviour
{
    public FloatUnityEvent onFinishEvent;
    // Start is called before the first frame update
    void Start()
    {

    }

    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            timer = 0f;
            OnFinish();
        }
    }

    void OnFinish()
    {
        onFinishEvent.Invoke(0.5f);

        gameObject.SetActive(false);
    }
}
