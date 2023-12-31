using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class DailySpin : MonoBehaviour
{
    [SerializeField] Transform wheel;
    [SerializeField] GameObject triangleObj, buttonObj;
    [SerializeField] Wallet wallet;
    [SerializeField] Customers customerController;
    [SerializeField] Progress progress;
    public DateTime lastSpun;
    [SerializeField] spinPartical sP;
    public UnityEvent reset;

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
    Vector3 initialScale = Vector3.one * 0.9071157f;

    bool currentlyActive;
    private void FixedUpdate()
    {
        gameObject.transform.localScale = initialScale * (Camera.main.orthographicSize / 5);
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
        result = rotation + 360 * 4 + UnityEngine.Random.Range(0, 360);
        speed = 2;
        startSpeed = 4;
        maxTime = 4; 
        timeRemaining = maxTime;
        lastSpun = DateTime.Now;
        progress.lastSpin = lastSpun;
    }

    void reward()
    {
        float val = (rotation) % 360;
        Debug.Log($"{val} {(val )%360} {(val%360)/360}from rotation {rotation}");

        if (val <90)
        {
            wallet.Premium += 10;
            sP.SetColor(0);
        }
        else if( val<180)
        {
            //s
            wallet.Currency += 40;
            sP.SetColor(1);
        }
        else if (val < 270)
        {
            customerController.SaleDay();
            sP.SetColor(2);
        }
        else
        {
            // m
            wallet.Currency += 100;
            sP.SetColor(3);
        }
        //StartCoroutine(hideTimer());
    }
    private void OnEnable()
    {
        lastSpun = progress.lastSpin;
        Debug.Log($"{lastSpun.Date}- { DateTime.Now.Date}");
        if (lastSpun.Date != DateTime.Now.Date)
        {
            reset.Invoke();
            //buttonObj.SetActive(true);
        }
        else
        {
            /*reset.Invoke();
            buttonObj.SetActive(false);*/
        }
        currentStage = stage.prespin;
        triangleObj.SetActive(true);
        
    }
    public void hide()
    {
        currentlyActive = !currentlyActive;
        gameObject.SetActive(currentlyActive);
    }
    
    IEnumerator hideTimer()
    {
        yield return new WaitForSeconds(3);
        hide();
    }
}
