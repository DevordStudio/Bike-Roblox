using TMPro;
using UnityEngine;
using YG;

public class Translator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [TextArea]
    [SerializeField] private string _turkishText;
    [TextArea]
    [SerializeField] private string _russianText;
    [TextArea]
    [SerializeField] private string _englishText;

    private void Start()
    {
        YandexGame.SwitchLangEvent += ChangeText;
        _text ??= GetComponent<TMP_Text>();
        ChangeText(YandexGame.lang);
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
