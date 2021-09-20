using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private LevelManager _levelManager;

    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.gameObject.GetComponent<PlayerController>() != null;
        this._levelManager.LoadNextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
