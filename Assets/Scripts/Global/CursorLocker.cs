using UnityEngine;
using YG;

public class CursorLocker : MonoBehaviour
{
    [SerializeField] private GameObject _ui;

    private bool _isVisible;

    private void Start()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _ui.SetActive(true);
            CursorControll(true);
        }
        else if (YandexGame.EnvironmentData.isMobile)
        {
            _ui.SetActive(false);
            CursorControll(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CursorControll(!_isVisible);
            Debug.Log("Изменён курсор");
        }
    }
    public void CursorControll(bool isVisible)
    {
        _isVisible = isVisible;
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
