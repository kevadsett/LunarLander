﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameModel Model;
	public GameObject Lander;
	public GameObject Debris;

	private static GameController _instance;

	public static GameController Instance {
		get {
			if (!_instance) {
				_instance = Camera.main.GetComponent<GameController> ();
			}
			return _instance;
		}
	}

	void Update() {
		if (Model.CurrentState == GameModel.State.Lose && Input.anyKeyDown) {
			Reset ();
		}
	}

	private void Reset() {
		SetRunningState ();
		Physics.gravity = Vector3.up * -1;
		ClearDebris ();
		Instantiate (Lander);
	}

	public void SetWinState() {
		SetState (GameModel.State.Win);
		Debug.Log ("You win");
	}

	public void SetLoseState() {
		SetState (GameModel.State.Lose);
	}

	public void SetRunningState() {
		SetState (GameModel.State.Running);
	}

	public void AddDebris(Transform t) {
		t.SetParent (Debris.transform);
	}

	private void ClearDebris() {
		foreach (Transform child in Debris.transform) {
			Destroy (child.gameObject);
		}
	}

	private void SetState(GameModel.State state) {
		Model.CurrentState = state;
	}


}
