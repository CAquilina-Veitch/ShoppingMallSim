using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class HiredWorkerUI : MonoBehaviour
{
    [SerializeField] WorkerInfo _info;

    Sprite workerSpecieSprite;
    [SerializeField] Sprite[] tirednessIndicators;
    [SerializeField] TextMeshProUGUI workerName;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] Image icon;
    [SerializeField] Image tirednessIcon;
    [SerializeField] Image colourBG;

    ScrollRect scroller;

    public Business bsns;

    bool isWorking;

    public currentWorkerProcess process;

    private void OnEnable()
    {
        workerSpecieSprite = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)info.specie].face;
        scroller = GetComponent<ScrollRect>();

    }

    public WorkerInfo info
    {
        set
        {
            _info = value;
            if (!(value.name == "null"||value.name ==null|| value.name == ""))
            {
                
                updateVisuals();
            }
            else
            {
                BlankFace();
            }

        }
        get
        {
            return _info;
        }
    }
    public void BlankFace()
    {

        //int tiredness = Mathf.RoundToInt((workerSpecieSprites.Length - 1 < 0 ? 0 : workerSpecieSprites.Length - 1) * w.energy);
        icon.sprite = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().blank;
        workerName.text = "Empty";
        level.text = null;
        Debug.Log(1);
    }
    public void updateVisuals()
    {
        if (_info.name == "null" || _info.name == "")
        {
            BlankFace();
        }
        else
        {
            UpdateTiredness();
            icon.sprite = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)info.specie].face;
            workerName.text = info.name;
            level.text = $"{info.level}";        
            /*                  ////THIS IS THE COLOUR SECTION IT LOOKS WORSE WITH IT ON......
            if (bsns.activeWorkers.Contains(this))
            {
                colourBG.color = new Color(0.568807f, 1, 0.5613208f, 1);
            }
            else
            {
                colourBG.color = Color.Lerp(new Color(0.5850837f, 0.6212634f, 0.7169812f, 1),new Color(0.7607843f,1, 0.9921569f,1),info.Energy/120f);
            }*/
            bsns.updateVisualWorkers();
        }



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
        if ((_info.name == "null" || _info.name == null || _info.name == ""))
        {
            Unswipe();
        }
        else
        {
            if (!Input.GetKey(KeyCode.Mouse0) && Input.touchCount == 0)
            {
                if (scroller.horizontalNormalizedPosition > 0.05 && scroller.horizontalNormalizedPosition < 0.95f)
                {
                    scroller.velocity = Vector2.right * 100 * scroller.horizontalNormalizedPosition;


                }
                else if (scroller.horizontalNormalizedPosition <= 0.05f)
                {
                    //fire

                    Unswipe();
                    UnhiredWorkers uhw = GameObject.FindGameObjectWithTag("UnhiredWorkers").GetComponent<UnhiredWorkers>();
                    Debug.LogError("AIUGUUSeWQTujheJHj");
                    if (uhw.unhiredWorkers.Count < 4)
                    {
                        Debug.LogError("AIUGUUSeWQTuj3333333333heJHj");
                        bsns.hiredWorkers.Remove(info);
                        Debug.LogError(bsns.hiredWorkers.Count + "         !1@#@#!@#");
                        uhw.unhiredWorkers.Add(info);
                        info = new WorkerInfo() { name = "null" };
                        _info = new WorkerInfo() { name = "null" };
                        BlankFace();
                        updateVisuals();
                        uhw.showList(true);

                        bsns.UpdateWorkerUI();
                        bsns.updateVisualWorkers();
                        foreach (UnhiredWorkerUI uhwui in uhw.unhiredWUI)
                        {
                            uhwui.updateVisuals();
                        }
                        uhw.showList(false);
                        uhw.showList(true);
                        bsns.UpdateWorkerUI();
                    }
                }
            }
        }
        

    }
    public void Unswipe()
    {
        scroller.horizontalNormalizedPosition = 1;
        scroller.velocity = Vector2.zero;
    }

    public void ToggleWork()
    {
        if (!(_info.name == "null" || _info.name == ""))
        {
            bsns.toggleWorker(this);
                    updateVisuals();
        }
        
    }
    

}
