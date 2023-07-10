using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UnhiredWorkerUI : MonoBehaviour
{
    WorkerInfo w;

    //[SerializeField] Sprite[] specieSprites;
    Sprite[] workerSpecieSprites;

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
        workerSpecieSprites = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)w.specie].face;
    }

    public WorkerInfo info
    {
        set
        {
            w = value;
            updateFace();
        }
        get
        {
            return w;
        }
    }
    public void init(UnhiredWorkers u)
    {
        UHWM = u;
        scroller = GetComponent<ScrollRect>();
        workerSpecieSprites = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[(int)w.specie].face;
    }

    public void updateFace()
    {
        Debug.Log(gameObject);
        Debug.Log(w);
        Debug.Log(w.energy);
        Debug.Log(workerSpecieSprites);
        Debug.Log(workerSpecieSprites.Length);

        int tiredness = Mathf.RoundToInt((workerSpecieSprites.Length - 1 < 0 ? 0 : workerSpecieSprites.Length - 1) * w.energy);
        icon.sprite = workerSpecieSprites[tiredness];
        workerName.text = w.name;
        level.text = $"{w.level}";
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
