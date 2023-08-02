using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class HiredWorkerUI : MonoBehaviour
{
    [SerializeField] WorkerInfo w;

    Sprite workerSpecieSprite;
    [SerializeField] Sprite[] tirednessIndicators;
    [SerializeField] TextMeshProUGUI workerName;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] Image icon;
    [SerializeField] Image tirednessIcon;
    [SerializeField] Image colourBG;

    [SerializeField] GameObject overlay;

    ScrollRect scroller;

    public bool selected = false;

    public Business bsns;

    bool isWorking;

    public int energy = 60;

    public currentWorkerProcess process;

    private void OnEnable()
    {
        workerSpecieSprite = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)w.specie].face;
        scroller = GetComponent<ScrollRect>();

    }

    public WorkerInfo info
    {
        set
        {
            if(value.name != "null")
            {
                w = value;
                updateVisuals();
            }
            else
            {
                BlankFace();
            }

        }
        get
        {
            return w;
        }
    }
    public void BlankFace()
    {
        //int tiredness = Mathf.RoundToInt((workerSpecieSprites.Length - 1 < 0 ? 0 : workerSpecieSprites.Length - 1) * w.energy);
        icon.sprite = null;
        workerName.text = "Empty";
        level.text = null;
    }
    public void updateVisuals()
    {
        UpdateTiredness();
        icon.sprite = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)w.specie].face;
        workerName.text = w.name;
        level.text = $"{w.level}";

        if (bsns.activeWorkers.Contains(this))
        {
            colourBG.color = new Color(0.568807f, 1, 0.5613208f, 1);
        }
        else
        {
            colourBG.color = Color.Lerp(new Color(0.5850837f, 0.6212634f, 0.7169812f, 1),new Color(0.7607843f,1, 0.9921569f,1),energy/60f);
        }
        bsns.updateVisualWorkers();

    }
    public void UpdateTiredness()
    {
        /*int tiredness = Mathf.RoundToInt((workerSpecieSprites.Length - 1 < 0 ? 0 : workerSpecieSprites.Length - 1) * w.energy);
        tirednessIcon.sprite = workerSpecieSprites[tiredness];*/
    }
    public void init(Business b)
    {
        bsns = b;
        updateVisuals();
        scroller = GetComponent<ScrollRect>();
    }
    public void swiped()
    {
        if (!Input.GetKey(KeyCode.Mouse0) && Input.touchCount == 0)
        {
            //Debug.Log(scroller.horizontalNormalizedPosition);
            if (scroller.horizontalNormalizedPosition > 0.05 && scroller.horizontalNormalizedPosition < 0.95f)
            {
                scroller.velocity = Vector2.right * 100 * scroller.horizontalNormalizedPosition;
                //scroller.horizontalNormalizedPosition = Mathf.Lerp(scroller.horizontalNormalizedPosition, 0, 0.5f);
                overlay.SetActive(false);
                Debug.LogWarning("BB" + scroller.horizontalNormalizedPosition);
                selected = false;

            }
        }
        else
        {
            if (scroller.horizontalNormalizedPosition <= 0.05f)
            {
                overlay.SetActive(true);
                //Debug.LogWarning("CC" + scroller.horizontalNormalizedPosition);
                selected = true;
            }
            else
            {
                //Debug.LogWarning("BB" + scroller.horizontalNormalizedPosition);
                overlay.SetActive(false);
                selected = false;
            }
        }
        /*
        if (bsns.selected.Contains(this) && !selected)
        {
            bsns.selected.Remove(this);
        }
        else if (!bsns.selected.Contains(this) && selected)
        {
            bsns.selected.Add(this);
        }*/

    }
    public void Unswipe()
    {
        scroller.horizontalNormalizedPosition = 1;
        selected = false;
    }

    public void ToggleWork()
    {
        bsns.toggleWorker(this);
        updateVisuals();
    }
    

}
