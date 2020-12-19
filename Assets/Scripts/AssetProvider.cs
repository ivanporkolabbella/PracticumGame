using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetProvider : MonoBehaviour, IPool
{
    private GameObject poolObject;

    public GameObject bullet;
    public int bulletPoolSize;

    public GameObject enemy1;
    public GameObject enemy2;

    public Dictionary<GameObject, Stack<GameObject>> pool = new Dictionary<GameObject, Stack<GameObject>>();

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

    public static GameObject GetAsset(GameAsset asset)
    {
        switch (asset)
        {
            case GameAsset.Bullet:
                return GetObjectFromPool(Instance.bullet);
            default:
                break;
        }
        return null;
    }

    private static GameObject GetObjectFromPool(GameObject requiredObject)
    {
        if (_instance.pool[requiredObject].Count > 0)
        {
            var obj = Instance.pool[requiredObject].Pop();
            obj.SetActive(true);
            obj.transform.SetParent(null);

            return obj;
        }
        else
        {
            return CreatePoolableObject(requiredObject);
        }
    }

    private static GameObject CreatePoolableObject(GameObject poolableObject)
    {
        var newObject = GameObject.Instantiate(poolableObject);

        var poolable = newObject.GetComponent<PoolableObject>();
        if (poolable == null) {
            poolable = newObject.AddComponent<PoolableObject>();
        }

        poolable.SetPool(poolableObject, Instance);

        return newObject;
    }

    public static void Prewarm()
    {
        if (_instance == null)
        {
            _instance = Resources.Load<AssetProvider>("AssetProvider");
            //prefill pool (this could be done through reflection)

            _instance.poolObject = GameObject.Instantiate(new GameObject());
            _instance.poolObject.name = "Pool";
            GameObject.DontDestroyOnLoad(_instance.poolObject);

            //bullet pool
            _instance.pool[_instance.bullet] = new Stack<GameObject>();
            FillPool(_instance.bullet, _instance.bulletPoolSize);
        }
    }

    private static void FillPool(GameObject templateObject, int numberOfInstances)
    {
        var pool = _instance.pool[templateObject];
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject newObject = CreatePoolableObject(templateObject);
            newObject.transform.SetParent(_instance.poolObject.transform);
            newObject.SetActive(false);
            pool.Push(newObject);
        }
    }

    public void ReturnToPool(GameObject objectToReturn, GameObject key)
    {
        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(poolObject.transform);
        pool[key].Push(objectToReturn);
    }
}

public enum GameAsset
{
    Bullet
}

public interface IPool
{
    void ReturnToPool(GameObject objectToReturn, GameObject key);
}

public class PoolableObject : MonoBehaviour
{
    private IPool pool;
    private GameObject key;

    public void SetPool(GameObject key, IPool pool)
    {
        this.key = key;
        this.pool = pool;
    }

    public void ReturnToPool()
    {
        pool.ReturnToPool(gameObject, key);
    }
}