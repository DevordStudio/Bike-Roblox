using UnityEngine;

public class TelegramLink : MonoBehaviour
{
    public void OpenLink()
    {
        Application.OpenURL("https://t.me/Devord_Studio");
        Debug.Log("Открыта ссылка на телеграмм");
    }
}
