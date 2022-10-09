using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateHandler : MonoBehaviour
{
  public float jumbleDistance=30f;
  public Vector3 offsetState;
    void Start()
    {
        //List all of the childrem
        //set all states to random colour
        Random.InitState(123); //same seed each time
        MeshRenderer[] mats=GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mats)
        {
            m.material.color=new Color(Random.Range(0.0f,0.7f),0,Random.Range(0.3f,1f),1);
            //m.material.color=new Color(0,0,Random.Range(0.3f,1f),1);
            
        }
    }

    public void StartGame()
    {
        print ("Start Game pressed");
        RandomizePieces();
    }

    void RandomizePieces()
    {
        MeshRenderer[] meshs=GetComponentsInChildren<MeshRenderer>();
        float angle=0;
        
        foreach (MeshRenderer m in meshs)
        {
            //move each piece outwards jumbleDistance 
            Vector3 direction=new Vector3(Mathf.Cos(Mathf.Deg2Rad*angle),0,Mathf.Sin(Mathf.Deg2Rad*angle));
            Vector3 meshCenter= m.bounds.center;
            m.transform.position=direction*(jumbleDistance/2f)-meshCenter+offsetState;
            angle+=360/(meshs.Length+0.5f);
        }
    }

}
