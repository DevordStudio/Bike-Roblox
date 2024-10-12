using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource[] _musicSources;
    [SerializeField] private AudioSource[] _effectSources;
    //[SerializeField] private 

    public void MusicVolume(Slider volumeSlider)
    {
        foreach (var source in _musicSources)
        {
            source.volume = volumeSlider.value / volumeSlider.maxValue;
        }
    }
    public void EffectVolume(Slider volumeSlider)
    {
        foreach (var source in _effectSources)
        {
            source.volume = volumeSlider.value / volumeSlider.maxValue;
        }
    }
}
