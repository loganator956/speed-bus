using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform PlayerTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, -10);
    }
}
