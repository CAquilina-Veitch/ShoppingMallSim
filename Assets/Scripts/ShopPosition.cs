using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPosition : MonoBehaviour
{
    [SerializeField] GameObject backWall;
    public SpriteRenderer[] npcs;
    [SerializeField] Sprite[] forwardSprites;
    [SerializeField] Sprite[] backSprites;


    public void ChangeSprite(int who, species to)
    {
        Sprite[] temp = who != 1 ? ref forwardSprites : ref backSprites;
        npcs[who].sprite = temp[(int)to] ;
    }
    public void ChangeSprite(int who)
    {
        npcs[who].sprite = null;
    }
}
