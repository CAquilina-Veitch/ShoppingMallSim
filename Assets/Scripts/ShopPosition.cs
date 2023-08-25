using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopPosition : MonoBehaviour
{
    [SerializeField] GameObject backWall;
    public SpriteRenderer[] npcs;
    [SerializeField] Sprite[] forwardSprites;
    [SerializeField] Sprite[] backSprites;
    public Transform[] wanderPoints;

    public UnityEvent switchToAd;
    public UnityEvent switchToNormal;
    public void ChangeSprite(int who, species to)
    {
        Sprite[] temp = who != 1 ? ref forwardSprites : ref backSprites;
        npcs[who].sprite = temp[(int)to] ;
    }
    public void ChangeSprite(int who)
    {
        npcs[who].sprite = null;
    }
    public void changeOrder(int to)
    {
        SpriteRenderer[] sRs = GetComponentsInChildren<SpriteRenderer>(true);
        foreach(SpriteRenderer sr in sRs)
        {
            sr.sortingOrder = to;
        }
    }

    public void switchOne()
    {
        switchToAd.Invoke();
    }
    public void switchTwo()
    {
        switchToNormal.Invoke();
    }
}
