using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryPointController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
    #else
        Debug.unityLogger.logEnabled = false;

     #endif
        var mainMenuVC = new MainMenuController();

        UNavigationController.SetRootViewController(mainMenuVC);
    }
}

public class MainMenuController : USceneController
{
    public MainMenuController() : base(SceneNames.MainMenu) { }

    public override void SceneWillAppear()
    {
        Debug.Log("Will appear: " + SceneName);
        var obj = GameObject.Find("MainMenu");
        //Debug.Log("Object grabbed: " + obj.name);
    }

    public override void SceneDidLoad()
    {
        Debug.Log("Did load : " + SceneName);
        var obj = GameObject.Find("MainMenu");
        Debug.Log("Object grabbed: " + obj.name);
    }
}

public struct SceneNames
{
    public static string MainMenu = "MainMenu";
}