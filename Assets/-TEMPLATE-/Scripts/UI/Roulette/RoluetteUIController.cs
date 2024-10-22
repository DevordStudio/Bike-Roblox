using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class RoluetteUIController : MonoBehaviour
{
    [SerializeField] private Roulette _roulette;
    [SerializeField] private string _prefixTimerInPanelRu = "Бесплатная крутка";
    [SerializeField] private string _prefixTimerInPanelEn = "Free spin";
    [SerializeField] private string _trText;
    [SerializeField] private string _prefixTimerInPanelTr;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _timerInPanelText;
    [SerializeField] private Button _startRouletteButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private int _spinCost = 100;
    [SerializeField] private BankVolute _bank;
    [SerializeField] private bool _freeSpin = false;
    [SerializeField] private float _remainingTime = 0f;
    [SerializeField] private float _cooldownTime = 300f;
    [SerializeField] private GameObject _freeSpinMarker;
    [SerializeField] private GameObject _timerGO;


    private void Start()
    {
        _startRouletteButton.onClick.AddListener(RotateRoulette);
        _roulette.EndRotateEvent += EndRotate;

        _remainingTime = YandexGame.savesData.TempKdRotateRoulette;
        if (_remainingTime > 0)
            StartCoroutine(CheckFreeSpinCoroutine());
        else
            UnlockFreeSpin();
        UpdateTimeUI(_remainingTime);
        foreach (var item in _roulette.Items)
        {
            item.ImageOnRoulette.sprite = item.Sprite;
        }
        if (_freeSpin)
        {
            _timerGO.SetActive(false);
            _freeSpinMarker.SetActive(true);
        }
        else
        {
            _timerGO.SetActive(true);
            _freeSpinMarker.SetActive(false);
        }
    }

    private IEnumerator CheckFreeSpinCoroutine()
    {
        while (_remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _remainingTime -= 1f;
            UpdateTimeUI(_remainingTime);
        }
        UnlockFreeSpin();
        SaveProgress();
    }

    private void UnlockFreeSpin()
    {
        _freeSpin = true;
        _remainingTime = 0;
        _timerGO.SetActive(false);
        _freeSpinMarker.SetActive(true);
    }

    private void RotateRoulette()
    {
        if (_freeSpin)
        {
            _freeSpin = false;
            _closeButton.interactable = false;
            _roulette.Rotate();
            _remainingTime = _cooldownTime;
            _timerGO.SetActive(true);
            _freeSpinMarker.SetActive(false);
            StartCoroutine(CheckFreeSpinCoroutine());
        }
        else if (_bank.GetMoney() >= _spinCost)
        {
            if (_roulette.Rotate())
            {
                _closeButton.interactable = false;
                _bank.DecreaseMoney(_spinCost);
            }
        }
    }

    private void EndRotate() => _closeButton.interactable = true;

    private void UpdateTimeUI(float time)
    {
        _timerText.text = FormatTime(time);
        if (YandexGame.EnvironmentData.language == "ru")
            _timerInPanelText.text = time > 0 ? _prefixTimerInPanelRu + " " + FormatTime(time) : "Доступна бесплатная крутка";
        else if (YandexGame.EnvironmentData.language == "tr")
            _timerInPanelText.text = time > 0 ? _prefixTimerInPanelTr + " " + FormatTime(time) : _trText;
        else 
            _timerInPanelText.text = time > 0 ? _prefixTimerInPanelEn + " " + FormatTime(time) : "Free spin unlocked";
    }

    private void OnApplicationQuit() => SaveProgress();

    private void SaveProgress()
    {
        YandexGame.savesData.TempKdRotateRoulette = _remainingTime;
        YandexGame.SaveProgress();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }
}