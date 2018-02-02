﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	public static int ONE = 1;
	public static int TWO = 2;
	public static int THREE = 3;
	public static int FOUR = 4;
	public static int FIVE = 5;
	public static int SIX = 6;

	public int value = Trigger.ONE;
	public bool active = false;

	public bool SetActive(bool active) {
		if (this.active != active) {
			this.active = active;
			var go1 = transform.GetChild(0).gameObject;
			var	go2 = transform.GetChild(1).gameObject;
			if (this.active) {
				// go1.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
				// go2.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0.75f, 1, 1));
				go2.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 0.85f, 1f));
			} else {
				// go2.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
				go2.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
			}
			return true;
		}
		return false;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
