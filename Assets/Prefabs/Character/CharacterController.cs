using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Rigidbody), typeof(CharacterMovement))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private ShadowDetector _shadowDetector;
    [SerializeField] private ParticleSystem _deathParticles;

    [Tooltip("The color the player has when it spawns and do not have any shadow")]
    [SerializeField] private Color _spawnColor;

    [Tooltip("The color the player has when it can be controlled and is inside of a shadow")]
    [SerializeField] private Color _playColor;

    [Header("Optional")]
    [SerializeField] private GameObject _walkTutorial;
    private bool _hasShownWalkTutorial = false;
    
    
    private Vector3 _spawnPosition;

    private bool _spawnModeActivated = true;
    private bool _deathOngoing = false;

    private MeshRenderer _meshRenderer;
    private Outline _outline;
    private Material _material;
    private CharacterMovement _playerMovement;

    private Animator _animator;

    public bool SpawnModeActivated {
        get => _spawnModeActivated;
    }

    private void Awake()
    {

        this._meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        this._outline = this.gameObject.GetComponent<Outline>();
        this._material = this.gameObject.GetComponent<Renderer>().material;
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
        this._playerMovement.UnsetDesiredPosition();
        this.TogglePlayerVisibility(false);
        StartCoroutine(this.Respawn());
    }

   private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.0f);
        this._deathOngoing = false;
        this.gameObject.transform.localPosition = this._spawnPosition;
        this.TogglePlayerVisibility(true);
        if (!this._shadowDetector.IsInsideShadow())  
        {
            this.EnableSpawnMode();
        }
    }

    private void RunDeathEffects()
    {
        this._deathParticles.Play();
    }

    private void TogglePlayerVisibility(bool show)
    {
        this._outline.enabled = show;
        this._meshRenderer.enabled = show;
    }

    private void DisabledSpawnMode()
    {
        this._spawnModeActivated = false;
        SetPlayerColor(this._playColor);
        Debug.Log("Hide Text");
        _animator.ResetTrigger("ShowText");
        _animator.SetTrigger("HideText");
    }

    private void EnableSpawnMode() 
    {
        this._spawnModeActivated = true;
        SetPlayerColor(this._spawnColor);
        Debug.Log("Show Text");
        _animator.ResetTrigger("HideText");
        _animator.SetTrigger("ShowText");
    }

    private void SetPlayerColor(Color color)
    {
        this._outline.OutlineColor = color;
        this._material.color = color;
        this._material.SetColor("_EmissionColor", color * 0.75f);
    }
}
