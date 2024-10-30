using UnityEngine;
using YG;

public class RevCode : MonoBehaviour
{
    [SerializeField] private float _timeDelay = 180;
    [SerializeField] private PanelAnim _reviewPanel;
    [SerializeField] private int _reward = 1000;
    [SerializeField] private float _timer;
    [SerializeField] private BankVolute _bank;

    private void Update()
    {
        if (YandexGame.EnvironmentData.reviewCanShow)
        {
            if (_timer >= _timeDelay)
            {
                _timer = 0;
                _reviewPanel.PlayAnimEnable();
            }
            else if (!_reviewPanel.GetTarget().activeSelf)
                _timer += Time.deltaTime;
        }
    }
    public void GiveReward()
    {
        _bank.IncreaseMoney(_reward);
        YandexGame.FullscreenShow();
        Debug.Log("Игрок получил награду за отзыв");
    }
}
