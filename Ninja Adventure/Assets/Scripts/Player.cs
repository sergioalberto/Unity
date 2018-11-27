using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float movementSpeed;
	public float jumpForce; // To indicate how high we want to jump

	private Rigidbody2D _myRigidbody;
	private Animator _myAnimator;
	[SerializeField]
	private bool _facingRight;
	private bool _attack;
	private bool _slide;
	[SerializeField]
	private Transform[] _groundPoints;
	[SerializeField]
	private float _groundRadius;
	[SerializeField]
	private LayerMask _whatIsGround;
	private bool _isGrounded;
	private bool _jump;
	[SerializeField]
	private bool _airControl;
	private bool _jumpAttack;

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
		_isGrounded = IsGrounded();
		HandleMovement(horizoltal);
		Flip(horizoltal);
		HandleAttack();
		HandleLayers();
		ResetValues();
	} 

	private void HandleMovement(float horizoltal){
		if (_myRigidbody.velocity.y < 0){
			_myAnimator.SetBool("land", true);
		}
		if (!_myAnimator.GetBool("slide") && !this._myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") 
			&& (_isGrounded || _airControl)){ // Do not move it while it is attacking
			_myRigidbody.velocity = new Vector2(horizoltal * movementSpeed, _myRigidbody.velocity.y);
		}
		if (_isGrounded && _jump){
			_isGrounded = false;
			_myRigidbody.AddForce(new Vector2(0, jumpForce));
			_myAnimator.SetTrigger("jump");
		}
		if (_slide && !this._myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide")){
			_myAnimator.SetBool("slide", true);
		} else if (this._myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide")){
			_myAnimator.SetBool("slide", false);
		}
		_myAnimator.SetFloat("speed", Mathf.Abs(horizoltal));
	}

	private void HandleAttack(){
		if (_attack && _isGrounded && !this._myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
			_myAnimator.SetTrigger("attack");
			_myRigidbody.velocity = Vector2.zero; // Do not run while it is attacking
		}
		if (_jumpAttack && !_isGrounded && !this._myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack")){
			_myAnimator.SetBool("jumpAttack", true);
		}
		if (!_jumpAttack && this._myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack")){
			_myAnimator.SetBool("jumpAttack", false);
		}
	}

	private void HandleInput(){
		if (Input.GetKeyDown(KeyCode.LeftShift)){
			_attack = true;
			_jumpAttack = true;
		} else if (Input.GetKeyDown(KeyCode.LeftControl)){
			_slide = true;
		} else if (Input.GetKeyDown(KeyCode.Space)){
			_jump = true;
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
		_jump = false;
		_jumpAttack = false;
	}

	private bool IsGrounded(){
		if (_myRigidbody.velocity.y <= 0){ // if we are standing on the ground
			foreach (Transform point in _groundPoints){
				Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, _groundRadius, _whatIsGround); // ckeck if these points are colliding with something
				for (int count = 0; count < colliders.Length; count ++){
					if (colliders[count].gameObject != gameObject){ // if the object is different from the player (colliding something else than the player without feet)
						_myAnimator.ResetTrigger("jump");
						_myAnimator.SetBool("land", false);
						return true;
					}
				}
			}
		}
		return false;
	}

	private void HandleLayers(){
		if (!_isGrounded){
			_myAnimator.SetLayerWeight(1, 1);
		} else {
			_myAnimator.SetLayerWeight(1, 0);
		}
	}
}
