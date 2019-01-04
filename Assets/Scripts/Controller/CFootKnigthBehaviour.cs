using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class CFootKnigthBehaviour : MonoBehaviour
{
    public delegate void TouchMushroom();

    public static event TouchMushroom OnTouchMushroom;

    public LayerMask whatIsBounceLayer;
    public LayerMask whatIsGround;

    private bool knigthGroundBool;
    private CKnigthBehaviour cKnigthBehaviour;


    private void Start()
    {
        
        cKnigthBehaviour = this.transform.parent.transform.parent.GetComponent<CKnigthBehaviour>();

    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, .2f, whatIsBounceLayer))
        { 
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, .3f);
            
            hit.collider.gameObject.GetComponent<CMushroomBehaviour>().InBounce();
            
            if (OnTouchMushroom != null)
                OnTouchMushroom();
        }


        cKnigthBehaviour.isGround = Physics.Raycast(transform.position, Vector3.down, .2f, whatIsGround);
    }
}
