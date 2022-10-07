using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class TopDownVehicleController : MonoBehaviour
{
    [Header("Vehicle Properties")]
    public float DriftFactor = 0.55f;
    public float AccelerationSpeed = 1f;
    private float _decelerationInput = 0.995f;
    public float MinDecelerateFactor = 0.995f;
    public float MaxDecelerateFactor = 0.9f;
    public AnimationCurve DecelerateCurve;
    public float MaxTurnSpeed = 5.0f;

    private PlayerInput _playerInput;
    public PlayerInput PlayerInput
    {
        get { return _playerInput; }
    }
    private InputAction _moveAction;

    private Rigidbody _rigidbody;

    private float accelerationInput = 0f;

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

        accelerationInput = GetAccelerationFromInput(dirInput);

        if (accelerationInput < 0.05f)
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
        ReduceOrthogonalVelocity();
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceVector = transform.forward * accelerationInput * AccelerationSpeed;

        _rigidbody.AddForce(engineForceVector, ForceMode.Force);
    }

    private void ReduceOrthogonalVelocity()
    {
        Vector3 forwardVelocity = transform.forward * Vector3.Dot(_rigidbody.velocity, transform.forward);
        Vector3 rightVelocity = transform.right * Vector3.Dot(_rigidbody.velocity, transform.right);

        _rigidbody.velocity = (forwardVelocity * _decelerationInput) + (rightVelocity * DriftFactor);
    }

    private Quaternion GetRotationFromInput(Vector2 input)
    {
        float angle = Vector2.SignedAngle(Vector3.up, input);
        return Quaternion.Euler(0, -angle, 0);
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
