using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerSelectUIController : NetworkBehaviour
{

    [SerializeField] private Button _lightPlayerButton;
    [SerializeField] private GameObject _lightPlayerTakenText;
    [SerializeField] private Button _characterPlayerButton;
    [SerializeField] private GameObject _charecterPlayerTakenText;
    [SerializeField] private Button _inspectorPlayerButton;
    [SerializeField] private GameObject _uiVisuals;

    private PlayerSelectManager _playerSelectManager;

    void Start()
    {
        _lightPlayerButton.onClick.AddListener(this.JoinAsLightPlayer);
        _characterPlayerButton.onClick.AddListener(this.JoinAsCharacterPlayer);
        _inspectorPlayerButton.onClick.AddListener(this.JoinAsInspectorPlayer);
        _playerSelectManager = FindObjectOfType<PlayerSelectManager>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        this.LookForTakenPlayers();
    }

    private void JoinAsLightPlayer()
    {
        if (this._playerSelectManager.CharacterAlreadyTaken(Character.Light)) return;
        this._playerSelectManager.ChooseCharacter(Character.Light);
        this.ToggleLightPlayerTaken(true);
        this.HideUI();
    }

    private void JoinAsCharacterPlayer()
    {
        if (this._playerSelectManager.CharacterAlreadyTaken(Character.Character)) return;
        this._playerSelectManager.ChooseCharacter(Character.Character);
        this.ToggleCharacterPlayerTaken(true);
        this.HideUI();
    }

    private void JoinAsInspectorPlayer()
    {
        this._inspectorPlayerButton.interactable = false;
        this._playerSelectManager.ChooseCharacter(Character.Inspector);
        this.HideUI();
    }

    private void ToggleLightPlayerTaken(bool isTaken)
    {
        this._lightPlayerButton.interactable = !isTaken;
        this._lightPlayerTakenText.SetActive(isTaken);
    }

    private void ToggleCharacterPlayerTaken(bool isTaken)
    {
        this._characterPlayerButton.interactable = !isTaken;
        this._charecterPlayerTakenText.SetActive(isTaken);
    }

    void LookForTakenPlayers()
    {
        this.ToggleCharacterPlayerTaken(this._playerSelectManager.CharacterAlreadyTaken(Character.Character));
        this.ToggleLightPlayerTaken(this._playerSelectManager.CharacterAlreadyTaken(Character.Light));
    }

    private void HideUI()
    {
        this._uiVisuals.SetActive(false);
    }

}
