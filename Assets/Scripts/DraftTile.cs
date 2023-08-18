using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DraftTile : MonoBehaviour
{
    SpriteRenderer sR;
    float flashSpeed = 5;
    public Vector2 coord;
    Color currentColour;
    float timeP;
    private void OnEnable()
    {
        sR = GetComponent<SpriteRenderer>();
        sR.enabled = false;
        currentColour = Color.white;
        sR.color = Color.white;
        coord = Vector2.positiveInfinity;
    }
    private void FixedUpdate()
    {
        if (sR.enabled)
        {
            float a = (Mathf.Sin(timeP * flashSpeed) + 1) / 2;
            timeP += Time.deltaTime;
            currentColour = Color.Lerp(currentColour, Color.white,0.1f);
            currentColour.a = a;
            sR.color = currentColour;
        }
        
    }

    public void moveDraft(Vector2 coordinate)
    {
        transform.position = coordinate.IsoCoordToWorldPosition() + Vector3.down * 0.5f;
        coord = coordinate;
        draftVisibility(true);

    }
    public void draftVisibility(bool to)
    {
        sR.color = Color.white;
        sR.enabled = to;
        coord = to ? coord : Vector2.positiveInfinity;
    }
    public void flashRed()
    {
        currentColour = Color.red;
        timeP = 0;
    }


}
