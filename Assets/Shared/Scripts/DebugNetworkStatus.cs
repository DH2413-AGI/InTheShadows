using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class DebugNetworkStatus : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _connectionStatusGUI;
    [SerializeField] TextMeshProUGUI _numberOfPlayersGUI;
    [SerializeField] NetworkManager _networkManager;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);    
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateStatusLabels();
    }

    void UpdateStatusLabels()
    {
        if (NetworkServer.active && NetworkClient.active)
        {
            _connectionStatusGUI.SetText($"<b>Host</b>: running via {Transport.activeTransport}");
        }
        // client only
        else if (NetworkClient.isConnected)
        {
            _connectionStatusGUI.SetText($"<b>Client</b>: connected to {_networkManager.networkAddress} via {Transport.activeTransport}");
        }

        _numberOfPlayersGUI.SetText($"Number of players: {_networkManager.numPlayers}");
    }
}
