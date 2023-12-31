using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    Vector2 zoomBounds = new Vector2(0.1f, 13f);
    Vector2 boundsMax = new Vector2(38, 38);
    Vector2 boundsMin = new Vector2(-38, 0);
    [SerializeField] float minPinchSpeed = 5.0F;
    [SerializeField] float speed = 4;
    [SerializeField] float varianceInDistances = 5.0F;
    int fingers;

    [SerializeField] Vector2 touchStart;
    private Camera cam;

    bool fingerDownOnCanvas;


    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 0 )
        {
            fingers = 0;
            if (!Input.GetMouseButton(0))
            {

                fingerDownOnCanvas = false;
            }
        }
        else
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    fingerDownOnCanvas = true;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("mouse down");
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("finger down");
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                fingerDownOnCanvas = true;
            }
        }
        if (Input.touchCount == 1)
        {

            Touch touch = Input.GetTouch(0);
            if (fingers == 2)
            {
                touchStart = touch.position;
                fingers = 1;
            }
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchStart = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 direction = touchStart - touch.position;
                    Vector3 newPosition = cam.transform.position + direction * cam.orthographicSize / Screen.height * 2;

                    Vector3 midPoint = new Vector3(0, 19);
                    Vector2 distanceCenter = (transform.position - midPoint);
                    //Debug.LogWarning(distanceCenter);
                    float multiplier = -Mathf.Abs(distanceCenter.y) + (boundsMax.x * 0.5f);

                    newPosition.x = Mathf.Clamp(newPosition.x, -multiplier * 2, multiplier * 2);
                    newPosition.y = Mathf.Clamp(newPosition.y, boundsMin.y, boundsMax.y);

                    cam.transform.position = newPosition;

                    touchStart = touch.position;
                }
            }
            
        }
        else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            fingers = 2;
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 curDist = touch0.position - touch1.position; //current distance between finger touches
            Vector2 prevDist = ((touch0.position - touch0.deltaPosition) - (touch1.position - touch1.deltaPosition)); //difference in previous locations using delta positions
            float touchDelta = curDist.magnitude - prevDist.magnitude;
            float speedTouch0 = touch0.deltaPosition.magnitude / touch0.deltaTime;
            float speedTouch1 = touch1.deltaPosition.magnitude / touch1.deltaTime;
            
            if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {

                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + (cam.orthographicSize / 50 * speed), zoomBounds.x, zoomBounds.y);
            }

            if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {

                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (cam.orthographicSize/50 * speed), zoomBounds.x, zoomBounds.y);
            }

        }
        else if (Input.GetMouseButton(0))
        {
            //Debug.Log("mouse held");
            if (!EventSystem.current.IsPointerOverGameObject()&&fingerDownOnCanvas)
            {
                //Debug.Log("drag");
                Vector3 direction = (Vector3)touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.Translate(direction);
            }
            else
            {
                //Debug.Log($"{EventSystem.current.IsPointerOverGameObject()} over? ---- {fingerDownOnCanvas} finger");
                fingerDownOnCanvas = false;
            }
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, (19 - Mathf.Abs(Mathf.Clamp(transform.position.y, 0, 38) - 19)) * -2, (19 - Mathf.Abs(Mathf.Clamp(transform.position.y, 0, 38) - 19)) * 2), Mathf.Clamp(transform.position.y, 0, 38), -10);
    }

    public void CenterCameraOnPosition(Vector3 target)
    {
        StartCoroutine(MoveCameraToTarget(target));
    }
    float targetZoomAmount = 5;
    float timeToMove;
    IEnumerator MoveCameraToTarget(Vector3 target)
    {
        target.z = -10;
        float elapsedTime = 0;
        float lerpDuration = 0.5f; // Total time for the lerping process
        float initialZoom = cam.orthographicSize;
        Vector3 initialPos = cam.transform.position;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;

            // Apply a sine wave function to 't' to create the easing effect
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            cam.transform.position = Vector3.Lerp(initialPos, target, t);
            cam.orthographicSize = Mathf.Lerp(initialZoom, targetZoomAmount, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = target;
        cam.orthographicSize = targetZoomAmount;
    }
    



}

