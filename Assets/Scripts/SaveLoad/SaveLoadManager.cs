using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager
{
    private ISerializationManager serializer = new MockSerializationManager();

    private static List<ISaveLoadable> activeObjects = new List<ISaveLoadable>();
    private static int IDCount = 0;

    private static Dictionary<int, Dictionary<string, object>> snapshot = new Dictionary<int, Dictionary<string, object>>();

    public static int AutogenerateID()
    {
        return IDCount++;
    }

    public static void RegisterObject(ISaveLoadable newObject)
    {
        if (activeObjects.Contains(newObject)) return;

        activeObjects.Add(newObject);
    }

    public static void DeregisterObject(ISaveLoadable objectToRemove)
    {
        activeObjects.Remove(objectToRemove);
    }

    public static void ClearAll()
    {
        activeObjects.Clear();
    }

    public static void CreateSnapshot()
    {
        snapshot.Clear();

        foreach (var element in activeObjects)
        {
            snapshot.Add(element.ObjectID, element.GetData());
        }
    }

    public static void ApplySnapshot()
    {
        //go through activeObjects
        //if it doesn't exist in snapshot - destroy it (back to pool)
        //if exists - apply data
        //on the end, if any objects left in snapshot - create new object and apply data to it

        //perhaps cache this for extra performance
        var snapshotCopy = new Dictionary<int, Dictionary<string, object>>(snapshot);
        var toRemove = new List<ISaveLoadable>();

        foreach (var element in activeObjects)
        {
            Dictionary<string, object> savedData;
            if (snapshot.TryGetValue(element.ObjectID, out savedData))
            {
                element.ApplyData(savedData);
                snapshotCopy.Remove(element.ObjectID);
            }
            else
            {
                toRemove.Add(element);
            }
        }

        //create elements missing (snapshotCopy)

        foreach (var snapshot in snapshotCopy.Values)
        {
            var newElement = LoadableAssetsProvider.GenerateLoadableObjectFromSnapshot(snapshot);
            newElement.Activate();
        }

        //remove objects created after taking a snapshot (toRemove)
        foreach (var element in toRemove)
        {
            activeObjects.Remove(element);
            element.GameObject.GetComponent<PoolableObject>().ReturnToPool();
        }
    }

    //persistance
    public void LoadSnapshot(string filename)
    {
        var data = serializer.Deserialize(filename);
        CreateSnapshotFromSaveData(data);
        ApplySnapshot();
    }

    private void CreateSnapshotFromSaveData(SaveData data)
    {
        //create snapshot
    }

    public void SaveSnapshot(string filename)
    {
        var data = GenerateSaveDataFromSnapshot();
        serializer.Serialize(data, filename);
    }

    private SaveData GenerateSaveDataFromSnapshot()
    {
        return new SaveData();
    }
}
