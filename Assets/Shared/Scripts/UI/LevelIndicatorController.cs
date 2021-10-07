using UnityEngine;
using TMPro;
public class LevelIndicatorController : MonoBehaviour
{
    [SerializeField] GameObject _levelIndicator;
    private LevelManager _levelManager;

    private TMP_Text m_TextComponent;
    private TextTyper _textTyperScript;

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    public void UpdateLevelText(int levelIndex)
    {
        if (_textTyperScript)
        {
            Destroy(_textTyperScript);
        }
        _textTyperScript = _levelIndicator.AddComponent<TextTyper>();
        m_TextComponent.text = "Level " + (levelIndex + 1);
    }

}
