﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileSelectionBehaviour))]
public class TileMovementBehaviour : MonoBehaviour
{
    public TileBehaviour ParentBehaviour { get => GetComponent<TileBehaviour>(); }

    public float averageSpeed;
    public AnimationCurve traversalCurve;

    public bool Moving { get; private set; }
    public Vector2Int gridRef;

    public UnityTileEvent OnTileStartedMoving;
    public UnityTileEvent OnTileFinishedMoving;

    private void Awake()
    {
        gridRef = Vector2Int.one * -1;
    }

    public void SyncPositionToGridRef(bool instant)
    {
        MoveToGridRef(gridRef.x, gridRef.y, instant);
    }

    /// <summary>
    /// Move the tile to the world position of the given grid reference
    /// </summary>
    public void MoveToGridRef(int x, int y, bool instant)
    {
        Vector2Int newGridRef = new Vector2Int(x, y);

        if (instant)
        {
            transform.position = TileGridManager.Instance.GridToWorldSpace(x, y, transform.parent.position.z);
            gridRef.x = x;
            gridRef.y = y;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(MoveToGridRefCoroutine(newGridRef));
        }
    }

    /// <summary>
    /// Coroutine animation to move the tile to the new grid reference
    /// </summary>
    /// <param name="newGridRef"></param>
    /// <returns></returns>
    private IEnumerator MoveToGridRefCoroutine(Vector2Int newGridRef)
    {
        // Check if the positions are equal already

        Vector3 sourcePosition = transform.position;
        Vector3 targetPosition = TileGridManager.Instance.GridToWorldSpace(newGridRef.x, newGridRef.y, transform.parent.position.z);

        if (sourcePosition.Equals(targetPosition))
        {
            yield break;
        }

        Moving = true;
        OnTileStartedMoving?.Invoke(ParentBehaviour);

        // Move to the next world position using the animation curve to control the speed

        float distance = Vector3.Distance(sourcePosition, targetPosition);
        float traversalTime = distance / averageSpeed;

        float t = 0;
        while (t < traversalTime)
        {
            transform.position = LerpUnclamped(sourcePosition, targetPosition, traversalCurve.Evaluate(t / traversalTime));
            t += Time.deltaTime;
            yield return null;
        }

        Moving = false;
        gridRef = newGridRef;
        transform.position = targetPosition;

        OnTileFinishedMoving?.Invoke(ParentBehaviour);
    }

    public Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
    {
        return a + ((b - a) * t);
    }

}
