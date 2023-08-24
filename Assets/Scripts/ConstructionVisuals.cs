using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using spr = UnityEngine.SpriteRenderer;
using os = OccupiedSpace;
using UnityEngine.UI;
using TMPro;
using System;

public class ConstructionVisuals : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] os oS;
    [SerializeField] RectMask2D mask;
    TimeSpan total;
    TimeSpan elapsed;
    DateTime timeIn;
    [SerializeField] TextMeshProUGUI timeLeft;
    float fraction;
    ConstructionTimePacket ctp;
    public void SetPacket(ConstructionTimePacket packet)
    {
        ctp = packet;
        canvas.SetActive(true);
        timeIn = packet.timeIn;
        total = packet.timeOut - packet.timeIn;
        fraction = Mathf.Min(0.01f * (float)total.TotalSeconds, 1);
        StartCoroutine(updateTimer());
    }
    public void SkipTimer()
    {
        total = TimeSpan.Zero;
        ctp.timeOut = DateTime.Now;
        Debug.LogWarning(oS.rM.currentConstructions.Contains(ctp));
        oS.rM.currentConstructions[oS.rM.currentConstructions.FindIndex(x => x.coord == ctp.coord)] = ctp;
        oS.rM.SortPackets();
    }
    
    IEnumerator updateTimer()
    {
        UpdatePercentage();
        if ((timeIn - DateTime.Now).TotalHours >= 1)
        {
            yield return new WaitForSeconds(60);
        }
        else
        {
            yield return new WaitForSeconds(fraction);
        }
        
        elapsed = DateTime.Now - timeIn;
        UpdatePercentage();
        if (elapsed.TotalSeconds >= total.TotalSeconds)
        {
            //done idk what to doehere
            Debug.Log($"{elapsed.TotalSeconds}, {elapsed}, elapsed: from {total.TotalSeconds}, {total} ");
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(updateTimer());
        }
    }
    void UpdatePercentage()
    {
        mask.padding = new Vector4(0, 0, 1.4f * (float)elapsed.TotalSeconds / (float)total.TotalSeconds);
        //timeLeft.text = (float)(total - elapsed).TotalMinutes > 60 ? $"{Mathf.RoundToInt((float)(total - elapsed).TotalHours)}:{((total - elapsed).TotalMinutes < 9.5f ? "0" : null)}{Mathf.RoundToInt((float)(total - elapsed).TotalMinutes)}" : $"{Mathf.RoundToInt((float)(total - elapsed).TotalMinutes)}:{((total - elapsed).TotalSeconds < 9.5f ? "0" : null)}{Mathf.RoundToInt((float)(total - elapsed).TotalSeconds)}";
        timeLeft.text = (total - elapsed).ConvertTimeSpanToDigitalClock();
    }

}
