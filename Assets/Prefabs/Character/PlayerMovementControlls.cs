using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerMovementControlls : MonoBehaviour
{
    private CharacterMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = this.gameObject.GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            int groundLayerMask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayerMask))
            {
                _playerMovement.WalkToPosition(hit.point);
            }
        }
    }
}
