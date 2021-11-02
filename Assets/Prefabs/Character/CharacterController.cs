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
    private bool _lockModeActivated = true;
    private bool _deathOngoing = false;
    private CharacterMovement _playerMovement;
    private Animator _needShadowAnimator;

    [SerializeField] private List<LightSensor> _lightSensorsRequired;

    [SyncVar]
    private float _currentHealth = 0;

    public bool SpawnModeActivated {
        get => _spawnModeActivated;
    }

    public bool PlayerCanWalk {
        get => !_spawnModeActivated && !_lockModeActivated;
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
        this._spawnPosition = this.gameObject.transform.localPosition;
        
        if (isServer) {
            this._currentHealth = this._playerStartHealth;
            this._shadowDetector.OnEnterShadow += this.OnEnterShadow;
            this.RpcEnableSpawnMode();
        }
    }


    private void Update() 
    {
        this.HandlePlayerHealth();

        _healthBar.fillAmount = this._currentHealth / this._playerStartHealth;
        this._heathUIAnimator.SetBool(
            "ShowHealthUI",
            this._currentHealth < this._playerStartHealth && this._currentHealth > 0.0f
        );

        this.CheckLightSensors();
    }

    private void CheckLightSensors()
    {
        if (!isServer) return;
        if (!this._shadowDetector.IsInsideShadow()) return;
        
        if (!AtLeastOneLightSensorActive() && !_lockModeActivated) {
            ToggleLockMode(true);
        }
        if (AtLeastOneLightSensorActive() && _lockModeActivated) {
            ToggleLockMode(false);
        }
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

        if (this._currentHealth <= 0.0f && !_deathOngoing)
        {
            this.CommandDie();
        }
    }

    private void OnEnterShadow()
    {
        this.RpcDisabledSpawnMode();
        this.ShowWalkTutorial();
    }

    private bool AtLeastOneLightSensorActive()
    {
        // If we have no light sensor connected to this player, always return true
        if (this._lightSensorsRequired.Count == 0) return true;

        foreach (var lightSensor in this._lightSensorsRequired)
        {
            Debug.Log("Light Sensor Has Light: " + lightSensor.HasLight);
            if (lightSensor.HasLight) return true;
        }
        return false;
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

    [Command(requiresAuthority = false)]
    public void CommandDie()
    {
        if (_deathOngoing) return;
        this._deathOngoing = true;

        this.RpcRunDeathEffects();
        this.RpcTogglePlayerVisibility(false);
        StartCoroutine(this.Respawn());

        this._playerMovement.UnsetDesiredPosition();
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.0f);

        this._currentHealth = this._playerStartHealth;
        this._deathOngoing = false;
        this.RpcTogglePlayerVisibility(true);
        if (!this._shadowDetector.IsInsideShadow())  
        {
            this.RpcEnableSpawnMode();
        }

        this.gameObject.transform.localPosition = this._spawnPosition;
    }

    [ClientRpc]
    private void RpcRunDeathEffects()
    {
        this._deathParticles.Play();
    }

    [ClientRpc]
    private void RpcTogglePlayerVisibility(bool show)
    {
        this._playerModel.SetActive(show);
    }

    [ClientRpc]
    private void RpcDisabledSpawnMode()
    {
        this._spawnModeActivated = false;
        this.ToggleSpawnCapsule(this._spawnModeActivated, this._lockModeActivated);
        _needShadowAnimator.ResetTrigger("ShowText");
        _needShadowAnimator.SetTrigger("HideText");
    }

    [ClientRpc]
    private void RpcEnableSpawnMode() 
    {
        this._spawnModeActivated = true;
        this.ToggleSpawnCapsule(this._spawnModeActivated, this._lockModeActivated);
        _needShadowAnimator.ResetTrigger("HideText");
        _needShadowAnimator.SetTrigger("ShowText");
    }

    [ClientRpc]
    private void ToggleLockMode(bool shouldEnable)
    {
        this._lockModeActivated = shouldEnable;
        this.ToggleSpawnCapsule(this._spawnModeActivated, this._lockModeActivated);
    }

    private void ToggleSpawnCapsule(bool isSpawnModeActivated, bool isLockModeActivated)
    {
        Animator spawnCapsuleAnimator = _spawnCapsule.GetComponent<Animator>();

        if (isSpawnModeActivated || isLockModeActivated) {
            spawnCapsuleAnimator.ResetTrigger("HideSpawnCapsule");
            spawnCapsuleAnimator.SetTrigger("ShowSpawnCapsule");
        }
        else {
            spawnCapsuleAnimator.ResetTrigger("ShowSpawnCapsule");
            spawnCapsuleAnimator.SetTrigger("HideSpawnCapsule");
        }
    }
}
