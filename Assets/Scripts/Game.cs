using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Game : MonoBehaviour
{
    public static Game I;
    protected Game() { }
    void Awake() {  I = this;  }

    //

    [Header("Main Menu")]
    public GameObject MainMenu;


    [Header("Airwing")]
    public Airwing Airwing;

    
    [Header("Airwing HUD")]
    public GameObject AirwingHUD;

    [Header("AirwingControl")]
    public AirwingController AirwingControl;

    [Header("Rejuvinate Object")]
    public GameObject RejuvinateObject;

    bool CurrentlyPlaying = false;
    double StartTime = -1;

    //

    void Start()
    {
      
        LockCursor();

        PlayMenu();

    }

    void Update()
    {
        if( Input.GetKeyDown("escape"))
            if(CurrentlyPlaying)
                PlayMenu();
            else
                PlayGame();
    }

    //

  

    public void PlayGame()
    {
        print("PlayGame");
        CurrentlyPlaying = true;
        Time.timeScale = 1;



        LockCursor();
        MainMenu.SetActive(false);
        AirwingHUD.SetActive(true);
    }


    public void PlayMenu()
    {
        print("PlayMenu");
        
        SetCamHome();

        CurrentlyPlaying = false;
       
        BoomBox.I.PauseEngines();

        MainMenu.SetActive(true);
        AirwingHUD.SetActive(false);

        StartCoroutine( StopTime() );

    }

    
    IEnumerator StopTime()
    {
        yield return 0;
        Time.timeScale = 0;

    }


    public bool IsPlayingGame()
    {
        return CurrentlyPlaying;
    }


    public void StartMatch()
    {
        StartTime = Time.time;

        PlayGame();
    }

    string MatchDuration()
    {
        if(StartTime == -1) {
            return "0:00";
        }
        else
        {
            double seconds = Time.time - StartTime;
            double minutes = System.Math.Floor( seconds / 60 );
            seconds = seconds % 60;
            string secondsTxt = "";
            if(seconds < 10) secondsTxt += "0";
            secondsTxt += seconds.ToString("F0");
            return minutes + ":" + secondsTxt;
        }
    }


    //

    public void SetCamHome()
    {
        AirwingControl.transform.position =  new Vector3(0, 200f, 0);
        AirwingControl.transform.rotation = Quaternion.identity;
        AirwingControl.ResetRig();
    }


    public void LockCursor()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


}
