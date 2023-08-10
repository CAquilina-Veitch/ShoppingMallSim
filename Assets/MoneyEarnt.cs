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
    Vector3 screenGoalPosition = new Vector3(340, 25, 0);

    [SerializeField] GameObject doneEffectPrefab;

    public void StartMoving(Business b)
    {
        startPosition = b.oS.coord;
        traveling = true;
        goalPosition = Camera.main.ScreenToWorldPoint(screenGoalPosition);
    }
    private void OnEnable()
    {
        screenGoalPosition = new Vector3(Screen.width * 0.45f, Screen.height * 0.06f);
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
                StartCoroutine(done());
            }
            float t = elapsed / travelTime;

            transform.position = Vector3.Lerp(startPosition,goalPosition,t);

            elapsed += Time.deltaTime;
        }

    }
    IEnumerator done()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject temp = Instantiate(doneEffectPrefab, transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(2);
        Destroy(temp);
        Destroy(gameObject);
    }



}
