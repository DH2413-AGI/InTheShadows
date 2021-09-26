using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterMovementControlls : MonoBehaviour
{
    private CharacterMovement _playerMovement;
    private LevelManager _levelManager;
    private PlayerSelectManager _playerSelectManager;

    [Header("Debug")]
    [SerializeField] private bool _allowLightToControllCharacter = false;

    private void Awake()
    {
        _playerMovement = this.gameObject.GetComponent<CharacterMovement>();
    }

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _playerSelectManager = FindObjectOfType<PlayerSelectManager>();
    }

    private void Update()
    {
        bool userAllowedToMoveCharacter = 
            (this._playerSelectManager.ChosenCharacter == Character.Character) || 
            (_allowLightToControllCharacter && this._playerSelectManager.ChosenCharacter == Character.Light);
        if (!userAllowedToMoveCharacter) return;

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            int groundLayerMask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayerMask))
            {
                Vector3 rawHitPoint = hit.point;
                Vector3 hitPointRelative = Quaternion.Inverse(_levelManager.LevelSpawnPosition.rotation) * (hit.point - _levelManager.LevelSpawnPosition.position);
                _playerMovement.WalkToPosition(hitPointRelative);
            }
        }
    }
}
