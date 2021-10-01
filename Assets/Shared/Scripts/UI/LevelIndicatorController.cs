using UnityEngine;
using TMPro;
using Mirror;
public class LevelIndicatorController : MonoBehaviour
{
    [SerializeField] GameObject _levelIndicator;
    private LevelManager _levelManager;

    private TMP_Text m_TextComponent;

    private int _currentLevel;

    private TextTyper _textTyperScript;

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _currentLevel = _levelManager.currentLevelIndex;
        UpdateLevelText();
    }

    void Update()
    {
        if (_levelManager.currentLevelIndex != _currentLevel)
        {
            UpdateLevelText();
            _currentLevel = _levelManager.currentLevelIndex;
        }
    }

    void UpdateLevelText()
    {
        if (_textTyperScript)
        {
            Destroy(_textTyperScript);
        }
        _textTyperScript = _levelIndicator.AddComponent<TextTyper>();
        m_TextComponent.text = "Level " + (_levelManager.currentLevelIndex + 1);
    }

}
