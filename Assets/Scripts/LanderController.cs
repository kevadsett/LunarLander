using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanderController : MonoBehaviour {

	public LanderModel Model;

	private float _previousVelocity;
	private float _previousRotation;

	void FixedUpdate() {
		float thrust = 0.0f;
		float rotationalThrust = 0.0f;
		if (Model.IsAlive) {
			GameObject thrusterFlare = transform.Find ("ThrusterFlare").gameObject;
			GameObject thrusterFlareLeft = transform.Find ("ThrusterFlareLeft").gameObject;
			GameObject thrusterFlareRight = transform.Find ("ThrusterFlareRight").gameObject;
			if (Model.Fuel > 0) {
				thrust = GetThrust ();
				if (thrust > 0) {
					thrusterFlare.SetActive (true);
				} else {
					thrusterFlare.SetActive (false);
				}
				rotationalThrust = GetSideThrust ();

				if (rotationalThrust > 0) {
					thrusterFlareLeft.SetActive (true);
					thrusterFlareRight.SetActive (false);
				} else if (rotationalThrust < 0) {
					thrusterFlareLeft.SetActive (false);
					thrusterFlareRight.SetActive (true);
				} else {
					thrusterFlareLeft.SetActive (false);
					thrusterFlareRight.SetActive (false);
				}

				Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
				rb.AddRelativeForce (Vector3.up * thrust * Time.deltaTime);
				rb.AddRelativeTorque (transform.forward * rotationalThrust * Time.deltaTime);
				_previousVelocity = rb.velocity.magnitude;
				_previousRotation = rb.rotation.z;
			} else {
				Model.Fuel = 0;
				HideAllThrusters ();
			}

			UIController.Instance.UpdateFuelScale (Model.Fuel, Model.MaxFuel);
		}
	}

	void OnCollisionEnter (Collision col) {
		if (col.transform.root.name == "Level") {
			if (_previousVelocity > Model.CrashVelocity || Mathf.Abs(_previousRotation) > Model.CrashRotation || Model.Fuel == 0) {
				Model.CurrentState = LanderModel.State.Dead;
				Explode ();
				HideAllThrusters ();
			} else if (col.transform.name == "LandingPlatform") {
				Model.CurrentState = LanderModel.State.Dead;
				GameController.Instance.SetWinState ();
				HideAllThrusters ();
			}
		}
	}

	private float GetThrust() {
		float thrust = 0.0f;
		if (Input.GetKey (KeyCode.UpArrow)) {
			Model.Fuel -= Model.FuelDepletionRate;
			thrust = Model.Thrust;
		}
		return thrust;
	}

	private float GetSideThrust() {
		float thrust = 0;
		if (Input.GetKey (KeyCode.LeftArrow)) {
			thrust = Model.SideThrust;
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			thrust = -Model.SideThrust;
		}
		return thrust;
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

	private void HideAllThrusters() {
		Transform thrusterFlare = transform.Find ("ThrusterFlare");
		if (thrusterFlare) {
			thrusterFlare.gameObject.SetActive (false);
		}
		Transform thrusterFlareLeft = transform.Find ("ThrusterFlareLeft");
		if (thrusterFlareLeft) {
			thrusterFlareLeft.gameObject.SetActive (false);
		}
		Transform thrusterFlareRight = transform.Find ("ThrusterFlareRight");
		if (thrusterFlareRight) {
			thrusterFlareRight.gameObject.SetActive (false);
		}
	}

}
