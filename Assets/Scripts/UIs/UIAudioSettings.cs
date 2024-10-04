using UnityEngine.UI;
using UnityEngine;

public class UIAudioSettings : UIBase
{
    public Slider sfxSlider;
    public Slider musicSlider;
    private void Start() {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume",1f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume",1f);
    }
    
    private void Update() {
        SoundManager.instance.SetVolume(sfxSlider.value);
        MusicManager.instance.SetVolume(musicSlider.value);
    }

}
