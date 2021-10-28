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
        Debug.Log("PlayerSelectManager");
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
    }

}

public enum Character {
    Light,
    Character,
    Inspector,
}