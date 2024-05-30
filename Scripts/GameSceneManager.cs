using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance; // Singleton

    void Awake()
    {
        if(instance != null && instance != this) // check if this class exists
        {
            DestroyImmediate(this.gameObject, true); // if exists destroy this class
            return;
        }
        instance = this; // else make it singleton

        DontDestroyOnLoad(this.gameObject); // make this object perminent so that it remains accessable when changing scenes
    }

    [SerializeField] private List<string> allScenes = new List<string>();
    private int currentScene = 0;

    public void LoadScene(int menuId) // function to load specific scene
    {
        if(menuId == currentScene || menuId < 0) // check if given scene id is valid
            return; // if invalid do not do anything

        if(menuId >= allScenes.Count) // if given id is out of range of allscenes ids
        {
            LoadMainMenu(); // load main menu
            return;
        }

        SceneManager.LoadScene(allScenes[menuId]); // else load scene with given id
        currentScene = menuId; // set current scene id to given id
    }

    public void LoadMainMenu()
    {
        LoadScene(0);
    }

    public void LoadGameScene()
    {
        LoadScene(1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // if game is running in unity stop play mode
        #else
        Application.Quit(); // if it built version exit the game application
        #endif
    }
}
