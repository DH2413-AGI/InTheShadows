using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSelectManager : NetworkBehaviour
{

    private Character _chosenCharacter;

    public Character ChosenCharacter {
        get => _chosenCharacter;
    }

    public SyncList<Character> _charactersTaken = new SyncList<Character>();

    public bool CharacterAlreadyTaken(Character character)
    {
        return _charactersTaken.Contains(character);
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChooseCharacter(Character character)
    {
        this._chosenCharacter = character;
        this.MarkCharacterAsTaken(character);
    }

    [Command(requiresAuthority=false)]
    private void MarkCharacterAsTaken(Character character)
    {
        this._charactersTaken.Add(character);
        this.CheckIfReadyToContinueToNextScene();
    }

    private void CheckIfReadyToContinueToNextScene()
    {
        var networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager.numPlayers == this._charactersTaken.Count) {
            networkManager.ServerChangeScene("LevelPlacement");
        }
    }
}

public enum Character {
    Light,
    Character,
    Inspector,
}