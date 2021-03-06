
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerRepresentator : NetworkBehaviour
{
    private PlayerSelectManager _playerSelectManager;
    private LevelPositionManager _levelPositionManager;

    [SerializeField] private GameObject _visuals;
    [SerializeField] private Vector3 _visualsOffset;
    [SerializeField] private Character PlayerToFollow;

    // Start is called before the first frame update
    void Start()
    {
        this._playerSelectManager = FindObjectOfType<PlayerSelectManager>();
        this._levelPositionManager = FindObjectOfType<LevelPositionManager>();

        this.gameObject.transform.SetPositionAndRotation(
            this._levelPositionManager.LevelSpawnPosition.position,
            this._levelPositionManager.LevelSpawnPosition.rotation
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkClient.ready) return;
        _visuals.SetActive(_playerSelectManager.ChosenCharacter != PlayerToFollow);

        if (_playerSelectManager.ChosenCharacter != PlayerToFollow) return;
        if (Camera.main == null) return;
        
        UpdatePosition(
            Camera.main.transform.position - this._levelPositionManager.LevelSpawnPosition.position,
            Camera.main.transform.rotation,
            Quaternion.Inverse(this._levelPositionManager.LevelSpawnPosition.rotation)
        );
    }

    [Command(requiresAuthority=false)]
    void UpdatePosition (Vector3 position, Quaternion localRotation, Quaternion globalRotation)
    {
        this.gameObject.transform.localPosition = globalRotation * position + _visualsOffset;
        this.gameObject.transform.localRotation = globalRotation * localRotation;
    }

}
 
