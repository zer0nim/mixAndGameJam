using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineBar : MonoBehaviour
{
    public enum ZiplineStatus {Normal, Valid, Invalid};
    public Sprite	normalSprite;
    public Sprite	validSprite;
    public Sprite	invalidSprite;
	public SpriteRenderer	spriteRenderer;

    public void	setSpriteStatus(ZiplineStatus status) {
		if (status == ZiplineStatus.Normal)
			spriteRenderer.sprite = normalSprite;
		else if (status == ZiplineStatus.Valid)
			spriteRenderer.sprite = validSprite;
		else if (status == ZiplineStatus.Invalid)
			spriteRenderer.sprite = invalidSprite;
    }
}
