﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerController : MonoBehaviour
{

    private DelayedExecutionTicket ticket;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        //spawn...
        var randomEnemyType = HelperFunctions.RandomEnumElement<EnemyType>();

        var enemy = EnemyProvider.GetEnemy(randomEnemyType);

        //keep spawning...
        ticket = DelayedExecutionManager.ExecuteActionAfterDelay(3000, () => {
            Spawn();
        });

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        DelayedExecutionManager.CancelTicket(ticket);
    }

    private void OnDestroy()
    {
        DelayedExecutionManager.CancelTicket(ticket);
    }
}

public class EnemyProvider
{
    public static GameObject GetEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Footman:
                return AssetProvider.GetAsset(GameAsset.Footman);
            case EnemyType.Archer:
                return AssetProvider.GetAsset(GameAsset.Archer);
        }

        return null;
    }
}

public enum EnemyType
{
    Footman, Archer
}

public static class HelperFunctions
{
    public static System.Random randomizer = new System.Random();

    public static T RandomEnumElement<T>()
    {
        var values = Enum.GetValues(typeof(T));
        var randomIndex = randomizer.Next(values.Length);

        T randomEnumValue = (T)values.GetValue(randomIndex);

        return randomEnumValue;
    }
}