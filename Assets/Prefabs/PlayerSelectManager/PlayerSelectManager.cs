using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerSelectManager : NetworkBehaviour
{

    private Character _chosenCharacter;

    public Character ChosenCharacter {
        get => _chosenCharacter;
    }

    private SyncList<PlayerSelection> _charactersTaken = new SyncList<PlayerSelection>();

    public bool CharacterAlreadyTaken(Character character)
    {
        return _charactersTaken.Find((playerSelect => (Character)playerSelect.character == character)) != null;
    }

    void Start()
    {
        _chosenCharacter = Character.Inspector;
        DontDestroyOnLoad(this.gameObject);
        InvokeRepeating(nameof(PrintPlayerData), 0.0f, 1.0f);
    }

    void Update() {
        if (!isServer) return;
        this.CheckForOfflinePlayers();
    }

    private void PrintPlayerData() {
        Debug.Log("---SELECTED CHARACTERS---");
        foreach (var playerSelection in _charactersTaken)
        {
            Debug.Log("ID: " + playerSelection.connectionId);
            Debug.Log("Character: " + playerSelection.character.ToString());
        }

        Debug.Log("---CONNECTED PLAYERS---");
        foreach (var connection in NetworkServer.connections)
        {
            Debug.Log("ID: " + connection.Value.connectionId);
        }
    }
    
    private void CheckForOfflinePlayers()
    {
        foreach (var playerSelection in _charactersTaken)
        {
            if (PlayerIsDisconnected(playerSelection)) {
                this._charactersTaken.RemoveAt(
                    this._charactersTaken.FindIndex(
                        playerSelectCheck => playerSelectCheck.connectionId == playerSelection.connectionId
                    )
                );
            }
        }
    }

    private bool PlayerIsDisconnected(PlayerSelection playerSelection)
    {
        if (!isServer) throw new InvalidOperationException("This function can not be called from the client");
        foreach (var connection in NetworkServer.connections)
        {
            if (playerSelection.connectionId == connection.Value.connectionId) {
                return false;
            }
        }
        return true;
    }

    public void ChooseCharacter(Character character)
    {
        this._chosenCharacter = character;
        this.MarkCharacterAsTaken((int)character);
    }

    [Command(requiresAuthority=false)]
    private void MarkCharacterAsTaken(int character, NetworkConnectionToClient sender = null)
    {
        Debug.Log((Character)character);
        this._charactersTaken.Add(new PlayerSelection {
            character = character,
            connectionId = sender.connectionId,
        });
    }

}

public class PlayerSelection {
    public int character;
    public int connectionId;
}

public enum Character {
    Character,
    Light,
    Inspector,
}