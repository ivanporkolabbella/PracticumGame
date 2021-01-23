using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{

}

public interface ISerializationManager
{
    void Serialize(SaveData data, string filename);
    SaveData Deserialize(string filename);
}


public class MockSerializationManager : ISerializationManager
{
    public SaveData Deserialize(string filename)
    {
        //do the deserialization
        return new SaveData();
    }

    public void Serialize(SaveData data, string filename)
    {
       
    }
}

public class BinarySerializationManager : ISerializationManager
{
    public SaveData Deserialize(string filename)
    {
        //do the deserialization
        return new SaveData();
    }

    public void Serialize(SaveData data, string filename)
    {

    }
}
