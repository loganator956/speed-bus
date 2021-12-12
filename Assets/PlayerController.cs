using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AnimationCurve SpeedCurve;
    public float SpeedCurveRate = 0.5f;
    public float MaxSpeed = 4f;
    // Start is called before the first frame update
    void Start()
    {

    }

    private Vector2 _inputs = new Vector2(0, 0);

    private float _speedCurveT = 0f;

    public bool EnableInputs = true;

    // Update is called once per frame
    void Update()
    {
        if (EnableInputs)
        {
            bool isMoving = false;
            Vector2 newInputs = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                isMoving = true;
                newInputs += Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                isMoving = true;
                newInputs += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                isMoving = true;
                newInputs += Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                isMoving = true;
                newInputs += Vector2.right;
            }
            if (isMoving)
            {
                _speedCurveT += Time.deltaTime * SpeedCurveRate;
                _inputs = newInputs;
            }
            else if (isMoving)
            {
                _speedCurveT += Time.deltaTime * SpeedCurveRate * -1f;
            }
            _speedCurveT = Mathf.Clamp01(_speedCurveT);
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_inputs.x * Time.fixedDeltaTime * SpeedCurve.Evaluate(_speedCurveT), 0, _inputs.y * Time.fixedDeltaTime * SpeedCurve.Evaluate(_speedCurveT));
    }
}
