//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    List<string> puzzles=new List<string>() {"","Africa","Asia","Australia","Canada","Caribbean","Central America","Europe","Middle East","Oceania","South America","United_States"};
    public static int CorrectPieces=0;
    GameObject africaParent;
    GameObject africaHole;
    GameObject usaParent;
    GameObject usaHole;
    GameObject ukParent;
    GameObject ukHole;

    Camera mainCamera;

    Button startButton;
    Button resetButton;
    Button usaButton;
    Button africaButton;
    Button ukButton;

    TextMeshProUGUI textTimer;
    TextMeshProUGUI textCorrect;
    TMPro.TMP_Dropdown puzzleDropdown;

    public Material worldMaterial;

    float seconds;

    bool GameStarted=false;

    GameObject goCurrentPuzzle;
    int currentPuzzle=-1;

    private void EnablePuzzle()
    {
        // africaHole.SetActive(goCurrentPuzzle==africaParent);
        // africaParent.SetActive(goCurrentPuzzle==africaParent);
        // usaHole.SetActive(goCurrentPuzzle==usaParent);
        // usaParent.SetActive(goCurrentPuzzle==usaParent);
        // ukHole.SetActive(goCurrentPuzzle==ukParent);
        // ukParent.SetActive(goCurrentPuzzle==ukParent);
        //-8.4, 18.58
    }

    public void AfricaOnClick()
    {
        goCurrentPuzzle=africaParent;
        EnablePuzzle();
        SetPuzzleButtonState();
    }
    public void UsaOnClick()
    {
        goCurrentPuzzle=usaParent;
        EnablePuzzle();
        SetPuzzleButtonState();
    }
    public void UKOnClick()
    {
        goCurrentPuzzle=ukParent;
        EnablePuzzle();
        SetPuzzleButtonState();
    }

    public void TestOnClick()
    {
        //get United_States
        FocusCamera(GetChildBounds(GameObject.Find("United_States")));

    
        //get extent
        //set camera above the centre of the mesh
        //zoom so that the mesh is centre bottom
    }

    public void Test2OnClick()
    {
        FocusCamera(GetChildBounds(GameObject.Find("Middle_East")));
    }

    public void OnValueChanged(int target)
    {
        string selection=puzzles[target].Replace(" ","_");
        GameObject go = GameObject.Find(selection);
        if (go!=null) 
            {
            FocusCamera(GetChildBounds(go));
            if (goCurrentPuzzle!=null) DeselectGO(goCurrentPuzzle);
            HighlightGO(go);
            goCurrentPuzzle=go;
            }
        else
            print ("Could not find: "+selection);
    }

  private void DeselectGO(GameObject go)
  {

        MeshRenderer[] mats=go.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mats)
        {
            m.material=worldMaterial;
            //m.material.color=new Color(0,0,Random.Range(0.3f,1f),1);
            
        }
  }

  private void HighlightGO(GameObject go)
  {
        MeshRenderer[] mats=go.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mats)
        {
            m.material.color=new Color(Random.Range(0.0f,0.7f),0,Random.Range(0.3f,1f),1);
            //m.material.color=new Color(0,0,Random.Range(0.3f,1f),1);
            
        }
  }

  private Bounds GetChildBounds(GameObject go)
    {
        MeshRenderer[] mats=go.GetComponentsInChildren<MeshRenderer>();
        MeshRenderer renderer=mats[0];
        Bounds b = renderer.bounds;
        foreach (MeshRenderer m in mats)
        {
            b.Encapsulate(m.bounds);            
        }
        return b;
    }

    private void FocusCamera(Bounds b)
    {
        GameObject mainCamera=GameObject.Find("Main Camera");
        Camera mCamera=mainCamera.GetComponent<Camera>();
        
         var distance = Mathf.Max(b.size.x,b.size.z) * 0.5f / Mathf.Tan(mCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        mainCamera.transform.position=new Vector3(b.center.x,distance,b.center.z);

    }

    public void SetPuzzleButtonState()
    {
        object test=null; //goCurrentPuzzle;
        africaButton.interactable=!(test==africaParent);
        usaButton.interactable=!(test==usaParent);
        ukButton.interactable=!(test==ukParent);
    }
    public void Start()
    {
        //cache gameobjects
        africaParent=GameObject.Find("africaParent");
        africaHole=GameObject.Find("africaHole");
        usaParent=GameObject.Find("usaParent");
        usaHole=GameObject.Find("usaHole");
        ukParent=GameObject.Find("ukParent");
        ukHole=GameObject.Find("ukHole");

        startButton=GameObject.Find("StartButton").GetComponent<Button>();
        resetButton=GameObject.Find("ResetButton").GetComponent<Button>();
        usaButton=GameObject.Find("UsaButton").GetComponent<Button>();
        africaButton=GameObject.Find("AfricaButton").GetComponent<Button>();        
        ukButton=GameObject.Find("UKButton").GetComponent<Button>();        

        textTimer=GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        textCorrect=GameObject.Find("CorrectText").GetComponent<TextMeshProUGUI>();
        puzzleDropdown=GameObject.Find("PuzzleDropdown").GetComponent<TMP_Dropdown>();
        
        UsaOnClick();
        ResetUI();
        PopulateDropdown();
        //SelectPuzzle(0);
    }

    

    public void PopulateDropdown()
    {
        //Dropdown d=dd.GetComponent<Dropdown>();
        //d.AddOptions(puzzles);
        puzzleDropdown.AddOptions(puzzles);
    }

    public void Update()
    {
        if (GameStarted)
        {
            seconds+=Time.deltaTime;

            textTimer.text=seconds.ToString("0.0");
            textCorrect.text=CorrectPieces.ToString("0")+" to go";
            if (CorrectPieces==0)
            {
                textCorrect.text="Complete";
                GameStarted=false;
            }
        }
    }

    public void StartGame()
    {
        print ("Start Game pressed");
        int pieces=goCurrentPuzzle.GetComponent<StateHandler>().RandomizePieces2();
        CorrectPieces=pieces;
        GameStarted=true;
        //CorrectPieces=0;
        startButton.interactable=false;
        resetButton.interactable=true;
        usaButton.interactable=false;
        africaButton.interactable=false;        
    }

    public void ResetGame()
    {
        print ("Reset Game pressed");
        GameStarted=false;
        SceneManager.LoadScene(0);
        ResetUI();
    }

    void ResetUI()
    {
        startButton.interactable=true;
        resetButton.interactable=false;
        SetPuzzleButtonState();
        textCorrect.text="Ready";
        textTimer.text="0.0";
    }
    public void QuitGame()
    {
        print ("Quit Game pressed");
        Application.Quit();
    }
}
