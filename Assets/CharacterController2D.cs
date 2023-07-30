using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterController2D : MonoBehaviour {
    Rigidbody2D rigidbody2D;
    [SerializeField] float speed = 2f; // this will show up in inspector
    Vector2 motionVector;
    Animator animator;

    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        motionVector = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")
            ); // getaxisraw return immediate value
        animator.SetFloat("horizontal", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("vertical", Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate() {
        Move();
    }

    private void Move() {
        rigidbody2D.velocity = motionVector * speed;
    }
}
