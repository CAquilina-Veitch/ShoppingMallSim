using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using spr = UnityEngine.SpriteRenderer;
using os = OccupiedSpace;
using UnityEngine.UI;
using TMPro;

public class ConstructionVisuals : MonoBehaviour
{
    spr sR;
    [SerializeField] os oS;
    [SerializeField] RectMask2D mask;
    float fraction;
    float totalTime;
    float elapsedTime;
    private void OnEnable()
    {
        sR = GetComponent<spr>();

        //make the walls appear or not bsed off cam pos;


    }
    public void SetPacket(ConstructionTimePacket packet)
    {
        elapsedTime = 0;
        totalTime = (packet.timeOut - packet.timeIn).Seconds;
        fraction = 0.01f * totalTime;
        StartCoroutine(updateTimer());
    }
    IEnumerator updateTimer()
    {
        yield return new WaitForSeconds(fraction);
        elapsedTime += fraction;
        UpdatePercentage();
        if (elapsedTime >= totalTime)
        {
            //done idk what to doehere
        }
        else
        {
            StartCoroutine(updateTimer());
        }
    }
    void UpdatePercentage()
    {
        mask.padding = new Vector4(0, 0, elapsedTime/totalTime);
    }

}
