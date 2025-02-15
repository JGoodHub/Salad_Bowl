﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovesTrackerBehaviour : MonoBehaviour
{
    private int movesRemaining;

    [Header("Event Triggers")]
    [Space]

    public UnityEvent OnMovesExhausted;

    private void Start()
    {
        TileChainManager.Instance.OnTileChainDestroyed.AddListener(DecrementMovesCounter);

        movesRemaining = GameCoordinator.Instance.ActiveLevel.moveLimit;
        MovesTrackerUI.Instance.SetMovesRemaining(movesRemaining);
    }

    /// <summary>
    /// Decrement the remaining moves and update the UI
    /// </summary>
    private void DecrementMovesCounter()
    {
        movesRemaining--;

        MovesTrackerUI.Instance.SetMovesRemaining(movesRemaining);

        if (movesRemaining <= 0 && TileQuotaBehaviour.Instance.CheckAllQuotasComplete() == false)
        {
            OnMovesExhausted?.Invoke();
        }
    }
}
