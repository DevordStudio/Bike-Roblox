using DG.Tweening;
using UnityEngine;

public class AnimRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 1f / (rotationSpeed / 360f), RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
}
