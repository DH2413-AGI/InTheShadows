using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelManager : NetworkBehaviour
{
    [Header("If active, press enter or two fingers to skip to next level")]
    [SerializeField] private bool _enableLevelSkipMode = false;

    [Header("If active, press tab to reset the levels")]
    [SerializeField] private bool _enableResetMode = false;

    [Header("Level settings")]
    [SerializeField] private int _startLevelIndex = 0;
    [SerializeField] private List<string> _levels;

    private int _currentLevelIndex = 0;

    private Pose _levelSpawnPosition = new Pose();


    public Pose LevelSpawnPosition
    {
        get => _levelSpawnPosition;
    }

    void Start()
    {
        _currentLevelIndex = _startLevelIndex;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (this._enableLevelSkipMode)
        {
            CheckForSkipInput();
        }
        if (this._enableResetMode)
        {
            CheckForResetInput();
        }
    }

    public void CheckForSkipInput()
    {
        bool mobileSkip = Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began;
        bool desktopSkip = Input.GetKeyDown(KeyCode.Return);
        if (mobileSkip | desktopSkip)
        {
            this.LoadNextLevel();
        }
    }

    public void CheckForResetInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isServer)
        {
            this.ResetLevels();
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

    public void ResetLevels()
    {
        _currentLevelIndex = _startLevelIndex;
        this.LoadLevel(this._currentLevelIndex);
    }

    public void LoadCurrentLevel()
    {
        this.LoadLevel(this._currentLevelIndex);
    }

    [Command(requiresAuthority = false)]
    public void LoadLevel(int levelIndex)
    {
        string levelNameToLoad = this._levels[levelIndex];
        var networkManager = FindObjectOfType<NetworkManager>();
        networkManager.ServerChangeScene(levelNameToLoad);
    }
}
