using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        //preloading of music and sound
        //common assets

        //setup singletons
        //AssetProvider.Prewarm();
        //network connections

        //create main controller
        var mainMenuVC = new MainMenuController();


        var dungeonVC = new DungeonController();
        UNavigationController.SetRootViewController(dungeonVC);
        //UNavigationController.SetRootViewController(mainMenuVC);
    }
}

public class DungeonController : USceneController
{
    public DungeonController() : base(SceneNames.Dungeon) { }

    public override void SceneDidLoad()
    {
        
    }
}