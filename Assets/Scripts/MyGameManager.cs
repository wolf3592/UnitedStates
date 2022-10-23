using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{

    public static int CorrectPieces=0;
    GameObject africaParent;
    GameObject africaHole;
    GameObject usaParent;
    GameObject usaHole;
    GameObject ukParent;
    GameObject ukHole;

    Button startButton;
    Button resetButton;
    Button usaButton;
    Button africaButton;
    Button ukButton;

    TextMeshProUGUI textTimer;
    TextMeshProUGUI textCorrect;

    float seconds;

    bool GameStarted=false;

    GameObject goCurrentPuzzle;

    private void EnablePuzzle()
    {
        africaHole.SetActive(goCurrentPuzzle==africaParent);
        africaParent.SetActive(goCurrentPuzzle==africaParent);
        usaHole.SetActive(goCurrentPuzzle==usaParent);
        usaParent.SetActive(goCurrentPuzzle==usaParent);
        ukHole.SetActive(goCurrentPuzzle==ukParent);
        ukParent.SetActive(goCurrentPuzzle==ukParent);
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
        
        UsaOnClick();
        ResetUI();
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
