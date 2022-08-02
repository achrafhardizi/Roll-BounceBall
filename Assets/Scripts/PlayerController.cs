using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D),typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    // Properties of the ball 

    public float maxRollSpeed;
    public float accelerationSpeed;
    public float decelarationSpeed;
    public float rollDegreeMultiplier;
    public float jumpSpeed;
    public LayerMask groundLayer;

    // Utility variables

    float diameter;
    bool isGrounded;

    // Game Components ( Collision, Gravity)

    Rigidbody2D rb2d  ;
    CircleCollider2D cc2d;

    // Start is called before the first frame update
    void Start()
    {
        // initialize the Game Components

        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CircleCollider2D>();
        diameter = 2 * Mathf.PI * cc2d.radius;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = cc2d.IsTouchingLayers(groundLayer);

        Move(Input.GetAxisRaw("Horizontal"));
        AnimationUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void AnimationUpdate()
    {
        float speed = rb2d.velocity.magnitude;

        if( speed > 0)
        {
            transform.Rotate(0, 0, (speed * -Mathf.Sign(rb2d.velocity.x) * Time.deltaTime / diameter)  * 360f * rollDegreeMultiplier);
        }
    }

    void Move(float h)
    {
        if(h != 0)
        {
            rb2d.velocity += h * accelerationSpeed * Vector2.right * Time.deltaTime;
            if(rb2d.velocity.x > maxRollSpeed || rb2d.velocity.x < -maxRollSpeed)
            {
                rb2d.velocity = new Vector2(h * maxRollSpeed, rb2d.velocity.y);
            }
        }
        else if(rb2d.velocity.x !=0)
        {
            float deceleratedSpeed = Mathf.Lerp(rb2d.velocity.x, 0, decelarationSpeed * Time.deltaTime);
            rb2d.velocity = new Vector2(deceleratedSpeed, rb2d.velocity.y); 
        }
    }

    void Jump()
    {
        if (!isGrounded) return;
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        cc2d.GetContacts(contacts);
        rb2d.velocity += contacts[0].normal * jumpSpeed;
    }
}
