using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] int currentLevelIndex;

    [SerializeField] List<LevelSO> levels = new List<LevelSO>();

    public LevelSO GetCurrentLevel()
    {
        return levels[currentLevelIndex];
    }

    public void LoadNewLevel()
    {
        currentLevelIndex++;
        SceneManager.LoadScene(0);
    }

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(0);
    }

    public bool IsLastLevel()
    {
        return currentLevelIndex == levels.Count - 1;
    }
}
