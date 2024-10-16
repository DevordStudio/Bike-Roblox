using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField, ShowIf(nameof(IsScale)), HideIf(nameof(IsScale))] private float _animScale = 1.3F;
    [SerializeField, ShowIf(nameof(IsSlide)), HideIf(nameof(IsNone))] private float _moveValue = 100;
    [SerializeField] private float _animTime = 0.6F;
    [SerializeField] private AnimType _animation;

    private bool IsNone()
    {
        if(_animation == AnimType.None)
            return true;
        return false;
    }
    private bool IsScale()
    {
        if(_animation == AnimType.Scale) 
            return true;
        return false;
    }
    private bool IsSlide()
    {
        if(_animation == AnimType.DownSlide)
            return true;
        return false;
    }
    private RectTransform _rect;
    private bool _isMoving;

    private enum AnimType
    {
        None,
        Scale,
        DownSlide,
    }
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }
    public void OnEnter()
    {
        switch (_animation)
        {
            case AnimType.None:
                return;
            case AnimType.Scale:
                ScaleOnEnter();
                break;
            case AnimType.DownSlide:
                MoveDown();
                break;
        }
    }
    public void OnExit()
    {
        switch (_animation)
        {
            case AnimType.None:
                return;
            case AnimType.Scale:
                ScaleOnExit();
                break;
            case AnimType.DownSlide:
                MoveUp();
                break;
        }
    }
    private void MoveDown()
    {
        Vector3 startPosition = _rect.anchoredPosition;
        _isMoving = true;
        _rect.DOAnchorPosY(startPosition.y - _moveValue, _animTime / 2).SetEase(Ease.OutQuad).OnComplete(() => _isMoving = false);
    }
    private void MoveUp()
    {
        Vector3 startPosition = _rect.anchoredPosition;
        _isMoving = true;
        _rect.DOAnchorPosY(startPosition.y + _moveValue, _animTime / 2).SetEase(Ease.OutQuad).OnComplete(() => _isMoving = false);
    }
    private void ScaleOnEnter() => transform.DOScale(_animScale, _animTime / 2);
    private void ScaleOnExit() => transform.DOScale(1F, _animTime / 2);
    private void OnDisable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        if (_isMoving)
        {
            _rect.anchoredPosition = _rect.anchoredPosition + Vector2.up * _moveValue;
        }
    }
}
