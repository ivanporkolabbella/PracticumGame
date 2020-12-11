using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryPointController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var second = new SecondController();
        second.SceneName = Scenes.ThirdScene.ToString();
        UNavigationController.SetRootViewController(second);
        //UNavigationController.SetRootViewController(new FirstController());
    }
}

public enum Scenes
{
    First, Second, InGameMenu, ThirdScene
}

public class SecondController : USceneController
{
    public SecondController() : base(Scenes.Second.ToString())
    {
    }

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        Debug.Log("did load: " + SceneName);
        setUpButtons();
    }

    private void setUpButtons()
    {
        var button = GameObject.Find("FirstButton").GetComponent<Button>();
        if (button != null)
        {
            button.GetComponentInChildren<Text>().text = "JUST CHANGED";

            button.onClick.AddListener(() =>
            {
                Debug.Log("Click on a button");
                var first = new FirstController();
                PushSceneController(first);
            });
        }
        else
        {
            Debug.Log("No button");
        }

        var button2 = GameObject.Find("MenuButton").GetComponent<Button>();
        button2.GetComponentInChildren<Text>().text = "OPEN MENU";
        if (button2 != null)
        {
            button2.onClick.AddListener(() =>
            {
                Debug.Log("Click on a button");
                AddChildSceneController(new MenuController());
            });
        }
        else
        {
            Debug.Log("No button");
        }
    }
}

public class FirstController : USceneController
{
    public FirstController() : base(Scenes.First.ToString())
    {
    }

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        Debug.Log("did load: " + SceneName);
        setUpButtons();
    }

    private void setUpButtons()
    {
        var button = GameObject.Find("MenuButton").GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = "OPEN MENU";
        if (button != null)
        { 
            button.onClick.AddListener(() =>
            {
                Debug.Log("Click on a button");
                AddChildSceneController(new MenuController());
            });
        }
        else {
            Debug.Log("No button");
        }

        var button2 = GameObject.Find("BackButton").GetComponent<Button>();
        button2.GetComponentInChildren<Text>().text = "GO BACK";
        if (button2 != null)
        {
            button2.onClick.AddListener(() =>
            {
                PopToParentSceneController();
            });
        }
        else
        {
            Debug.Log("No button");
        }

    }
}

public class MenuController : USceneController
{
    public MenuController() : base(Scenes.InGameMenu.ToString())
    {
    }

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        Debug.Log("did load: " + SceneName);
        setUpButtons();
    }

    private void setUpButtons()
    {
        var button = GameObject.Find("CloseButton").GetComponent<Button>();
        if (button != null)
        {
            button.GetComponentInChildren<Text>().text = "CLOSE";

            button.onClick.AddListener(() =>
            {
                Debug.Log("Closing Menu");
                RemoveFromParentSceneController();
            });
        }
        else
        {
            Debug.Log("No button");
        }
    }
}