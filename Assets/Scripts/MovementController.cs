using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	public Rigidbody2D	rb;
	public float		moveSpeed = 8f;
	// jump section
	[Range(1, 10)]
	public float		jumpVelocity = 5f;
	public float		groundedHeight = 0.2f;

	public LayerMask	groundMask;

	Vector2	_moveDir;
	// jump section
	bool	_jumpRequest = false;
	bool	_grounded = false;
	Vector2 _playerSize;
	Vector2	_boxSize;


	void	Awake() {
		_playerSize = GetComponent<BoxCollider2D>().size;
		_boxSize = new Vector2(_playerSize.x, groundedHeight);
	}

	void    Update() {
		_moveDir.x = Input.GetAxis("Horizontal");
		_moveDir.y = Input.GetAxis("Vertical");
		_moveDir = Vector2.ClampMagnitude(_moveDir, 1);

		if (Input.GetButtonDown("Jump") && _grounded)
			_jumpRequest = true;
	}


	void	FixedUpdate() {
		walk();

		if (_jumpRequest)
			jump();
		else {
			Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (_playerSize.y + _boxSize.y) * 0.5f;
			_grounded = (Physics2D.OverlapBox(boxCenter, _boxSize, 0f, groundMask) != null);
		}
	}


	void	walk() {
		rb.velocity = new Vector2(_moveDir.x * moveSpeed, rb.velocity.y);
	}

	void	jump() {
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.velocity += Vector2.up * jumpVelocity;

		_jumpRequest = false;
		_grounded = false;
	}
}
