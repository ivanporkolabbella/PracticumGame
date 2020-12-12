using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetProvider : MonoBehaviour
{
    public GameObject bullet;
    public GameObject enemy1;
    public GameObject enemy2;

    private static AssetProvider _instance;
    private static AssetProvider Instance
    {
        get {
            if (_instance == null) {
                _instance = Resources.Load<AssetProvider>("AssetProvider");
                Debug.Log("Loaded Asset:" + _instance.name);
            }

            return _instance;
        }
    }

    public static GameObject GetAsset(GameAsset asset)
    {
        switch (asset)
        {
            case GameAsset.Bullet:
                return Instantiate(Instance.bullet);
            default:
                break;
        }
        return null;
    }

    public static void Prewarm()
    {
        if (_instance == null)
        {
            _instance = Resources.Load<AssetProvider>("AssetProvider");
        }
    }
}

public enum GameAsset
{
    Bullet
}
