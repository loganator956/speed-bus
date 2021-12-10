using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGridScript : MonoBehaviour
{
    private Transform _camTransform;

    private void Awake()
    {
        _camTransform = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = _camTransform.position;
        transform.position = new Vector3(Mathf.Round(camPos.x), Mathf.Round(camPos.y), 10f);
    }
}
