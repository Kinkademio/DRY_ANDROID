using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    private float _baseGameVolume = 0.5f;
    [SerializeField] private Slider _soundSlider;

    void Start()
    {
        loadVolume();
    }

    public void loadVolume()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", _baseGameVolume);
        }
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        _soundSlider.value = PlayerPrefs.GetFloat("volume");
    }

    public void changeVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        AudioListener.volume = volume;
        _soundSlider.value = PlayerPrefs.GetFloat("volume");
    }
}
