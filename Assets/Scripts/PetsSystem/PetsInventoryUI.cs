using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class PetsInventoryUI : MonoBehaviour
{
    [SerializeField] private PetInventory _invent;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _cellGOContainer;
    [SerializeField] private Image _currentPetImg;
    [SerializeField] private TMP_Text _currentPetName;
    [SerializeField] private GameObject _imagePet;
    [SerializeField] private GameObject _buttonEquip;
    [SerializeField] private GameObject _buttonUnequip;
    [SerializeField] private GameObject _buttonDelete;
    [SerializeField] private TMP_Text _petsCount;
    [SerializeField] private TMP_Text _equipedPetsCount;

    [SerializeField] private PetCell _currentCell;
    private List<PetCell> _cells = new List<PetCell>();

    private void Start()
    {
        PetCell.OnCellSelected += UpdateUI;
        _invent.OnPetAdded += GenerateCell;
        _invent.OnInventoryChanged += UpdateCountText;
        YandexGame.SwitchLangEvent += SwitchLanduage;
        foreach (var pet in _invent.GetPets())
            GenerateCell(pet);
        CheckCurrentCell();
        if (_invent.GetPets().Count > 0) UpdateUI(_currentCell);
        //else ToogleUI(false);
        SwitchLanduage(YandexGame.lang);
        UpdateCountText();
    }
    private void OnDestroy()
    {
        PetCell.OnCellSelected -= UpdateUI;
        _invent.OnInventoryChanged -= UpdateCountText;
        _invent.OnPetAdded -= GenerateCell;
    }
    private void SwitchLanduage(string lang)
    {
        if (!_currentCell) return;
        if (lang == "ru")
            _currentPetName.text = _currentCell.Pet.PetData.NameRus;
        else if (lang == "tr")
            _currentPetName.text = _currentCell.Pet.PetData.NameTr;
        else
            _currentPetName.text = _currentCell.Pet.PetData.NameEn;
    }
    private void GenerateCell(PetInInventory pet)
    {
        var cell = Instantiate(_cellPrefab, _cellGOContainer.transform);
        var cellCode = cell.GetComponent<PetCell>();
        if (_cells.Count == 0) _currentCell = cellCode;
        _cells.Add(cellCode);
        InitCell(cellCode, pet);
        UpdateCountText();
    }
    private void UpdateCountText()
    {
        _petsCount.text = $"{_invent.GetPets().Count}/{_invent.MaxPetsInInventory}";
        _equipedPetsCount.text = $"{_invent.GetEquipedCount()}/{_invent.MaxEquippedPets}";
    }
    private void RemoveCell(PetInInventory removablePet)
    {
        PetCell removableCell = _cells.Find(p => p.Pet == removablePet);
        _cells.Remove(removableCell);
        Destroy(removableCell.gameObject);
        UpdateCountText();
    }
    //private void InitCell(PetCell cellCode, PetInInventory petCode)
    //{
    //    cellCode.Pet = petCode;
    //    cellCode.IconPet.sprite = petCode.PetData.Sprite;
    //    cellCode.IconEquiped.SetActive(petCode.IsEquiped);
    //
    private void InitCell(PetCell cellCode, PetInInventory petCode)
    {
        if (petCode.PetData == null)
        {
            Debug.LogError("PetData is null. Cannot initialize cell.");
            return;
        }
        cellCode.Pet = petCode;
        cellCode.IconPet.sprite = petCode.PetData.Sprite;
        cellCode.IconEquiped.SetActive(petCode.IsEquiped);
    }
    private void CheckCurrentCell()
    {
        if (_currentCell == null)
        {
            ToogleUI(false);
        }
        else
        {
            ToogleUI(true);
            //print(_currentCell);
        }
    }
    private void UpdateUI(PetCell cellCode)
    {
        if (_currentCell)
            _currentCell.IconSelected.SetActive(false);
        cellCode.IconSelected.SetActive(true);
        _currentCell = cellCode;
        _currentPetImg.sprite = cellCode.Pet.PetData.Sprite;
        SwitchLanduage(YandexGame.lang);
        CheckCurrentCell();
        if (cellCode.Pet.IsEquiped)
        {
            cellCode.IconEquiped.SetActive(true);
            _buttonEquip.SetActive(false);
            _buttonUnequip.SetActive(true);
        }
        else
        {
            cellCode.IconEquiped.SetActive(false);
            _buttonEquip.SetActive(true);
            _buttonUnequip.SetActive(false);
        }
    }
    private void ToogleUI(bool active)
    {
        _imagePet.SetActive(active);
        _currentPetName.gameObject.SetActive(active);
        _buttonDelete.gameObject.SetActive(active);
        _buttonEquip.gameObject.SetActive(active);
        _buttonUnequip.gameObject.SetActive(active);
    }
    public void Equip()
    {
        _invent.EquipPet(_currentCell.Pet.Id);
        UpdateUI(_currentCell);
    }
    public void UnEquip()
    {
        _invent.UnEquip(_currentCell.Pet.Id);
        UpdateUI(_currentCell);
    }
    public void Remove()
    {
        _invent.DeletePet(_currentCell.Pet.Id);
        RemoveCell(_currentCell.Pet);
        _currentCell = null;
        CheckCurrentCell();
    }
}
