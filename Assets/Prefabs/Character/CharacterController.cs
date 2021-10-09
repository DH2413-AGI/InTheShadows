using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using Mirror;

[RequireComponent(typeof(Rigidbody), typeof(CharacterMovement))]
public class CharacterController : NetworkBehaviour
{
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private ShadowDetector _shadowDetector;
    [SerializeField] private ParticleSystem _deathParticles;

    [SerializeField] private GameObject _spawnCapsule;

    [Header("Optional")]
    [SerializeField] private GameObject _walkTutorial;
    private bool _hasShownWalkTutorial = false;
    
    
    private Vector3 _spawnPosition;

    private bool _spawnModeActivated = true;
    private bool _deathOngoing = false;

    // private MeshRenderer _meshRenderer;
    // private Outline _outline;
    // private Material _material;
    private CharacterMovement _playerMovement;


    private Animator _animator;

    public bool SpawnModeActivated {
        get => _spawnModeActivated;
    }

    private void Awake()
    {

        // this._meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        // this._outline = this.gameObject.GetComponent<Outline>();
        // this._material = this.gameObject.GetComponent<Renderer>().material;
        this._playerMovement = this.gameObject.GetComponent<CharacterMovement>();
        this._animator = this.gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        this._spawnPosition = this.gameObject.transform.localPosition;
        _shadowDetector.OnLeavingShadow += this.Die;
        _shadowDetector.OnEnterShadow += this.DisabledSpawnMode;
        _shadowDetector.OnEnterShadow += this.ShowWalkTutorial;
        
        this.EnableSpawnMode();
    }


    private void Update() 
    {
    }

    private void ShowWalkTutorial()
    {
        if(this._hasShownWalkTutorial || this._walkTutorial == null) return;
        this._walkTutorial.SetActive(true);
        this._hasShownWalkTutorial = true;
    }

    public void HideWalkTutorial()
    {
        if(this._walkTutorial == null) return;
        this._walkTutorial.SetActive(false);
    }

    public void Die()
    {
        if (_deathOngoing) return;
        this._deathOngoing = true;

        this.RunDeathEffects();
        this.TogglePlayerVisibility(false);
        StartCoroutine(this.Respawn());

        if (isServer) 
        {
            this._playerMovement.UnsetDesiredPosition();
        }
    }

   private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.0f);
        this._deathOngoing = false;
        this.TogglePlayerVisibility(true);
        if (!this._shadowDetector.IsInsideShadow())  
        {
            this.EnableSpawnMode();
        }

        if (isServer) 
        {
            this.gameObject.transform.localPosition = this._spawnPosition;
        }
    }

    private void RunDeathEffects()
    {
        this._deathParticles.Play();
    }

    private void TogglePlayerVisibility(bool show)
    {
        this._playerModel.SetActive(show);
    }

    private void DisabledSpawnMode()
    {
        Debug.Log("Disable Spawn Mode");
        this._spawnModeActivated = false;
        Animator spawnCapsuleAnimator = _spawnCapsule.GetComponent<Animator>();
        spawnCapsuleAnimator.ResetTrigger("ShowSpawnCapsule");
        spawnCapsuleAnimator.SetTrigger("HideSpawnCapsule");
        _animator.ResetTrigger("ShowText");
        _animator.SetTrigger("HideText");
    }

    private void EnableSpawnMode() 
    {
        Debug.Log("Enable Spawn Mode");
        Animator spawnCapsuleAnimator = _spawnCapsule.GetComponent<Animator>();
        this._spawnModeActivated = true;
        spawnCapsuleAnimator.ResetTrigger("HideSpawnCapsule");
        spawnCapsuleAnimator.SetTrigger("ShowSpawnCapsule");
        _animator.ResetTrigger("HideText");
        _animator.SetTrigger("ShowText");
    }

    private void SetPlayerColor(Color color)
    {
        //TODO: Add some other effect
        // this._outline.OutlineColor = color;
        // this._material.color = color;
        // this._material.SetColor("_EmissionColor", color * 0.75f);
    }
}
