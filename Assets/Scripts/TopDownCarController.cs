using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    public float DriftFactor = 0.55f;
    public float AccelerationSpeed = 30f;
    public float TurnSpeed = 3.5f;
    public float DecelerateFactor = 0.05f;

    public AnimationCurve TurnSpeedCurve;

    public static bool EnableDriving = true;

    float accelerationInput, steerInput, rotationAngle;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnableDriving)
        {
            accelerationInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
        else
        {
            steerInput = accelerationInput = 0f;
        }
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        ReduceOrthogonalVelocity();
    }

    private void ApplyEngineForce()
    {
        Vector2 engineForceVector = transform.up * accelerationInput * AccelerationSpeed;

        _rigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        rotationAngle -= steerInput * TurnSpeed * (TurnSpeedCurve.Evaluate(_rigidbody2D.velocity.magnitude));

        _rigidbody2D.MoveRotation(rotationAngle);
    }

    /// <summary>
    /// Reduces orthogonal velocity (left/right basically)
    /// </summary>
    private void ReduceOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = (forwardVelocity * DecelerateFactor) + (rightVelocity * DriftFactor);
    }
}
