using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    BoxCollider2D boxCollider;                      // Box Collider 2D for using BoxCast()
    [HideInInspector] public ContactList contacts;  // Collision contacts
    [HideInInspector] public int faceDirX;          // direction we are facing

    public LayerMask collisionMask;                 // Add layers which have collisions, such as walls and floors
    public float skinWidth = 0.025f;                // Safe check distance for checking collisions

    float minMoveDist = 0.01f;                      // Prevent character jittering, when its standing still

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        faceDirX = 1;
    }

    /// <summary>
    /// Move's the gameobject and checks gameobjects for collisions.
    /// </summary>
    /// <param name="moveAmount"></param>
    public void Move(Vector2 moveAmount)
    {
        if (moveAmount.magnitude >= minMoveDist || contacts.inAir)
        {
            contacts.Reset();
            HorizontalCollisions(ref moveAmount);
            VerticalCollisions(ref moveAmount);

            if (!float.IsNaN(moveAmount.x) && !float.IsNaN(moveAmount.y))
                transform.Translate(moveAmount);
            else
                Debug.LogError("moveAmount x or y was a NaN, which might have caused problems");
        }
    }

    /// <summary>
    /// Checks collisions in y axis.
    /// </summary>
    /// <param name="moveAmount"></param>
    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float directionX = Mathf.Sign(moveAmount.x);
        float rayDistance = Mathf.Abs(moveAmount.y) + skinWidth;

        // Box cast for collisions
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up * directionY, rayDistance, collisionMask);
        if (hit)
        {
            ColliderDistance2D colliderDistance = hit.collider.Distance(boxCollider);
            if (colliderDistance.isOverlapped && colliderDistance.isValid)
            {
                moveAmount.y = (colliderDistance.pointA - colliderDistance.pointB).y;
            }
            else
            {
                moveAmount.y = (hit.distance - skinWidth) * directionY;
            }

            contacts.above = directionY == 1;
            contacts.below = directionY == -1;

            if (contacts.below)
                contacts.inAir = false;

        }

    }

    /// <summary>
    /// Checks collisions in x axis.
    /// </summary>
    /// <param name="moveAmount"></param>
    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = faceDirX;
        float rayDistance = Mathf.Abs(moveAmount.x) + skinWidth;

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.right * directionX, rayDistance, collisionMask);
        if (hit)
        {
            ColliderDistance2D colliderDistance = hit.collider.Distance(boxCollider);
            if (colliderDistance.isOverlapped && colliderDistance.isValid)
            {
                moveAmount.x = (colliderDistance.pointA - colliderDistance.pointB).x;
            }
            else
            {
                moveAmount.x = (hit.distance - skinWidth) * directionX;
            }

            contacts.right = directionX == 1;
            contacts.left = directionX == -1;
        }
    }

    /// <summary>
    /// Update the x direction we are facing.
    /// </summary>
    /// <param name="moveAmount"></param>
    /// <returns></returns>
    public int UpdateFaceDirX(Vector2 moveAmount)
    {
        return faceDirX = (int)(Mathf.Sign(moveAmount.x));
    }

    /// <summary>
    /// Stores information of collision contacts and if we are in air.
    /// </summary>
    public struct ContactList
    {
        public bool above, below, left, right;
        public bool inAir;

        public void Reset()
        {
            above = below = left = right = false;
        }
    }
}
