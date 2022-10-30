//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    List<string> puzzles=new List<string>() {"Africa","Asia","Australia","Canada","Central America","Europe","Middle East","South America","United_States"};
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
    // Button usaButton;
    // Button africaButton;
    // Button ukButton;
    Button prevButton;
    Button nextButton;

    TextMeshProUGUI textTimer;
    TextMeshProUGUI textCorrect;

    TextMeshProUGUI debugText;

    TextMeshProUGUI hoverText;
    TMPro.TMP_Dropdown puzzleDropdown;

    public Material worldMaterial;
    public Material holeMaterial;
    public Material groundMaterial;

    float seconds;

    bool GameStarted=false;

    GameObject goSelectedPuzzle;
    GameObject goPuzzle;
    GameObject goPuzzleHole;

    Bounds boundsPuzzleArea;

    GameObject goWorld;
    GameObject goBackground;
    GameObject goGround; 
    int currentPuzzleNumber=-1;
    float puzzleLayer=0.02f;
    float holeLayer=0.01f;
    Vector3 offsetState;
    Vector3 startPosition=new Vector3 (-25,0,18);
    Vector3 currentPosition;

    public Rect rectVisible;
    public Rect rectPuzzleGround;

    int puzzleStartCount=0;
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

    private void FocusCamera(Bounds b,bool FadeIn=true,bool ExplodeAfter=false)
    {
        print("Focus Camera Bounds: "+b);
        GameObject mainCamera=GameObject.Find("Main Camera");
        Camera mCamera=mainCamera.GetComponent<Camera>();
        if (FadeIn) FadeInWorld();
        var distance = (Mathf.Max(b.size.x,b.size.z)*1.0f) * 0.5f / Mathf.Tan(mCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        if (ExplodeAfter)
        {
            LeanTween.move(mainCamera,new Vector3(b.center.x,distance,b.center.z),0.8f).setEaseInOutCubic().setOnComplete(ExplodePieces);
        }
        else
        {
            LeanTween.move(mainCamera,new Vector3(b.center.x,distance,b.center.z),0.8f).setEaseInOutCubic();
        }
    }

    private Rect VisibleGround()
    {
        GameObject mainCamera=GameObject.Find("Main Camera");
        Camera mCamera=mainCamera.GetComponent<Camera>();
        var frustumHeight = 2.0f * mainCamera.transform.position.y * Mathf.Tan(mCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * mCamera.aspect;
        return new Rect(mainCamera.transform.position.x-frustumWidth*0.5f,mainCamera.transform.position.z-frustumHeight*0.5f,frustumWidth,frustumHeight);
    }

    // public void SetPuzzleButtonState()
    // {
    //     object test=null; //goCurrentPuzzle;
    //     // africaButton.interactable=!(test==africaParent);
    //     // usaButton.interactable=!(test==usaParent);
    //     // ukButton.interactable=!(test==ukParent);
    // }
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
        prevButton=GameObject.Find("PrevButton").GetComponent<Button>();
        nextButton=GameObject.Find("NextButton").GetComponent<Button>();
        
        // usaButton=GameObject.Find("UsaButton").GetComponent<Button>();
        // africaButton=GameObject.Find("AfricaButton").GetComponent<Button>();        
        // ukButton=GameObject.Find("UKButton").GetComponent<Button>();        

        textTimer=GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        textCorrect=GameObject.Find("CorrectText").GetComponent<TextMeshProUGUI>();
        //puzzleDropdown=GameObject.Find("PuzzleDropdown").GetComponent<TMP_Dropdown>();
        hoverText=GameObject.Find("HoverText").GetComponent<TextMeshProUGUI>();
        debugText=GameObject.Find("DebugText").GetComponent<TextMeshProUGUI>();


        goWorld=GameObject.Find("New_World");
        goBackground=GameObject.Find("BackGround");
        goGround=GameObject.Find("Ground");
        goPuzzle=GameObject.Find("puzzleCurrent");
        goPuzzleHole=GameObject.Find("holeCurrent");

        //worldMaterial = (Material)Resources.Load("world", typeof(Material));

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
        rectVisible=VisibleGround();
    }

    public void StartGame()
    {
        //empty puzzleCurrent
        //copy current region into puzzleCurrent

        DeselectGO(goSelectedPuzzle);
        CorrectPieces=CopyRegion(goSelectedPuzzle,goPuzzle,null,puzzleLayer,"");
        CopyRegion(goSelectedPuzzle,goPuzzleHole,holeMaterial,holeLayer,"_");
        PrepareDrag(goPuzzle);
        //CopyRegionToHole(goSelectedPuzzle);
        HighlightGO(goPuzzle);
        FadeOutWorld();
        StartGameUI();
        ZoomOut();
        GameStarted=true;
        //ExplodePieces();
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

  private void ZoomOut()
  {
    float zoomOutAmount=0.75f;
    Bounds b = GetChildBounds(goSelectedPuzzle);
    //print("Zoom Out bounds "+b);
    //b.Expand(b.size.z*2);
    Bounds b2=new Bounds(new Vector3(b.center.x,b.center.y,b.center.z+(b.size.z*(1-zoomOutAmount))),new Vector3(b.size.x,b.size.y,b.size.z*(1/zoomOutAmount)));
    
    //print("Zoom Out bounds "+b2);
    //b.center=new Vector3(0,0,0);
    FocusCamera(b2,false,true);
  }

  private void CopyRegionToHole(GameObject goSelectedPuzzle)
  {
        if (goPuzzleHole == null) goPuzzleHole=GameObject.Find("holeCurrent");
        foreach (Transform t in goPuzzleHole.transform)
        {
            GameObject.Destroy(t.transform.gameObject);
        }
        print ("destroy hole children finished");
        // GameObject[] children=goP.GetComponentsInChildren<GameObject>();
        // print ("starting loop");
        // foreach (GameObject c in children)
        // {
        //     GameObject.Instantiate(c,goCurrentPuzzle.transform);
        // }

        foreach (Transform child in goSelectedPuzzle.transform)
        {
            //print("Foreach loop: " + child);
            Transform tNew=GameObject.Instantiate(child,goPuzzleHole.transform,true);
            tNew.position=new Vector3(tNew.position.x,holeLayer,tNew.position.z);
            tNew.GetComponent<MeshRenderer>().material=holeMaterial;
            //tNew.gameObject.name=tNew.gameObject.name+" HOLE";
        }
  }

  public void FadeOutWorld()
    {
        //LeanTween.value(transform.gameObject,updateWorldMaterialColour,Color.white,groundMaterial.color,1f);
        //LeanTween.move(world)
        //LeanTween.value(transform.gameObject,updateWorldMaterialAlpha, 1f, 0f, 1f );
        //LeanTween.color()
        //goWorld.SetActive(false);
        SetWorldColor(new Color(.55f,.55f,.55f,1));
    }

    public void FadeInWorld()
    {
        //Hashtable options=new Hashtable();
        //LeanTween.value(transform.gameObject,updateWorldMaterialAlpha, worldMaterial.color.a,1f,1f );
        //LeanTween.value(gameObject,updateWorldMaterialColour,worldMaterial.color,Color.white,1f);
        //goWorld.SetActive(true);
        SetWorldColor(Color.white);
        
    }

    void SetWorldColor(Color c)
    {
        MeshRenderer[] worldMeshes=goWorld.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in worldMeshes)
        {
            mr.material.color=c;
        }
    }

    public void updateWorldMaterialAlpha(float v)
    {
        //print(v);
        worldMaterial.color=new Color(1,1,1,v);
    }

    public void updateWorldMaterialColour(Color v)
    {
        //print(v);
        worldMaterial.color=v;
    }

    public void ExplodePieces()
    {
        //print(currentPuzzleFloatHeight);
        // foreach (Transform t in goPuzzle.transform)
        // {
        //     //print(t.gameObject);
            
        //     LeanTween.move(t.gameObject,new Vector3(Random.Range(-2f,2f),puzzleLayer,Random.Range(-2f,2f)),0.8f).setEaseInOutCubic();
        // }
        RandomizePieces2();
    }

    public int RandomizePieces2()
    {
        float iMinZ=0;
        rectPuzzleGround=VisibleGround();
        //MeshRenderer[] meshs=GetComponentsInChildren<MeshRenderer>();
        startPosition=new Vector3(rectPuzzleGround.x,puzzleLayer,rectPuzzleGround.y+rectPuzzleGround.height);
        print("Start Position: "+startPosition);
        currentPosition=startPosition;
        iMinZ=startPosition.z;

        List<Transform> order=new List<Transform>();
        foreach (Transform t in goPuzzle.transform)
        {
            if (t.gameObject.activeSelf) 
                {
                    //print(t.gameObject.name);
                    order.Add(t);
                }
        }        
        
        int countPieces;


        //print ("Number of Pieces "+order.Count);
        countPieces=order.Count;
        //CorrectPieces=countPieces;
        Random.InitState((int)Time.time);
        while (order.Count>0)
        {
            int index=Random.Range(0,order.Count-1);
            Transform meshToMove=order[index];
            Bounds meshBounds= meshToMove.GetComponent<MeshRenderer>().bounds;
            //print (meshBounds);
            //take position and subtract meshbounds center * half of meshBounds.Size;
            //print(currentPosition);
            Vector3 mbSize=new Vector3(meshBounds.size.x*0.5f,0,-meshBounds.size.z*0.5f);
            Vector3 mbCenter=new Vector3(meshBounds.center.x,0,meshBounds.center.z);
            //print("Current Position: "+currentPosition);
            
            //check if offscreen
            if ((currentPosition+mbSize).x>(rectPuzzleGround.x+rectPuzzleGround.width))
            {
                //move CurrentPosition down to iMinZ
                currentPosition=new Vector3(startPosition.x,startPosition.y,iMinZ);
            }
            Vector3 v = currentPosition+mbSize-mbCenter;
            print(meshToMove.gameObject.name+" position:"+v+" size: "+meshBounds.size);
            iMinZ=Mathf.Min(iMinZ,currentPosition.z-meshBounds.size.z);
            //Vector3 v2=new Vector3(rectPuzzleGround.x,0,rectPuzzleGround.y+rectPuzzleGround.height); //align to left edge
            //Vector3 v=new Vector3(rectPuzzleGround.x+meshBounds.size.x*0.5f-meshBounds.center.x,0,0); //align to left edge
            //Vector3 v=new Vector3(0,0,(rectPuzzleGround.y+rectPuzzleGround.height)-meshBounds.size.z*0.5f-meshBounds.center.z); //align to top edge
            meshToMove.position=v; //GetNextPlace();
            //print(meshToMove.position);
            currentPosition+=new Vector3(meshBounds.size.x,0,0);
            order.RemoveAt(index);
        }

        return countPieces;
    }

  private Vector3 GetPuzzleBoundsStartPosition()
  {
        rectPuzzleGround=VisibleGround();
        
        return new Vector3(rectPuzzleGround.x-rectPuzzleGround.width*0.5f,puzzleLayer*2,rectPuzzleGround.y+rectPuzzleGround.height*0.5f);
  }

  Vector3 GetNextPlace()
    {
        
        Vector3 newPosition=currentPosition;
        currentPosition=currentPosition+new Vector3(rectPuzzleGround.width*0.1f,0,0);
        if (currentPosition.x>25)
            currentPosition=new Vector3(startPosition.x,puzzleLayer*2,currentPosition.z-rectPuzzleGround.width*0.1f);

        return newPosition;
    }

    private void DestroyChildren(GameObject goP)
    {
        int counter=0;
        foreach (Transform t in goP.transform)
        {
            t.gameObject.SetActive(false);
            GameObject.Destroy(t.gameObject);
            counter++;
        }
        print ("destroy finished:" +counter);
    }

    private int CopyRegion(GameObject gFrom,GameObject gTo,Material m,float pLayer,string cloneRename)
    {
        //return number of pieces

        int counter=0;

        // GameObject[] children=goP.GetComponentsInChildren<GameObject>();
        // print ("starting loop");
        // foreach (GameObject c in children)
        // {
        //     GameObject.Instantiate(c,goCurrentPuzzle.transform);
        // }

        //goPuzzle.transform.position=new Vector3(0,puzzleLayer,0);
        counter=0;
        foreach (Transform child in gFrom.transform)
        {
            if (child.gameObject.activeSelf)
            {
            //print("Foreach loop: " + child);
            Transform tNew=GameObject.Instantiate(child,gTo.transform,true);
            //tNew.position=new Vector3(tNew.position.x,0.02f,tNew.position.z);
            //tNew.localPosition=new Vector3(0,-0.01f,0);
            tNew.position=new Vector3(tNew.position.x,pLayer,tNew.position.z);
            if (m!=null) tNew.GetComponent<MeshRenderer>().material=m;
            tNew.gameObject.name=tNew.gameObject.name.Replace("(Clone)",cloneRename);

            counter++;
            }
        }
        //goPuzzle.transform.position=new Vector3(0,puzzleLayer,0);
        print ("Copy finished:" +counter);
        return counter;
    }

    private void PrepareDrag(GameObject gParent)
    {
        foreach (Transform child in gParent.transform)
        {
            //child.gameObject.AddComponent<MeshCollider>(); 
            child.gameObject.tag="drag";
        }
        //debugText.text="cannot find Algeria_puzz";
        // GameObject algeria=GameObject.Find("Algeria_puzz");
        // Physics.SyncTransforms();
        // if (algeria!=null)
        // {
        //     debugText.text="Not Found Algeria_puzz mesh_collider";
        //     MeshCollider mc=algeria.GetComponent<MeshCollider>();
        //     if (mc!=null) 
        //     {
        //   //      debugText.text=mc.sharedMesh.name;
        //     }


        // }
        
    }

    public void ResetGame()
    {
        print ("Reset Game pressed");
        GameStarted=false;
        DestroyChildren(goPuzzle);
        DestroyChildren(goPuzzleHole);
        FadeInWorld();
        SelectPuzzle(currentPuzzleNumber);
        //SceneManager.LoadScene(0);
        ResetUI();
    }

    void ResetUI()
    {
        startButton.interactable=true;
        resetButton.interactable=false;
        prevButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        textCorrect.text="Ready";
        textTimer.text="0.0";
    }

    void StartGameUI()
    {
        startButton.interactable=false;
        resetButton.interactable=true;
        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        textCorrect.text="# to go";
        textTimer.text="0.0";
    }

    public void QuitGame()
    {
        print ("Quit Game pressed");
        Application.Quit();
    }
}


