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
        this.MarkLightPlayerTaken();
        this.HideUI();
    }

    private void JoinAsCharacterPlayer()
    {
        if (this._playerSelectManager.CharacterAlreadyTaken(Character.Character)) return;
        this._playerSelectManager.ChooseCharacter(Character.Character);
        this.MarkCharacterPlayerTaken();
        this.HideUI();
    }

    private void JoinAsInspectorPlayer()
    {
        this._inspectorPlayerButton.interactable = false;
        this._playerSelectManager.ChooseCharacter(Character.Inspector);
        this.HideUI();
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

    private void HideUI()
    {
        this._uiVisuals.SetActive(false);
    }

}
