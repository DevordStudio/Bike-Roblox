using NaughtyAttributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    [SerializeField] private Transform _circle;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _offset;
    [SerializeField] private float _speedRotate = 0.5f;
    [SerializeField] private float _minCountCircleRotate = 1;
    [SerializeField] private AnimationCurve _movementCurve;
    public ItemRoulette[] Items;
    [SerializeField] private BankVolute _bank;
    //[SerializeField] private Fade[] _fades;
    private bool _isRotate = false;

    public event Action EndRotateEvent;

    [Header("Gizmos")]
    [SerializeField] private float _radius = 150f;
    private Vector3 _beginRotation = Vector3.zero;


    /// <summary>
    /// Запускает рулетку
    /// </summary>
    /// <returns>true/false успешен ли запуск рулетки</returns>
    [ContextMenu("Start Rotate")]
    public bool Rotate()
    {
        if (_isRotate)
            return false;
        _audioSource.Play();
        StartCoroutine(nameof(Rotating));
        return true;
    }

    /// <summary>
    /// Считает шансы призов в рулетке
    /// </summary>
    /// <returns>Индекс предмета из рулетки</returns>
    private int CalculatorChanceItem()
    {
        int allChances = 0;
        int tempChances = 0;
        int length = Items.Length;

        for (int i = 0; i < length; i++)
        {
            allChances += Items[i].Chance;
        }

        int chance = UnityEngine.Random.Range(0, allChances);

        for (int i = 0; i < length; i++)
        {
            if (i > 0)
            {
                if (chance > tempChances && chance <= Items[i].Chance + tempChances) return i;
            }
            else
            {
                if (chance <= Items[i].Chance) return i;
            }

            tempChances += Items[i].Chance;
        }

        return 0;
    }

    private IEnumerator Rotating()
    {
        _isRotate = true;

        //for (int i = 0; i < _fades.Length; i++)
        //{
        //    yield return new WaitForSeconds(0.1F);
        //    _fades[i].StartAnimation();
        //}

        var t = 0f;
        var indexPrize = CalculatorChanceItem();
        var anglePrize = 360 / Items.Length;
        var lastAngle = 0f;
        var to = Vector3.zero;

        while (t <= 1)
        {
            t += Time.deltaTime * _speedRotate;

            to.z = _movementCurve.Evaluate(t) * (_minCountCircleRotate * 360 + anglePrize * (indexPrize + 1) - anglePrize / 2) + _offset;
            _circle.localEulerAngles = Vector3.Lerp(_beginRotation, to, t);

            // Звук рулетки
            //if (lastAngle + anglePrize <= to.z)
            //{
            //    lastAngle = to.z;
            //    _audioSource.Play();
            //}

            yield return null;
        }

        int multiplay = 0;

    // Нормализация угла под Unity
    loop:
        _beginRotation = new Vector3(0, 0, _circle.localEulerAngles.z - 360 * multiplay);
        if (_beginRotation.z > 180)
        {
            multiplay++;
            goto loop;
        }

        Debug.Log($"Из рулетки выпало {Items[indexPrize].Name}");
        Items[indexPrize].ThisItemChoiceEvent?.Invoke();
        Items[indexPrize].Drop(_bank);
        EndRotateEvent?.Invoke();

        _isRotate = false;
    }
    private void OnDrawGizmos()
    {
        if (_circle == null)
            return;

        // рисуем линии, которые разделяют круг на несколько частей
        int length = Items.Length;
        for (int i = 0; i < length; i++)
        {
            float angle = 360 / length * i;
            Vector3 startPoint = _circle.position;
            Vector3 endPoint = _circle.position + new Vector3(Mathf.Sin((angle + _offset) * Mathf.Deg2Rad) * _radius, Mathf.Cos((angle + _offset) * Mathf.Deg2Rad) * _radius, 0f);

            Gizmos.color = i <= 1 ? Color.black : Color.green;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }

}

[System.Serializable]
public class ItemRoulette
{
    [SerializeField] private EggCode _egg;
    public int MoneyReward;
    public RewardType Type;
    public Image ImageOnRoulette;
    public TMP_Text Text;
    public Sprite Sprite;
    public string Name;
    [Range(1, 99)] public int Chance;
    // Это событие будет вызыватся при выпадении этого приза
    public UnityEvent ThisItemChoiceEvent;
    public enum RewardType
    {
        None,
        Egg,
        Money
    }
    public void Drop(BankVolute bank)
    {
        switch (Type)
        {
            case RewardType.None:
                return;
            case RewardType.Egg:
                _egg.GetEgg();
                break;
            case RewardType.Money:
                bank.IncreaseMoney(MoneyReward);
                break;
        }
    }
}