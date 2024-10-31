using UnityEngine;
using YG;

public class InputBlocker : MonoBehaviour
{
    [SerializeField] private bicycle_code bike;
    [SerializeField] private mobileControls mobileControls;
    [SerializeField] private keyboardControls keyboardControls;
    public void ToogleControl(bool enabled)
    {
        bike.gameObject.SetActive(enabled);
        if (YandexGame.EnvironmentData.isMobile)
            mobileControls.enabled = enabled;
        if (YandexGame.EnvironmentData.isDesktop)
            keyboardControls.enabled = enabled;
    }
}
