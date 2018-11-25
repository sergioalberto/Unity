using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D _myRigidbody;
	private Animator _myAnimator;
	[SerializeField]
	private bool _facingRight;
	private bool _attack;
	private bool _slide;

	public float movementSpeed;

	// Use this for initialization
	void Start () {
		movementSpeed = movementSpeed != 0 ? movementSpeed : 10;
		_facingRight = true;
		_myRigidbody = GetComponent<Rigidbody2D>();
		_myAnimator = GetComponent<Animator>();
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
		if (!_myAnimator.GetBool("slide") && !this._myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){ // Do not move it while it is attacking
			_myRigidbody.velocity = new Vector2(horizoltal * movementSpeed, _myRigidbody.velocity.y);
		}
		if (_slide && !this._myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide")){
			_myAnimator.SetBool("slide", true);
		} else if (this._myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide")){
			_myAnimator.SetBool("slide", false);
		}
		_myAnimator.SetFloat("speed", Mathf.Abs(horizoltal));
	}

	private void HandleAttack(){
		if (_attack && !this._myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
			_myAnimator.SetTrigger("attack");
			_myRigidbody.velocity = Vector2.zero; // Do not run while it is attacking
		}
	}

	private void HandleInput(){
		if (Input.GetKeyDown(KeyCode.LeftShift)){
			_attack = true;
		} else if (Input.GetKeyDown(KeyCode.LeftControl)){
			_slide = true;
		}
	}

	private void Flip(float horizoltal){
		if (horizoltal > 0 && !_facingRight || horizoltal < 0 && _facingRight){
			_facingRight = !_facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	private void ResetValues(){
		_attack = false;
		_slide = false;
	}

}
