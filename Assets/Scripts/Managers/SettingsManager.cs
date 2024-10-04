using UnityEngine;
public class SettingsManager : SingletonMonoBehaviour<SettingsManager>{
    public void SaveSettings(){
        print("Saving Settings");
        PlayerPrefs.SetFloat("sfxVolume", SoundManager.instance.sfx2DSource.volume);
        PlayerPrefs.SetFloat("musicVolume", MusicManager.instance.musicSource.volume);
        PlayerPrefs.SetInt("qualityLevel", QualitySettings.GetQualityLevel());
    }
  
    public void ResetSettings(){
        PlayerPrefs.SetFloat("sfxVolume",1);
        PlayerPrefs.SetFloat("musicVolume", 1);
        PlayerPrefs.SetInt("qualityLevel", 2);
    }
    private void OnApplicationQuit() {
        SaveSettings();
    }
}