using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinPartical : MonoBehaviour
{
    [SerializeField] Color[] clrs;
    [SerializeField] ParticleSystem pS;
    
    public void SetColor(int prize)
    {
        pS.startColor = clrs[prize];
        pS.Play();
    }

}
