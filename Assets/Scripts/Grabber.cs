using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{

    GameObject selectedObject;

    // Update is called once per frame
    void Update()    
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject==null)
            {
                RaycastHit hit=CastRay();
                if (hit.collider!=null)
                {
                    if (!hit.collider.CompareTag("drag")) return;
                    selectedObject=hit.collider.gameObject;
                    Cursor.visible=true;
                }
            }
            else
            {
                Vector3 position=new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition= Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position=new Vector3(worldPosition.x,0f,worldPosition.z);
                Cursor.visible=true;
                selectedObject=null;
            }
        }

        if (selectedObject!=null)
        {
            Vector3 position=new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition= Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position=new Vector3(worldPosition.x,.25f,worldPosition.z);
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
