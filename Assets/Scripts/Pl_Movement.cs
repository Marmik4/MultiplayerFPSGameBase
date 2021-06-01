using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pl_Movement : MonoBehaviourPunCallbacks
{
    CharacterController controller;
    public float speed = 15f;
    public float speed_Decrease = 5f;
    public float g = -9.81f;
    public float jumpHeight = 5f;

    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask goundLayer;
    public GameObject FPSCam;
    public GameObject[] Hitboxes;

    public Animator anim;
    public GameObject PlayerMesh;

    Vector3 velocity;
    private float t_speed;
    private float t_height;
    //private float crouch_height = 1f;
    //private Vector3 default_center;
    bool isGrounded;
    //private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        FPSCam.SetActive(photonView.IsMine);
        t_speed = speed;
        t_height = controller.height;
        //default_center = controller.center;
    }

    void Update()   
    {
        if (!photonView.IsMine)
        {
            gameObject.layer = 11;
            foreach (GameObject h in Hitboxes) h.layer = 11;
        }
        if (!photonView.IsMine) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, goundLayer);

        if(isGrounded && velocity.y<0)
        {
            velocity.y = -2f;
            anim.SetBool("Walk", false);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0 && isGrounded)
        {
            anim.SetBool("Walk", true);
            //PlayerMesh.GetComponent<DiffereIK>().enabled = false;
        }
        else
        {
            anim.SetBool("Walk", false);
            //PlayerMesh.GetComponent<DiffereIK>().enabled = true;
        }
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight*-2f*g);
            anim.SetBool("IsJumping", true);
        }
        else
        { anim.SetBool("IsJumping", false); }
        velocity.y += g * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.C))
        {
            anim.SetBool("Crouch", true);
            speed -= speed_Decrease;
            //controller.height = Mathf.Lerp(controller.height, crouch_height, 20f * Time.deltaTime);
            //Vector3 newCenter = new Vector3(0, 1f, 0);
            //controller.center = Vector3.Lerp(controller.center, newCenter, 20f * Time.deltaTime);
            controller.height = t_height - 0.85f;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            anim.SetBool("Crouch", false);
            speed = t_speed;
           // controller.height = Mathf.Lerp(controller.height, t_height, 20f * Time.deltaTime);
            //controller.center = Vector3.Lerp(controller.center, default_center, 20f * Time.deltaTime);
            controller.height = t_height;
        }
    }
}
