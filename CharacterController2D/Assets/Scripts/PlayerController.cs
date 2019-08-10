using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpSpeed = 7f;
    public float gravModifier = 2f;
    public float gravity = -9.8f;
    Vector2 velocity;

    Vector2 input;
    CharacterController2D cc;
    SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        cc = GetComponent<CharacterController2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        input.x = Input.GetAxisRaw("Horizontal");

        // Apply input to velocity
        velocity.x = input.x * moveSpeed;

        // Stop velocity.y if collisions are detected above or below
        if (cc.contacts.below || cc.contacts.above)
            velocity.y = 0f;

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (cc.contacts.below)
            {
                velocity.y = jumpSpeed;
                cc.contacts.inAir = true;
            }
        }

        // Apply gravity, even if we are standing still
        velocity.y += gravity * gravModifier * Time.deltaTime;

        // Update face direction in x axis
        if (input.x != 0)
            cc.UpdateFaceDirX(velocity);
       
        // Move the character
        cc.Move(velocity * Time.deltaTime);

        anim.SetFloat("velocityX", velocity.y);
        anim.SetFloat("velocityY", velocity.y);
        anim.SetBool("grounded", cc.contacts.below);

        sprite.flipX = cc.faceDirX == -1;
    }

}
