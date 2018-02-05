using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {
	float t = 0;
	int state = 0;
	bool showing = true;

	void OnEnable() {		
	}
	void OnDisable() {
	}

	public void Show() {
		// transform.GetChild(0).gameObject.GetComponent<TileScript>().Appear();
		if (!showing) {
			foreach(Transform child in transform){
				if (child.CompareTag("Tile")) {
					child.gameObject.GetComponent<BoxCollider>().enabled = false;
					child.localScale = new Vector3(0, 0, 0);
				}
			}
			state = 1;
			t = 0;
			showing = true;
		}
	}

	public void Hide() {
		if (showing) {
			foreach(Transform child in transform){
				if (child.CompareTag("Tile")) {
					child.gameObject.GetComponent<BoxCollider>().enabled = false;
					child.localScale = new Vector3(0, 0, 0);
				}
			}
			showing = false;
			// state = 1;
			// t = 0;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 1) {
			t += 2 * Time.deltaTime;
			if (t <= 1) {
				foreach(Transform child in transform){
					if (child.CompareTag("Tile")) {
						child.localScale = new Vector3(1*t, 1*t, 1*t);
					}
				}
			} else {
				foreach(Transform child in transform){
					if (child.CompareTag("Tile")) {
						child.localScale = new Vector3(1, 1, 1);
						child.gameObject.GetComponent<BoxCollider>().enabled = true;
					}
				}
				state = 0;
			}
		}
	}
}
