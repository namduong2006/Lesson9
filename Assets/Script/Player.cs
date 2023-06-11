using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    [SerializeField]private Rigidbody rb;    
    public float speedmove=1f;
    public float jump = 300f;           
    private bool isCrouch=false;
    private bool isJump=true;
    void Start()
    {         
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();        
        rb.centerOfMass = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        MovePlayer();
        
    }
    public void MovePlayer()
    {
        // di chuyen 

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(v, 0f, h);
        movement.Normalize();       
        transform.Translate(movement * speedmove * Time.deltaTime, Space.World);


        // xoay

        if (movement != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 720f * Time.deltaTime);
        }
        //chuyen dong animation walk

        if (h != 0 || v != 0)
        {
            anim.SetBool("isWalk", true);
        }
        else anim.SetBool("isWalk", false);

        // chuyen dong run

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            anim.SetBool("isRun", true);
            speedmove = 2.5f;
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            anim.SetBool("isRun", false);
            speedmove = 1f;
        }
        // ngoi

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            speedmove = 0.5f;
            isCrouch = true;
            anim.SetBool("isSit", isCrouch);
        }
        bool isMoving = (v != 0 || h != 0);
        if (isCrouch && isMoving)
        {
            anim.SetBool("isSitWalk", true);
        }
        else { anim.SetBool("isSitWalk", false); }
        if (Input.GetKeyUp(KeyCode.RightControl))
        {
            speedmove = 1f;
            anim.SetBool("isSitWalk", false);
            anim.SetBool("isSit", false);
        }


        // nhay
        
        
        if (Input.GetButtonDown("Jump")&&isJump==true)
        {
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
            anim.Play("JumpUp");
            isJump = false;
        }
 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            isJump = true;           
        }
    }    
}
