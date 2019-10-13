using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeZipline : MonoBehaviour
{
	public float	speed = 5f;

	bool				_onZipline = false;
	Vector3				_target;
	Rigidbody2D			_rb;
	BetterJump			_betterJumpScript;
	MovementController	_movementController;
	BoxCollider2D		_boxCollider2D;
	float				_saveGravityScale;
	float				_lastFinishTime;
	float				_minTimeOffset = .2f;

	void Awake() {
		_target = transform.position;
		_rb = GetComponent<Rigidbody2D>();
		_betterJumpScript = GetComponent<BetterJump>();
		_movementController = GetComponent<MovementController>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_lastFinishTime = Time.time;
	}

	void	FixedUpdate() {
		if (_onZipline) {
			float distance = Vector3.Distance(transform.position, _target);
			if (distance <= 0.1f) {
				_lastFinishTime = Time.time;
				_onZipline = false;
				_target = transform.position;
				_rb.gravityScale = _saveGravityScale;
				_betterJumpScript.enabled = true;
				_movementController.enabled = true;
				_boxCollider2D.enabled = true;
			} else {
				Vector2 newPos = Vector2.MoveTowards((Vector2)transform.position, (Vector2)_target, speed * Time.deltaTime);
				transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("zipline") && Time.time - _lastFinishTime > _minTimeOffset) {
			_onZipline = true;
			ZiplineBar ziplineBarScript = other.GetComponentInParent<ZiplineBar>();
			_target = ziplineBarScript.getEndPos();
			_target.z = transform.position.z;
			_saveGravityScale = _rb.gravityScale;
			_betterJumpScript.enabled = false;
			_movementController.enabled = false;
			_boxCollider2D.enabled = false;
			_rb.gravityScale = 0.0f;
			_rb.velocity = Vector2.zero;
		}
	}
}
