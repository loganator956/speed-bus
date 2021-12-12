using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPopupScript : MonoBehaviour
{
    public FloatUnityEvent onFinishEvent;
    public AnimationCurve TCurve; // used to map t1 to t2 (t2 the actual value, makes it appear a bit smoother, I think)
    public float TSpeed = 1f;

    public Transform SliderThing;
    public Vector3 SliderPositionA, SliderPositionB;
    // Start is called before the first frame update
    void Start()
    {

    }

    float rawT = 0f;
    bool direction = true;

    private const float _badRangeSize = (70f / 192f); // this is manually calculated. However this means that the range cannot be changed

    // Update is called once per frame
    void Update()
    {
        if (direction)
        {
            rawT += Time.deltaTime * TSpeed;
            if (rawT >= 1) { direction = !direction; };
        }
        else
        {
            rawT -= Time.deltaTime * TSpeed;
            if (rawT <= 0) { direction = !direction; };
        }
        // apply position of slider
        SliderThing.localPosition = Vector3.Lerp(SliderPositionA, SliderPositionB, GetRealT(rawT));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnFinish();
        }
    }

    float GetRealT(float raw)
    {
        return (TCurve.Evaluate(raw));
    }

    void OnFinish()
    {
        float finalT = GetRealT(rawT);

        float percentageOfPassengers = 0f;
        if (finalT > _badRangeSize && finalT < 1 - _badRangeSize)
        {
            // within the good range
            percentageOfPassengers = 1f;
        }
        else
        {
            // within the bad range
            if (finalT < _badRangeSize)
            {
                // percent through the bad range
                percentageOfPassengers = finalT / _badRangeSize;
            }
            else if (finalT > _badRangeSize)
            {
                // percentage through the bad range
                percentageOfPassengers = 1 - ((finalT - (1 - _badRangeSize)) / _badRangeSize);
            }
        }

        onFinishEvent.Invoke(percentageOfPassengers);

        gameObject.SetActive(false);
    }
}
