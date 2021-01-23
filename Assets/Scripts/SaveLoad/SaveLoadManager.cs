using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager
{
    private static List<ISaveLoadable> objectList = new List<ISaveLoadable>();
    private static int IDCount = 0;

    private static Dictionary<int, Dictionary<string, object>> snapshot = new Dictionary<int, Dictionary<string, object>>();

    public static int AutogenerateID()
    {
        return IDCount++;
    }

    public static void RegisterObject(ISaveLoadable newObject)
    {
        objectList.Add(newObject);
    }

    public static void DeregisterObject(ISaveLoadable objectToRemove)
    {
        objectList.Remove(objectToRemove);
    }

    public static void CleanAll()
    {
        objectList.Clear();
    }

    public static void CreateSnapshot()
    {
        snapshot.Clear();

        foreach (var element in objectList)
        {
            snapshot.Add(element.ObjectID, element.GetData());
        }
    }

    public static void ApplySnapshot()
    {
        foreach (var element in objectList)
        {
            element.ApplyData(snapshot[element.ObjectID]);
        }
    }


}
