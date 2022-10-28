using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Target Properties")]
    public Transform TargetTransform;

    [Header("FOV Effects")]
    public bool ApplyFOVEffects = true;
    public AnimationCurve VelocityToFOV;

    // Component References
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (TargetTransform == null)
        {
            return;
        }
        transform.position = TargetTransform.position;
        if (ApplyFOVEffects)
        {
            Rigidbody rb = TargetTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _camera.fieldOfView = VelocityToFOV.Evaluate(rb.velocity.magnitude);
            }
        }
    }
}
