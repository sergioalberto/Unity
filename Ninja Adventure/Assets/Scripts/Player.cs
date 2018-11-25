using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidbody;

	[SerializeField]
	private float movementSpeed;
	private bool facingRight;
	private Animator myAnimator;
	private bool attack;

	// Use this for initialization
	void Start () {
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}
	
	void Update(){
		HandleInput();	
	}

	// Update is called once per frame
	void FixedUpdate () {
		float horizoltal = Input.GetAxis("Horizontal");
		HandleMovement(horizoltal);
		Flip(horizoltal);
		HandleAttack();
		ResetValues();
	}

	private void HandleMovement(float horizoltal){
		if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){ // Do not move it while it is attacking
			myRigidbody.velocity = new Vector2(horizoltal * movementSpeed, myRigidbody.velocity.y);
		}
		myAnimator.SetFloat("speed", Mathf.Abs(horizoltal));
	}

	private void HandleAttack(){
		if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
			myAnimator.SetTrigger("attack");
			myRigidbody.velocity = Vector2.zero; // Do not run while it is attacking
		}
	}

	private void HandleInput(){
		if (Input.GetKeyDown(KeyCode.LeftShift)){
			attack = true;
		}
	}

	private void Flip(float horizoltal){
		if (horizoltal > 0 && !facingRight || horizoltal < 0 && facingRight){
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	private void ResetValues(){
		attack = false;
	}

}
