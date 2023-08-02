using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    Business business;
    [SerializeField]Vector2[] isoPath;
    [SerializeField]Vector3[] realPath;
    float timePerTile = 0.8f;
    float t = 0;
    Vector2 fromCoord, toCoord;
    [SerializeField]SpriteRenderer sR;

    animalType aT;


    public void init(Business goal)
    {
        business = goal;
        isoPath = business.oS.pathFromEntrance.ToArray();
        realPath = isoPath.isoCoordToWorldPosition();
        aT = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[Random.Range(0, System.Enum.GetValues(typeof(species)).Length)];
        //StartCoroutine(walkPath());
        sR.sprite = aT.walkCycleBack[0];
        StartCoroutine(MoveNPC());
    }

    IEnumerator walkPath()
    {
        Vector2 enterPosition = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.down;
        fromCoord = enterPosition;
        toCoord = Vector2.zero;
        t = 0;
        sR.enabled = true;
        for(int i = 0;i< realPath.Length;i++)
        {
            transform.position = toCoord;
            fromCoord = realPath[i];
            toCoord = realPath[i];
            t = 0;
            yield return new WaitForSeconds(timePerTile);
        }
    }

    public float walkSpeed = 0.8f; // Set the desired speed of the NPC in units per second

    private int currentPointIndex = 0;


    IEnumerator MoveNPC()
    {
        Debug.Log(1);
        while (currentPointIndex < realPath.Length - 1)
        {
            Debug.Log(2);
            // Calculate the current segment's duration based on the distance and desired speed
            Vector3 start = realPath[currentPointIndex];
            Vector3 end = realPath[currentPointIndex + 1];
            float segmentDistance = Vector3.Distance(start, end);
            float segmentDuration = segmentDistance / walkSpeed;

            float elapsedTime = 0;

            while (elapsedTime < segmentDuration)
            {
                float t = elapsedTime / segmentDuration;
                transform.position = Vector3.Lerp(start, end, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Move to the next segment
            currentPointIndex++;
        }

        // If the NPC reached the last point, you can reset or stop the movement here.
        // For example, you could loop the movement or disable the script to stop the NPC.
        // This depends on your specific use case.
    }




    /*public void FixedUpdate()
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
    }*/


}
