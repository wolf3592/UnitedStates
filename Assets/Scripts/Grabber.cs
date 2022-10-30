using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Grabber : MonoBehaviour
{

    GameObject selectedObject;
    GameObject hoverObject;

    Color hoverOriginalColour;
    Vector3 selectedObjectOffset;

    public TextMeshProUGUI infoText;
    public Camera mainCamera;
    public Vector3 correctPosition=new Vector3(-13.62f,0,-10.75f);

    float puzzleLayer=0.02f;

    // Update is called once per frame
    void Update()    
    {
        //infoText.text="Grabber Update";
        RaycastHit hit;
        hit=CastRay();
        GameObject overObject=null;
        if (hit.collider!=null && hit.collider.CompareTag("drag")) overObject=hit.collider.gameObject;
        //print(hit.collider);
        //if (hit.collider==null) infoText.text="Grabber Hit null";

        //If not dragging
        if (selectedObject ==null)
        {
            
            //if the overobject has changed
            if (overObject!=hoverObject)
            {   
                if (hoverObject!=null)
                {
                    //not hovering over old object so reset its colour
                    hoverObject.GetComponent<MeshRenderer>().material.color=hoverOriginalColour;
                }

                if (overObject==null)
                {
                    hoverObject=null;
                    if (infoText!=null) infoText.text="";
                }
                else
                {
                    hoverObject=overObject;
                    if (infoText!=null) infoText.text=hit.collider.gameObject.name.Replace("_"," ");
                    //store colour then set to green
                    hoverOriginalColour=hoverObject.GetComponent<MeshRenderer>().material.color;
                    hoverObject.GetComponent<MeshRenderer>().material.color=Color.green;

                }                
            }

        }


        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject==null)
            {
                
                if (hit.collider!=null)
                {
                    if (!hit.collider.CompareTag("drag")) return;
                    CameraZoom.locked=true;
                    selectedObject=hit.collider.gameObject;
                    selectedObjectOffset=hit.point-hit.collider.gameObject.transform.position;
                    selectedObjectOffset=new Vector3(selectedObjectOffset.x,0,selectedObjectOffset.z);
                    Cursor.visible=true;
                    if (infoText!=null) infoText.text=selectedObject.name.Replace("_"," ");
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject!=null)
            {
                Vector3 position=new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition= Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position=new Vector3(worldPosition.x,puzzleLayer,worldPosition.z)-selectedObjectOffset;
                if (DistanceFromTarget(selectedObject.transform.position)<0.25f)
                {
                    selectedObject.transform.position=new Vector3(correctPosition.x,puzzleLayer,correctPosition.z);
                    selectedObject.tag="correct";
                    selectedObject.GetComponent<MeshRenderer>().material.color=Color.white;
                    MyGameManager.CorrectPieces--;
                    hoverObject=null;
                    
                }
                print ("Object Dropped:"+selectedObject.transform.position);
                Cursor.visible=true;
                selectedObject=null;
                CameraZoom.locked=false;
                //if (infoText!=null) infoText.text="";
            }
        }

        if (selectedObject!=null)
        {
            Vector3 position=new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition= Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position=new Vector3(worldPosition.x,puzzleLayer+0.01f,worldPosition.z)-selectedObjectOffset;
        }
        
    }

    float DistanceFromTarget(Vector3 position)
    {
        float distance=(position-correctPosition).magnitude;
        print (distance);
        return distance;
    }

    RaycastHit CastRay()
    {
        Vector3 screenMousePosFar= new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.farClipPlane
        );
        Vector3 screenMousePosNear= new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.nearClipPlane
        );

        

        Vector3 worldMousePositionFar= mainCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePositionNear= mainCamera.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        //infoText.text=worldMousePositionNear.ToString();
        Physics.Raycast(worldMousePositionNear,worldMousePositionFar-worldMousePositionNear,out hit);

        //Camera.main.transform.LookAt(worldMousePositionFar);

        return hit;
    }
}
