using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerMovement : MonoBehaviourPun
{
	public Vector2 velocity;
	public float acceleration;
	public float decceleration;
	public float hp = 100;
	public float movementModifier;
	public float movementReduction;
	public int acidLevel;
	public bool hitbyacid;

	[SerializeField] private float speedCap;
	[SerializeField] private float jumpVelocity;
	[SerializeField] private float jumpCutoffVelocity;
	[SerializeField] private float walljumpHeight;
	[SerializeField] private float walljumpDistance;
	[SerializeField] private LayerMask platformLayer;

	private bool isJumping;
	private bool wallJumping;
	private string direction;
	private string moving;
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
		if (!photonView.IsMine) return;
		movementModifier -= movementReduction;
		movementReduction = 0;

		if (movementModifier < 0)
		{
			movementModifier = 0;
		}

		//Grounded Check
		if (OnGround() && body.velocity.y <= 0)
		{
			isJumping = false;
		}

		//Left and right acceleration which communicates using the moving variable to the fixedupdate to allow for consistent movement on different framerates
		//Left acceleration
		if (Input.GetAxisRaw("Horizontal") < 0 && body.velocity.x >= -speedCap)
		{
			moving = "left";
			direction = "left";
		}

		//Right Acceleration
		else if (Input.GetAxisRaw("Horizontal") > 0 && body.velocity.x <= speedCap)
		{
			moving = "right";
			direction = "right";
		}

		else
		{
			moving = "none";
		}


		//Jumping
		if (Input.GetButtonDown("Jump") && OnGround())
		{
			isJumping = true;
			body.velocity = new Vector2(body.velocity.x, jumpVelocity);
		}


		//Jumping Cutoff
		if (Input.GetButtonUp("Jump"))
		{
			if (isJumping && body.velocity.y > jumpCutoffVelocity)
			{
				body.velocity = new Vector2(body.velocity.x, jumpCutoffVelocity) * movementModifier;
			}
			isJumping = false;
		}



		//Wall Sliding and jumping
		if (OnWall() && !OnGround())
		{
			if (body.velocity.y < -2.5)
			{
				body.velocity = new Vector2(body.velocity.x, -2.5f);
			}
			if (Input.GetButtonDown("Jump"))
			{
				wallJumping = true;
			}
		}



		//Reset the gravity
		else
		{
			body.gravityScale = initialGravity;
		}
	}

    private void FixedUpdate()
	{

		if (wallJumping)
		{

			if (direction == "left")
			{
				body.velocity = new Vector2(walljumpDistance, walljumpHeight);
			}

			if (direction == "right")
			{
				body.velocity = new Vector2(-walljumpDistance, walljumpHeight);
			}
			wallJumping = false;
		}


		//Left Acceleration
		if (moving == "left")
		{
			body.velocity = new Vector2(body.velocity.x - acceleration, body.velocity.y) * movementModifier;
		}

		//Right Acceleration
		else if (moving == "right")
		{
			body.velocity = new Vector2(body.velocity.x + acceleration, body.velocity.y) * movementModifier;
		}

		else
		{
			//Left decceleration
			if (body.velocity.x > 0)
			{
				body.velocity = new Vector2(body.velocity.x - decceleration, body.velocity.y) * movementModifier;
				if (body.velocity.x < 0)
				{
					body.velocity = new Vector2(0, body.velocity.y) * movementModifier;
				}
			}

			//Right decceleration
			if (body.velocity.x < 0)
			{
				body.velocity = new Vector2(body.velocity.x + decceleration, body.velocity.y) * movementModifier;
				if (body.velocity.x > 0)
				{
					body.velocity = new Vector2(0, body.velocity.y) * movementModifier;
				}
			}
		}

		//Acid projectile detection (Yes this requires its own special case)
		if (hitbyacid)
		{
			if (acidLevel < 4)
			{
				acidLevel += 1;
			}
			StopAllCoroutines();
			StartCoroutine(acidTick(10, 0.5f));
			hitbyacid = false;
		}
	}


	//Check if we're on the ground
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

	public IEnumerator acidTick(float acidDuration, float acidInterval)
    {
		for (int i = 0; i < acidDuration; i++)
        {
			hp -= acidLevel;
			yield return new WaitForSeconds(acidInterval);
        }
		acidLevel = 0;
	}
}
