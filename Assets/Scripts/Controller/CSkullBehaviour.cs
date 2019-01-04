using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkullBehaviour : MonoBehaviour
{
    public delegate void PlaySound(int id, float pitch);
    public static event PlaySound OnPlaySound;

    public Transform player;

    private void LateUpdate()
    {
        
        Vector3 relativePos = player.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (OnPlaySound != null)
                OnPlaySound(1,1);
            
            
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Surface_Water"))
        {
            Destroy(this.gameObject);
        }
    }
}
