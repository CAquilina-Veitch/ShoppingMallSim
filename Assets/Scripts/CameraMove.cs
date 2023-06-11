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
    Vector2 boundsMax = new Vector2(2,0);
    // Update is called once per frame
    private void Start()
    {
        cam = Camera.main;
        scrollAmount = 0.1f;
    }
    void Update()
    {
         if (Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.W) == true) { basePosition += Vector3.up * moveSpeed * Time.deltaTime; }
         if (Input.GetKey(KeyCode.DownArrow) == true || Input.GetKey(KeyCode.S) == true) { basePosition -= Vector3.up * moveSpeed * Time.deltaTime; }


         if (Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true) { basePosition += Vector3.right * moveSpeed * Time.deltaTime; }
         if (Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.A) == true) { basePosition -= Vector3.right * moveSpeed * Time.deltaTime; }

        if (Input.mouseScrollDelta.y != 0)
        {
            scrollAmount = Mathf.Clamp(scrollAmount - Input.mouseScrollDelta.y * scrollSpeed, 0.1f, 1);
            
        }
        scale = Mathf.Lerp(0, boundsMax.x + 1 *ratio, scrollAmount);
        cam.orthographicSize = scale;
        basePosition = new Vector3(Mathf.Clamp(basePosition.x, 0, 2*boundsMax.x * (1 - scrollAmount)), Mathf.Clamp(basePosition.y, 0, 2*boundsMax.y * (1 - scrollAmount)), -10);
        transform.position = basePosition;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, boundsMax.x*2), Mathf.Clamp(transform.position.y, 0, boundsMax.y), transform.position.z);
    }
}
