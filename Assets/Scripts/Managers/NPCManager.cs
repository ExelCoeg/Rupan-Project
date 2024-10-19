using System.Collections.Generic;
using UnityEngine;
public class NPCManager : SingletonMonoBehaviour<NPCManager>
{
    [SerializeField] List<Transform> outPoint = new List<Transform>();
    [Header("NPC Manager")]
    [SerializeField] private int healedNPC;
    [SerializeField] private List<WalkPointList> walkPoints = new List<WalkPointList>();
  
    public void HealNPC(){
        healedNPC++;
    }
    public int GetHealedNPC(){
        return healedNPC;
    }
    public Vector3 GetRandomOutPoint(){
        return outPoint[Random.Range(0, outPoint.Count-1)].position;
    }
    public WalkPointList GetWalkPointList(int index){
        return walkPoints[index];
    }
}

[System.Serializable]
public class WalkPointList{
    public List<Transform> walkPoints = new List<Transform>();

    public Vector3 GetRandomWalkPoint(){
        return walkPoints[Random.Range(0, walkPoints.Count-1)].position;
    }
}
