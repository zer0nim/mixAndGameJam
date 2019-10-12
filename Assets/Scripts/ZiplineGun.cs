using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineGun : MonoBehaviour
{
	public Camera		cam;
	public GameObject	zipline;
	public LayerMask	layerMask;
	public float		snappingRadius = 5f;

	bool		_inPlacement = false;
	Vector3		_worldPos;
	GameObject	_ziplineInst;
	GameObject	_lastTarget = null;
	Color		_lastTargetColor;


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
			_ziplineInst.transform.position = _worldPos;

			Collider2D[] colliders = Physics2D.OverlapCircleAll (_worldPos, snappingRadius, layerMask);
			float		nearestDistance = float.MaxValue;
			GameObject	target = null;
			// get the nearest collider
			foreach (Collider2D collider in colliders) {
				float distance = (_worldPos - collider.transform.position).sqrMagnitude;
				if (distance < nearestDistance) {
					nearestDistance = distance;
					target = collider.gameObject;
				}
			}

			if (target != null && _lastTarget != target) {
				SpriteRenderer	spriteR = target.GetComponentInParent<SpriteRenderer>();
				if (_lastTarget != null) {
					_lastTarget.GetComponentInParent<SpriteRenderer>().color = _lastTargetColor;
				}
				_lastTargetColor = spriteR.color;
				spriteR.color = new Color(.9f, 0f, 0f);
			}
			else if (target == null && _lastTarget != null) {
				_lastTarget.GetComponentInParent<SpriteRenderer>().color = _lastTargetColor;
			}
			_lastTarget = target;
		}
	}

	void	updateWorldPos() {
		_worldPos = cam.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
		_worldPos.z = transform.position.z;
	}
}
