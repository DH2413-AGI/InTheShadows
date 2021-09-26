using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerSelectUIController : MonoBehaviour
{

    [SerializeField] private Button _lightPlayerButton;
    [SerializeField] private GameObject _lightPlayerTakenText;
    [SerializeField] private Button _characterPlayerButton;
    [SerializeField] private GameObject _charecterPlayerTakenText;
    [SerializeField] private Button _inspectorPlayerButton;

    [SerializeField] private PlayerSelectManager _playerSelectManager;
    [SerializeField] private GameObject _waitingForPlayersUI;

    void Start()
    {
        _lightPlayerButton.onClick.AddListener(this.JoinAsLightPlayer);
        _characterPlayerButton.onClick.AddListener(this.JoinAsCharacterPlayer);
        _inspectorPlayerButton.onClick.AddListener(this.JoinAsInspectorPlayer);
    }

    private void JoinAsLightPlayer()
    {
        this._playerSelectManager.ChooseCharacter(Character.Light);
        this.MarkLightPlayerTaken();
        this.ShowWaitingUI();
    }

    private void JoinAsCharacterPlayer()
    {
        this._playerSelectManager.ChooseCharacter(Character.Character);
        this.MarkCharacterPlayerTaken();
        this.ShowWaitingUI();
    }

    private void JoinAsInspectorPlayer()
    {
        this._inspectorPlayerButton.interactable = false;
        this._playerSelectManager.ChooseCharacter(Character.Inspector);
        this.ShowWaitingUI();
    }

    private void MarkLightPlayerTaken()
    {
        this._lightPlayerButton.interactable = false;
        this._lightPlayerTakenText.SetActive(true);
    }

    private void MarkCharacterPlayerTaken()
    {
        this._characterPlayerButton.interactable = false;
        this._charecterPlayerTakenText.SetActive(true);
    }

    void Update()
    {
        this.LookForTakenPlayers();
    }

    void LookForTakenPlayers()
    {
        if (this._playerSelectManager.CharacterAlreadyTaken(Character.Light)) 
        {
            this.MarkLightPlayerTaken();
        }
        if (this._playerSelectManager.CharacterAlreadyTaken(Character.Character)) 
        {
            this.MarkCharacterPlayerTaken();
        }
    }

    private void ShowWaitingUI()
    {
        this._waitingForPlayersUI.SetActive(true);
    }

}
