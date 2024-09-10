using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NPCPoint{
    
}
public class NPCManager : SingletonMonoBehaviour<NPCManager>
{
    [SerializeField] List<NPCPoint> nPCPoints = new List<NPCPoint>();
    public Vector3 GetRandomWalkPoint(NPCPoint npcPoint){
        return Vector3.zero;
    }
}
