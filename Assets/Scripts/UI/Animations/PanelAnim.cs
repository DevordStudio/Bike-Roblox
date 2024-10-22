using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PanelAnim : MonoBehaviour
{
    [SerializeField] private PanelAnimType _animationType;
    [SerializeField, ShowIf("IsSlideAnim"), HideIf("IsNone")] private RectTransform[] _animPoints;
    [SerializeField, ShowIf("IsScaleAnim"), HideIf("IsNone")] private float[] scaleValues;
    [SerializeField] private float _totalDuration;
    [SerializeField] private RectTransform _rectTransform;
    //private void Start()
    //{
    //    PlayAnimEnable();
    //}
    private bool IsSlideAnim()
    {
        if (_animationType == PanelAnimType.SlideAnim)
            return true;
        else return false;
    }
    private bool IsScaleAnim()
    {
        if (_animationType == PanelAnimType.ScaleAnim)
            return true;
        else return false;
    }
    private bool IsNone()
    {
        if (_animationType == PanelAnimType.None) return true;
        else return false;
    }

    public void PlayAnimEnable()
    {
        //if(_rectTransform.gameObject.activeSelf) return;

        switch (_animationType)
        {
            case PanelAnimType.SlideAnim:
                SlideAnimEnable();
                break;
            case PanelAnimType.ScaleAnim:
                ScaleAnimEnable();
                break;
            case PanelAnimType.None:
                _rectTransform.gameObject.SetActive(true);
                break;
        }
    }
    public void PlayAnimDisable()
    {
        switch (_animationType)
        {
            case PanelAnimType.SlideAnim:
                SlideAnimDisable();
                break;
            case PanelAnimType.ScaleAnim:
                ScaleAnimDisable();
                break;
            case PanelAnimType.None:
                _rectTransform.gameObject.SetActive(false);
                break;
        }
    }
    private void SlideAnimEnable()
    {
        if (_animPoints == null || _animPoints.Length == 0)
        {
            Debug.LogError("Массив точек пустой!");
            return;
        }
        _rectTransform.position = _animPoints[0].position;
        _rectTransform.gameObject.SetActive(true);

        float timePerPoint = _totalDuration / _animPoints.Length;

        Sequence sequence = DOTween.Sequence();

        foreach (RectTransform point in _animPoints)
        {
            sequence.Append(_rectTransform.DOMove(point.position, timePerPoint));
        }
        sequence.Play();
    }
    private void SlideAnimDisable()
    {
        if (_animPoints == null || _animPoints.Length == 0)
        {
            Debug.LogError("Массив точек пустой!");
            return;
        }

        _rectTransform.position = _animPoints[_animPoints.Length - 1].position;

        float timePerPoint = _totalDuration / _animPoints.Length;

        Sequence sequence = DOTween.Sequence();

        for (int i = _animPoints.Length - 1; i >= 0; i--)
        {
            sequence.Append(_rectTransform.DOMove(_animPoints[i].position, timePerPoint));
        }
        sequence.AppendCallback(() =>
        {
            _rectTransform.gameObject.SetActive(false);
        });
        sequence.Play();
    }
    private void ScaleAnimEnable()
    {
        if (scaleValues == null || scaleValues.Length == 0)
        {
            Debug.LogError("Массив значений пустой!");
            return;
        }

        _rectTransform.localScale = Vector3.one * scaleValues[0];
        _rectTransform.gameObject.SetActive(true);

        float timePerStep = _totalDuration / scaleValues.Length;

        Sequence sequence = DOTween.Sequence();

        foreach (float scaleValue in scaleValues)
        {
            sequence.Append(_rectTransform.DOScale(Vector3.one * scaleValue, timePerStep));
        }
        sequence.Play();
    }
    private void ScaleAnimDisable()
    {
        if (scaleValues == null || scaleValues.Length == 0)
        {
            Debug.LogError("Массив значений пустой!");
            return;
        }

        _rectTransform.localScale = Vector3.one * scaleValues[scaleValues.Length - 1];

        float timePerStep = _totalDuration / scaleValues.Length;

        Sequence sequence = DOTween.Sequence();

        for (int i = scaleValues.Length - 1; i >= 0; i--)
        {
            sequence.Append(_rectTransform.DOScale(Vector3.one * scaleValues[i], timePerStep));
        }
        sequence.AppendCallback(() =>
        {
            _rectTransform.gameObject.SetActive(false);
        });
        sequence.Play();
    }
}
public enum PanelAnimType
{
    None,
    SlideAnim,
    ScaleAnim
}
