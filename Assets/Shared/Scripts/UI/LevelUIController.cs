using UnityEngine;
using System.Collections;
public class LevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject _levelClearText;
    public void ShowLevelClearText()
    {
        StartCoroutine(InstantiateLevelClear());
    }

    IEnumerator InstantiateLevelClear()
    {
        GameObject levelText = Instantiate(_levelClearText, transform);
        yield return new WaitForSeconds(2f);
        Destroy(levelText);
    }
}
