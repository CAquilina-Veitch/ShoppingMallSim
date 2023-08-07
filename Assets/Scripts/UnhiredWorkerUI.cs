using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UnhiredWorkerUI : MonoBehaviour
{
    [SerializeField] WorkerInfo _info;

    //[SerializeField] Sprite[] specieSprites;
    Sprite workerFaceSprite;

    [SerializeField] TextMeshProUGUI workerName;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] Image icon;

    [SerializeField] GameObject overlay;

    ScrollRect scroller;

    public bool selected = false;

    public UnhiredWorkers UHWM;

    private void OnEnable()
    {
        scroller = GetComponent<ScrollRect>();
        workerFaceSprite = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)info.specie].face;
    }

    public WorkerInfo info
    {
        set
        {
            _info = value;
            updateVisuals();
        }
        get
        {
            return _info;
        }
    }
    public void init(UnhiredWorkers u)
    {
        UHWM = u;
        scroller = GetComponent<ScrollRect>();
        workerFaceSprite = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)info.specie].face;
    }

    public void updateVisuals()
    {
        //Debug.Log($"updating face of {info.name}");
        icon.sprite = workerFaceSprite;
        workerName.text = info.name;
        level.text = $"{info.level}";
    }

    public void swiped()
    {
        if (!Input.GetKey(KeyCode.Mouse0) && Input.touchCount == 0)
        {
            //Debug.LogWarning("BB" + scroller.horizontalNormalizedPosition);
            if (scroller.horizontalNormalizedPosition > 0.05&& scroller.horizontalNormalizedPosition < 0.95f)
            {
                scroller.velocity = Vector2.right * 100* scroller.horizontalNormalizedPosition;
                //scroller.horizontalNormalizedPosition = Mathf.Lerp(scroller.horizontalNormalizedPosition, 0, 0.5f);
                overlay.SetActive(false);
                //Debug.LogWarning("BB" + scroller.horizontalNormalizedPosition);
                selected = false;
                
            }
        }
        else
        {
            if (scroller.horizontalNormalizedPosition >= 0.95f)
            {
                overlay.SetActive(true);
                //Debug.LogWarning("CC" + scroller.horizontalNormalizedPosition);
                selected = true;
            }
            else
            {
                overlay.SetActive(false);
                selected = false;
            }
        }
        if (UHWM.selected.Contains(this)&&!selected)
        {
            UHWM.selected.Remove(this);
        }
        else if(!UHWM.selected.Contains(this) && selected)
        {
            UHWM.selected.Add(this);
        }

    }

    


}
