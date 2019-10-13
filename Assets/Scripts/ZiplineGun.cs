using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineGun : MonoBehaviour
{
	public Camera		cam;
	public GameObject	zipline;
	public LayerMask	layerMask;
	public float		snappingRadius = 5f;
	public float		borderOffset = .32f;

	bool		_inPlacement = false;
	Vector3		_worldPos;
	GameObject	_ziplineInst;
	GameObject	_target = null;
	Vector2		_targetBox;

	void Update() {
		if (Input.GetButtonDown("Fire1") && _inPlacement) {
			_inPlacement = false;
		}
		else if (Input.GetButtonDown("Skill1")) {
			_inPlacement = !_inPlacement;
			if (_inPlacement) {
				updateWorldPos();
				_ziplineInst = Instantiate(zipline);
				_ziplineInst.transform.position = _worldPos;
			} else {
				Destroy(_ziplineInst);
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
				_ziplineInst.transform.position = _worldPos;
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
				_ziplineInst.transform.position = new Vector3(xPos, yPos, _ziplineInst.transform.position.z);
			}
		}
	}

	void	updateWorldPos() {
		_worldPos = cam.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
		_worldPos.z = transform.position.z;
	}
}
