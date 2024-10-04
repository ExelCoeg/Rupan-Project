using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Damagetest : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other) {
        IDamagable idamageAble = other.GetComponent<IDamagable>();
        if (idamageAble != null)
        {
            Debug.Log("Damage test running");
            idamageAble.TakeDamage((int)damage);
        }
    }
}
