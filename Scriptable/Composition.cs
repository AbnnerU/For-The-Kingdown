using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Composition", menuName = "Asset/New team composition")]
public class Composition : ScriptableObject
{
    public float combatExecutionInterval;
    public float pushaseExecutionInterval;

    [Header("Atack")]
    [Range(10, 100)]
    public int agressiveValue;
    public int minUnitysToStartAtack;
    public int minUnityToKeepAtacking;
    public int acceptableEnemyUnitysDifference;

    [Header("Defend")]
    public int minUnityToDefend;

    //public int maxUnitysBuyPerCicle;
    

    [Header("Miners")]
    [Range(0, 4)]
    public int maxMiners;
    [Range(0, 100)]
    public int buyNewMinerChance;
    public int maxTryBuyMinerTentatives;   
    public int maxMinersBuyPerCicle;

    [Header("Unitys")]
    [Range(0, 100)]
    public int buyNewUnityChance;
    public int maxTryBuyUnityTentatives;
    public int maxUnityBuyPerCicle;

    [Header("Composition")]
    public int unitysAmount;
    public List<UnityAmountInComposition> finalComposition;

    [Header("BuyPriority")]
    public List<UnityPrefab> buyPriority;
}

[System.Serializable]
public struct UnityAmountInComposition
{
    public UnityName unity;
    public int amount;
}

[System.Serializable]
public struct UnityPrefab
{
    public UnityName unity;
    public GameObject prefab;
}

public enum UnityName
{
    WARRIOR,
    TANK,
    RANGE,
    HEALER,
    //MINER
}
