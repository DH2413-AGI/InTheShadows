using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinGameUIController : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinGameButton;
    [SerializeField] private TMP_InputField _joinGameInput;

    void Start()
    {
        this._hostButton.onClick.AddListener(this.HostGame);
        this._joinGameButton.onClick.AddListener(this.JoinGame);

        if (LocalStorage.LastIPAdress != null) {
            _joinGameInput.text = LocalStorage.LastIPAdress;
        }
    }

    void HostGame()
    {
        if (NetworkServer.active) return;
        this._networkManager.StartHost();
    }

    void JoinGame()
    {
        if (NetworkClient.active) return;
        string networkAdress = _joinGameInput.text.ToString();
        LocalStorage.LastIPAdress = networkAdress;
        this._networkManager.networkAddress = networkAdress;
        this._networkManager.StartClient();
    }

}
