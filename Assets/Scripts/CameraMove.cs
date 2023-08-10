using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera cam;
    public float moveSpeed = 3f;
    public float scrollSpeed = 0.05f;
    public float scrollAmount = 0f;
    Vector3 basePosition;
    float ratio = 5 / 10.8f;
    float scale = 5f;
    Vector2 boundsMax = new Vector2(38, 38.2f);
    Vector2 boundsMin = new Vector2(-38, 0);
    // Update is called once per frame
    private void Start()
    {
        cam = Camera.main;
        scrollAmount = 0.1f;
    }



    public Vector3 mouseDown;
    public Vector3 mouseCamPos;
    public Vector3 diff;

    void Update()
    {

/*
        //Movement
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            mouseDown = Input.mousePosition;
            mouseCamPos = transform.position;
        }
        else if(Input.GetKey(KeyCode.Mouse1))
        {
            diff = mouseDown - Input.mousePosition;
            transform.position = diff - cam.ScreenToWorldPoint(mouseDown);
        }*/
        








        if (Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.W) == true) { basePosition += transform.up * moveSpeed * Time.deltaTime; }
         if (Input.GetKey(KeyCode.DownArrow) == true || Input.GetKey(KeyCode.S) == true) { basePosition -= transform.up * moveSpeed * Time.deltaTime; }


         if (Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true) { basePosition += transform.right * moveSpeed * Time.deltaTime; }
         if (Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.A) == true) { basePosition -= transform.right * moveSpeed * Time.deltaTime; }

        if (Input.mouseScrollDelta.y != 0)
        {
            scrollAmount = Mathf.Clamp(scrollAmount - Input.mouseScrollDelta.y * scrollSpeed, 0.1f, 1);
            
        }/*
        scale = Mathf.Lerp(0, boundsMax.x + 1 *ratio, scrollAmount);
        cam.orthographicSize = scale;
        *//*basePosition = new Vector3(Mathf.Clamp(basePosition.x, 0, 2*boundsMax.x * (1 - scrollAmount)), Mathf.Clamp(basePosition.y, 0, 2*boundsMax.y * (1 - scrollAmount)), -10);
        */transform.position = basePosition + Vector3.forward*-10;/*
        */transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundsMin.x, boundsMax.x), Mathf.Clamp(transform.position.y, boundsMin.y, boundsMax.y), transform.position.z);
    }
}
