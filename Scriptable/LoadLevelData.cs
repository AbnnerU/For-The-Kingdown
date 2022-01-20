using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Load Data", menuName = "Asset/Level Load Data")]
public class LoadLevelData : ScriptableObject
{
    public string levelToLoad;
}