using System.Collections;
using Unity.VisualScripting;
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

    private Camera _cameraMain;
    private controlHub _controll;
    private int _currentStep;

    private void Start()
    {
        _controll = FindAnyObjectByType<controlHub>();
        _cameraMain = Camera.main;
        if (YandexGame.savesData.TutorShown)
        {
            PlayThisPart();
            _cameraMain.gameObject.SetActive(false);
            _cam.SetActive(true);
        }
    }
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
        _allParts[_currentStep].TutorUI.SetActive(false);
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
            Button buttonNext = _PCTutor.GetComponentInChildren<Button>();
            buttonNext.interactable = false;
            buttonNext.onClick.AddListener(AnimCameraStart);
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
        Button button = _mapTutor.GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.AddListener(CloseTutor);
        StartCoroutine(AnimCamera());
    }
    private IEnumerator AnimCamera()
    {
        if (Vector3.Distance(_cam.transform.position, _camPoint.position) > 0.1F)
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camPoint.position, _cameraAnimTime);
        else
        {
            _mapTutor.SetActive(true);
            Invoke(nameof(ActivateMapButton), _buttonBlockDelay);
            yield break;
        }
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
        _mapTutor.SetActive(false);
        _PCTutor.SetActive(false);
        _mobileControlls.SetActive(false);
        _mapTutor.SetActive(false);
        _mobileControlls.SetActive(YandexGame.EnvironmentData.isMobile);
        _cam.SetActive(false);
        _cameraMain.gameObject.SetActive(true);
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
