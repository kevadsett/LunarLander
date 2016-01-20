using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct LanderModel {

	public float Thrust;
	public float MaxThrust;
	public float Acceleration;

	public float CrashVelocity;

	public float SideThrust;
	public float MaxSideThrust;
	public float SideAcceleration;

	public float CrashRotation; // TODO: Make this relative to landing platform

	public enum State { 
		Alive,
		Dead
	}
	public State CurrentState;

	public float MaxFuel;
	public float Fuel;
	public float FuelDepletionRate;
}
