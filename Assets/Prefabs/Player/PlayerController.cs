using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ShadowDetector))]
public class PlayerController : MonoBehaviour
{
    private Vector3 _spawnPosition;
    private ShadowDetector _shadowDetector;
    private Rigidbody _rigidbody;


    // Walk settings
    private float _walkingSpeed = 0.75f;
    private bool _shouldWalkToDesiredPosition = false;
    private Vector3 _desiredPosition;
    [SerializeField] private AnimationCurve _startWalkingAcceleration = AnimationCurve.Constant(0, 5.0f, 1.0f);

    private void Awake()
    {
        this._shadowDetector = this.gameObject.GetComponent<ShadowDetector>();
        this._rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }


    private void Start()
    {
        this._spawnPosition = this.gameObject.transform.localPosition;
        _shadowDetector.OnLeavingShadow += this.Die;
    }

    private void Update() 
    {
    }

    private void FixedUpdate()
    {
        if (this._shouldWalkToDesiredPosition) this.MoveTowradsDesiredPosition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.UnsetDesiredPosition();
    }

    public void WalkToPosition(Vector3 position)
    {
        this._desiredPosition = position;
        this._shouldWalkToDesiredPosition = true;
    }

    private void UnsetDesiredPosition()
    {
        this._shouldWalkToDesiredPosition = false;
        this._desiredPosition = Vector3.zero;
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

    public void Die()
    {
        this.UnsetDesiredPosition();
        this.Respawn();
    }

    private void Respawn()
    {
        this.gameObject.transform.localPosition = this._spawnPosition;
    }
}
