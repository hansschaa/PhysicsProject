using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTargetBallBehaviour : MonoBehaviour
{
    public delegate void EnterTarget(bool isEnter);

    public static event EnterTarget OnEnterTarget;
   
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
            
            if (OnEnterTarget != null)
                OnEnterTarget(true);


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (OnEnterTarget != null)
                OnEnterTarget(false);
        }
    }
}
