using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EggUIControll : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private PetInventory _invent;
    [Space(3)]
    [Header("Bank Volute")]
    [SerializeField] private BankVolute _bank;
    [Space(3)]
    [SerializeField, Foldout("EggDropAnim")] private int _shakeCount = 5;
    [SerializeField, Foldout("EggDropAnim")] private float _shakeDuration = 0.1F;
    [SerializeField, Foldout("EggDropAnim")] private float _shakeAngle = 15F;
    [SerializeField, Foldout("EggDropAnim")] private Image _eggImg;
    [SerializeField, Foldout("EggDropAnim")] private Image _petImg;
    [SerializeField, Foldout("EggDropAnim")] private GameObject _dropUI;
    [SerializeField, Foldout("EggDropAnim")] private PanelAnim _eggAnim;
    [SerializeField, Foldout("EggDropAnim")] private GameObject _glow;
    [SerializeField, Foldout("EggUI")] private PanelAnim _uiEggAnim;
    [SerializeField, Foldout("EggUI")] private Image _petImgPrefab;
    [SerializeField, Foldout("EggUI")] private GameObject _petImgParent;
    [SerializeField, Foldout("EggUI")] private Image _eggImgUI;
    [SerializeField, Foldout("EggUI")] private Button _buttonGetEgg;
    [SerializeField, Foldout("EggUI")] private GameObject _panelNotice;
    [SerializeField, Foldout("EggUI")] private TMP_Text _eggPrice;
    [SerializeField, Foldout("EggUI")] private Color _colorNotEnoughMoney = Color.red;
    [SerializeField, Foldout("EggUI")] private Color _colorEnoughMoney = Color.white;
    private EggCode _currentEgg;
    private void Start()
    {
        BankVolute.OnMoneyValueChanged += UpdateUI;
        _invent.OnInventoryChanged += UpdateUI;
    }
    private void OnDestroy()
    {
        BankVolute.OnMoneyValueChanged -= UpdateUI;
        _invent.OnInventoryChanged -= UpdateUI;
    }
    public void OpenEggUI(EggCode egg)
    {
        _currentEgg = egg;
        _eggImgUI.color = egg.GetColor();
        _buttonGetEgg.onClick.RemoveAllListeners();
        _buttonGetEgg.onClick.AddListener(egg.BuyEgg);
        _eggPrice.text = egg.GetPrice().ToString();
        var lastPets = _petImgParent.GetComponentsInChildren<Image>();
        foreach (var pet in lastPets)
        {
            if (pet.gameObject != _petImgParent) Destroy(pet.gameObject);
        }
        var pets = egg.GetPets();
        foreach (var pet in pets)
        {
            var petImgGO = Instantiate(_petImgPrefab, _petImgParent.transform);
            petImgGO.sprite = pet.Sprite;
        }
        _uiEggAnim.PlayAnimEnable();
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (_invent.GetPets().Count >= _invent.MaxPetsInInventory)
        {
            _buttonGetEgg.gameObject.SetActive(false);
            _panelNotice.gameObject.SetActive(true);
        }
        else
        {
            _buttonGetEgg.gameObject.SetActive(true);
            _panelNotice.gameObject.SetActive(false);
            if (_currentEgg)
            {
                if (_bank.GetMoney() >= _currentEgg.GetPrice())
                {
                    _buttonGetEgg.gameObject.SetActive(true);
                    _eggPrice.color = _colorEnoughMoney;
                }
                else
                {
                    _buttonGetEgg.gameObject.SetActive(false);
                    _eggPrice.color = _colorNotEnoughMoney;
                }
            }
        }

    }
    public void EggAnim(Color eggColor, Sprite petSprite)
    {
        _eggImg.color = eggColor;
        _petImg.sprite = petSprite;
        _petImg.gameObject.SetActive(false);
        _glow.SetActive(false);
        _dropUI.GetComponent<EventTrigger>().enabled = false;
        OpenEggAnim();
    }
    private void OpenEggAnim()
    {
        //PanelAnim anim = _eggImg.GetComponent<PanelAnim>();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => _eggAnim.PlayAnimEnable());
        sequence.AppendCallback(() => ShakeAndDisable());
        sequence.Play();
        _dropUI.SetActive(true);
        //_eggImg.gameObject.SetActive(true);
    }
    private void ShakeAndDisable()
    {
        Sequence shakeSequence = DOTween.Sequence();
        float shakeDur = _shakeDuration;
        for (int i = 0; i < _shakeCount; i++)
        {
            shakeSequence.Append(_eggImg.rectTransform.DOLocalRotate(new Vector3(0, 0, _shakeAngle), shakeDur).SetEase(Ease.InOutSine));
            shakeSequence.Append(_eggImg.rectTransform.DOLocalRotate(new Vector3(0, 0, -_shakeAngle), shakeDur).SetEase(Ease.InOutSine));
        }
        shakeSequence.Append(_eggImg.rectTransform.DOLocalRotate(Vector3.zero, shakeDur).SetEase(Ease.InOutSine))
                     .OnComplete(() =>
                     {
                         _eggImg.gameObject.SetActive(false);
                         _petImg.gameObject.SetActive(true);
                         _glow.SetActive(true);
                         _dropUI.GetComponent<EventTrigger>().enabled = true;
                     });
    }
}
