using UnityEngine;
using YG;

public class TGFlag : MonoBehaviour
{
    [SerializeField] private GameObject TGButton;
    void Start()
    {
        var value = YandexGame.GetFlag("TGFlag");
        if (value == null) return;
        if (value == "true") TGButton.SetActive(true);
        else TGButton.SetActive(false);
    }
}
