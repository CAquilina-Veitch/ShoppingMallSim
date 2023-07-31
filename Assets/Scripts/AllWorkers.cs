using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class AllWorkers : MonoBehaviour
{
    public List<HiredWorkerUI> recoveringHiredWorkers = new List<HiredWorkerUI>();
    public List<UnhiredWorkerUI> recoveringFiredWorkers = new List<UnhiredWorkerUI>();
    public List<HiredWorkerUI> workingWorkers = new List<HiredWorkerUI>();

    

    private void Update()
    {
        System.DateTime a = new System.DateTime();
    }
    public void StartWork(HiredWorkerUI who)
    {
        if (!workingWorkers.Contains(who))
        {
            if (recoveringHiredWorkers.Contains(who))
            {
                UpdateAllEnergies();
                recoveringHiredWorkers.Remove(who);
            }
            workingWorkers.Add(who);
        }
    }
    public void StopWork(HiredWorkerUI who)
    {

    }
    public void ConvertRecovering(UnhiredWorkerUI whoWas)
    {

    }
    public void ConvertRecovering(HiredWorkerUI whoWas)
    {

    }
    private void OnEnable()
    {
        StartCoroutine(UpdateEverySecond());
    }
    IEnumerator UpdateEverySecond()
    {
        UpdateAllEnergies();
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateEverySecond());
    }
    public void UpdateAllEnergies()
    {

    }

}
