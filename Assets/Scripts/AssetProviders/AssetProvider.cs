using System.Collections;
using UnityEngine;

public class AssetProvider : AssetBaseProvider
{
    public GameObject bullet;
    public int bulletPoolSize;

    [Header("Enemies")]
    public int defaultPoolSize = 10;
    public GameObject enemy1;
    public GameObject enemy2;

    private static AssetProvider _instance;
    private static AssetProvider Instance
    {
        get {
            if (_instance == null) {
                _instance = Resources.Load<AssetProvider>("AssetProvider");
            }

            return _instance;
        }
    }

    protected override AssetBaseProvider GetInstance()
    {
        return Instance;
    }

    public static GameObject GetAsset(GameAsset asset)
    {
        switch (asset)
        {
            case GameAsset.Bullet:
                return Instance.GetObjectFromPool(Instance.bullet);
            case GameAsset.Footman:
                return Instance.GetObjectFromPool(Instance.enemy1);
            case GameAsset.Archer:
                return Instance.GetObjectFromPool(Instance.enemy2);
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
            //prefill pool (this could be done through reflection)

            _instance.poolObject = new GameObject();
            _instance.poolObject.name = "Pool";
            GameObject.DontDestroyOnLoad(_instance.poolObject);

            //bullet pool
            Instance.InstatiatePool(_instance.bullet, _instance.bulletPoolSize);

            Instance.InstatiatePool(_instance.enemy1, _instance.defaultPoolSize);
            Instance.InstatiatePool(_instance.enemy2, _instance.defaultPoolSize);
        }
    }
}

public enum GameAsset
{
    Bullet, Footman, Archer
}

