using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusNoiseScript : MonoBehaviour
{
    public AnimationCurve VelocityToPitchCurve;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _audioSource.pitch = VelocityToPitchCurve.Evaluate(_rigidbody2D.velocity.magnitude);
    }
}
