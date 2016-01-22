using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct LanderModel {

	public float Thrust;
	public float SideThrust;

	public float CrashVelocity;
	public float CrashRotation; // TODO: Make this relative to landing platform

	public enum State { 
		Alive,
		Dead
	}
	public State CurrentState;

	public bool IsAlive {
		get {
			return CurrentState == State.Alive;
		}
	}

	public float MaxFuel;
	public float Fuel;
	public float FuelDepletionRate;
}
