using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineBar : MonoBehaviour
{
    public enum ZiplineStatus {Normal, Valid, Invalid};

	public float		cableWidth = .1f;
	public Transform	localCableOffset;
	public Material		cableMaterial;
    public Sprite		normalSprite;
    public Sprite		validSprite;
    public Sprite		invalidSprite;
	public SpriteRenderer	spriteRenderer;

	LineRenderer	_line = null;
	GameObject		_ziplineEnd = null;
	Vector3			_startPos;
	Vector3			_endPos;
	bool			_constantUpdate = true;

	void FixedUpdate() {
		if (_ziplineEnd != null && _constantUpdate) {
			updateLinePos();
		}
	}

    public void	setSpriteStatus(ZiplineStatus status) {
		if (status == ZiplineStatus.Normal)
			spriteRenderer.sprite = normalSprite;
		else if (status == ZiplineStatus.Valid)
			spriteRenderer.sprite = validSprite;
		else if (status == ZiplineStatus.Invalid)
			spriteRenderer.sprite = invalidSprite;
    }

	public void initZipline(GameObject ziplineEnd) {
		_ziplineEnd = ziplineEnd;
		_line = gameObject.AddComponent<LineRenderer>();
		_line.material = cableMaterial;
		_line.positionCount = 2;
		_line.startWidth = cableWidth;
		_line.endWidth = cableWidth;
		_line.useWorldSpace = true;
		_line.numCapVertices = 50;
		_line.sortingOrder = 4;
	}

	public void	confirmCreation() {
		_constantUpdate = false;
		updateLinePos();
		addColider();
	}

	void	addColider() {
		float	lineLength;
		Vector3	center;

		// create child gameObject and add a collider to it
		BoxCollider2D col = new GameObject("Collider").AddComponent<BoxCollider2D>();
		col.transform.parent = _line.transform;

		// set size and position
		lineLength = Vector3.Distance(_startPos, _endPos);
		col.size = new Vector2(lineLength, cableWidth);
		center = (_startPos + _endPos) / 2;
		col.transform.position = center;

		// set rotation
		Vector2 diff = _startPos - _endPos;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		col.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

		col.isTrigger = true;
	}

	void	updateLinePos() {
		_startPos = transform.position + localCableOffset.localPosition;
		_endPos = _ziplineEnd.transform.position + localCableOffset.localPosition;
		_line.SetPosition(0, _startPos);
		_line.SetPosition(1, _endPos);
	}
}
