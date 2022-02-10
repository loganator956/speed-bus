using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusStopIcon : MonoBehaviour
{
    public Transform Target;
    public BusStopScript BusStop;
    public Sprite DisabledSprite, CompleteSprite, UnsatisfiedSprite;
    public Color DisabledColour, CompleteColour, UnsatisfedColour;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(Target.position);

        // check for status
        if (BusStop.PassengerRequest == 0)
        {
            _image.sprite = DisabledSprite;
            _image.color = DisabledColour;
        }
        else if (BusStop.CheckSatisfication())
        {
            _image.sprite = CompleteSprite;
            _image.color = CompleteColour;
        }
        else
        {
            _image.sprite = UnsatisfiedSprite;
            _image.color = UnsatisfedColour;
        }
    }
}
