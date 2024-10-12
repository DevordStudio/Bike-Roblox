using System;
using UnityEngine;
using YG;

public class LanguageSwitcher : MonoBehaviour
{
    private void Start()
    {
        ChangeLanguage(YandexGame.EnvironmentData.language);
    }
    public void ChangeLanguage(string language)
    {
        switch (language)
        {
            case "en":
                YandexGame.SwitchLanguage("en");
                break;
            case "tr":
                YandexGame.SwitchLanguage("tr");
                break;
            case "ru":
                YandexGame.SwitchLanguage("ru");
                break;
        }
        Debug.Log($"язык изменЄн на {language}");
    }
}
