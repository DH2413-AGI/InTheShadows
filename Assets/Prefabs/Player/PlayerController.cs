using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ShadowDetector))]
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _playerNavMeshAgent;
    private Vector3 _spawnPosition;
    private ShadowDetector _shadowDetector;

    private void Awake()
    {
        this._shadowDetector = this.gameObject.GetComponent<ShadowDetector>();
        this._playerNavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        this._spawnPosition = this.gameObject.transform.localPosition;
        _shadowDetector.OnLeavingShadow += this.Die;
    }

    public void WalkToPosition(Vector3 position)
    {
        this._playerNavMeshAgent.SetDestination(position);
    }

    public void Die()
    {
        this.Respawn();
    }

    private void Respawn()
    {
        this._playerNavMeshAgent.ResetPath();
        this.gameObject.transform.position = this._spawnPosition;
    }
}
