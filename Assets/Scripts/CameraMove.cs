using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField] float minPinchSpeed = 5.0F;
    [SerializeField] float speed = 4;
    [SerializeField] float varianceInDistances = 5.0F;


    [SerializeField] Vector2 touchStart;
    private Camera cam;


    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 direction = touchStart - touch.position;
                cam.transform.Translate(direction * cam.orthographicSize / Screen.height * 2);
                touchStart = touch.position;
            }
        }
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 curDist = touch0.position - touch1.position; //current distance between finger touches
            Vector2 prevDist = ((touch0.position - touch0.deltaPosition) - (touch1.position - touch1.deltaPosition)); //difference in previous locations using delta positions
            float touchDelta = curDist.magnitude - prevDist.magnitude;
            float speedTouch0 = touch0.deltaPosition.magnitude / touch0.deltaTime;
            float speedTouch1 = touch1.deltaPosition.magnitude / touch1.deltaTime;


            Vector2 touch0StartPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1StartPos = touch1.position - touch1.deltaPosition;
            Vector2 midpointStart = (touch0StartPos + touch1StartPos) / 2f;
            Vector2 midpointNow = (touch0.position + touch1.position) / 2f;

            Vector3 direction = midpointStart - midpointNow;
            cam.transform.Translate(direction * cam.orthographicSize / Screen.height * 2);


            if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {

                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + (cam.orthographicSize / 50 * speed), zoomBounds.x, zoomBounds.y);
            }

            if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {

                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (cam.orthographicSize/50 * speed), zoomBounds.x, zoomBounds.y);
            }

        }
    }
    Vector2 zoomBounds = new Vector2(0.1f,13f);


}

