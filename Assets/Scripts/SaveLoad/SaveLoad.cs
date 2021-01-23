using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadable
{
    Dictionary<string, object> GetData();
    void ApplyData(Dictionary<string, object> data);
    int ObjectID { get; }
}

public class SaveLoad : MonoBehaviour, ISaveLoadable
{
    public int ObjectID { get; private set; } = SaveLoadManager.AutogenerateID();
    private Dictionary<string, object> data = new Dictionary<string, object>();

    private struct Params
    {
        public static string id = "id";
        public static string position = "position";
        public static string rotation = "rotation";
        public static string scale = "scale";
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveLoadManager.RegisterObject(this);
    }

    public virtual void ApplyData(Dictionary<string, object> data)
    {
        transform.position = (Vector3)data[Params.position];
        transform.rotation = (Quaternion)data[Params.rotation];
        transform.localScale = (Vector3)data[Params.scale];
    }

    public virtual Dictionary<string, object> GetData()
    {
        data.Clear();
        data.Add(Params.id, ObjectID);
        data.Add(Params.position, transform.position);
        data.Add(Params.rotation, transform.rotation);
        data.Add(Params.scale, transform.localScale);

        return data;
    }
}
