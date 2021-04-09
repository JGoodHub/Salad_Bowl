﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCounterUI : Singleton<TileCounterUI>
{

    public GameObject tileCounterPrefab;
    public Transform counterEntriesParent;

    private Dictionary<TileSelectionBehaviour.Type, TileCounterEntry> counterEntries;

    public void CreateAndPopulateQuotaEntries(LevelQuotaData.Quota[] quotas)
    {
        counterEntries = new Dictionary<TileSelectionBehaviour.Type, TileCounterEntry>();

        for (int i = 0; i < quotas.Length; i++)
        {
            TileCounterEntry counterEntry = Instantiate(tileCounterPrefab, counterEntriesParent).GetComponent<TileCounterEntry>();

            counterEntry.SetImage(GameCoordinator.Instance.TileLoadouts.GetLoadoutByType(quotas[i].type).image);
            counterEntry.SetCounter(0);
            counterEntry.SetTotal(quotas[i].target);
            counterEntry.SetCompleted(false);

            counterEntries.Add(quotas[i].type, counterEntry);
        }
    }

    public void SetCounterForType(TileSelectionBehaviour.Type type, int newCount)
    {
        if (counterEntries.ContainsKey(type) == false)
            return;

        counterEntries[type].SetCounter(newCount);
    }

    public void SetCompletetionForType(TileSelectionBehaviour.Type type, bool complete)
    {
        if (counterEntries.ContainsKey(type) == false)
            return;

        counterEntries[type].SetCompleted(complete);
    }


}
