using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UnhiredWorkerUI : MonoBehaviour
{
    WorkerInfo w;

    [SerializeField] Sprite[] specieSprites;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] Image icon;

    [SerializeField] GameObject overlay;

    ScrollRect scroller;

    public bool selected = false;

    public UnhiredWorkers UHWM;

    private void OnEnable()
    {
        scroller = GetComponent<ScrollRect>();
    }

    public WorkerInfo info
    {
        set
        {
            w = value;
            updateFace();
        }
    }

    public void updateFace()
    {
        icon.sprite = specieSprites[(int)w.specie];
        name.text = w.name;
        level.text = $"{w.level}";
    }

    public void swipeAmount(float amt)
    {
        Debug.Log(scroller.horizontalNormalizedPosition);
        if (!Input.GetKey(KeyCode.Mouse0) && Input.touchCount == 0)
        {
            if(scroller.horizontalNormalizedPosition > 0.05&& scroller.horizontalNormalizedPosition < 0.95f)
            {
                scroller.velocity = Vector2.right * 100* scroller.horizontalNormalizedPosition;
                //scroller.horizontalNormalizedPosition = Mathf.Lerp(scroller.horizontalNormalizedPosition, 0, 0.5f);
                overlay.SetActive(false);
                Debug.LogWarning("BB" + scroller.horizontalNormalizedPosition);
                selected = false;
            }
        }
        else
        {
            if (scroller.horizontalNormalizedPosition >= 0.95f)
            {
                overlay.SetActive(true);
                Debug.LogWarning("CC" + scroller.horizontalNormalizedPosition);
                selected = true;
            }
            else
            {
                overlay.SetActive(false);
                selected = false;
            }
        }

    }

    


}
