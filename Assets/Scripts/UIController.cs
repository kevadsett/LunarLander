using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private static UIController _instance;

	private Transform _fuelRect;
	private Transform _statusText;

	void Start() {
		_fuelRect = transform.Find("Fuel");
		_statusText = transform.Find("Status");
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
		RectTransform fuelRect = _fuelRect.GetComponent<RectTransform> ();
		fuelRect.localScale = new Vector3 (currentFuel / maxFuel, 1, 1);
	}

	public void UpdateStatusText(string newStatus) {
		_statusText.GetComponent<Text> ().text = newStatus;
	}
}
