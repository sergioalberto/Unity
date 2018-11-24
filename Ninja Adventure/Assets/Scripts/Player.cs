using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidbody;

	[SerializeField]
	private float movementSpeed;
	private bool facingRight;
	private Animator myAnimator;

	// Use this for initialization
	void Start () {
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float horizoltal = Input.GetAxis("Horizontal");
		HandleMovement(horizoltal);
		Flip(horizoltal);
	}

	private void HandleMovement(float horizoltal){
		myRigidbody.velocity = new Vector2(horizoltal * movementSpeed, myRigidbody.velocity.y);
		myAnimator.SetFloat("speed", Mathf.Abs(horizoltal));
	}

	private void Flip(float horizoltal){
		if (horizoltal > 0 && !facingRight || horizoltal < 0 && facingRight){
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

}
