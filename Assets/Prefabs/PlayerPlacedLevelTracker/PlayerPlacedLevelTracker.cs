using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class PlayerPlacedLevelTracker : NetworkBehaviour
{
    [SyncVar]
    public int NumberOfPlayerPlacedLevel = 0;

    public Action OnStart;

    private NetworkManager _networkManager;

    private LevelManager _levelManager;

    [SerializeField] private GameObject _waitingForPlayersUI;

    void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
        this.OnStart();
        Debug.Log("Player Ready Tracker: Start");
    }

    public void MarkPlayerPlacedLevel()
    {
        this._waitingForPlayersUI.SetActive(true);
        this.CmdMarkPlayerPlacedLevel();
    }

    [Command(requiresAuthority=false)]
    public void CmdMarkPlayerPlacedLevel()
    {
        this.NumberOfPlayerPlacedLevel++;
        bool allPlayersReady = this._networkManager.numPlayers == this.NumberOfPlayerPlacedLevel;
        if (allPlayersReady) {
            FindObjectOfType<LevelManager>().LoadCurrentLevel();
        }
    }

}
