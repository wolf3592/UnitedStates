using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct StateMovement
{
    public string Name;
    public float position;
    public float direction;
}

public class StateHandler : MonoBehaviour
{
    Transform[] t;
    StateMovement[] positions;
    public float MaxHeight=10;
    public float speed=0.001f;

    public GameObject objectToPlace;
    public Camera gameCamera;

    public Text infoText;
    // Start is called before the first frame update
    void Start()
    {
        //List all of the childrem
        t= this.gameObject.GetComponentsInChildren<Transform>();
        print(t.Length);
        positions=new StateMovement[t.Length];
        for (int i = 1; i < t.Length; i++)
        {
            positions[i]=new StateMovement();
            positions[i].direction=0;
            positions[i].position=0;
            positions[i].Name=t[i].name;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < t.Length; i++)
        {
            if (positions[i].direction!=0)    
            {
                float p=positions[i].position;

                p+=speed*positions[i].direction;
                if (p>=1) positions[i].direction=-positions[i].direction;
                if (p<0) positions[i].direction=0;
                p=Mathf.Clamp(p,0,MaxHeight);
                positions[i].position=p;
                Vector3 lp=t[i].transform.position;

                t[i].transform.position=new Vector3( lp.x,Mathf.Lerp(0,MaxHeight,p), lp.z);
            }
        }

        int s=Random.Range(1,49);
        if (positions[s].direction==0) 
        {
            //positions[s].direction=1;
        }

        Ray ray=gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray,out hitInfo,100f))
        {

            infoText.text=(hitInfo.collider.gameObject.name).Replace("_"," ");
            
            objectToPlace.SetActive(true);
            objectToPlace.transform.position=hitInfo.point;

            //objectToPlace.transform.rotation=Quaternion.FromToRotation(Vector3.up,hitInfo.normal);
        }
        else
        {
            objectToPlace.SetActive(false);
            infoText.text="";
        }

    }
}
