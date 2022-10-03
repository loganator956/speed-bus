using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class BusNoiseScript : MonoBehaviour
{
    // Default: (0.0, 0.4) to (4.0, 3.0)
    public AnimationCurve VelocityToPitchCurve;

    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _audioSource.pitch = VelocityToPitchCurve.Evaluate(_rigidbody2D.velocity.magnitude);
    }
}
