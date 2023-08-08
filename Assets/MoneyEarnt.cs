using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEarnt : MonoBehaviour
{
    float travelTime = 2;

    bool traveling;
    float i = 0;
    Vector3 startPosition;
    Vector3 goalPosition;
    Vector3 screenGoalPosition = new Vector3(0,0,0);
    public void StartMoving(Business b)
    {
        startPosition = b.oS.coord;
        traveling = true;
        goalPosition = Camera.main.ScreenToWorldPoint(screenGoalPosition);
    }


    private void FixedUpdate()
    {
        
    }



}
