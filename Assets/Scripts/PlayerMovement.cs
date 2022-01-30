using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration;
    public float decceleration;

    [SerializeField] private float speedCap;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float jumpCutoffVelocity;
    [SerializeField] private float walljumpHeight;
    [SerializeField] private float walljumpDistance;
    [SerializeField] private LayerMask platformLayer;

    private bool isJumping;
    private string direction;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float initialGravity;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        initialGravity = body.gravityScale;
    }

    private void Update()
    {
        //Grounded Check
        if (OnGround() && body.velocity.y <= 0)
        {
            isJumping = false;
        }

        //Left and right acceleration
        //Left acceleration
        if (Input.GetAxisRaw("Horizontal") < 0 && body.velocity.x >= -speedCap)
        {
            body.velocity = new Vector2(body.velocity.x - acceleration, body.velocity.y);
            direction = "left";
        }

        //Right Acceleration
        if (Input.GetAxisRaw("Horizontal") > 0 && body.velocity.x <= speedCap)
        {
            body.velocity = new Vector2(body.velocity.x + acceleration, body.velocity.y);
            direction = "right";
        }


        //Left and right decceleration
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            //Left decceleration
            if (body.velocity.x > 0)
            {
                body.velocity = new Vector2(body.velocity.x - decceleration, body.velocity.y);
                if (body.velocity.x < 0)
                {
                    body.velocity = new Vector2(0, body.velocity.y);
                }
            }

            //Right decceleration
            if (body.velocity.x < 0)
            {
                body.velocity = new Vector2(body.velocity.x + decceleration, body.velocity.y);
                if (body.velocity.x > 0)
                {
                    body.velocity = new Vector2(0, body.velocity.y);
                }
            }
        }



        //Jumping
        if (Input.GetButtonDown("Jump") && OnGround())
        {
            isJumping = true;
            body.velocity = new Vector2(body.velocity.x, jumpVelocity);
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (isJumping && body.velocity.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, jumpCutoffVelocity);
            }
            isJumping = false;
        }



        //Wall Jumping
        if (OnWall() && !OnGround())
        {

            if (body.velocity.y < -2.5)
            {
                body.velocity = new Vector2(body.velocity.x, -2.5f);
            }

            if (Input.GetButtonDown("Jump"))
            {

                if (direction == "left")
                {
                    body.velocity = new Vector2(walljumpDistance, walljumpHeight);
                }

                if (direction == "right")
                {
                    body.velocity = new Vector2(-walljumpDistance, walljumpHeight);
                }
            }
        }



        //Reset the gravity
        else
        {
            body.gravityScale = initialGravity;
        }
    }


    //Check if were on the ground
    private bool OnGround()
    {
        Vector2 hitboxSize = boxCollider.bounds.size;
        hitboxSize.x *= 0.75f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, hitboxSize, 0, Vector2.down, 0.1f, platformLayer);

        return raycastHit.collider != null;
    }


    //Check if were on a wall in a retardedly hacky way
    private bool OnWall()
    {
        Vector2 hitboxSize = boxCollider.bounds.size;
        hitboxSize.y *= 0.4f;
        //I have absolutely no clue how it also detects left but it seems to work so idfk
        RaycastHit2D rightCheck = Physics2D.BoxCast(boxCollider.bounds.center, hitboxSize, 0, Vector2.right, 0.1f, platformLayer);
        return rightCheck.collider != null;
    }
}
