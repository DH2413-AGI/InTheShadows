using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelController : MonoBehaviour
{

    private CharacterMovement _characterMovement;
    [SerializeField] private GameObject _characterModel;
    [SerializeField] private float _rotationDamping = 1.0f;

    [SerializeField] private Animator _modelAnimator;

    void Awake()
    {
        this._characterMovement = this.gameObject.GetComponent<CharacterMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this._characterMovement.IsMoving) {
            LookAtWalkingDirection();
            _modelAnimator.SetBool("Walking", true);
        }
        else {
            _modelAnimator.SetBool("Walking", false);
        }
    }

    private void LookAtWalkingDirection()
    {
        Vector3 lookPos = this._characterMovement.DesiredWalkingPosition - this._characterModel.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        this._characterModel.transform.rotation = Quaternion.Slerp(
            this._characterModel.transform.rotation, 
            rotation, 
            Time.deltaTime * _rotationDamping
        );
    }
}
