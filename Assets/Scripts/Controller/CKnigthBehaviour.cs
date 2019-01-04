using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CKnigthBehaviour : MonoBehaviour
{
    #region "Events"
    public delegate void PlaySound(int id, float pitch);
    public static event PlaySound OnPlaySound;
    
    public delegate void ShowSaveMessage();
    public static event ShowSaveMessage OnShowSaveMessage;
    
    public delegate void ShowUIButton(EUIButton euiButton);
    public static event ShowUIButton OnShowUIButton;
    
    public delegate void HideUIButton();
    public static event HideUIButton OnHideUIButton;
    #endregion
 
    
    #region"Movement"
    public EPlayerInputMode ePlayerInputMode;
    public bool isGround;
    public float moveSpeed;
    public float turnSpeed;
    public float jumpForce;
    private Rigidbody m_rb;
    public float strength;
    public float mushroomForce;
    #endregion


    #region "MyComponents"
    private CharacterController controller;
    private Animator anim;
    private Vector3 lastPosition;
    private GameObject currentHinge;
    #endregion
   

    #region "Cameras"
    public GameObject cannonNormalCamera;
    public GameObject playerNormalCamera;
    public GameObject impulseCannonCamera;
    #endregion

    
    #region "Other"
    
    #endregion

    public bool collidedCannon;
    
    private void Start()
    {
        lastPosition = this.transform.position;
            
            
        m_rb = GetComponent<Rigidbody>();
        collidedCannon = false;
        ePlayerInputMode = EPlayerInputMode.FREEMOVE;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        
        GetInput();
        //print("Is grounded: " + controller.isGrounded);
    }

    public void FixedUpdate()
    {
        if(ePlayerInputMode == EPlayerInputMode.FREEMOVE)
            Movement();
    }




    private void OnEnable()
    {
        CSwordBehaviour.OnSwordCollision += CollisionWhitEnemy;
        CFootKnigthBehaviour.OnTouchMushroom += ApplyVerticalForce;
    }

    private void OnDisable()
    {
        CSwordBehaviour.OnSwordCollision -= CollisionWhitEnemy;    
        CFootKnigthBehaviour.OnTouchMushroom -= ApplyVerticalForce;
    }

    public void OnGround()
    {
        isGround = true;
    }

    private void CollisionWhitEnemy(Rigidbody rb)
    {
        if (this.anim.GetBool("Attacking"))
        {
            rb.AddForce(transform.forward * strength,ForceMode.Impulse);
            if (OnPlaySound != null)
                OnPlaySound(0,1);
            
        } 
    }

    private void ApplyVerticalForce()
    {
        print("apply");
        m_rb.velocity = new Vector3(m_rb.velocity.x, mushroomForce, m_rb.velocity.z);
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            if (!anim.GetBool("Attacking"))
            {
                anim.SetBool("Running", true);
                anim.SetInteger("Condition", 1);
            }

        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
            if (!anim.GetBool("Attacking"))
            {
                anim.SetBool("Running", true);
                anim.SetInteger("Condition", 1);
            }
            
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("Running", false);
            anim.SetInteger("Condition", 0);
        }

        if(Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        
        if(Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        
        if (Input.GetMouseButtonDown(1) && isGround)
        {
            isGround = false;
            m_rb.velocity = new Vector3(m_rb.velocity.x, jumpForce , m_rb.velocity.z);
        }


       
    }

    
    void GetInput()
    {
        if (ePlayerInputMode == EPlayerInputMode.FREEMOVE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                if (anim.GetBool("Running"))
                {
                    anim.SetBool("Running", false);
                    anim.SetInteger("Condition", 0);
                }
            
                if (!anim.GetBool("Running"))
                {
                    Attacking();
                } 
            }
        }

        if (ePlayerInputMode == EPlayerInputMode.INHINGE)
        {
            
            if (Input.GetKey(KeyCode.W))
            {
                GetComponent<Rigidbody>().velocity += transform.forward * .5f;
                

            }

            if (Input.GetKey(KeyCode.S))
            {
                GetComponent<Rigidbody>().velocity += transform.forward * -.5f;
            
            }
            
            if (Input.GetKey(KeyCode.E))
            {
                
                currentHinge = null;
                Destroy(GetComponent<HingeJoint>());
                ePlayerInputMode = EPlayerInputMode.FREEMOVE;
                //GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * .02f, ForceMode.Impulse);
               

            }
        }
    }

    private void Attacking()
    {
        StartCoroutine("AttackRoutine");
    }

    IEnumerator AttackRoutine()
    {
        anim.SetBool("Attacking", true);
        anim.SetInteger("Condition", 2);
        yield return new WaitForSeconds(.12f);
        anim.SetInteger("Condition",0);
        anim.SetBool("Attacking", false);
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cannon"))
        {
            if (ePlayerInputMode == EPlayerInputMode.CANNON)
            {
                if (OnShowUIButton != null)
                    OnShowUIButton(EUIButton.Q);
            }
            
            else if(ePlayerInputMode == EPlayerInputMode.FREEMOVE)
            {
                if (OnShowUIButton != null)
                    OnShowUIButton(EUIButton.E);
            }

            if (Input.GetKeyDown(KeyCode.E) && collidedCannon)
            {
                anim.SetBool("Running",false);
                anim.SetBool("Attacking",false);
                anim.SetInteger("Condition", 0);
                
                ePlayerInputMode = EPlayerInputMode.CANNON;
                other.GetComponent<CCannonBehaviour>().enabled = true;
                cannonNormalCamera.SetActive(true);
                playerNormalCamera.SetActive(false);
            }
            
            else if (Input.GetKeyDown(KeyCode.Q) && collidedCannon)
            {
                playerNormalCamera.SetActive(true);
                cannonNormalCamera.SetActive(false);
                other.GetComponent<CCannonBehaviour>().enabled = false;
                ePlayerInputMode = EPlayerInputMode.FREEMOVE;
            }
        }
        
        else if (other.CompareTag("ImpulseCannon"))
        {
            if (ePlayerInputMode == EPlayerInputMode.INCANNON)
            {
                if (OnShowUIButton != null)
                    OnShowUIButton(EUIButton.Q);
            }
            
            else if(ePlayerInputMode == EPlayerInputMode.FREEMOVE)
            {
                if (OnShowUIButton != null)
                    OnShowUIButton(EUIButton.E);
            }
            
            if (Input.GetKeyDown(KeyCode.E) && collidedCannon)
            {
                transform.position = other.gameObject.transform.GetChild(0).transform.position;
                GetComponent<Rigidbody>().isKinematic = true;
                
                anim.SetBool("Running",false);
                anim.SetBool("Attacking",false);
                anim.SetInteger("Condition", 0);
                
                ePlayerInputMode = EPlayerInputMode.INCANNON;
                other.GetComponent<CCannonImpulseBehaviour>().enabled = true;
                impulseCannonCamera.SetActive(true);
                playerNormalCamera.SetActive(false);
            }
            
            else if (Input.GetKeyDown(KeyCode.Q) && collidedCannon)
            {
                playerNormalCamera.SetActive(true);
                impulseCannonCamera.SetActive(false);
                other.GetComponent<CCannonImpulseBehaviour>().enabled = false;
                ePlayerInputMode = EPlayerInputMode.FREEMOVE;
            }
        }

        else if(other.CompareTag("Treasure") && Input.GetKeyDown(KeyCode.E))
        {
            other.GetComponent<CTreasureBehaviour>().Open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Axe"))
        {
            OnPlaySound(4, 1);
            Restart();
        }

        else if (other.CompareTag("Hinge") && currentHinge == null)
        {
            currentHinge = other.gameObject;
            ePlayerInputMode = EPlayerInputMode.INHINGE;
            anim.SetBool("Running",false);
            anim.SetBool("Attacking",false);
            anim.SetInteger("Condition", 0);
            gameObject.AddComponent<HingeJoint>();
            
            //GetComponent<Rigidbody>().useGravity = false;
            GetComponent<HingeJoint>().connectedBody = other.GetComponent<Rigidbody>();
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SavePoint") && lastPosition != other.transform.position)
        {
            if (OnShowSaveMessage != null)
            {
                if(OnPlaySound != null)
                    OnPlaySound(5, 1);
                
                OnShowSaveMessage();
                lastPosition = other.gameObject.transform.position;
            }
        }

        if (other.CompareTag("Cannon"))
        {
            if (OnHideUIButton != null)
                OnHideUIButton();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Surface_Water"))
        {
            Restart();
        }   
    }

    public void Restart()
    {
        this.transform.position = lastPosition;
        ePlayerInputMode = EPlayerInputMode.FREEMOVE;
        OnHideUIButton();
    }

    public void Shooted(Vector3 velocity)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = velocity;
        ePlayerInputMode = EPlayerInputMode.FREEMOVE;

        OnHideUIButton();
    }
}
