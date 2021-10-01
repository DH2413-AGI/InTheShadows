using UnityEngine;
using System.Collections;
using TMPro;
public class TextTyper : MonoBehaviour
{
    private TMP_Text m_TextComponent;
    public float delay = 0.03f;
    public float afterDelay = 1f;

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        m_TextComponent.maxVisibleCharacters = 0;
    }
    void Start()
    {
        StartCoroutine(TypeText());
    }
    IEnumerator TypeText()
    {
        for (int i = 0; i < m_TextComponent.textInfo.characterCount + 1; i++)
        {
            m_TextComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }
    }

}