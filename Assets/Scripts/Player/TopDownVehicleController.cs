using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class TopDownVehicleController : MonoBehaviour
{
    [Header("Vehicle Properties")]
    public float AccelerationSpeed = 30f;
    public float MaxTurnSpeed = 5.0f;
    [Header("Vehicle Physics")]
    [Range(0f, 1f)]
    public float MinDecelerateFactor = 0.995f; // Decelerate Factor influences the amount of speed the player KEEPS. So a factor of 0.995f means that the player keeps 0.995f of their velocity each frame
    [Range(0f, 1f)]
    public float MaxDecelerateFactor = 0.9f;
    public AnimationCurve DecelerateCurve; // Determines how the decelerate factor changes from min to max over time (evaluates the T value of lerp between min and max decelerate factors from time)
    public float DriftFactor = 0.55f; // The amount of sideways velocity to keep (0.55 * right velocity)

    // Unity Input System
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    // Components
    private Rigidbody _rigidbody;

    // Temp Input Values
    private float _accelerationInput = 0f;
    private float _decelerationInput = 0.995f;

    // Constants
    private const float SteerAngleDeadzone = 0.05f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        /*_playerInput.onControlsChanged += OnControlsChanged;*/

        _moveAction = _playerInput.actions["Movement"];

        _rigidbody = GetComponent<Rigidbody>();
    }

    float decelerateT = 0f;

    private const float DecelerationTRate = 2f;

    // Update is called once per frame
    void Update()
    {
        Vector2 dirInput = _moveAction.ReadValue<Vector2>();
        Debug.Log(dirInput);
        if (dirInput.magnitude > SteerAngleDeadzone)
        {
            Quaternion rotation = GetRotationFromInput(dirInput);
            float angle = Quaternion.Angle(rotation, transform.rotation);
            _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, MaxTurnSpeed / angle/*0.2f*/));
        }

        _accelerationInput = GetAccelerationFromInput(dirInput);

        if (_accelerationInput < 0.05f)
        {
            // no inputs (slowdown)
            decelerateT += Time.deltaTime;
            decelerateT = Mathf.Clamp01(decelerateT);
        }
        else
        {
            decelerateT -= Time.deltaTime * DecelerationTRate;
            decelerateT = Mathf.Clamp01(decelerateT);
        }
        _decelerationInput = Mathf.Lerp(MinDecelerateFactor, MaxDecelerateFactor, decelerateT);
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplyPhysicsForces();
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceVector = transform.forward * _accelerationInput * AccelerationSpeed;

        _rigidbody.AddForce(engineForceVector, ForceMode.Force);
    }

    private void ApplyPhysicsForces()
    {
        Vector3 forwardVelocity = transform.forward * Vector3.Dot(_rigidbody.velocity, transform.forward);
        Vector3 rightVelocity = transform.right * Vector3.Dot(_rigidbody.velocity, transform.right);

        _rigidbody.velocity = (forwardVelocity * _decelerationInput) + (rightVelocity * DriftFactor);
    }

    private Quaternion GetRotationFromInput(Vector2 input)
    {
        float angle = Vector2.SignedAngle(Vector3.up, input);
        return Quaternion.Euler(0, -angle + 45f, 0);
    }

    private float GetAccelerationFromInput(Vector2 input)
    {
        return Mathf.Clamp01(input.magnitude);
    }

    private void OnDisable()
    {
        // TODO: reset the current velocity stuff
    }
}
