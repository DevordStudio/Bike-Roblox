using UnityEngine;
using YG;

public class OurGamesLink : MonoBehaviour
{
    public void OpenLink()
    {
        Application.OpenURL($"https://yandex.{YandexGame.EnvironmentData.domain}/games/developer/86560");
        Debug.Log("ќткрыта ссылка на яндекс игры");
    }
}
