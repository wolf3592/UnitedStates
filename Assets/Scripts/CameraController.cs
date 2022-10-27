using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject ground;

    public float groundMargin;
    public Camera mainCamera;
    public float ScrollSpeed;
    public float ZoomSpeed;

    public float ZoomMin;
    public float ZoomMax;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera.transform.position=new Vector3(150,0,150);
    }

    // Update is called once per frame
    void Update()
    {
        //Create a vector from (0,25,0) to MousePointerWorldPosition
        //Lerp camera position onto this line depending on zoom value (Y value)

        
        float zoomSpeedFactor=mainCamera.transform.position.y/25f*ScrollSpeed;
        //Vector3 input=new Vector3(Input.GetAxisRaw("Horizontal")*ScrollSpeed,-Input.GetAxisRaw("Mouse ScrollWheel")*ZoomSpeed,Input.GetAxisRaw("Vertical")*ScrollSpeed)*Time.deltaTime;
        Vector3 input=new Vector3(Input.GetAxisRaw("Horizontal")*zoomSpeedFactor,-Input.GetAxisRaw("Mouse ScrollWheel")*ZoomSpeed,Input.GetAxisRaw("Vertical")*zoomSpeedFactor)*Time.deltaTime;
        
        mainCamera.transform.Translate(input,Space.World);
        
        Bounds boundsGround=ground.GetComponent<MeshRenderer>().bounds;
        //print(boundsGround);
        ClampXZ(mainCamera.transform,1f,boundsGround.size.x/2.0f,boundsGround.size.z/2.0f,groundMargin);
        ClampY(mainCamera.transform,ZoomMin,ZoomMax);
    }

    void ClampXZ(Transform t,float groundUnit,float groundScaleX,float groundScaleZ,float margin)
    {
        float xMax=groundUnit*groundScaleX-margin;
        float xMin=-xMax;
        float zMax=groundUnit*groundScaleZ-margin;
        float zMin=-zMax;
        //plane is originally 1x1 - centred at origin (so 10x10 is -50 to +50)
        Vector3 newPos=new Vector3(Mathf.Clamp(t.position.x,xMin,xMax),t.position.y,Mathf.Clamp(t.position.z,zMin,zMax));
        t.position=newPos;
    }

    void ClampY(Transform t,float yMin,float yMax)
    {
        Vector3 newPos=new Vector3(t.position.x,Mathf.Clamp(t.position.y,yMin,yMax),t.position.z);
        t.position=newPos;
    }


}
