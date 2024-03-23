using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    //Vector3 velocity;
    Rigidbody rb;
    public float speed;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate() //!
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); //move
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){

        }
        if(Input.GetKeyDown(KeyCode.D)){
            
        }
    }
}
