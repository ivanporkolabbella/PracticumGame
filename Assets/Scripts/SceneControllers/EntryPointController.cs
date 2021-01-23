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
        AssetProvider.Prewarm();
        InputManager.Activate();
        //network connections

        //create main controller
        var mainMenuVC = new MainMenuController();
        //UNavigationController.SetRootViewController(mainMenuVC);

        var dungeonVC = new DungeonController();
        //UNavigationController.SetRootViewController(dungeonVC);

        var particleTest = new ParticleTestController();
        //UNavigationController.SetRootViewController(particleTest);
        

        var saveLoadVC = new LoadSaveTestController();
        UNavigationController.SetRootViewController(saveLoadVC);
    }
}

public class LoadSaveTestController : USceneController
{
    public LoadSaveTestController() : base(SceneNames.SaveLoadTest) { }

    public override void SceneWillAppear()
    {
        base.SceneWillAppear();
        SaveLoadManager.ClearAll();
    }

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();

        var loadButton = GameObject.Find("LoadButton").GetComponent<Button>();

        if (loadButton == null) return;

        loadButton.onClick.AddListener(() => {
            Debug.Log("Load!");
            SaveLoadManager.ApplySnapshot();
        });

        var saveButton = new UButton("SaveButton");

        saveButton.OnClick(() => {
            Debug.Log("Save!");
            SaveLoadManager.CreateSnapshot();
        });

        var generateBoxButton = new UButton("GenerateButton");

        generateBoxButton.OnClick(() => {
            var box = AssetProvider.GetAsset(GameAsset.Box);
            box.transform.position = HelperFunctions.RandomVector(5, 5, 5);
        });

        //saveButton.SetText("New text");
    }
}

public enum LoadableGameObject
{
    Box
}

public class LoadableAssetsProvider
{
    public static GameObject GetLoadableGameObject(LoadableGameObject loadable)
    {
        return AssetProvider.GetAsset(GameAssetTypeFromLoadableObject(loadable));
    }

    public static GameAsset GameAssetTypeFromLoadableObject(LoadableGameObject loadable)
    {
        switch (loadable)
        {
            case LoadableGameObject.Box:
                return GameAsset.Box;
            default:
                return GameAsset.Archer;
        }
    }

    public static ISaveLoadable GenerateLoadableObjectFromSnapshot(Dictionary<string, object> snapshotData)
    {
        var type = (LoadableGameObject)snapshotData["type"];
        var newObject = GetLoadableGameObject(type).GetComponent<ISaveLoadable>();
        newObject.ApplyData(snapshotData);

        return newObject;
    }
}