using UnityEngine;
using YG;

public class ControllSwitcher : MonoBehaviour
{
    [SerializeField] private mobileControls _mobileController;
    [SerializeField] private GameObject _mobileUI;
    [SerializeField] private keyboardControls _keyboardController;

    void Start()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _mobileController.enabled = false;
            _keyboardController.enabled = true;
            _mobileUI.SetActive(false);
        }
        if (YandexGame.EnvironmentData.isMobile)
        {
            _mobileController.enabled = true;
            _keyboardController.enabled = false;
            _mobileUI.SetActive(true);
        }
    }
}
