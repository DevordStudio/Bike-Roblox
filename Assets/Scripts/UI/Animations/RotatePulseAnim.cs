using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class RotatePulseAnim : MonoBehaviour
{
    [SerializeField] private AnimType _animationType;
    [SerializeField, ShowIf("IsRotate"), HideIf("IsNone")] private float _rotationSpeed = 100F;
    [SerializeField, ShowIf("IsPulse"), HideIf("IsNone")] private float _scaleFactor = 1.5F;
    [SerializeField, ShowIf("IsPulse"), HideIf("IsNone")] private float _animationTime = 1F;
    [SerializeField, HideIf("IsNone")] private Transform _target;
    [SerializeField, HideIf("IsNone")] private bool _playOnAwake = true;
    #region BoolMarks
    private bool IsNone()
    {
        if (_animationType == AnimType.None) return true;
        return false;
    }
    private bool IsPulse()
    {
        if (_animationType == AnimType.Pulse) return true;
        return false;
    }
    private bool IsRotate()
    {
        if (_animationType == AnimType.Rotate) return true;
        return false;
    }
    #endregion
    public enum AnimType
    {
        None,
        Pulse,
        Rotate
    }
    private void Start()
    {
        if (_playOnAwake && _target)
            Play();
    }
    public void Play()
    {
        switch (_animationType)
        {
            case AnimType.None:
                return;
            case AnimType.Pulse:
                AnimPulse();
                break;
            case AnimType.Rotate:
                AnimRotate();
                break;
        }
    }
    private void AnimRotate()
    {
        _target.transform.DOLocalRotate(new Vector3(0, 0, 360), 360f / _rotationSpeed, RotateMode.FastBeyond360)
                  .SetEase(Ease.Linear)
                  .SetLoops(-1, LoopType.Restart);
    }
    private void AnimPulse()
    {
        _target.transform.DOScale(_scaleFactor, _animationTime / 2)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo);
    }
}
