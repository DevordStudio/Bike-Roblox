using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private TutorialParts[] _allParts;
    [SerializeField] private GameObject _cam;
    [SerializeField] private GameObject _PCTutor;
    [SerializeField] private GameObject _mobileTutor;
    [SerializeField] private GameObject _mapTutor;
    [SerializeField] private float _buttonBlockDelay;
    [SerializeField] private float _cameraAnimTime;
    [SerializeField] private Transform _camPoint;
    [SerializeField] private GameObject[] _hidingUI;
    [SerializeField] private GameObject _mobileControlls;
    [SerializeField] private Button _skipTutor;
    [SerializeField] private InputBlocker _input;
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private GameObject _timerBeforeADs;

    private int _currentStep;
    private void Start()
    {
        if (!YandexGame.savesData.TutorShown)
        {
            _skipTutor.onClick.AddListener(CloseTutor);
            Debug.Log("Врубаю обучение");
            _input.ToogleControl(false);
            PlayThisPart();
            _cameraMain.gameObject.SetActive(false);
            _cam.SetActive(true);
            _timerBeforeADs.SetActive(false);
        }
        else
        {
            _cam.SetActive(false);
            _cameraMain.gameObject.SetActive(true);
            _skipTutor.gameObject.SetActive(false);
        }
    }
    //private void Start()
    //{
    //    _cameraMain = Camera.main;
    //    if (YandexGame.savesData.TutorShown)
    //    {
    //        PlayThisPart();
    //        _cameraMain.gameObject.SetActive(false);
    //        _cam.SetActive(true);
    //    }
    //}
    public void PlayThisPart()
    {
        foreach (var part in _allParts)
        {
            part.TutorUI.SetActive(part == _allParts[_currentStep]);
        }
        _allParts[_currentStep].ButtonNext.interactable = false;
        if (_currentStep < _allParts.Length - 1)
            _allParts[_currentStep].ButtonNext.onClick.AddListener(PlayThisPart);
        else
            _allParts[_currentStep].ButtonNext.onClick.AddListener(ControllTutor);
        Invoke(nameof(ActivateButton), _allParts[_currentStep].TimeBlockButton);
    }
    private void ControllTutor()
    {
        _allParts[_currentStep-1].TutorUI.SetActive(false);
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _PCTutor.SetActive(true);
            Button buttonNext = _PCTutor.GetComponentInChildren<Button>();
            buttonNext.interactable = false;
            buttonNext.onClick.AddListener(AnimCameraStart);
            Invoke(nameof(ActivatePCButton), _buttonBlockDelay);
            foreach (var item in _hidingUI)
            {
                item.SetActive(false);
            }
        }
        else if (YandexGame.EnvironmentData.isMobile)
        {
            _mobileTutor.SetActive(true);
            Button buttonNext = _mobileTutor.GetComponentInChildren<Button>();
            buttonNext.interactable = false;
            buttonNext.onClick.AddListener(AnimCameraStart);
            Debug.Log("Включен мобильный туториал");
            Invoke(nameof(ActivateMobileButton), _buttonBlockDelay);
            foreach (var item in _hidingUI)
            {
                item.SetActive(false);
            }
        }
    }
    private void AnimCameraStart()
    {
        _mobileControlls.SetActive(false);
        _PCTutor.SetActive(false);
        _mobileTutor.SetActive(false);
        print("Начата анимация камеры");
        Button button = _mapTutor.GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.AddListener(CloseTutor);
        StartCoroutine(AnimCamera());
    }
    private IEnumerator AnimCamera()
    {
        float elapsedTime = 0f;
        Vector3 startingPos = _cam.transform.position;

        while (Vector3.Distance(_cam.transform.position, _camPoint.position) > 0.1F)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _cameraAnimTime);
            _cam.transform.position = Vector3.Lerp(startingPos, _camPoint.position, t);
            yield return null;
        }

        _mapTutor.SetActive(true);
        Invoke(nameof(ActivateMapButton), _buttonBlockDelay);
    }
    private void CloseTutor()
    {
        foreach (var item in _hidingUI)
        {
            item.SetActive(true);
        }
        foreach (var item in _allParts)
        {
            item.TutorUI.SetActive(false);
        }
        StopAllCoroutines();
        _mapTutor.SetActive(false);
        _PCTutor.SetActive(false);
        _mobileTutor.SetActive(false);
        _mobileControlls.SetActive(false);
        _mapTutor.SetActive(false);
        _mobileControlls.SetActive(YandexGame.EnvironmentData.isMobile);
        _cam.SetActive(false);
        _cameraMain.gameObject.SetActive(true);
        _input.ToogleControl(true);
        _skipTutor.gameObject.SetActive(false);
        _timerBeforeADs.gameObject.SetActive(true);
        YandexGame.savesData.TutorShown = true;
    }
    private void ActivateMapButton()
    {
        Button button = _mapTutor.GetComponentInChildren<Button>();
        button.interactable = true;
    }
    private void ActivateMobileButton()
    {
        Button button = _mobileTutor.GetComponentInChildren<Button>();
        button.interactable = true;
    }
    private void ActivatePCButton()
    {
        Button buttonNext = _PCTutor.GetComponentInChildren<Button>();
        buttonNext.interactable = true;
    }
    public void ActivateButton()
    {
        _allParts[_currentStep].ButtonNext.interactable = true;
        _currentStep++;
    }
}
[System.Serializable]
public class TutorialParts
{
    public GameObject TutorUI;
    public Button ButtonNext;
    public float TimeBlockButton;
}
