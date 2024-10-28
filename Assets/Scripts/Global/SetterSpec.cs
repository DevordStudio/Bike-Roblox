using UnityEngine;
using YG;

public class SetterSpec : MonoBehaviour
{
    [SerializeField] private GameObject Button;
    void Start()
    {
        var value = YandexGame.GetFlag("Setter");
        if (value == null) return;
        if (value == "true") Button.SetActive(true);
        else Button.SetActive(false);
    }
}
