using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CCannonBehaviour : MonoBehaviour
{
    public delegate void PlaySound(int id, float pitch);
    public static event PlaySound OnPlaySound;
    
    
    public CKnigthBehaviour cKnigthBehaviour;
    public Transform cameraPosition;
    public Transform cameraTransform;
    private Vector3 shootingAngle;

    public float shootVelocity;

    public Transform points;
    public GameObject cannonBall;
  
    
    private void Start()
    {
        cameraTransform.position = cameraPosition.position;
        shootingAngle = new Vector3(0,45,90);
        UpdateGraphic();
    }

    private void OnEnable()
    {
        CTargetBallBehaviour.OnEnterTarget += OnChangeColor;
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

    private void OnChangeColor(bool isEnter)
    {
        if (isEnter)
        {
            ChangeColor(Color.red);
        }

        else
        {
            ChangeColor(Color.white);
        }
    }

    private void ChangeColor(Color color)
    {
        for (int i = 0; i < points.childCount; i++)
        {
            points.GetChild(i).GetComponent<Renderer> ().material.SetColor ("_Color", color);
        }
    }

    private void Update()
    {
        GetInput();

    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //ResetGraphicPosition();
            shootingAngle += new Vector3(0,0,-.25f);
            UpdateGraphic();
            
        }
        
        else if (Input.GetKey(KeyCode.A))
        {
            //ResetGraphicPosition();
            shootingAngle += new Vector3(0,0,.25f);
            UpdateGraphic();
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            shootingAngle += new Vector3(0, .25f,0);;
            UpdateGraphic();
            
        }
        
        else if (Input.GetKey(KeyCode.S))
        {
            
            shootingAngle += new Vector3(0, -.25f,0);
            UpdateGraphic();
        }
        
        else if (Input.GetMouseButtonDown(0))
        {

            ShootBall();
        }
    }

    private void ShootBall()
    {
        
        float rotationZ =  Mathf.Deg2Rad * shootingAngle.z;
        float rotationY =  Mathf.Deg2Rad * shootingAngle.y;
        
        GameObject ball = Instantiate(cannonBall, points.position, Quaternion.identity);
        
        ball.GetComponent<Rigidbody>().velocity = new Vector3((float)(Mathf.Cos(rotationY) * Mathf.Cos(rotationZ)),
                                                    (float)Mathf.Sin(rotationY),
                                                    (float)(Mathf.Cos(rotationY) * Mathf.Sin(rotationZ))) * shootVelocity ;
        
        if (OnPlaySound != null)
            OnPlaySound(0, 1);
        
        
        Destroy(ball, 5f);
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
