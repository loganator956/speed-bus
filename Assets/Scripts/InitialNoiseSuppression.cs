using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialNoiseSuppression : MonoBehaviour
{
    float timer = 0f;
    float initialVolume = 0f;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource =GetComponent<AudioSource>();
        initialVolume = audioSource.volume;
        audioSource.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 4f)
        {
            timer += Time.deltaTime;
            if (timer > 4f)
            {
                audioSource.volume = initialVolume;
            }
        }
    }
}
