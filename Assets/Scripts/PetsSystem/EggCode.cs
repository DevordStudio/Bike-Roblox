using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EggCode : MonoBehaviour
{
    [SerializeField] private PetsData[] _pets;
    [SerializeField] private int _eggPrice;
    [SerializeField] private BankVolute _bank;
    [SerializeField] private Sprite _eggSprite;
    [SerializeField] private EggUIControll _uiHandler;

    private bool _playerIsNear;

    private void Start()
    {
        _uiHandler ??= FindAnyObjectByType<EggUIControll>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out bicycle_code player)) _playerIsNear = true;
    }
    private void Update()
    {
        if (_playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            GetEgg();//помен€ть логику покупки €йца
        }
    }
    public void GetEgg()
    {
        if (_bank.GetMoney() >= _eggPrice)
        {
            _bank.DecreaseMoney(_eggPrice);
            int randomInd = Random.Range(0, _pets.Length);
            PetsData droppedPet = _pets[randomInd];
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => _uiHandler.EggAnim(_eggSprite, droppedPet.Sprite));
            sequence.AppendCallback(() => droppedPet.Drop());
            sequence.Play();
        }
    }
}
