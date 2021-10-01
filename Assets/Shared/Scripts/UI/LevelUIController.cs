using UnityEngine;
using System.Collections;
public class LevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject _levelClearText;
    [SerializeField] private GameObject _levelIndicatorText;

    private GameObject _levelIndicator;

    public void UpdateLevelIndicator(int levelIndex)
    {
        if (_levelIndicator)
        {
            Destroy(_levelIndicator);
        }
        _levelIndicator = Instantiate(_levelIndicatorText, transform);
        _levelIndicator.GetComponent<LevelIndicatorController>().UpdateLevelText(levelIndex);
    }

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
