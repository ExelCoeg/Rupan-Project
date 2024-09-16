using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Door : InteractableObject
{
    public bool isOpen = false;
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1f;
    [Header("Rotation Configs")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float forwardDirection = 0;
    private Vector3 startRotation;
    private Vector3 forward;

    private Coroutine animationCoroutine;
    public override void Interacted()
    {
        if(isOpen){
            Close();
        }else{
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Open(player.transform.position);
        }
    }
    
    public override void Awake() {
        base.Awake();
        startRotation = transform.rotation.eulerAngles;
        forward = transform.forward;
    }
    public void Open(Vector3 userPosition){
        if(!isOpen){
            if(animationCoroutine != null){
                StopCoroutine(animationCoroutine);
            }
            if(isRotatingDoor){
                float dot = Vector3.Dot(forward, (userPosition - transform.position).normalized);
                Debug.Log($"Dot: {dot.ToString("N3")}");
                animationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }
    private IEnumerator DoRotationOpen(float forwardAmount){
        Quaternion StartRotation = transform.rotation;
        Quaternion endRotation;
        if(forwardAmount >= forwardDirection)
            endRotation = Quaternion.Euler(new Vector3(0,startRotation.y + rotationAmount,0));
        else{
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y - rotationAmount, 0));
        }
        isOpen = true;
        float time = 0;
        while(time<1){
            transform.rotation = Quaternion.Slerp(StartRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed; 
        }
        
    }
    public void Close(){
        if(isOpen){
            if(animationCoroutine != null){
                StopCoroutine(animationCoroutine);
            }
            if(isRotatingDoor){
                animationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }
    private IEnumerator DoRotationClose(){
        Quaternion StartRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(startRotation);

        isOpen = false;
        
        float time = 0;
        while(time<1){
            transform.rotation = Quaternion.Slerp(StartRotation, endRotation, time);
            yield return null;
            time+= Time.deltaTime * speed;
        }
    }
}
