using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    Business business;
    [SerializeField]Vector2[] isoPath;
    [SerializeField]Vector3[] realPath;
    float timePerTile = 2.5f;
    [SerializeField]SpriteRenderer sR;

    animalType aT;
    [SerializeField]Animator anim;
    ShopPosition goalShopPositions;
    public Customers cM;
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
        direction = (goalShopPositions.wanderPoints[1].position - goalShopPositions.wanderPoints[0].position).normalized;
        sR.flipX = (direction.x > 0 && direction.y < 0) || (direction.x < 0 && direction.y > 0) ? true : false;
        anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);// up is 0, down is 1
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
        sR.sortingOrder = 5;
        while (timesInStore > 0)
        {
            Debug.Log(timesInStore + "times in store");
            yield return new WaitForSeconds(Random.Range(0, 4f));
            Vector3 start = goalShopPositions.wanderPoints[1].position;
            Vector3 end = goalShopPositions.wanderPoints[2].position;
            int r = Random.Range(0, 2);
            end.x = transform.position.x + (goalShopPositions.wanderPoints[2].localPosition.x * r == 0 ? 1 : -1);

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
            anim.SetFloat("ydir", direction.y > 0 ? 1 : 0);// up is 0, down is 1
            while (elapsedTime < timePerTile)
            {
                float t = elapsedTime / timePerTile;
                transform.position = Vector3.Lerp(end, start, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            anim.SetFloat("ydir", direction.y > 0 ? 0.51f : 0.49f);
            timesInStore--;

        }


        sR.sortingOrder = 3;


        Vector3 _midpoint = goalShopPositions.wanderPoints[1].position;
        Vector3 _midcheckout = goalShopPositions.wanderPoints[3].position;
        Vector3 _atcheckout = goalShopPositions.wanderPoints[4].position;

        direction = (_midpoint - _midcheckout).normalized;
        sR.flipX = (direction.x > 0 && direction.y < 0) || (direction.x < 0 && direction.y > 0) ? true : false;
        anim.SetFloat("ydir", direction.y > 0 ? 1 : 0);// up is 0, down is 1



        _elapsed = 0;
        while (_elapsed < timePerTile/2)
        {
            float t = _elapsed / timePerTile;
            transform.position = Vector3.Lerp(_midpoint, _midcheckout, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }
        sR.flipX = !sR.flipX;
        _elapsed = 0;
        while (_elapsed < timePerTile / 2)
        {
            float t = _elapsed / timePerTile;
            transform.position = Vector3.Lerp(_midcheckout, _atcheckout, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }

        anim.SetFloat("ydir", direction.y > 0 ? 0.51f : 0.49f);
        yield return new WaitForSeconds(5 - business.activeWorkers.Count);
        cM.makeSale(business);

        anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);

        _elapsed = 0;
        while (_elapsed < timePerTile / 2)
        {
            float t = _elapsed / timePerTile;
            transform.position = Vector3.Lerp(_atcheckout, _midcheckout, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }
        sR.flipX = !sR.flipX;
        _elapsed = 0;
        while (_elapsed < timePerTile / 2)
        {
            float t = _elapsed / timePerTile;
            transform.position = Vector3.Lerp(_midcheckout, _midpoint, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }

        currentPointIndex = 0;
        realPath = GlobalFunctions.reverseArray(realPath);
        while (currentPointIndex < realPath.Length - 1)
        {
            Debug.Log(realPath[currentPointIndex]);
            Vector3 start = realPath[currentPointIndex];
            Vector3 end = realPath[currentPointIndex + 1];

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

            // Move to the next segment
            currentPointIndex++;
        }
        Debug.LogWarning("finished path length " +realPath.Length);



    }

}
