using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InitialNetworkSetupManager : NetworkBehaviour
{
    [SerializeField] private PlayerSelectManager _playerSelectManagerPrefab;
    [SerializeField] private PlayerSelectUIController _playerSelectUIControllerPrefab;
    [SerializeField] private LevelManager _levelManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        var playerSelectManager = Instantiate(_playerSelectManagerPrefab, Vector3.zero, Quaternion.identity);
        var playerSelectUIController = Instantiate(_playerSelectUIControllerPrefab, Vector3.zero, Quaternion.identity);
        var levelManager = Instantiate(_levelManagerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(playerSelectManager.gameObject);
        NetworkServer.Spawn(playerSelectUIController.gameObject);
        NetworkServer.Spawn(levelManager.gameObject);

        levelManager.LoadCurrentLevel();
    }
}
