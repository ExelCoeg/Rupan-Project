using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : SingletonMonoBehaviour<CutsceneManager>
{
    public List<PlayableDirector> cutscenes;
   
    public void PlayCutscene(int index)
    {
        cutscenes[index].Play();   
    }
}
