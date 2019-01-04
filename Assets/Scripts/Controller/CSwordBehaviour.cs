using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSwordBehaviour : MonoBehaviour
{
    public delegate void SwordCollision(Rigidbody rb);

    public static event SwordCollision OnSwordCollision;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (OnSwordCollision != null)
                OnSwordCollision(other.gameObject.GetComponent<Rigidbody>());
        }  
    }
    
   
}
