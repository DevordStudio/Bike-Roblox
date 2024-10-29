using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EggCode : MonoBehaviour
{
    [SerializeField] private PetsData[] _pets;
    [SerializeField] private int _eggPrice;
    [SerializeField] private BankVolute _bank;
    [SerializeField] private EggUIControll _uiHandler;
    [SerializeField] private GameObject _worldEggUI;
    [SerializeField] private MeshRenderer _meshRender;

    private void Start()
    {
        //_meshRender = GetComponent<MeshRenderer>();
        _uiHandler ??= FindAnyObjectByType<EggUIControll>();
        Button buttonOpen = _worldEggUI.GetComponentInChildren<Button>();
        buttonOpen.onClick.AddListener(OpenUI);
        _worldEggUI.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out bicycle_code player))
            _worldEggUI.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out bicycle_code player))
            _worldEggUI.SetActive(false);
    }
    [Button]
    private void OpenUI()
    {
        _uiHandler.OpenEggUI(this);
    }
    [Button]
    public void BuyEgg()
    {
        if (_bank.GetMoney() >= _eggPrice)
        {
            _bank.DecreaseMoney(_eggPrice);
            GetEgg();
            Debug.Log("яйцо куплено");
        }
    }
    public void GetEgg()
    {
        int randomInd = UnityEngine.Random.Range(0, _pets.Length);
        PetsData droppedPet = _pets[randomInd];
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => _uiHandler.EggAnim(_meshRender.material.color, droppedPet.Sprite));
        sequence.AppendCallback(() => droppedPet.Drop());
        sequence.Play();
    }
    public PetsData[] GetPets() => _pets;
    public Color GetColor() => _meshRender.material.color;
    public int GetPrice () => _eggPrice;
}
