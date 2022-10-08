using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public static bool locked=false;
    public float currentZoom; //0 to 1
    public float zoomSpeed=0.01f;
    public Vector2 zoomMinMax;
    public float zoomPercentInverse=30;

    public int dampingFrames=5;

    int dampFrames;

    public float farClipPlane=10;
    //public Vector2 zoomRange;
    //public Camera cameraObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (locked) return;
        float zoomMovement=Input.GetAxisRaw("Mouse ScrollWheel")*zoomSpeed*Time.deltaTime;
        
        //if (zoomMovement!=0)
        {
        currentZoom+=zoomMovement;
        currentZoom=Mathf.Clamp(currentZoom,zoomMinMax.x,zoomMinMax.y);
        CameraZoomCalculation();
        }
        
    }

    void CameraZoomCalculation()
    {
        //take camera origin;
        Vector3 originCamera=new Vector3(0,25,0);
        
        Vector3 screenMousePosFar= new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            farClipPlane
        );

        Vector3 worldMousePositionFar= Camera.main.ScreenToWorldPoint(screenMousePosFar)*(1-currentZoom);
        //Vector3 worldMousePositionNear= Camera.main.ScreenToWorldPoint(originCamera);

        Vector3 newPosition=((worldMousePositionFar-originCamera)*currentZoom)+originCamera;
        //print (Camera.main.farClipPlane);
        Camera.main.transform.position=newPosition;
        
    }

}
