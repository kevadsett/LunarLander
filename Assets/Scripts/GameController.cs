using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameModel Model;
	public GameObject Lander;
	public GameObject Debris;

	private static GameController _instance;

	public static GameController Instance {
		get {
			if (!_instance) {
				_instance = GameObject.Find("MainCamera").GetComponent<GameController> ();
			}
			return _instance;
		}
	}

	void Update() {
		if ((Model.CurrentState == GameModel.State.Lose || Model.CurrentState == GameModel.State.Win) && Input.anyKeyDown) {
			Reset ();
		}
	}

	private void Reset() {
		SetRunningState ();
		Physics.gravity = Vector3.up * -1;
		ClearDebris ();
		Instantiate (Lander);
		SetRunningState ();
	}

	public void SetWinState() {
		SetState (GameModel.State.Win);
		UIController.Instance.UpdateStatusText ("You win");
	}

	public void SetLoseState() {
		SetState (GameModel.State.Lose);
		UIController.Instance.UpdateStatusText ("You lose");
	}

	public void SetRunningState() {
		SetState (GameModel.State.Running);
		UIController.Instance.UpdateStatusText ("");
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
