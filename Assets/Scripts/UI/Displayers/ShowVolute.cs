using TMPro;
using UnityEngine;

public class ShowVolute : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    private void UpdateVolute(int money) => _moneyText.text = money.ToString();
    private void Start() => BankVolute.OnMoneyValueChanged += UpdateVolute;
    private void OnDestroy() => BankVolute.OnMoneyValueChanged -= UpdateVolute;
}
