using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	public Rigidbody2D	rb;
	public float		moveSpeed = 10f;
	[Range(1, 10)]
	public float		jumpVelocity = 5f;
	[Range(1, 10)]
	public float		fallMultiplier = 2.5f;
	[Range(1, 10)]
	public float		lowJumpMultiplier = 2f;

	Vector2	_moveDir;

	// Update is called once per frame
	void    Update() {
		_moveDir.x = Input.GetAxis("Horizontal");
		_moveDir.y = Input.GetAxis("Vertical");
		// movement = Vector2.ClampMagnitude(movement, 1);

		if (Input.GetButtonDown("Jump"))
			jump();
	}


	void	FixedUpdate() {
		walk();

		// Better jumping
		if (rb.velocity.y < 0) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
		} else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
		}
	}


	void	walk() {
		rb.velocity = new Vector2(_moveDir.x * moveSpeed, rb.velocity.y);
		// rb.velocity = new Vector2(
		// 	_moveDir.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
	}

	void	jump() {
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.velocity += Vector2.up * jumpVelocity;
	}
}
