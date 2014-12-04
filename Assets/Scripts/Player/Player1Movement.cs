﻿using UnityEngine;
using System.Collections;

public class Player1Movement : MonoBehaviour {
	enum direction {up, down, left, right, upleft, upright, downleft, downright};
	direction playerDirection = direction.up;
	Rigidbody playerRigidbody; //Reference to the player's rigidbody
	Animator anim; //Reference to the animator's component

	public GameObject reticle;
	private Transform reticleLocation;
	bool isCasting = false;

	void Awake() {
		//Setup references
		playerRigidbody = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();

		Instantiate (reticle, new Vector3(transform.position.x, -1f, transform.position.y), Quaternion.Euler(new Vector3(90, 0, 0)));
		reticleLocation = GameObject.Find ("P1Reticle(Clone)").GetComponent<Transform>();
	}

	void Move(float h, float v) {
		//control running animation
		bool walking = h != 0f || v != 0f;
		anim.SetBool ("IsWalking", walking);

		//determine facing direction
		if (h == -1) {
			playerDirection = direction.left;
		} else if (h == 1) {
			playerDirection = direction.right;
		}
		
		if (v == -1) {
			playerDirection = direction.down;
		} else if (v == 1) {
			playerDirection = direction.up;
		}
		
		if (h == -1 && v == -1) {
			playerDirection = direction.downleft;
		} else if (h == 1 && v == -1) {
			playerDirection = direction.downright;
		} else if (h == -1 && v == 1) {
			playerDirection = direction.upleft;
		} else if (h == 1 && v == 1) {
			playerDirection = direction.upright;
		}

		Vector3 tempVector = Vector3.forward;
		switch ((int)playerDirection) {
		case 1:
			tempVector = Vector3.back;
			break;
		case 2:
			tempVector = Vector3.left;
			break;
		case 3:
			tempVector = Vector3.right;
			break;
		case 4:
			tempVector = new Vector3 (-1, 0, 1);
			break;
		case 5:
			tempVector = new Vector3 (1, 0, 1);
			break;
		case 6:
			tempVector = new Vector3 (-1, 0, -1);
			break;
		case 7:
			tempVector = new Vector3 (1, 0, -1);
			break;
		default:
			tempVector = Vector3.forward;
			break;
		}
		Quaternion newDirection = Quaternion.LookRotation (tempVector, Vector3.up);
		playerRigidbody.MoveRotation (newDirection);

		float offsetX = 0;
		float offsetZ = 0;
		float moveSpeed = 0.08f;
		
		//determine movement
		if (h == -1) {
			offsetX = -moveSpeed;
		} else if (h == 1) {
			offsetX = moveSpeed;
		}
		
		if (v == -1) {
			offsetZ = -moveSpeed;
		} else if (v == 1) {
			offsetZ = moveSpeed;
		}

		transform.position = new Vector3 (transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ);
	}

	void Attack() {
		if (Input.GetButtonDown ("Fire1")) {
			anim.SetTrigger ("Attack");
		}
	}

	void PsynergyActivate() {
		if (Input.GetButtonDown ("Fire2")) {
			isCasting = true;
			anim.SetBool ("IsWalking", false);
			reticleLocation.position = new Vector3(transform.position.x, 3f, transform.position.z);
		}
	}

	void PsynergyMove(float h, float v) {
		float offsetX = 0;
		float offsetZ = 0;
		float moveSpeed = 0.03f;

		//determine movement
		if (h == -1) {
			offsetX = -moveSpeed;
		} else if (h == 1) {
			offsetX = moveSpeed;
		}
		
		if (v == -1) {
			offsetZ = -moveSpeed;
		} else if (v == 1) {
			offsetZ = moveSpeed;
		}

		reticleLocation.position = new Vector3 (reticleLocation.position.x + offsetX, reticleLocation.position.y, reticleLocation.position.z + offsetZ);
	}

	void PsynergyRelease() {
		if (Input.GetButtonDown ("Fire2")) {
			isCasting = false;
			reticleLocation.position = new Vector3(0, -1f, 0);
		}
	}

	// Update is called once per frame
	void Update() {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		if (isCasting) {
			PsynergyMove (h, v);
			PsynergyRelease ();
		} else {
			Move (h, v);
			Attack ();
			PsynergyActivate ();
		}
	}
}
