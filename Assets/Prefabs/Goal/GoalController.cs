using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GoalController : NetworkBehaviour
{
    private LevelManager _levelManager;

    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        bool isPlayer = other.gameObject.GetComponent<CharacterController>() != null;
        this._levelManager.LoadNextLevel();
    }
}
