using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {
	int state = 0;
	float t = 0;

	public void SetState(int state) {
		this.state = state;
		if (this.state == 1) { // shrink
			t = 0;
		}
		else if (this.state == 2) { // flash
			t = 1;
			// gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f));
			gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1, 1, 1));
		}
	}

	public void Spawn() {
		gameObject.SetActive(true);
	}

	public void Gone() {
		SetState(1);
	}

	public void Flash() {
		SetState(2);
	}

	public void SetGlow(bool value) {
		// if (value)
		// 	gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
		// else
		// 	gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 0f, 0f));
	}

	public void LightOn() {
		// gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.1f, 0.1f, 0.1f));
		// on = true;
	}

	public void LightOff() {
		// print("LightOff");
		// gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0, 0, 0));
		// on = false;
	}

	// Use this for initialization
	void Start () {
		// SetState(2);
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {
			// Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// RaycastHit hit;
			// bool hitSomething = Physics.Raycast(ray, out hit, 100);
			// if (on) LightOff();
			// if (hitSomething) {
			// 	if (hit.transform.CompareTag("Token")) {
			// 		LightOn();
			// 	}
			// }
		} else if (state == 1) {
			t += 1f * Time.deltaTime;
			// var a = Mathf.Lerp(1.0f, 0f, t*t);
			var a = 1 - t;
			gameObject.transform.localScale = new Vector3(a, a, a);
			gameObject.transform.Rotate(Vector3.up, 320 * Time.deltaTime, Space.World);
			if (t >= 1) {
				state = 0;
				gameObject.transform.localScale = new Vector3(1, 1, 1);
				gameObject.transform.rotation = Quaternion.identity;
				gameObject.transform.localPosition = new Vector3(0, 0, 0);
				gameObject.SetActive(false);
			}
		} else if (state == 2) {
			t -= 1.5f * Time.deltaTime;
			var a = Mathf.Lerp(0f, 1f, t*t);
			gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1 * a, 1 * a, 1 * a, 1));
			if (t <= 0) {
				state = 0;
			}
		}
	}
}
