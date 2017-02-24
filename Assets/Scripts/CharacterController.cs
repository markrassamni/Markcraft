using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterController : MonoBehaviour {

	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpHeight;

	private Rigidbody rigidBody;
	private Animator anim;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		Vector3 moveChar = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"),0,CrossPlatformInputManager.GetAxis("Vertical"));
		transform.position += moveChar * Time.deltaTime * moveSpeed;
		if (moveChar != Vector3.zero){
			anim.SetBool("isWalking", true);
		} else {
			anim.SetBool("isWalking", false);
		}
	}
}
