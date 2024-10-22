using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EggUIControll : MonoBehaviour
{
    [SerializeField] private int _shakeCount = 5;
    [SerializeField] private float _shakeDuration = 0.1F;
    [SerializeField] private float _shakeAngle = 15F;
    [SerializeField] private Image _eggImg;
    [SerializeField] private Image _petImg;
    [SerializeField] private GameObject _dropUI;
    [SerializeField] private GameObject _repeatPanel;
    [SerializeField] private PanelAnim _eggAnim;

    public Sprite EggTest;
    public Sprite PetTest;
    //public void ToogleRepeatUI(bool active, int reward)
    //{
    //    _repeatPanel.SetActive(active);
    //    if (active)
    //    {
    //        TMP_Text text = _repeatPanel.GetComponent<TMP_Text>();
    //        text.text = $"У вас уже есть этот питомец. Вы получите {reward} монет";
    //    }
    //}
    public void Start()
    {
        EggAnim(EggTest, PetTest);
    }
    public void EggAnim(Sprite eggSprite, Sprite petSprite)
    {
        _eggImg.sprite = eggSprite;
        _petImg.sprite = petSprite;
        _petImg.gameObject.SetActive(false);
        OpenEggAnim();
    }
    private void OpenEggAnim()
    {
        _dropUI.SetActive(true);
        //PanelAnim anim = _eggImg.GetComponent<PanelAnim>();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => _eggAnim.PlayAnimEnable());
        sequence.AppendCallback(() => ShakeAndDisable());
        //sequence.AppendCallback(() => _petImg.gameObject.SetActive(true));
        sequence.Play();
    }
    private void ShakeAndDisable()
    {
        Sequence shakeSequence = DOTween.Sequence();

        float shakeDur = _shakeDuration;
        //float _shakeAcceleration = shakeDur / 4;
        for (int i = 0; i < _shakeCount; i++)
        {
            shakeSequence.Append(_eggImg.transform.DORotate(new Vector3(0, 0, _shakeAngle), shakeDur).SetEase(Ease.InOutSine));
            shakeSequence.Append(_eggImg.transform.DORotate(new Vector3(0, 0, -_shakeAngle), shakeDur).SetEase(Ease.InOutSine));
            //shakeDur -= _shakeAcceleration;
        }

        shakeSequence.Append(_eggImg.transform.DORotate(Vector3.zero, shakeDur).SetEase(Ease.InOutSine))
                     .OnComplete(() =>
                     {
                         _eggImg.gameObject.SetActive(false);
                         _petImg.gameObject.SetActive(true);
                     });
    }
}
