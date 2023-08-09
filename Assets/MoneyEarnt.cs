using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEarnt : MonoBehaviour
{
    float travelTime = 2;
    bool traveling;
    float elapsed = 0;
    Vector3 startPosition;
    Vector3 goalPosition;
    Vector3 screenGoalPosition = new Vector3(0,0,0);
    public void StartMoving(Business b)
    {
        startPosition = b.oS.coord;
        traveling = true;
        goalPosition = Camera.main.ScreenToWorldPoint(screenGoalPosition);
    }
    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void FixedUpdate()
    {

        if (traveling)
        {
            if (elapsed > travelTime)
            {
                elapsed = travelTime;
                traveling = false;
                Debug.LogWarning("Done");
            }
            float t = elapsed / travelTime;

            transform.position = Vector3.Lerp(startPosition,goalPosition,t);

            elapsed += Time.deltaTime;
        }

    }



}
