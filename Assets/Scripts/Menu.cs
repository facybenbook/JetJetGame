using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public void StartMatch()
    {
        Game.I.StartMatch();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
