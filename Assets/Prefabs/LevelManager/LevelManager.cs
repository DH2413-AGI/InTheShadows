using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelManager : NetworkBehaviour
{
    [Header("If active, press enter or two fingers to skip to next level")]
    [SerializeField] private bool _enableLevelSkipMode = false;
    [SerializeField] private AudioSource _audioSource;

    [Header("If active, press tab to reset the levels")]
    [SerializeField] private bool _enableResetMode = false;

    [Header("Level settings")]
    [SerializeField] private int _startLevelIndex = 0;
    [SerializeField] private List<string> _levels;
    int currentLevelIndex = 0;

    private LevelUIController _levelUIController;



    void Start()
    {
        Debug.Log("LevelManager Start!");
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
        if (this._enableResetMode)
        {
            CheckForResetInput();
        }
    }

    public void CheckForSkipInput()
    {
        if (!isServer) return;
        bool mobileSkip = Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began;
        bool desktopSkip = Input.GetKeyDown(KeyCode.Return);
        if (mobileSkip | desktopSkip)
        {
            Debug.Log("Load next level");
            this.LoadNextLevelAfterClear();
        }
    }

    public void CheckForResetInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isServer)
        {
            this.ResetLevels();
        }
    }

    public void LoadNextLevel()
    {
        this.currentLevelIndex++;
        if (this.currentLevelIndex >= this._levels.Count) return;
        LoadLevel(this.currentLevelIndex);
    }

    public void ResetLevels()
    {
        currentLevelIndex = _startLevelIndex;
        this.LoadLevel(this.currentLevelIndex);
    }

    public void LoadCurrentLevel()
    {
        this.LoadLevel(this.currentLevelIndex);
    }

    public void LoadNextLevelAfterClear()
    {
        PlayWinSound();
        ShowLevelClearText();
        LoadNextLevel();
    }

    [ClientRpc]
    private void ShowLevelClearText()
    {
        _levelUIController.ShowLevelClearText();
    }

    [ClientRpc]
    private void PlayWinSound()
    {
        _audioSource.Play();
    }

    [ClientRpc]
    private void UpdateLevelIndicator(int levelIndex)
    {
        _levelUIController.UpdateLevelIndicator(levelIndex);
    }

    [Command(requiresAuthority = false)]
    public void LoadLevel(int levelIndex)
    {
        UpdateLevelIndicator(levelIndex);
        string levelNameToLoad = this._levels[levelIndex];
        var networkManager = FindObjectOfType<NetworkManager>();
        networkManager.ServerChangeScene(levelNameToLoad);
    }
}
