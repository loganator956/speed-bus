using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownVehicleController : MonoBehaviour
{
    public float DriftFactor = 0.55f;
    public float AccelerationSpeed = 40f;
    public float DecelerateFactor = 0.05f;
    public float MaxTurnSpeed = 5.0f;

    private MainControls mainControls;

    private Rigidbody2D _rigidbody2D;

    private float accelerationInput = 0f;

    private const float SteerAngleDeadzone = 0.05f;

    private void Awake()
    {
        mainControls = new MainControls();
        mainControls.Enable();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dirInput = mainControls.Bus.Movement.ReadValue<Vector2>();

        if (dirInput.magnitude > SteerAngleDeadzone)
        {
            Quaternion rotation = GetRotationFromInput(dirInput);
            float angle = Quaternion.Angle(rotation, transform.rotation);
            Debug.Log(angle);
            Debug.Log(MaxTurnSpeed / angle);
            _rigidbody2D.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, MaxTurnSpeed / angle/*0.2f*/));
        }

        accelerationInput = GetAccelerationFromInput(dirInput);
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ReduceOrthogonalVelocity();
    }

    private void ApplyEngineForce()
    {
        Vector2 engineForceVector = transform.up * accelerationInput * AccelerationSpeed;

        _rigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ReduceOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = (forwardVelocity * DecelerateFactor) + (rightVelocity * DriftFactor);
    }

    private Quaternion GetRotationFromInput(Vector2 input)
    {
        float angle = Vector2.SignedAngle(Vector3.up, input);
        return Quaternion.Euler(0, 0, angle);
    }

    private float GetAccelerationFromInput(Vector2 input)
    {
        return Mathf.Clamp01(input.magnitude);
    }
}
