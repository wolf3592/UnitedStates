using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StateHandler : MonoBehaviour
{
  public float jumbleGridSize=3.5f;

  public int jumbleNumtoMove=10;
  public Vector3 offsetState;
  
  Vector3 startPosition=new Vector3 (-25,0,18);
  public Vector3 currentPosition;




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

    public void Update()
    {

        //update the timer if we've started
    }


    void RandomizePieces()
    {
        // MeshRenderer[] meshs=GetComponentsInChildren<MeshRenderer>();
        // float angle=jumbleStartAngle;
        // int counter=0;
        // foreach (MeshRenderer m in meshs)
        // {
        
        
        //     //move each piece outwards jumbleDistance 
        //     Vector3 direction=new Vector3(Mathf.Cos(Mathf.Deg2Rad*angle)*(jumbleStartDistance),0,Mathf.Sin(Mathf.Deg2Rad*angle)*(jumbleStartDistance));
        //     Vector3 meshCenter= m.bounds.center;
        //     m.transform.position=direction-meshCenter+offsetState;
        //     angle+=  jumbleDeltaAngle;//360/(meshs.Length+0.5f);

        //     counter++;
        //     //if (angle>jumbleEndAngle) break;
        //     if (counter>jumbleNumtoMove) break;
        // }
    }

    public int RandomizePieces2()
    {
        MeshRenderer[] meshs=GetComponentsInChildren<MeshRenderer>();
        currentPosition=startPosition;

        List<MeshRenderer> order=new List<MeshRenderer>(meshs);
        int countPieces;


        print (order.Count);
        countPieces=order.Count;
        //CorrectPieces=countPieces;
        Random.InitState((int)Time.time);
        while (order.Count>0)
        {
            int index=Random.Range(0,order.Count-1);
            MeshRenderer meshToMove=order[index];
            Vector3 meshCenter= meshToMove.bounds.center;
            meshToMove.transform.position=GetNextPlace()-meshCenter+offsetState;
            order.RemoveAt(index);
        }

        return countPieces;
    }

    Vector3 GetNextPlace()
    {
        Vector3 newPosition=currentPosition;
        currentPosition=currentPosition+new Vector3(jumbleGridSize,0,0);
        if (currentPosition.x>25)
            currentPosition=new Vector3(startPosition.x,0,currentPosition.z-jumbleGridSize);

        return newPosition;
    }

}
