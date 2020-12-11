﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UNavigationController
{
    private static readonly Stack<USceneController> _controllersStack = new Stack<USceneController>();
    public static USceneController ActiveController => _controllersStack.Peek();

    public static USceneController RootController { get; private set; }

    public static void PresentViewController(USceneController controller)
    {
        if (_controllersStack.Count == 0)
        {
            RootController = controller;
        }

        _controllersStack.Push(controller);

        SceneManager.LoadScene(controller.SceneName);
    }

    public static void RemoveViewController()
    {
        if (_controllersStack.Count > 0)
        {
            _controllersStack.Pop();
            ActiveController.RegisterLoad();
            SceneManager.LoadScene(ActiveController.SceneName);
        }
        else
        {
            //Debug.LogWarning("Navigational Stack is EMPTY! Loading Root VC!");
            SceneManager.LoadScene(RootController.SceneName);
        }
    }

    public static void SetRootViewController(USceneController controller)
    {
        _controllersStack.Clear();

        controller.RegisterLoad();

        PresentViewController(controller);
    }

    public static void PopToRootViewController()
    {
        _controllersStack.Clear();
        RootController.SceneWillAppear();
        RootController.RegisterLoad();
        PresentViewController(RootController);
    }
}
