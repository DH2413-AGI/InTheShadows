using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _startLevelIndex = 0;
    [SerializeField] private List<string> _levels;
    private int _currentLevelIndex = 0;

    private Pose _levelSpawnPosition = new Pose();

    public Pose LevelSpawnPosition {
        get => _levelSpawnPosition;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _currentLevelIndex = _startLevelIndex;
    }

    void Update()
    {
        //OBS! This is temporary, delete this when we have a game logic
        if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began)
        {
            this.LoadNextLevel();
        }
    }

    public void UpdateLevelSpawnPosition(Pose pose)
    {
        this._levelSpawnPosition = pose;
    }

    public void LoadNextLevel()
    {
        this._currentLevelIndex++;
        if (this._currentLevelIndex >= this._levels.Count) return;
        LoadLevel(this._currentLevelIndex);
    }

    public void LoadCurrentLevel()
    {
        this.LoadLevel(this._currentLevelIndex);
    }

    public void LoadLevel(int levelIndex) 
    {
        string levelNameToLoad = this._levels[levelIndex];
        SceneManager.LoadScene(levelNameToLoad);
    }
}
