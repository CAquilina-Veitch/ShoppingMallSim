using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class Wallet : MonoBehaviour
{
    public TextMeshProUGUI uiCurrency, uiPremium;



    private int standardCurrency, premiumCurrency;

    public int visualCurrency, visualPremium;

    private float textTransitionTime = 0.25f;
    float s,p;
    float stm, ptm;

    bool sl, pl;

    bool sd, pd;
    Vector2 gc, gp;

    float maxSize = 1.7f;
    [SerializeField] Progress progress;

    public void loadToCurrent()
    {
        Currency = progress.money[0];
        Premium = progress.money[1];
    }


    public void Start()
    {
        Currency += 100;
    }

    public int Currency
    {
        get
        {
            return standardCurrency;
        }
        set
        {
            gc = new Vector2(standardCurrency, value);
            sl = true;
            s = 0;
            sd = true;
            standardCurrency = value;
        }
    }
    public int Premium
    {
        get
        {
            return premiumCurrency;
        }
        set
        {
            gp = new Vector2(Premium, value);
            pl = true;
            p = 0;
            pd = true;
            premiumCurrency = value;
        }
    }
    public bool hasCurrency(bool isPremium,int amount)
    {
        if (isPremium)
        {
            return premiumCurrency >= amount;
        }
        else
        {
            return standardCurrency >= amount;
        }
    }
    public bool trySpend(bool isPremium, int amount)
    {
        if (!hasCurrency(isPremium, amount))
        {
            return false;
        }
        else
        {
            if (isPremium)
            {
                Premium -= amount;
            }
            else
            {
                Currency -= amount;
            }
            return true;
        }
    }
    private void FixedUpdate()
    {
        if (sl)
        {
            if (s >= 1)
            {
                sl = false;
                visualCurrency = standardCurrency;
            }
            else
            {
                visualCurrency = Mathf.RoundToInt(Mathf.Lerp(gc.x, gc.y, s));
                s += Time.deltaTime / textTransitionTime;
            }
            uiCurrency.text = $"{visualCurrency}";

        }
        if (pl)
        {
            if (p >= 1)
            {
                pl = false;
                visualPremium = premiumCurrency;
            }
            else
            {
                visualPremium = Mathf.RoundToInt(Mathf.Lerp(gp.x, gp.y, p));
                p += Time.deltaTime / textTransitionTime;
            }
            uiPremium.text = $"{visualPremium}";

        }
        if (sd)
        {
            if (sl)
            {
                stm *= 1.2f;
            }
            else
            {
                stm *= 0.8f;
                if (stm < 1)
                {
                    sd = false;
                }
            }
            stm = Mathf.Clamp(stm, 1, maxSize);
            uiCurrency.transform.localScale = Vector3.one * stm;
        }
        if (pd)
        {
            if (pl)
            {
                ptm *= 1.2f;
            }
            else
            {
                ptm *= 0.8f;
                if (ptm < 1)
                {
                    pd = false;
                }
            }
            ptm = Mathf.Clamp(stm, 1, maxSize);
            uiPremium.transform.localScale = Vector3.one * ptm;
        }
    }


    //dev
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Currency += 10000;
        }
    }

}
