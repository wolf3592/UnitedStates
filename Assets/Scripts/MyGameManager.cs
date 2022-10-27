//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    List<string> puzzles=new List<string>() {"Africa","Asia","Australia","Canada","Caribbean","Central America","Europe","Middle East","Oceania","South America","United_States"};
    public static int CorrectPieces=0;
    // GameObject africaParent;
    // GameObject africaHole;
    // GameObject usaParent;
    // GameObject usaHole;
    // GameObject ukParent;
    // GameObject ukHole;

    Camera mainCamera;

    Button startButton;
    Button resetButton;
    Button usaButton;
    Button africaButton;
    Button ukButton;

    TextMeshProUGUI textTimer;
    TextMeshProUGUI textCorrect;

    TextMeshProUGUI hoverText;
    TMPro.TMP_Dropdown puzzleDropdown;

    public Material worldMaterial;
    public Material groundMaterial;

    float seconds;

    bool GameStarted=false;

    GameObject goSelectedPuzzle;
    GameObject goCurrentPuzzle;
    GameObject goCurrentPuzzleHole;
    
    int currentPuzzleNumber=-1;
    public float currentPuzzleFloatHeight=0.01f;

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

    // public void AfricaOnClick()
    // {
    //     goSelectedPuzzle=africaParent;
    //     EnablePuzzle();
    //     SetPuzzleButtonState();
    // }
    // public void UsaOnClick()
    // {
    //     goSelectedPuzzle=usaParent;
    //     EnablePuzzle();
    //     SetPuzzleButtonState();
    // }
    // public void UKOnClick()
    // {
    //     goSelectedPuzzle=ukParent;
    //     EnablePuzzle();
    //     SetPuzzleButtonState();
    // }

    public void TestOnClick()
    {
        //get United_States
        //FocusCamera(GetChildBounds(GameObject.Find("United_States")));

    
        //get extent
        //set camera above the centre of the mesh
        //zoom so that the mesh is centre bottom
    }

    public void Test2OnClick()
    {
        //FocusCamera(GetChildBounds(GameObject.Find("Middle_East")));
    }

    public void OnValueChanged(int target)
    {
        SelectPuzzle(target);
    }

    private void DeselectGO(GameObject go)
    {
        if (go==null) return;
        MeshRenderer[] mats=go.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mats)
        {
            m.material=worldMaterial;
            //m.material.color=new Color(0,0,Random.Range(0.3f,1f),1);
            
        }
    }

    private void HighlightGO(GameObject go)
    {
        hoverText.text=go.name.Replace("_"," ");
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

    private void FocusCamera(Bounds b,bool tween=true)
    {
        GameObject mainCamera=GameObject.Find("Main Camera");
        Camera mCamera=mainCamera.GetComponent<Camera>();
        FadeInWorld();
        var distance = (Mathf.Max(b.size.x,b.size.z)*1.0f) * 0.5f / Mathf.Tan(mCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        if (tween)
        {
        LeanTween.move(mainCamera,new Vector3(b.center.x,distance,b.center.z),0.8f).setEaseInOutCubic();
        }
        else
        {
            print("Move over bounds");
            LeanTween.move(mainCamera,new Vector3(b.center.x,distance,b.center.z),0.8f).setEaseInOutCubic();
        }
    }

    public void SetPuzzleButtonState()
    {
        object test=null; //goCurrentPuzzle;
        // africaButton.interactable=!(test==africaParent);
        // usaButton.interactable=!(test==usaParent);
        // ukButton.interactable=!(test==ukParent);
    }
    public void Start()
    {
        //cache gameobjects
        // africaParent=GameObject.Find("africaParent");
        // africaHole=GameObject.Find("africaHole");
        // usaParent=GameObject.Find("usaParent");
        // usaHole=GameObject.Find("usaHole");
        // ukParent=GameObject.Find("ukParent");
        // ukHole=GameObject.Find("ukHole");

        startButton=GameObject.Find("StartButton").GetComponent<Button>();
        resetButton=GameObject.Find("ResetButton").GetComponent<Button>();
        // usaButton=GameObject.Find("UsaButton").GetComponent<Button>();
        // africaButton=GameObject.Find("AfricaButton").GetComponent<Button>();        
        // ukButton=GameObject.Find("UKButton").GetComponent<Button>();        

        textTimer=GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        textCorrect=GameObject.Find("CorrectText").GetComponent<TextMeshProUGUI>();
        //puzzleDropdown=GameObject.Find("PuzzleDropdown").GetComponent<TMP_Dropdown>();
        hoverText=GameObject.Find("HoverText").GetComponent<TextMeshProUGUI>();


        //UsaOnClick();
        ResetUI();
        //PopulateDropdown();

        SelectPuzzle(0,false);
    }

    private void SelectPuzzle(int index,bool tween=true)
    {
        string selection=puzzles[index].Replace(" ","_");


        currentPuzzleNumber=index;
        GameObject go = GameObject.Find(selection);
        if (go!=null) 
            {
            FocusCamera(GetChildBounds(go),tween);
            if (goSelectedPuzzle!=null) DeselectGO(goSelectedPuzzle);
            HighlightGO(go);
            goSelectedPuzzle=go;
            }
        else
            {
                if (goSelectedPuzzle!=null) DeselectGO(goSelectedPuzzle);
                goSelectedPuzzle=null;
                print ("Could not find: "+selection);    
            }
    }

    public void NextButtonOnClick()
    {
    currentPuzzleNumber++;
    if (currentPuzzleNumber==puzzles.Count) currentPuzzleNumber=0;
    SelectPuzzle(currentPuzzleNumber);
    }

    public void PrevButtonOnClick()
    {
    currentPuzzleNumber--;
    if (currentPuzzleNumber<0) currentPuzzleNumber=puzzles.Count-1;
    SelectPuzzle(currentPuzzleNumber);
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
        //empty puzzleCurrent
        //copy current region into puzzleCurrent
        DeselectGO(goSelectedPuzzle);
        CopyRegionToCurrent(goSelectedPuzzle);
        HighlightGO(goCurrentPuzzle);
        FadeOutWorld();
        ExplodePieces();
        // print ("Start Game pressed");
        // int pieces=goCurrentPuzzle.GetComponent<StateHandler>().RandomizePieces2();
        // CorrectPieces=pieces;
        // GameStarted=true;
        // //CorrectPieces=0;
        // startButton.interactable=false;
        // resetButton.interactable=true;
        // usaButton.interactable=false;
        // africaButton.interactable=false;        
    }

    public void FadeOutWorld()
    {
        LeanTween.value(transform.gameObject,updateWorldMaterialColour,Color.white,groundMaterial.color,1f);
        //LeanTween.move(world)
        //LeanTween.value(transform.gameObject,updateWorldMaterialAlpha, 1f, 0f, 1f );
        //LeanTween.color()
    }

    public void FadeInWorld()
    {
        Hashtable options=new Hashtable();
        //LeanTween.value(transform.gameObject,updateWorldMaterialAlpha, worldMaterial.color.a,1f,1f );
        LeanTween.value(gameObject,updateWorldMaterialColour,worldMaterial.color,Color.white,1f);
    }

    public void updateWorldMaterialAlpha(float v)
    {
        worldMaterial.color=new Color(1,1,1,v);
    }

    public void updateWorldMaterialColour(Color v)
    {
        worldMaterial.color=v;
    }

    public void ExplodePieces()
    {
        print(currentPuzzleFloatHeight);
        foreach (Transform t in goCurrentPuzzle.transform)
        {
            //print(t.gameObject);
            
            LeanTween.move(t.gameObject,new Vector3(Random.Range(-2f,2f),currentPuzzleFloatHeight,Random.Range(-2f,2f)),0.8f).setEaseInOutCubic();
        }
    }

    private void CopyRegionToCurrent(GameObject goP)
    {
        if (goCurrentPuzzle == null) goCurrentPuzzle=GameObject.Find("puzzleCurrent");
        foreach (Transform t in goCurrentPuzzle.transform)
        {
            GameObject.Destroy(t.transform.gameObject);
        }
        print ("destroy finished");
        // GameObject[] children=goP.GetComponentsInChildren<GameObject>();
        // print ("starting loop");
        // foreach (GameObject c in children)
        // {
        //     GameObject.Instantiate(c,goCurrentPuzzle.transform);
        // }



        foreach (Transform child in goP.transform)
        {
            //print("Foreach loop: " + child);
            Transform tNew=GameObject.Instantiate(child,goCurrentPuzzle.transform,true);
            tNew.position=new Vector3(tNew.position.x,currentPuzzleFloatHeight,tNew.position.z);
        }
        
        print ("finished");
        // -43.71, -0.02, -41.5
        //0,180,0
        //319.4,1,323

        //
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
