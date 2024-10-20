using TMPro;
using UnityEngine;
using YG;

public class Translator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _turkishText;
    [SerializeField] private string _russianText;
    [SerializeField] private string _englishText;

    private void Start()
    {
        YandexGame.SwitchLangEvent += ChangeText;
        _text ??= GetComponent<TMP_Text>();
        ChangeText(YandexGame.EnvironmentData.language);
    }
    private void OnDestroy() => YandexGame.SwitchLangEvent -= ChangeText;
    private void ChangeText(string lang)
    {
        switch (lang)
        {
            case "ru":
                _text.text = _russianText;
                break;
            case "en":
                _text.text = _englishText;
                break;
            case "tr":
                _text.text = _turkishText;
                break;
        }
    }
}
