using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	private static UIController _instance;

	private Transform _fuel;

	void Start() {
		_fuel = transform.Find("Fuel");
	}

	public static UIController Instance {
		get {
			if (!_instance) {
				_instance = GameObject.Find("UICanvas").GetComponent<UIController> ();
			}
			return _instance;
		}
	}

	public void UpdateFuelScale(float currentFuel, float maxFuel) {
		RectTransform fuelRect = _fuel.GetComponent<RectTransform> ();
		fuelRect.localScale = new Vector3 (currentFuel / maxFuel, 1, 1);
	}
}
