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
    ShopPosition goalShopPositions;

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
        goalShopPositions = business.visualPositions;
        realPath[realPath.Length - 1] = goalShopPositions.wanderPoints[0].position;
        Vector3 direction = Vector3.up;
        while (currentPointIndex < realPath.Length - 1)
        {
            Debug.Log(2);
            Vector3 start = realPath[currentPointIndex];
            Vector3 end = realPath[currentPointIndex + 1];

            float elapsedTime = 0;

            direction = (end - start).normalized;

            sR.flipX = (direction.x > 0 && direction.y < 0) || (direction.x < 0 && direction.y > 0)?true:false;
            anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);// up is 0, down is 1

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
        //npc at the end, (entrance)

        float _elapsed = 0;
        while (_elapsed < timePerTile)
        {
            float t = _elapsed / timePerTile;
            transform.position = Vector3.Lerp(goalShopPositions.wanderPoints[0].position, goalShopPositions.wanderPoints[1].position, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }
        //at middle of store
        anim.SetFloat("ydir", direction.y > 0 ? 0.49f : 0.51f);// up is 0, down is 1

        int timesInStore = Random.Range(0, 3);
        while (timesInStore > 0)
        {
            yield return new WaitForSeconds(Random.Range(0, 4f));
            Vector3 start = goalShopPositions.wanderPoints[1].position;
            Vector3 end = goalShopPositions.wanderPoints[1].position;
            end.x *= Random.Range(0, 2) == 0 ? 1 : -1;

            float elapsedTime = 0;

            direction = (end - start).normalized;
            sR.flipX = (direction.x > 0 && direction.y < 0) || (direction.x < 0 && direction.y > 0) ? true : false;
            anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);// up is 0, down is 1

            while (elapsedTime < timePerTile)
            {
                float t = elapsedTime / timePerTile;
                transform.position = Vector3.Lerp(start, end, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            anim.SetFloat("ydir", direction.y > 0 ? 0.49f : 0.51f);

            yield return new WaitForSeconds(Random.Range(0, 4f));
            elapsedTime = 0;
            sR.flipX = !sR.flipX;

            while (elapsedTime < timePerTile)
            {
                float t = elapsedTime / timePerTile;
                transform.position = Vector3.Lerp(end, start, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }


        }




    }

}
