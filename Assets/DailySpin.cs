using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailySpin : MonoBehaviour
{
    [SerializeField] Transform wheel;
    [SerializeField] Wallet wallet;
    [SerializeField] Customers customerController;
    enum stage
    {
        prespin, spinning,finished
    };
    [SerializeField] stage currentStage;
    [SerializeField] float rotation;
    [SerializeField] float speed = 5;
    float startSpeed;
    float result;

    [SerializeField] float timeRemaining = 0;
    float maxTime;
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
                Debug.Log(t);
                rotation = Mathf.Lerp(wheel.rotation.z, result, t);
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
        float val = (wheel.rotation.z + 24.35f) % 360;
        int prizeNumber = (int)val % 90;
        Debug.Log(prizeNumber);
        switch (prizeNumber)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                Debug.LogError(prizeNumber);
                break;
        }
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
}
