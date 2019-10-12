using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineGun : MonoBehaviour
{
	public Camera		cam;
	public GameObject	zipline;
	public LayerMask	layerMask;

	bool		_inPlacement = false;
	Vector3		_worldPos;
	GameObject	_ziplineInst;

	Ray			_ray;
	RaycastHit	_hit;

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

			_ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(_ray.origin, _ray.direction, 1f, layerMask);
			if (hit) {
				print("We hit " + hit.collider.name);
			}
		}
	}

	void	updateWorldPos() {
		_worldPos = cam.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
		_worldPos.z = transform.position.z;
	}
}
