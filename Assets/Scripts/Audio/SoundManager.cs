using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    public AudioSource sfx2DSource;
    public float volume = 1f;

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

    public void PlaySound2D(string soundName, float speed = 1)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
        sfx2DSource.pitch = speed;
    }
     public void SetVolume(float volume){
        sfx2DSource.volume = volume;
     }
}
