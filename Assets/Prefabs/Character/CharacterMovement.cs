using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody), typeof(CharacterController))]
public class CharacterMovement : NetworkBehaviour
{

    // Walk settings
    [SerializeField] private float _walkingSpeed = 0.75f;
    [SerializeField] private GameObject _walkingMarkerPrefab;

    private bool _shouldWalkToDesiredPosition = false;
    private Rigidbody _rigidbody;
    private CharacterController _playerController;
    private GameObject _currentWalkingMarker;

    [SyncVar]
    private bool _isCurrentlyMoving = false;

    [SyncVar]
    private Vector3 _desiredPosition;

    void Awake()
    {
        this._rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        this._playerController = this.gameObject.GetComponent<CharacterController>();
    }

    public Vector3 DesiredWalkingPosition {
        get => this._desiredPosition;
    }

    public bool IsMoving {
        get => this._isCurrentlyMoving;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;
        this.UnsetDesiredPosition();
    }


    private void FixedUpdate()
    {
        if (!isServer) return;
        if (this._shouldWalkToDesiredPosition) this.MoveTowradsDesiredPosition();
    }

    private void MoveTowradsDesiredPosition()
    {
        Vector3 desiredPositionReachable = new Vector3(this._desiredPosition.x, this.gameObject.transform.position.y, this._desiredPosition.z);
        float distanceToDesiredPosition = Vector3.Distance(this.gameObject.transform.position, desiredPositionReachable);

        this._rigidbody.MovePosition(Vector3.MoveTowards(
            this.gameObject.transform.position, 
            desiredPositionReachable, 
            this._walkingSpeed * Time.deltaTime
        ));

        if (distanceToDesiredPosition < 0.1f) this.UnsetDesiredPosition();
    }


    [Command(requiresAuthority=false)]
    public void WalkToPosition(Vector3 position)
    {
        // We should not be able to walk if the player is on spawn mode
        if (this._playerController.SpawnModeActivated) return;
        this._desiredPosition = position;
        this._shouldWalkToDesiredPosition = true;
        this._isCurrentlyMoving = true;
        this.RemoveCurrentWalkingMarker();
        this.SetWalkingMarker(position);
        this._playerController.HideWalkTutorial();
    }

    [Command(requiresAuthority=false)]
    public void UnsetDesiredPosition()
    {
        this._shouldWalkToDesiredPosition = false;
        this._isCurrentlyMoving = false;
        this._desiredPosition = Vector3.zero;
        this.RemoveCurrentWalkingMarker();
    }

    [ClientRpc]
    private void SetWalkingMarker(Vector3 position)
    {
        this._currentWalkingMarker = Instantiate(
            _walkingMarkerPrefab, 
            Vector3.zero, 
            Quaternion.identity,
            FindObjectOfType<LevelPositioner>().gameObject.transform
        );

        this._currentWalkingMarker.transform.localPosition = position;
    }

    [ClientRpc]
    private void RemoveCurrentWalkingMarker()
    {
        Destroy(this._currentWalkingMarker);
    }

}
