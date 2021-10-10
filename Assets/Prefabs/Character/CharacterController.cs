using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private float _playerStartHealth = 1.5f;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Animator _heathUIAnimator;

    [Header("Optional")]
    [SerializeField] private GameObject _walkTutorial;

    private bool _hasShownWalkTutorial = false;
    private Vector3 _spawnPosition;
    private bool _spawnModeActivated = true;
    private bool _deathOngoing = false;
    private CharacterMovement _playerMovement;
    private Animator _needShadowAnimator;

    [SyncVar]
    private float _currentHealth = 0;

    public bool SpawnModeActivated {
        get => _spawnModeActivated;
    }

    public float CurrentHealth {
        get => _currentHealth;
    }


    private void Awake()
    {

        this._playerMovement = this.gameObject.GetComponent<CharacterMovement>();
        this._needShadowAnimator = this.gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        this._currentHealth = this._playerStartHealth;
        this._spawnPosition = this.gameObject.transform.localPosition;
        this._shadowDetector.OnEnterShadow += this.OnEnterShadow;
        
        this.EnableSpawnMode();
    }


    private void Update() 
    {
        this.HandlePlayerHealth();
        _healthBar.fillAmount = this._currentHealth / this._playerStartHealth;
        this._heathUIAnimator.SetBool(
            "ShowHealthUI", 
            this._currentHealth < this._playerStartHealth && this._currentHealth > 0.0f
        );
    }

    private void HandlePlayerHealth() 
    {
        if (!isServer) return;
        if (this._spawnModeActivated) return;
        if (!this._shadowDetector.IsInsideShadow()) {
            if (this._currentHealth > 0.0f) {
                this._currentHealth -= Time.deltaTime;
            }
        }
        else {
            if (this._currentHealth < this._playerStartHealth) {
                // The health should regenerate faster than getting hurt 
                this._currentHealth += Time.deltaTime * 2.0f;
            }
        }

        if (this._currentHealth <= 0.0f)
        {
            this.Die();
        }
    }

    private void OnEnterShadow()
    {
        this.DisabledSpawnMode();
        this.ShowWalkTutorial();
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

        this._currentHealth = this._playerStartHealth;
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
        _needShadowAnimator.ResetTrigger("ShowText");
        _needShadowAnimator.SetTrigger("HideText");
    }

    private void EnableSpawnMode() 
    {
        Debug.Log("Enable Spawn Mode");
        Animator spawnCapsuleAnimator = _spawnCapsule.GetComponent<Animator>();
        this._spawnModeActivated = true;
        spawnCapsuleAnimator.ResetTrigger("HideSpawnCapsule");
        spawnCapsuleAnimator.SetTrigger("ShowSpawnCapsule");
        _needShadowAnimator.ResetTrigger("HideText");
        _needShadowAnimator.SetTrigger("ShowText");
    }

    private void SetPlayerColor(Color color)
    {
        //TODO: Add some other effect
        // this._outline.OutlineColor = color;
        // this._material.color = color;
        // this._material.SetColor("_EmissionColor", color * 0.75f);
    }
}
