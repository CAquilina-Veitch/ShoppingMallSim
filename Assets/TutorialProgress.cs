using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialProgress : MonoBehaviour
{
    [SerializeField] Button[] sections;
    public int currentProgress;

    public int UpdateProgress()
    {
        try
        {
            Debug.Log($"THIS IS HERE TO FIX THE GAME DO NOT DELETE THIS DEBUG LOG HAHAH{ sections[sections.Length-1].gameObject.activeInHierarchy}");
            currentProgress = 6;
            for (int i = 0; i < sections.Length; i++)
            {
                //Debug.Log(i);
                currentProgress = sections[i].gameObject.activeInHierarchy ? i : currentProgress;

            }
        }
        catch
        {

        }
            
        return currentProgress;
    }
    public void SetProgress(int i)
    {
        currentProgress = i;
        
        for(int j = 0; j < currentProgress; j++)
        {
            sections[j].onClick.Invoke();
        }

    }

}
