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

	void FixedUpdate() {
		if (_ziplineEnd != null) {
			_line.SetPosition(0, transform.position + localCableOffset.localPosition);
			_line.SetPosition(1, _ziplineEnd.transform.position + localCableOffset.localPosition);
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
}
