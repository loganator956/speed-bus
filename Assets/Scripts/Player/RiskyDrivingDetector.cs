using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RiskyDrivingDetector : MonoBehaviour
{
    private float _highSpeedTimer = 0f;
    const float HighSpeedBonusAwardThreshold = 5f;
    const int HighSpeedRewardBonus = 1;
    const float HighSpeedMinimumVelocity = 12.5f;

    const int CollisionPunishment = 2;
    const float CollisionForceThreshold = 5f;

    private Rigidbody _rigidbody;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        if (_rigidbody.velocity.magnitude > HighSpeedMinimumVelocity)
        {
            // is going fast
            _highSpeedTimer += Time.deltaTime;
            if (_highSpeedTimer > HighSpeedBonusAwardThreshold)
            {
                _highSpeedTimer = 0f;
                _scoreManager.AwardPlayer(HighSpeedRewardBonus, "High Speed", transform.position);
            }
        }
        else
        {
            _highSpeedTimer -= Time.deltaTime * 2;
            _highSpeedTimer = Mathf.Max(_highSpeedTimer, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > CollisionForceThreshold)
        {
            _scoreManager.PunishPlayer(CollisionPunishment, "Crash", collision.GetContact(0).point);
        }
    }
}
