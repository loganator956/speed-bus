using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public int PlayerScore { get; private set; }

    public UnityEvent<int> PlayerScoreChanged = new UnityEvent<int>();

    public void AwardPlayer(int amount, string message, Vector3 worldPos)
    {
        PlayerScore += amount;
        PlayerScoreChanged.Invoke(PlayerScore);
        // TODO: Message
    }

    public void PunishPlayer(int amount, string message, Vector3 worldPos)
    {
        PlayerScore -= Math.Abs(amount);
        PlayerScoreChanged.Invoke(PlayerScore);
        // TODO: Message in game world
    }
}
