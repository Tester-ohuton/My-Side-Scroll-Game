using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Vector3 moveDirection = Vector3.zero;
    //GameObject retryObject;
    public int speed;
    private Rigidbody rb;
    private float h, v;
    private bool isGameOver;
    // Use this for initialization
    void Start()
    {
        isGameOver = true;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        //retryObject = GameObject.Find("GameObject");
    }//Start()

    // Update is called once per frame
    void Update()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");

        //if (h != 0 || v != 0)
        //{
        //    moveDirection = speed * new Vector3(h, 0, v);
        //    moveDirection = transform.TransformDirection(moveDirection);
        //    rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        //}

        //if (h == 0)
        //{
        //    moveDirection = speed * new Vector3(0, 0, v);
        //    moveDirection = transform.TransformDirection(moveDirection);
        //    rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        //}

        //if (v == 0)
        //{
        //    moveDirection = speed * new Vector3(h, 0, 0);
        //    moveDirection = transform.TransformDirection(moveDirection);
        //    rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        //}
        

    }
    
}