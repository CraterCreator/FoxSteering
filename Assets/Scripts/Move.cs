using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAgent))]
public class Move : MonoBehaviour
{
    Animator animator;
    Rigidbody rigid;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            rigid.AddForce(transform.forward * 20);
            animator.SetFloat("moveSpeed", 6);
        }
        else
        {
            animator.SetFloat("moveSpeed", 0);
        }
        if(Input.GetKey(KeyCode.S))
        {
            rigid.AddForce(transform.forward * -1 * 10);
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -3, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 3, 0);
        }
    }
}
