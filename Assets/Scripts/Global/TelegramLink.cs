using UnityEngine;
using UnityEngine.UI;

public class TelegramLink : MonoBehaviour
{
    [SerializeField] private Button _buttonTG;
    private void Start()
    {
        _buttonTG.onClick.AddListener(OpenLink);
    }
    private void OpenLink()
    {
        Application.OpenURL("https://t.me/Devord_Studio");
        Debug.Log("Открыта ссылка на телеграмм");
    }
}
