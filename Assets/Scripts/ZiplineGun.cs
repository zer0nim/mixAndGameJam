using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineGun : MonoBehaviour
{
	public BoxCollider2D		boxCollider;
	public MovementController	movementController;
	public Camera				cam;
	public GameObject			zipline;
	public LayerMask			layerMask;
	public float				snappingRadius = 5f;
	public float				borderOffset = .32f;

	bool		_inPlacement = false;
	Vector3		_worldPos;
	GameObject	_endZiplineInst = null;
	ZiplineBar	_endZiplineBarScript;
	GameObject	_startZiplineInst = null;
	ZiplineBar	_startZiplineBarScript;
	float		_startYOffset;
	bool		_wasGroundedLast = true;
	GameObject	_target = null;
	Vector2		_targetBox;

	void Awake() {
		_startYOffset = boxCollider.bounds.size.y / 2;
	}

	void Update() {
		if (Input.GetButtonDown("Fire1") && _inPlacement && _target != null && movementController.grounded()) {
			_inPlacement = false;
			_endZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Normal);
			_startZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Normal);
			_startZiplineBarScript.confirmCreation();
			_startZiplineInst = null;
			_endZiplineInst = null;
			_startZiplineBarScript = null;
			_endZiplineBarScript = null;
		}
		else if (Input.GetButtonDown("Skill1")) {
			_inPlacement = !_inPlacement;
			if (_inPlacement) {
				updateWorldPos();

				// instanciate start ziplineBar
				_startZiplineInst = Instantiate(zipline);
				_startZiplineInst.transform.position = new Vector3(transform.position.x, transform.position.y - _startYOffset, transform.position.z - 1.0f);
				_startZiplineBarScript = _startZiplineInst.GetComponent<ZiplineBar>();
				_startZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Valid);

				// instanciate end ziplineBar
				_endZiplineInst = Instantiate(zipline);
				_endZiplineInst.transform.position = new Vector3(_worldPos.x, _worldPos.y, _worldPos.z - 1.0f);
				_endZiplineBarScript = _endZiplineInst.GetComponent<ZiplineBar>();
				_endZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Valid);

				_startZiplineBarScript.initZipline(_endZiplineInst);
			} else {
				Destroy(_endZiplineInst);
				Destroy(_startZiplineInst);
				_startZiplineInst = null;
				_endZiplineInst = null;
			}
		}
	}

	void	FixedUpdate() {
		if (_inPlacement) {
			updateWorldPos();

			Collider2D[] colliders = Physics2D.OverlapCircleAll (_worldPos, snappingRadius, layerMask);
			float		nearestDistance = float.MaxValue;
			// get the nearest collider
			_target = null;
			foreach (Collider2D collider in colliders) {
				float distance = (_worldPos - collider.transform.position).sqrMagnitude;
				if (distance < nearestDistance && collider.GetType() == typeof(BoxCollider2D)) {
					nearestDistance = distance;
					_target = collider.gameObject;
					_targetBox = (Vector2)((BoxCollider2D)collider).bounds.size;
				}
			}

			// if no target found simply follow the cursor
			if (_target == null) {
				_endZiplineInst.transform.position = new Vector3(_worldPos.x, _worldPos.y, _worldPos.z - 1.0f);
				_endZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Invalid);
			}
			// else snap to the target
			else {
				Vector2 t_pos = _target.transform.position;
				float xPos = _worldPos.x;
				float minP = t_pos.x - _targetBox.x / 2 + borderOffset;
				float maxP = t_pos.x + _targetBox.x / 2 - borderOffset;
				if (xPos < minP)
					xPos = minP;
				if (xPos > maxP)
					xPos = maxP;
				float yPos = t_pos.y + _targetBox.y / 2;
				_endZiplineInst.transform.position = new Vector3(xPos, yPos, _endZiplineInst.transform.position.z);
				_endZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Valid);
			}

			// update start zipline position
			_startZiplineInst.transform.position = new Vector3(transform.position.x, transform.position.y - _startYOffset, transform.position.z - 1.0f);
			if (movementController.grounded() != _wasGroundedLast) {
				if (_wasGroundedLast)
					_startZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Invalid);
				else
					_startZiplineBarScript.setSpriteStatus(ZiplineBar.ZiplineStatus.Valid);
			}
			_wasGroundedLast = movementController.grounded();
		}
	}

	void	updateWorldPos() {
		_worldPos = cam.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
		_worldPos.z = transform.position.z;
	}
}
