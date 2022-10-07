using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateHandler : MonoBehaviour
{
  // Start is called before the first frame update
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


}
