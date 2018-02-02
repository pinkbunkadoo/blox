using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
	GameObject token;

	Vector3 offset;
	Vector3 target;

	public void Refocus() {
		// target = token.transform.position + offset;
	}

	// Use this for initialization
	void Start () {
		// token = GameObject.FindWithTag("Token");
		// offset = transform.position - token.transform.position;
		// target = transform.position;
	}

	// Update is called once per frame
	void Update () {

	}

	void LateUpdate () {
		// float step = 2f * Time.deltaTime;
		// transform.position = Vector3.MoveTowards(transform.position, target, step);
	}
}
