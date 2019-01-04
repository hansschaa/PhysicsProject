using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCannonImpulseBehaviour : MonoBehaviour
{
    private Vector3 shootingAngle;
    public float shootVelocity;
    public Transform points;
    public GameObject player;
    public GameObject playerCamera;
    public GameObject impulseCannonCamera;
    
    private void Start()
    {
        shootingAngle = new Vector3(0,45,90);
        UpdateGraphic();
    }
    
    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            shootVelocity += .1f;
            UpdateGraphic();

        }

        else if (Input.GetKey(KeyCode.S))
        {

            shootVelocity -= .1f;
            UpdateGraphic();
        }

        else if (Input.GetMouseButtonDown(0))
        {
            ShootPlayer();
        }
    }

    public void ShootPlayer()
    {
        float rotationZ =  Mathf.Deg2Rad * shootingAngle.z;
        float rotationY =  Mathf.Deg2Rad * shootingAngle.y;
        
        Vector3 velocity = new Vector3((float)(Mathf.Cos(rotationY) * Mathf.Cos(rotationZ)),
                               (float)Mathf.Sin(rotationY),
                               (float)(Mathf.Cos(rotationY) * Mathf.Sin(rotationZ))) * shootVelocity ;

        player.GetComponent<CKnigthBehaviour>().Shooted(velocity);
        ResetGraphicPosition();

        /*
        player.GetComponent<Rigidbody>().isKinematic = false;
        
        player.GetComponent<Rigidbody>().velocity = new Vector3((float)(Mathf.Cos(rotationY) * Mathf.Cos(rotationZ)),
                                                      (float)Mathf.Sin(rotationY),
                                                      (float)(Mathf.Cos(rotationY) * Mathf.Sin(rotationZ))) * shootVelocity ;

        player.GetComponent<CKnigthBehaviour>().ePlayerInputMode = EPlayerInputMode.FREEMOVE;*/
        
        playerCamera.SetActive(true);
        impulseCannonCamera.SetActive(false);


        GetComponent<CCannonImpulseBehaviour>().enabled = false;
        /*
        if (OnPlaySound != null)
            OnPlaySound(0, 1);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CKnigthBehaviour>().collidedCannon = true;
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CKnigthBehaviour>().collidedCannon = false;
        }
    }
    
    public void UpdateGraphic()
    {
        ResetGraphicPosition();
        float rotationZ =  Mathf.Deg2Rad * shootingAngle.z;
        float rotationY =  Mathf.Deg2Rad * shootingAngle.y;

        Vector3 velocityIni = new Vector3((float)(Mathf.Cos(rotationY) * Mathf.Cos(rotationZ)),
                                  (float)Mathf.Sin(rotationY),
                                  (float)(Mathf.Cos(rotationY) * Mathf.Sin(rotationZ))) * shootVelocity ;

        Vector3 posIni = points.GetChild(0).transform.position;

        for (int i = 0; i < points.childCount; i++)
        {
            //Update the position of the mini ball
            UpdateGraphicBallPosition(points.GetChild(i).gameObject, i,ref velocityIni,ref posIni);
        }
    }

    private void UpdateGraphicBallPosition(GameObject o, int i, ref Vector3 velocity, ref Vector3 pos)
    {

        //Add Gravity
        velocity += new Vector3(0, -9.81f , 0) * Time.deltaTime;
        
        pos += velocity * Time.deltaTime;
        
        o.transform.position = pos;
    }

    public void ResetGraphicPosition()
    {
        foreach (Transform point in points)
        {
            point.localPosition = Vector3.zero;
        }
    }
}
