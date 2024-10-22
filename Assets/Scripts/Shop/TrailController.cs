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

    /*[HideInInspector]*/
    public TrailCell currentCell;
    /*[HideInInspector]*/
    public TrailCell lastCell;

    private void Start()
    {
        GenerateCells();
        _trailRenderer.material = currentCell.TrailData.Material;
        UpdateUI();
    }
    public void UpdateUI()
    {
        if (currentCell)
        {
            if (!currentCell.TrailData.Material.GetTexture("_BaseMap"))
            {
                _currentCellIcon.material = null;
                _currentCellIcon.color = currentCell.TrailData.Material.color;
            }
            else
            {
                _currentCellIcon.color = Color.white;
                _currentCellIcon.material = currentCell.TrailData.Material;
            }
            //_name.text = currentCell.TrailData.Name;
            if (YandexGame.EnvironmentData.language == "ru")
                _name.text = currentCell.TrailData.NameRus;
            else if (YandexGame.EnvironmentData.language == "en")
                _name.text = currentCell.TrailData.NameEn;
            else if (YandexGame.EnvironmentData.language == "tr")
                _name.text = currentCell.TrailData.NameTr;
            else _name.text = currentCell.TrailData.NameEn;
            //SpeedBoost
            if (!currentCell.TrailData.IsBought && !currentCell.TrailData.IsEquiped)//�� ������ � �� ������
            {
                _buttonBuy.SetActive(true);
                _buttonEquip.SetActive(false);
                _buttonEquiped.SetActive(false);
            }
            else if (currentCell.TrailData.IsBought && !currentCell.TrailData.IsEquiped)
            {
                _buttonBuy.SetActive(false);
                _buttonEquip.SetActive(true);
                _buttonEquiped.SetActive(false);
            }
            else
            {
                _buttonBuy.SetActive(false);
                _buttonEquip.SetActive(false);
                _buttonEquiped.SetActive(true);
            }
        }
        else
        {
            _buttonBuy.SetActive(false);
            _buttonEquip.SetActive(false);
            _buttonEquiped.SetActive(false);
        }//��������� �� ���� currentCell �������
    }
    public void Buy()
    {
        if (currentCell && !currentCell.TrailData.IsBought)
        {
            if (_bank.GetMoney() >= currentCell.TrailData.Price)
            {
                currentCell.TrailData.Buy();
                _bank.DecreaseMoney(currentCell.TrailData.Price);
                Debug.Log($"����� {currentCell.TrailData.Name} ��� ������ �� {currentCell.TrailData.Price}");
            }
            else
            {
                _notice.text = $"���������� �����! ��� ����� ��� {_bank.GetMoney() - currentCell.TrailData.Price} ����� ����� ������ ���� �������.";
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
        if (!data.Material.GetTexture("_BaseMap"))
            cell.Icon.color = cell.TrailData.Material.color;
        else
        {
            cell.Icon.color = Color.white;
            cell.Icon.material = cell.TrailData.Material;
        }
        cell.IconEquiped.gameObject.SetActive(data.IsEquiped);
        cell.TrailController = this;
        if (data.IsEquiped)
        {
            currentCell = cell;
            lastCell = cell;
        }
    }
}
