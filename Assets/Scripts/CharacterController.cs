using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterController : MonoBehaviour {

	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpHeight;
	[SerializeField] private float range = 10f;

	private Rigidbody rigidBody;
	private Animator anim;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		Vector3 moveChar = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"),0,CrossPlatformInputManager.GetAxis("Vertical"));
		if (moveChar != Vector3.zero){
			anim.SetBool("isWalking", true);
			//Quaternion targetRotation = Quaternion.LookRotation(moveChar, Vector3.up);
			//transform.rotation = Quaternion.LookRotation(moveChar, Vector3.up);
		} else {
			anim.SetBool("isWalking", false);
		}
		transform.position += moveChar * Time.deltaTime * moveSpeed;

		if(GameManager.Instance.IsJumping){
			//transform.Translate(Vector3.up * jumpHeight * Time.deltaTime, Space.World);
			if (rigidBody.velocity.y == 0){
				anim.SetTrigger("Jump");
				rigidBody.AddForce(Vector3.up * Time.deltaTime * jumpHeight, ForceMode.Impulse);
				GameManager.Instance.IsJumping = false;
			}
		}
		if(GameManager.Instance.IsBuilding){
			anim.SetTrigger("Punch");
			ModifyTerrain.Instance.AddBlock(range, (byte) TextureType.rock.GetHashCode());
			GameManager.Instance.IsBuilding = false;
		}
		if(GameManager.Instance.IsPunching){
			anim.SetTrigger("Punch");
			ModifyTerrain.Instance.DestroyBlock(range, (byte) TextureType.air.GetHashCode());
			GameManager.Instance.IsPunching = false;
		}
	}
}
