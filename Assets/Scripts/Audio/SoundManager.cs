using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
    public static SoundManager instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    public AudioSource sfx2DSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update() {
        if(GameManager.instance.isPaused){
            sfx2DSource.Pause();
        }
        else{
            sfx2DSource.UnPause();
        }
    }
    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        // PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
}