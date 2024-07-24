using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Monier/New Level")]
public class LevelSO : ScriptableObject
{
    public int levelNumber;
    public int initialColletablesToSpawn;
    public int maxCollectablesInLevel;
    public int initialEnemiesToSpawn;
    public int maxEnemiesInLevel;
}
