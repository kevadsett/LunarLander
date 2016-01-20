using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanderController : MonoBehaviour {

	public LanderModel Model;

	private float _previousVelocity;
	private float _previousRotation;

	void FixedUpdate() {
		Transform ThrusterFlare = transform.Find ("ThrusterFlare");
		Transform ThrusterFlareLeft = transform.Find ("ThrusterFlareLeft");
		Transform ThrusterFlareRight = transform.Find ("ThrusterFlareRight");
		if (Model.CurrentState == LanderModel.State.Alive) {
			if (Model.Fuel > 0) { 
				if (Input.GetKey (KeyCode.UpArrow)) {
					Model.Fuel -= Model.FuelDepletionRate;
					ThrusterFlare.localScale = new Vector3 (1, 0.4f, 1);
					if (Model.Thrust < Model.MaxThrust) {
						Model.Thrust += Model.Acceleration * Time.deltaTime;
					}
				} else {
					ThrusterFlare.localScale = new Vector3 (1, 0.0f, 1);
					Model.Thrust = 0;
				}

				if (Input.GetKey (KeyCode.LeftArrow)) {
					Model.Fuel -= Model.FuelDepletionRate / 2;
					ThrusterFlareLeft.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
					ThrusterFlareRight.localScale = new Vector3 (0.2f, 0, 0.2f);
					if (Model.SideThrust < Model.MaxSideThrust) {
						Model.SideThrust += Model.SideAcceleration * Time.deltaTime;
					}
				} else if (Input.GetKey (KeyCode.RightArrow)) {
					Model.Fuel -= Model.FuelDepletionRate / 2;
					ThrusterFlareLeft.localScale = new Vector3 (0.2f, 0, 0.2f);
					ThrusterFlareRight.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
					if (Model.SideThrust > -Model.MaxSideThrust) {
						Model.SideThrust -= Model.SideAcceleration * Time.deltaTime;
					}
				} else {
					ThrusterFlareLeft.localScale = new Vector3 (0.2f, 0, 0.2f);
					ThrusterFlareRight.localScale = new Vector3 (0.2f, 0, 0.2f);
					if (Model.SideThrust > 0) {
						Model.SideThrust -= (Model.SideAcceleration * 2) * Time.deltaTime;
					} else if (Model.SideThrust < 0) {
						Model.SideThrust += (Model.SideAcceleration * 2) * Time.deltaTime;
					}
				}
				Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
				rb.AddRelativeForce (Vector3.up * Model.Thrust);
				rb.AddRelativeTorque (transform.forward * Model.SideThrust);
				_previousVelocity = rb.velocity.magnitude;
				_previousRotation = rb.rotation.z;
			} else {
				Model.Fuel = 0;
				ThrusterFlare.localScale = new Vector3 (0, 0, 0);
				ThrusterFlareLeft.localScale = new Vector3 (0, 0, 0);
				ThrusterFlareRight.localScale = new Vector3 (0, 0, 0);
			}

			UIController.Instance.UpdateFuelScale (Model.Fuel, Model.MaxFuel);
		}
	}

	void OnCollisionEnter (Collision col) {
		if (col.transform.root.name == "Level") {
			if (_previousVelocity > Model.CrashVelocity || Mathf.Abs(_previousRotation) > Model.CrashRotation ) {
				Model.CurrentState = LanderModel.State.Dead;
				Explode ();
			} else if (col.transform.name == "LandingPlatform") {
				Model.CurrentState = LanderModel.State.Dead;
				GameController.Instance.SetWinState ();
			}
		}
	}

	private void Explode() {
		Physics.gravity = Vector3.up * -9.81f;
		List<Transform> children = new List<Transform> ();
		foreach (Transform child in transform) {
			if (child.name.Contains ("ThrusterFlare")) {
				Destroy (child.gameObject);
			} else {
				child.gameObject.AddComponent<BoxCollider> ();
				child.gameObject.AddComponent<Rigidbody> ();
				child.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ;
				children.Add (child);
			}
		}
		foreach (Transform child in children) {
			GameController.Instance.AddDebris (child);
		}
		transform.DetachChildren ();
		Destroy (gameObject);
		GameController.Instance.SetLoseState ();
	}

}
