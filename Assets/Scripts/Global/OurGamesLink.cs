using UnityEngine;
using UnityEngine.UI;
using YG;

public class OurGamesLink : MonoBehaviour
{
    [SerializeField] private Button _buttonMyGames;
    void Start()
    {
        _buttonMyGames.onClick.AddListener(OpenLink);
    }

    private void OpenLink()
    {
        Application.OpenURL($"https://yandex.{YandexGame.EnvironmentData.domain}/games/developer/86560");
        Debug.Log("ќткрыта ссылка на яндекс игры");
    }
}
