using System;

[Serializable]
public struct GameModel {

	public enum State {
		Running,
		Win,
		Lose
	}

	public State CurrentState;
	public int Points;
	public int time;
}
