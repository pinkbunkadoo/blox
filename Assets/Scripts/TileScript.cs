using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {
	int state;
	float t = 0;

	public void Appear() {
		state = 1;
		t = 0;
		gameObject.GetComponent<BoxCollider>().enabled = false;
	}

	public void Disappear() {
		state = 2;
		t = 1;
		gameObject.GetComponent<BoxCollider>().enabled = false;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 1) {
			if (t <= 1) {
				t += Time.deltaTime;
				transform.localScale = new Vector3(1*t, 1*t, 1*t);
			} else {
				transform.localScale = new Vector3(1, 1, 1);
				gameObject.GetComponent<BoxCollider>().enabled = true;
				state = 0;
			}
		} else if (state == 2) {
			if (t >= 0) {
				t -= Time.deltaTime;
				transform.localScale = new Vector3(1*t, 1*t, 1*t);
			} else {
				transform.localScale = new Vector3(0, 0, 0);
				// gameObject.GetComponent<BoxCollider>().enabled = false;
				state = 0;
			}
		}
	}
}
