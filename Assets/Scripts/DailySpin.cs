using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailySpin : MonoBehaviour
{
    [SerializeField] Transform wheel;
    [SerializeField] GameObject triangleObj, buttonObj;
    [SerializeField] Wallet wallet;
    [SerializeField] Customers customerController;
    enum stage
    {
        prespin, spinning,finished
    };
    stage currentStage;
    float rotation;
    float speed = 5;
    float startSpeed;
    float result;

    float timeRemaining = 0;
    float maxTime;

    bool currentlyActive;
    private void FixedUpdate()
    {
        if (currentStage == stage.prespin)
        {
            rotation += Time.deltaTime*speed;
            wheel.rotation = Quaternion.Euler(0, 0, -rotation);
        }
        else if (currentStage == stage.spinning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime*speed;
                timeRemaining = timeRemaining < 0 ? 0 : timeRemaining;
                float t = timeRemaining / maxTime;
                rotation = Mathf.Lerp(result,wheel.rotation.z * Mathf.Rad2Deg, t);
                wheel.rotation = Quaternion.Euler(0, 0, -rotation);

                speed = Mathf.Lerp(0.1f,startSpeed,t);
            }
            else
            {
                currentStage = stage.finished;
                reward();
            }

        }
        else
        {

        }

        //print(rotation);
    }

    public void StartSpin()
    {
        currentStage = stage.spinning;
        result = rotation + 360 * 4 + Random.Range(0, 360);
        speed = 2;
        startSpeed = 4;
        maxTime = 4; 
        timeRemaining = maxTime;
    }

    void reward()
    {
        float val = (rotation) % 360;//wheel.rotation.z*Mathf.Rad2Deg + 24.35f) % 360;
        Debug.Log($"{val} {(val )%360} {(val%360)/360}from rotation {rotation}");

        if (val <90)
        {
            wallet.Premium += 10;
        }
        else if( val<180)
        {
            wallet.Currency += 100;
        }
        else if (val < 270)
        {
            customerController.SaleDay();
        }
        else
        {
            wallet.Currency += 40;
        }
    }
    private void OnEnable()
    {
        currentStage = stage.prespin;
        triangleObj.SetActive(true);
        buttonObj.SetActive(true);
    }
    public void hide()
    {
        currentlyActive = !currentlyActive;
        gameObject.SetActive(currentlyActive);
    }
}
