using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    [Tooltip("Текст на канвасе")]
    [SerializeField] private TextMeshProUGUI _message;//Важно чтобы поле с текстом у самого компонента не было пустым, иначе вылезет ошибка
    [Tooltip("Время МЕЖДУ отображением двух половин одной буквы")]
    [SerializeField] private float _timeBeetweenHalf = 0.5F;
    [Tooltip("Время между отображением двух букв")]
    [SerializeField] private float _timeBetweenChar = 0.3F;
    [Tooltip("Время отображения одной половины буквы")]
    [SerializeField] private float _smoothTime = 0.1F;

    private List<float> _leftAlphas;
    private List<float> _rightAlphas;
    private bool _isAnimating = false;

    //Тут стоит уточнить что в TextMeshPro у каждой буквы имеется четыре вершины => 
    //Каждую букву можно представить в виде квадрата/прямоугольника.
    //При этом у каждой вершины имеется свой индекс.На этом и строится данная анимация

    private void Update()
    {
        if (_isAnimating) SwitchColor();
    }
    private void Start()
    {
        _leftAlphas = new float[_message.text.Length].ToList();
        _rightAlphas = new float[_message.text.Length].ToList();
        PlayAnim();
    }
    public void PlayAnim()
    {
        ShowText(false);//Чтобы текст можно было запустить несколько раз
        _isAnimating = true;
        StartCoroutine(Smooth(0));
    }
    public void ShowInstant()
    {
        _isAnimating = false;
        ShowText(true);
    }

    ///<summary>
    ///Отображает текст.
    ///</summary>
    ///<param name="instant">
    ///Если <c>true</c>, текст отображатеся сразу
    ///Если <c>false</c>, включается анимация
    /// </param>
    private void ShowText(bool instant)
    {
        StopAllCoroutines();
        DOTween.Kill(1);

        for (int i = 0; i < _leftAlphas.Count; i++)
        {
            _leftAlphas[i] = instant ? 255 : 0;
            _rightAlphas[i] = instant ? 255 : 0;
        }

        SwitchColor();

        //if (instant)
        //{
        //    //_message.ForceMeshUpdate();
        //    _message.UpdateVertexData();
        //}
    }

    private void SwitchColor()
    {
        _message.ForceMeshUpdate();
        for (int i = 0; i < _leftAlphas.Count; i++)
        {
            if (_message.textInfo.characterInfo[i].character != '\n' &&
                _message.textInfo.characterInfo[i].character != ' ')
            {
                int meshIndex = _message.textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = _message.textInfo.characterInfo[i].vertexIndex;

                Color32[] vertexColors = _message.textInfo.meshInfo[meshIndex].colors32;

                vertexColors[vertexIndex + 0].a = (byte)_leftAlphas[i];
                vertexColors[vertexIndex + 1].a = (byte)_leftAlphas[i];
                vertexColors[vertexIndex + 2].a = (byte)_rightAlphas[i];
                vertexColors[vertexIndex + 3].a = (byte)_rightAlphas[i];
            }
            _message.UpdateVertexData();
        }
        _message.UpdateVertexData();
    }
    private IEnumerator Smooth(int i)
    {
        if (i >= _leftAlphas.Count)
            yield break;

        DOTween.To(
            () => _leftAlphas[i],
            x => _leftAlphas[i] = x,
            255,
            _smoothTime)
            .SetEase(Ease.Linear)
            .SetId(1);
        yield return new WaitForSeconds(_timeBeetweenHalf);

        DOTween.To(
            () => _rightAlphas[i],
            x => _rightAlphas[i] = x,
            255,
            _smoothTime)
            .SetEase(Ease.Linear)
            .SetId(1);
        yield return new WaitForSeconds(_timeBetweenChar);
        StartCoroutine(Smooth(i + 1));
    }
}
