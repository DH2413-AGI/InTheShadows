using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LightSource : NetworkBehaviour
{

    private PlayerSelectManager _playerSelectManager;
    private LevelManager _levelManager;
    private GameObject _followTarget;

    [SerializeField] private GameObject _visuals;
    [SerializeField] private Vector3 _visualsOffset;

    // Start is called before the first frame update
    void Start()
    {
        this._playerSelectManager = FindObjectOfType<PlayerSelectManager>();
        this._levelManager = FindObjectOfType<LevelManager>();

        this.gameObject.transform.SetPositionAndRotation(
            this._levelManager.LevelSpawnPosition.position,
            this._levelManager.LevelSpawnPosition.rotation
        );

        if (_playerSelectManager.ChosenCharacter == Character.Light) 
        {
            _visuals.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_playerSelectManager.ChosenCharacter != Character.Light) return;
        if (Camera.main == null) return;
        
        UpdatePosition(
            Camera.main.transform.position - this._levelManager.LevelSpawnPosition.position,
            Camera.main.transform.rotation,
            Quaternion.Inverse(this._levelManager.LevelSpawnPosition.rotation)
        );
        Debug.Log(Camera.main.transform.rotation);
    }

    [Command(requiresAuthority=false)]
    void UpdatePosition (Vector3 position, Quaternion localRotation, Quaternion globalRotation)
    {
        this.gameObject.transform.localPosition = globalRotation * position + _visualsOffset;
        this.gameObject.transform.localRotation = globalRotation * localRotation;
    }

}
