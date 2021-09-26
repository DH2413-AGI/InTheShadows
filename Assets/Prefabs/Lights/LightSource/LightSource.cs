using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LightSource : NetworkBehaviour
{

    private PlayerSelectManager _playerSelectManager;
    private LevelManager _levelManager;
    private GameObject _followTarget;

    // Start is called before the first frame update
    void Start()
    {
        this._playerSelectManager = FindObjectOfType<PlayerSelectManager>();
        this._levelManager = FindObjectOfType<LevelManager>();

        this.gameObject.transform.SetPositionAndRotation(
            this._levelManager.LevelSpawnPosition.position,
            this._levelManager.LevelSpawnPosition.rotation
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerSelectManager.ChosenCharacter != Character.Light) return;
        if (Camera.main == null) return;
        
        UpdatePosition(
            Camera.main.transform.position - this._levelManager.LevelSpawnPosition.position,
            Camera.main.transform.rotation * this._levelManager.LevelSpawnPosition.rotation
        );

        Debug.Log(this.gameObject.transform.localPosition);
    }

    [Command(requiresAuthority=false)]
    void UpdatePosition (Vector3 position, Quaternion rotation)
    {
        this.gameObject.transform.localPosition = position;
        this.gameObject.transform.localRotation = rotation;
    }

}