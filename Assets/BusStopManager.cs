using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopManager : MonoBehaviour
{
    public GameObject SliderPopup;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public FloatUnityEvent ShowSliderPopup()
    {
        FloatUnityEvent newEvent = new FloatUnityEvent();

        SliderPopup.GetComponent<SliderPopupScript>().onFinishEvent = newEvent;
        SliderPopup.SetActive(true);

        return newEvent;
    }
}
