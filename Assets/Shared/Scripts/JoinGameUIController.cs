using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

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
    }

    void HostGame()
    {
        this._networkManager.StartHost();
    }

    void JoinGame()
    {
        string networkAdress = _joinGameInput.text.ToString();
        Debug.Log(networkAdress);
        this._networkManager.networkAddress = networkAdress;
        this._networkManager.StartClient();
    }

}