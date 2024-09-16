using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NPCManager : SingletonMonoBehaviour<NPCManager>
{
    [SerializeField] List<Transform> outPoint = new List<Transform>();
    [Header("NPC Manager")]
    [SerializeField] private int unHeadledNPC;
    [SerializeField] List<NPC> npcList = new List<NPC>();
    void Start() {
        unHeadledNPC = npcList.Count;
    }
    public void HealNPC(){
        Debug.Log("INI eror??");
        unHeadledNPC--;
    }
    public int GetUnHealedNPC(){
        return unHeadledNPC;
    }
    public Vector3 GetRandomOutPoint(){
        return outPoint[Random.Range(0, outPoint.Count-1)].position;
    }
}
