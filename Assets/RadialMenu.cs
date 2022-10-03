using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class RadialMenu : MonoBehaviour
{
    private float _value = 0f;
    public float Value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnValueChanged.Invoke(Value);
        }
    }
    public UnityEvent<float> OnValueChanged;

    private Image _image;

    private void Awake()
    {
        OnValueChanged.AddListener(OnValueChanged_Invoked);

        _image = GetComponent<Image>();
    }

    private void OnValueChanged_Invoked(float val)
    {
        _image.fillAmount = Mathf.Lerp(_image.fillAmount, val, 0.75f);
    }
}
