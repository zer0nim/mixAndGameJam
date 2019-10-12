using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
	public Rigidbody2D	rb;
    [Range(1, 10)]
	public float		fallMultiplier = 2.5f;
	[Range(1, 10)]
	public float		lowJumpMultiplier = 2f;

    void FixedUpdate() {
        // Better jumping
		if (rb.velocity.y < 0) {
			rb.gravityScale = fallMultiplier;
		} else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
			rb.gravityScale = lowJumpMultiplier;
		} else {
            rb.gravityScale = 1f;
        }
    }
}
