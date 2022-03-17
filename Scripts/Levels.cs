using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "all levels", menuName = "Custom/AllLevels")]
public class Levels : ScriptableObject
{
    [SerializeField] private List<LevelData> allLevels;
    public LevelData GetLevel(int i)
    {
        if(i < allLevels.Count)
        {
            return allLevels[i];
        }
        else
        {
            return allLevels[0];
        }
    }
}
