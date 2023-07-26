using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    Business business;
    Vector2[] path;
    float timePerTile = 0.8f;
    float t = 0;
    Vector2 fromCoord, toCoord;
    [SerializeField]SpriteRenderer sR;
    public void init(Business goal)
    {
        business = goal;
        path = business.oS.p.ToArray();
        StartCoroutine(walkPath());
    }

    IEnumerator walkPath()
    {
        Vector2 enterPosition = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.down;
        fromCoord = enterPosition;
        toCoord = Vector2.zero;
        t = 0;
        sR.enabled = true;
        for(int i = 0;i<path.Length;i++)
        {
            transform.position = toCoord;
            fromCoord = path[i];
            toCoord = path[i];
            t = 0;
            yield return new WaitForSeconds(timePerTile);
        }
    }
    public void FixedUpdate()
    {
        if (t < 3)
        {
            t += timePerTile / 0.02f;
            if (t > 1)
            {
                t = 3;
                transform.position = Vector2.Lerp(fromCoord, toCoord, 1);
            }
            else
            {
                transform.position = Vector2.Lerp(fromCoord, toCoord, t);
            }
        }
    }


}
