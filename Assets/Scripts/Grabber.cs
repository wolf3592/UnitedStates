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

    // Update is called once per frame
    void Update()    
    {
        RaycastHit hit;
        hit=CastRay();
        GameObject overObject=null;
        if (hit.collider!=null && hit.collider.CompareTag("drag")) overObject=hit.collider.gameObject;

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
                selectedObject.transform.position=new Vector3(worldPosition.x,0,worldPosition.z)-selectedObjectOffset;
                print ("Object Dropped:"+selectedObject.transform.position.y);
                Cursor.visible=true;
                selectedObject=null;
                //if (infoText!=null) infoText.text="";
            }
        }

        if (selectedObject!=null)
        {
            Vector3 position=new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition= Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position=new Vector3(worldPosition.x,0+.25f,worldPosition.z)-selectedObjectOffset;
        }
        
    }

    RaycastHit CastRay()
    {
        Vector3 screenMousePosFar= new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );
        Vector3 screenMousePosNear= new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );

        Vector3 worldMousePositionFar= Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePositionNear= Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePositionNear,worldMousePositionFar-worldMousePositionNear,out hit);
        return hit;
    }
}
