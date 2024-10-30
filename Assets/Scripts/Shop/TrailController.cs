using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using YG;

public class TrailController : MonoBehaviour
{
    [SerializeField] private TrailData[] _trails;
    [SerializeField] private GameObject _trailCellPrefab;
    [SerializeField] private GameObject _trailCellsParent;
    [SerializeField] private Image _currentCellIcon;
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private GameObject _buttonEquip;
    [SerializeField] private GameObject _buttonEquiped;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _stats;
    [SerializeField] private TMP_Text _notice;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private BankVolute _bank;
    [SerializeField] private TMP_Text _priceText;

    private Vector2 _texScale;
    private Sprite _spriteSave;

    /*[HideInInspector]*/
    public TrailCell currentCell;
    /*[HideInInspector]*/
    public TrailCell lastCell;
    public TrailCell lastSelected;

    private void Start()
    {
        GenerateCells();
        _texScale = _trailRenderer.textureScale;
        if (currentCell.TrailData.IsGradient)
        {
            _trailRenderer.textureMode = LineTextureMode.Stretch;
            _trailRenderer.textureScale = new Vector2(1, 1);
        }
        else
        {
            _trailRenderer.textureMode = LineTextureMode.Tile;
            _trailRenderer.textureScale = _texScale;
        }
        _trailRenderer.material = currentCell.TrailData.Material;
        //Equip();
        YandexGame.SwitchLangEvent += SwitchLanguage;
        _spriteSave = _currentCellIcon.sprite;
        UpdateUI();
    }
    private void OnDestroy()
    {
        YandexGame.SwitchLangEvent -= SwitchLanguage;
    }
    public void UpdateUI()
    {
        if (currentCell)
        {
            _currentCellIcon.material = null;
            _currentCellIcon.color = Color.white;
            _currentCellIcon.sprite = _spriteSave;
            if (!currentCell.TrailData.Sprite)
            {

                if (!currentCell.TrailData.Material.GetTexture("_BaseMap"))
                {
                    _currentCellIcon.color = currentCell.TrailData.Material.color;
                }
                else
                {
                    _currentCellIcon.color = Color.white;
                    _currentCellIcon.material = currentCell.TrailData.Material;
                }
            }
            else
            {
                _currentCellIcon.sprite = currentCell.TrailData.Sprite;
            }
            //_name.text = currentCell.TrailData.Name;
            //SpeedBoost
            SwitchLanguage(YandexGame.lang);
            if (!currentCell.TrailData.IsBought && !currentCell.TrailData.IsEquiped)//НЕ КУПЛЕН И НЕ ВЫБРАН
            {
                _buttonBuy.SetActive(true);
                _buttonEquip.SetActive(false);
                _buttonEquiped.SetActive(false);
                if (_priceText && !currentCell.TrailData.IsDonate)
                {
                    _priceText.gameObject.SetActive(true);
                    _priceText.text = currentCell.TrailData.Price.ToString();
                    if (_bank.GetMoney() >= currentCell.TrailData.Price) _priceText.color = Color.white;
                    else _priceText.color = Color.red;
                }
                else _priceText.gameObject.SetActive(false);
            }
            else if (currentCell.TrailData.IsBought && !currentCell.TrailData.IsEquiped)
            {
                _buttonBuy.SetActive(false);
                _buttonEquip.SetActive(true);
                _buttonEquiped.SetActive(false);
                _priceText.gameObject.SetActive(false);
            }
            else
            {
                _buttonBuy.SetActive(false);
                _buttonEquip.SetActive(false);
                _buttonEquiped.SetActive(true);
                _priceText.gameObject.SetActive(false);
            }
        }
        else
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(false);
            _priceText.gameObject.SetActive(false);
        }//Отключить всё если currentCell нулевой
    }
    private void SwitchLanguage(string lang)
    {
        if (lang == "ru")
            _name.text = currentCell.TrailData.NameRus;
        else if (lang == "tr")
            _name.text = currentCell.TrailData.NameTr;
        else
            _name.text = currentCell.TrailData.NameEn;
    }
    public void Buy()
    {
        if (currentCell && !currentCell.TrailData.IsBought)
        {
            if (_bank.GetMoney() >= currentCell.TrailData.Price)
            {
                currentCell.TrailData.Buy();
                _bank.DecreaseMoney(currentCell.TrailData.Price);
                Debug.Log($"Трейл {currentCell.TrailData.Name} был куплен за {currentCell.TrailData.Price}");
            }
            else
            {
                _notice.text = $"Недосточно монет! Вам нужно ещё {_bank.GetMoney() - currentCell.TrailData.Price} монет чтобы купить этот предмет.";
                _notice.gameObject.SetActive(true);
                TextFade(_notice);
            }
        }
        UpdateUI();
    }
    public void Equip()
    {
        if (currentCell && currentCell.TrailData.IsBought && !currentCell.TrailData.IsEquiped)
        {
            lastCell.TrailData.IsEquiped = false;
            currentCell.TrailData.IsEquiped = true;
            lastCell.UpdateCell();
            currentCell.UpdateCell();
            lastCell = currentCell;
            if (currentCell.TrailData.IsGradient)
            {
                _trailRenderer.textureMode = LineTextureMode.Stretch;
                _trailRenderer.textureScale = new Vector2(1, 1);
            }
            else
            {
                _trailRenderer.textureMode = LineTextureMode.Tile;
                _trailRenderer.textureScale = _texScale;
            }
            _trailRenderer.material = currentCell.TrailData.Material;
        }
        UpdateUI();
    }
    public void TextFade(TMP_Text text)
    {
        Sequence sequence = DOTween.Sequence();
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, 1);
        sequence.Append(text.DOColor(new Color(color.r, color.g, color.b, 0), 3.5F));
        sequence.AppendCallback(() =>
        {
            text.gameObject.SetActive(false);
        });
    }
    public void GenerateCells()
    {
        foreach (var trail in _trails)
        {
            var cell = Instantiate(_trailCellPrefab, _trailCellsParent.transform);
            TrailCell traillCell = cell.GetComponent<TrailCell>();
            InitCell(traillCell, trail);
        }
    }
    public void InitCell(TrailCell cell, TrailData data)
    {
        cell.TrailData = data;
        if (!cell.TrailData.Sprite)
        {
            if (!data.Material.GetTexture("_BaseMap"))
                cell.Icon.color = cell.TrailData.Material.color;
            else
            {
                cell.Icon.color = Color.white;
                cell.Icon.material = cell.TrailData.Material;
            }
        }
        else
        {
            cell.Icon.color = Color.white;
            cell.Icon.sprite = cell.TrailData.Sprite;
        }
        cell.IconEquiped.gameObject.SetActive(data.IsEquiped);
        cell.TrailController = this;
        if (data.IsEquiped)
        {
            currentCell = cell;
            lastCell = cell;
            lastSelected = cell;
            lastSelected.IconSelected.SetActive(true);
        }
    }
}
