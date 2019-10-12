using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	public Rigidbody2D	rb;
	public float		moveSpeed = 10f;
	[Range(1, 10)]
	public float		jumpVelocity = 5f;

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
