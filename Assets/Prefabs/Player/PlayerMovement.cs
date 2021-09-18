using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{

    // Walk settings
    [SerializeField] private float _walkingSpeed = 0.75f;
    [SerializeField] private GameObject _walkingMarkerPrefab;

    private bool _shouldWalkToDesiredPosition = false;
    private Vector3 _desiredPosition;
    private Rigidbody _rigidbody;
    private PlayerController _playerController;
    private GameObject _currentWalkingMarker;

    void Awake()
    {
        this._rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        this._playerController = this.gameObject.GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.UnsetDesiredPosition();
    }


    private void FixedUpdate()
    {
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

    public void WalkToPosition(Vector3 position)
    {
        // We should not be able to walk if the player is on spawn mode
        if (this._playerController.SpawnModeActivated) return;
        this._desiredPosition = position;
        this._shouldWalkToDesiredPosition = true;
        this.RemoveCurrentWalkingMarker();
        this.SetWalkingMarker();
        this._playerController.HideWalkTutorial();
    }

    public void UnsetDesiredPosition()
    {
        this._shouldWalkToDesiredPosition = false;
        this._desiredPosition = Vector3.zero;
        this.RemoveCurrentWalkingMarker();
    }

    private void SetWalkingMarker()
    {
        this._currentWalkingMarker = Instantiate(_walkingMarkerPrefab, this._desiredPosition, Quaternion.identity);
    }

    private void RemoveCurrentWalkingMarker()
    {
        Destroy(this._currentWalkingMarker);
    }

}
