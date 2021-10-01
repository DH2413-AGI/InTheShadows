using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelManager : NetworkBehaviour
{
    [SyncVar]

    [Header("If active, press enter or two fingers to skip to next level")]
    [SerializeField] private bool _enableLevelSkipMode = false;

    [Header("Level settings")]
    [SerializeField] private int _startLevelIndex = 0;
    [SerializeField] private List<string> _levels;

    public int currentLevelIndex = 0;

    private LevelUIController _levelUIController;

    private Pose _levelSpawnPosition = new Pose();

    public Pose LevelSpawnPosition
    {
        get => _levelSpawnPosition;
    }

    void Start()
    {
        _levelUIController = FindObjectOfType<LevelUIController>();
        currentLevelIndex = _startLevelIndex;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (this._enableLevelSkipMode)
        {
            CheckForSkipInput();
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

    public void UpdateLevelSpawnPosition(Pose pose)
    {
        this._levelSpawnPosition = pose;
    }

    public void LoadNextLevel()
    {
        this.currentLevelIndex++;
        if (this.currentLevelIndex >= this._levels.Count) return;
        LoadLevel(this.currentLevelIndex);
    }

    public void LoadCurrentLevel()
    {
        this.LoadLevel(this.currentLevelIndex);
    }

    public void LoadNextLevelAfterClear()
    {
        _levelUIController.ShowLevelClearText();
        LoadNextLevel();
    }

    [Command(requiresAuthority = false)]
    public void LoadLevel(int levelIndex)
    {
        string levelNameToLoad = this._levels[levelIndex];
        var networkManager = FindObjectOfType<NetworkManager>();
        networkManager.ServerChangeScene(levelNameToLoad);
    }
}
