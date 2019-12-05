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

    [Header("Aim HUD")]
    public GameObject AimHUD;

    [Header("Cam Track")]
    public CamTrack CamTrk;

    [Header("Rejuvinate Object")]
    public GameObject RejuvinateObject;

    bool IsMobile = false;
    bool CurrentlyPlaying = false;
    double StartTime = -1;
    Vector3 InitialCamPos;

    //

    void Start()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
            UnlockCursor();
        #else
            IsMobile = true;
            SetAndroidBack();
        #endif

        InitialCamPos = CamTrk.Cam.transform.position;

        Screen.SetResolution(1024, 768, false);
        CamTrk.CheckFrustum();

        MainMenu.SetActive(true);
        AimHUD.SetActive(false);
        CamTrk.ControlCanvas.SetActive(false);

        StartCoroutine( StartingMenu() );

    }

    void Update()
    {

        if( IsMobile)
            HandleAndroidBack();
        else
            if( Input.GetKeyDown("escape"))
                if(CurrentlyPlaying)
                    PlayMenu();
                else
                    PlayGame();
    }

    //

    IEnumerator StartingMenu()
    {
        yield return 0;

        PlayMenu();
    }

    public void PlayGame()
    {
        print("PlayGame");
        CurrentlyPlaying = true;
        Time.timeScale = 1;
        CamTrk.Cam.GetComponent<Camera>().farClipPlane = 200000f;

        LockCursor();
        MainMenu.SetActive(false);
        AimHUD.SetActive(true);
        if(IsMobile)
            CamTrk.ControlCanvas.SetActive(true);
    }


    public void PlayMenu()
    {
        print("PlayMenu");
        CurrentlyPlaying = false;
        Time.timeScale = 0;
        CamTrk.Cam.GetComponent<Camera>().farClipPlane = 59000;

        BoomBox.I.PauseEngines();

        SetCamHome();
        MainMenu.SetActive(true);
        AimHUD.SetActive(false);

        if(IsMobile)
            CamTrk.ControlCanvas.SetActive(false);
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
        CamTrk.SetCamPos(InitialCamPos);
        CamTrk.SetCamRot(Quaternion.identity);
    }

    public bool MobileMode()
    {
        return IsMobile;
    }

    public void LockCursor()
    {
        if( IsMobile)  return;
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        if( IsMobile)  return;
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void SetAndroidBack()
    {
        Input.backButtonLeavesApp = true;
    }

    void HandleAndroidBack()
    {
        if( Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
