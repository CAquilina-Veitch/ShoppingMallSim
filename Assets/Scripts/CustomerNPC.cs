using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    Business business;
    [SerializeField]Vector2[] isoPath;
    [SerializeField]Vector3[] realPath;
    float timePerTile = 2.5f;
    [SerializeField]SpriteRenderer sR;

    [SerializeField] animalType aT;
    [SerializeField]Animator anim;
    ShopPosition goalShopPositions;
    public Customers cM;


    public void init(Business goal)
    {
        business = goal;
        isoPath = business.oS.pathFromEntrance.ToArray();
        realPath = isoPath.IsoCoordToWorldPosition();
        aT = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<Animal>().animalTypes[Random.Range(0, System.Enum.GetValues(typeof(species)).Length)];
        //StartCoroutine(walkPath());
        sR.sprite = aT.walkCycleBack[0];
        anim.SetInteger("Species", (int)aT.specie);
        StartCoroutine(MoveNPC());
    }


    private int currentPointIndex = 0;


    IEnumerator MoveNPC()
    {        sR.sortingOrder = 0;
        ////////////////////////////////////////////////customer is spawned in now moves toward
        float _eT = 0;
        int m = Random.Range(0, 2) == 0 ? -1 : 1;
        anim.SetFloat("ydir", 0);
        sR.flipX = m > 0;
        while (_eT < timePerTile)
        {
            float t = _eT / timePerTile;
            transform.position = Vector3.Lerp(new Vector3(m*0.8f,-0.4f), Vector3.zero, t);

            _eT += Time.deltaTime;
            yield return null;
        }

        goalShopPositions = business.visualPositions;
        realPath[realPath.Length - 1] = goalShopPositions.wanderPoints[0].position;
        Vector3 direction = Vector3.up;
        while (currentPointIndex < realPath.Length - 1)
        {
            Debug.Log(2);
            Vector3 start = realPath[currentPointIndex];
            start.z = -0.2f;
            Vector3 end = realPath[currentPointIndex + 1];
            end.z = -0.2f;

            float elapsedTime = 0;

            direction = (end - start).normalized;

            sR.flipX = direction.x < 0;
            anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);// up is 0, down is 1
            bool newOrder = false;

            while (elapsedTime < timePerTile)
            {
                sR.sortingOrder = transform.position.EquivilantSortingLayer();
                float t = elapsedTime / timePerTile;
                if (!newOrder && t >= 0.5f)
                {
                    newOrder = true;

                    //sR.sortingOrder = -2*(int)transform.position.y;
                    Vector3 temp = GlobalFunctions.WorldToIsoCoord(end);
                    sR.sortingOrder = -(int)(temp.x + temp.y);
                    Debug.LogWarning("THIS HAPPENED TO SWTCH TO " + sR.sortingOrder);
                }
                else
                {
                    //Debug.LogWarning($"{newOrder} , {t}");
                }
                transform.position = Vector3.Lerp(start, end, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Move to the next segment
            currentPointIndex++;
        }
        //npc at the end, (entrance)
        direction = (goalShopPositions.wanderPoints[1].position - goalShopPositions.wanderPoints[0].position).normalized;
        sR.flipX = direction.x < 0;
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
        sR.sortingOrder = transform.position.EquivilantSortingLayer();
        while (timesInStore > 0)
        {
            Debug.Log(timesInStore + "times in store");
            yield return new WaitForSeconds(Random.Range(0, 4f));
            Vector3 start = goalShopPositions.wanderPoints[1].position;
            Vector3 end = goalShopPositions.wanderPoints[2].position;
            int r = Random.Range(0, 2);
            end.x += (Mathf.Abs(goalShopPositions.wanderPoints[2].localPosition.x) * (r == 0 ? 0 : 2));

            float elapsedTime = 0;

            direction = (end - start).normalized;
            sR.flipX = direction.x < 0;
            anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);// up is 0, down is 1

            while (elapsedTime < timePerTile)
            {
                float t = elapsedTime / timePerTile;
                transform.position = Vector3.Lerp(start, end, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            sR.flipX = !sR.flipX;
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
        Vector3 _midpoint = goalShopPositions.wanderPoints[1].position;
        Vector3 _midcheckout = goalShopPositions.wanderPoints[3].position;
        Vector3 _atcheckout = goalShopPositions.wanderPoints[4].position;

        direction = (_midpoint - _midcheckout).normalized;
        sR.flipX = direction.x > 0;//jc
        anim.SetFloat("ydir", direction.y > 0 ? 1 : 0);// up is 0, down is 1



        _elapsed = 0;
        while (_elapsed < (timePerTile / 2))
        {
            float t = _elapsed / (timePerTile / 2);
            transform.position = Vector3.Lerp(_midpoint, _midcheckout, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }
        sR.flipX = !sR.flipX;
        _elapsed = 0;
        while (_elapsed < (timePerTile / 2))
        {
            float t = _elapsed / (timePerTile/2);
            transform.position = Vector3.Lerp(_midcheckout, _atcheckout, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }

        anim.SetFloat("ydir", direction.y > 0 ? 0.51f : 0.49f);
        sR.flipX = !sR.flipX;
        yield return new WaitForSeconds(5 - business.activeWorkers.Count);
        cM.makeSale(business);
        anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);

        _elapsed = 0;
        while (_elapsed < timePerTile / 2)
        {
            float t = _elapsed / (timePerTile / 2);
            transform.position = Vector3.Lerp(_atcheckout, _midcheckout, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }
        sR.flipX = !sR.flipX;
        _elapsed = 0;
        while (_elapsed < timePerTile / 2)
        {
            float t = _elapsed / (timePerTile / 2);
            transform.position = Vector3.Lerp(_midcheckout, _midpoint, t);

            _elapsed += Time.deltaTime;
            yield return null;
        }

        currentPointIndex = 0;
        realPath = GlobalFunctions.ReverseArray(realPath);
        _elapsed = 0;


        sR.flipX = !((realPath[0] - _midpoint).x > 0);
        anim.SetFloat("ydir", 0);// up is 0, down is 1


        while (_elapsed < timePerTile / 2)
        {
            float t = _elapsed / (timePerTile / 2);
            transform.position = Vector3.Lerp(_midpoint, realPath[0], t);

            _elapsed += Time.deltaTime;
            yield return null;
        }


        while (currentPointIndex < realPath.Length - 1)
        {
            Vector3 start = realPath[currentPointIndex];
            start.z = -0.2f;
            Vector3 end = realPath[currentPointIndex + 1];
            end.z = -0.2f;
            float elapsedTime = 0;

            direction = (end - start).normalized;

            sR.flipX = direction.x < 0;
            anim.SetFloat("ydir", direction.y > 0 ? 0 : 1);// up is 0, down is 1

            while (elapsedTime < timePerTile)
            {
                sR.sortingOrder = transform.position.EquivilantSortingLayer();
                float t = elapsedTime / timePerTile;
                transform.position = Vector3.Lerp(start, end, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Move to the next segment
            currentPointIndex++;
        }

        _eT = 0;
        anim.SetFloat("ydir", 1);
        sR.flipX = m < 0;
        while (_eT < timePerTile)
        {
            float t = _eT / timePerTile;
            transform.position = Vector3.Lerp(new Vector3(0,0,-0.25f),new Vector3(m * 0.8f, -0.4f,-0.25f), t);

            _eT += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);


    }

}
