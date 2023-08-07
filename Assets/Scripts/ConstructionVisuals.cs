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
    private void OnEnable()
    {
        sR = GetComponent<spr>();

        //make the walls appear or not bsed off cam pos;


    }

}
