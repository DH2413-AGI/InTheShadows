using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = this.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                _playerController.WalkToPosition(hit.point);
            }
        }
    }
}
