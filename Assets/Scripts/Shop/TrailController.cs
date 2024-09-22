using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [HideInInspector] public TrailCell currentCell;
    [HideInInspector] public TrailCell lastCell;

    [HideInInspector] public static TrailController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        _trails = null;
        _trails = Resources.LoadAll<TrailData>("ShopItems/TrailDatas");
        GenerateCells();
    }
    public void UpdateUI()
    {
        if (currentCell)
        {
            _currentCellIcon.color = currentCell.TrailData.Material.color;
            _name.text = currentCell.TrailData.Name;
            //SpeedBoost
            if (!currentCell.TrailData.IsBought && !currentCell.TrailData.IsEquiped)//НЕ КУПЛЕН И НЕ ВЫБРАН
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
        else { }//Отключить всё если currentCell нулевой
    }
    public void Buy()
    {
        if (currentCell && !currentCell.TrailData.IsBought)
        {
            if (BankVolute.Instance.Money >= currentCell.TrailData.Price)
            {
                currentCell.TrailData.IsBought = true;
                BankVolute.Instance.Money -= currentCell.TrailData.Price;
                Debug.Log($"Трейл {currentCell.TrailData.Name} был куплен за {currentCell.TrailData.Price}");
            }
            else
            {
                _notice.text = $"Недосточно монет! Вам нужно ещё {BankVolute.Instance.Money - currentCell.TrailData.Price} монет чтобы купить этот предмет.";
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
            _trailRenderer.material = currentCell.TrailData.Material;
        }
        UpdateUI();
    }
    public void TextFade(TMP_Text text)
    {
        Color color = text.color;
        Sequence sequence = DOTween.Sequence();
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
        cell.Icon.color = data.Material.color;
        cell.IconEquiped.gameObject.SetActive(data.IsEquiped);
    }
}
