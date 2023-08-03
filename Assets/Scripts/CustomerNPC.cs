using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    Business business;
    [SerializeField]Vector2[] isoPath;
    [SerializeField]Vector3[] realPath;
    float timePerTile = 1.3f;
    [SerializeField]SpriteRenderer sR;

    animalType aT;
    [SerializeField]Animator anim;

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


    private int currentPointIndex = 0;


    IEnumerator MoveNPC()
    {
        Debug.Log(1);
        while (currentPointIndex < realPath.Length - 1)
        {
            Debug.Log(2);
            Vector3 start = realPath[currentPointIndex];
            Vector3 end = realPath[currentPointIndex + 1];

            float elapsedTime = 0;

            Vector3 direction = (end - start).normalized;

            sR.flipX = (direction.x > 0 && direction.y < 0) || (direction.x < 0 && direction.y > 0)?true:false;
            Debug.Log(direction);
            anim.SetFloat("ydir", direction.y);

            while (elapsedTime < timePerTile)
            {
                float t = elapsedTime / timePerTile;
                transform.position = Vector3.Lerp(start, end, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Move to the next segment
            currentPointIndex++;
        }
        //npc at the end
    }

}
