using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopManager : MonoBehaviour
{
    public GameObject SliderPopup;

    public FloatUnityEvent ShowSliderPopup()
    {
        FloatUnityEvent newEvent = new FloatUnityEvent();

        SliderPopup.GetComponent<SliderPopupScript>().onFinishEvent = newEvent;
        SliderPopup.SetActive(true);

        return newEvent;
    }
}
