using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour {

	public GameObject pivot;
	public GameObject token;
	public GameObject glowPrefab;
	public GameObject sphere;

	// int mapWidth = 5;
	// int mapHeight = 5;
	static float cubeSize = 1f;
	static float cubeHalfSize = cubeSize * 0.5f;

	// int[] map = {
	// 	1, 1, 1, 1, 1,
	// 	1, 0, 0, 1, 1,
	// 	1, 1, 0, 1, 1,
	// 	1, 1, 1, 1, 1,
	// 	1, 1, 1, 1, 1
	// 	};

	int goal;

	float posX = 0;
	float posY = 0;

	float lastX = 0;
	float lastY = 0;
	float dx = 0;
	float dy = 0;

	float baseRotation = 0;
	float xRotation = 0;
	float zRotation = 0;

	// float f = 0;

	// float rotX = 0;
	// float rotZ = 0;

	Vector3 lastPos;
	Vector3 mouseDownPos;
	bool dragLock = false;

	float dragX = 0;
	float dragZ = 0;

	GameObject target;
	Vector3 targetPos;
	Quaternion targetRotation;
	Quaternion rotSave;

	UnityEngine.Plane targetPlane;

	Vector3 dragDirection;

	GameObject message;

	Transform parent;

	Vector3 rotPoint;
	Vector3 pivotOffset;

	Vector3 focus;

	bool finished = false;

	GameObject cameraPivot;
	float cameraRotation = 0;

	void RotateCameraBy(float deg) {
		cameraPivot.transform.Rotate(Vector3.up, deg);
		Camera.main.transform.LookAt(focus);
	}

// Test 2d grid coordinate position for valid move
	bool TestPosition(float x, float y) {
		RaycastHit hit;

		Vector3 p = new Vector3(x, 0, y);

		if (Physics.Raycast(p, -Vector3.up, out hit, 1f)) {
			if (hit.transform.CompareTag("Trigger")) {
			} else if (hit.transform.CompareTag("Tile")) {
			} else {
			}
			return true;
		} else { // No tile found
		}
		return false;

	}

	void SetPosition(float x, float y) {
		RaycastHit hit;

		Vector3 p = new Vector3(x, 0, y);

		if (Physics.Raycast(p, -Vector3.up, out hit, 1f)) {

			if (hit.transform.CompareTag("Trigger")) {
				GameObject go = hit.transform.gameObject;
				int value = go.GetComponent<Trigger>().value;
				int side = 0;

				if (Mathf.Round(token.transform.up.y) == -1) { // 1
					side = 1;
				}
				else if (Mathf.Round(token.transform.up.y) == 1) { // 6
					side = 6;
				}
				else if (Mathf.Round(token.transform.forward.y) == 1) { // 4
					side = 4;
				}
				else if (Mathf.Round(token.transform.forward.y) == -1) { // 3
					side = 3;
				}
				else if (Mathf.Round(token.transform.right.y) == -1) { // 5
					side = 5;
				}
				else if (Mathf.Round(token.transform.right.y) == 1) { // 2
					side = 2;
				}

				if (value == side) {
					if (go.GetComponent<Trigger>().SetActive(true)) {
					}
				}
			}
		}

		sphere.transform.position = new Vector3(x, 0, y);
	}

	// Use this for initialization
	void Start () {
		token = GameObject.FindWithTag("Token");
		sphere = GameObject.Find("Sphere");
		// cursor = GameObject.Find("Cursor");
		// cursorEnd = GameObject.Find("CursorEnd");
		cameraPivot = GameObject.Find("CameraPivot");

		parent = pivot.transform.parent;
		posX = pivot.transform.localPosition.x;
		posY = pivot.transform.localPosition.z;

		sphere.transform.position = new Vector3(posX, 0, posY);

		focus = new Vector3(0, -0.6f, 0);

		cameraRotation = -45;
		RotateCameraBy(-cameraRotation);

		GameObject[] trigs = GameObject.FindGameObjectsWithTag("Trigger");
		goal = trigs.Length;

		message = GameObject.Find("Canvas/Message");		
		showMessage("Stage");
	}

	void showMessage(string msg) {
		message.GetComponent<Message>().Display(msg);
	}

	void Step() {
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		// Test whether user has clicked token
		if (Input.GetMouseButtonDown(0)) {
			baseRotation = 0;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100)) {
				if (hit.transform.tag == "TokenPivot" && hit.normal == Vector3.up) {
					// print(hit.transform);
					dx = 0;
					dy = 0;
					xRotation = 0;
					zRotation = 0;
					dragX = 0;
					dragZ = 0;

					pivotOffset = new Vector3(0, 0, 0);
					rotSave = pivot.transform.rotation;

					target = token;
				} else {
					target = null;
				}
			}
			lastX = Input.mousePosition.x;
			lastY = Input.mousePosition.y;
		}

		// If the left mouse button is depressed, either update the token (if selected) or rotate the view around the board
		if (Input.GetMouseButton(0)) {
			Vector3 v1 = (Camera.main.transform.position - new Vector3(0, Camera.main.transform.position.y, 0)).normalized;
			// GameObject.Find("Canvas/Text1").GetComponent<Text>().text = "Cam:" + v1.ToString();

			float angle = cameraRotation;
			// GameObject.Find("Canvas/Text2").GetComponent<Text>().text = "Deg:" + angle.ToString("n2");

			float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
			float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

			dx = dx + Input.mousePosition.x - lastX;
			dy = dy + Input.mousePosition.y - lastY;

			if (target) {
				if (dragLock == false) {
					float cx = dx * cos - dy * sin;
					float cy = dx * sin + dy * cos;

					Vector3 dir = new Vector3(cx, 0, cy).normalized;
					// GameObject.Find("Canvas/Text3").GetComponent<Text>().text = "Dir:" + dir.ToString();

					float d = Mathf.Sqrt(dx * dx + dy * dy);
					// GameObject.Find("Canvas/Text3").GetComponent<Text>().text = "d:" + d;

					if (dragX == 0 && dragZ == 0 && Mathf.Abs(d) > 5) {
						float _x = dir.x;
						float _z = dir.z;
						Vector3 temp = sphere.transform.position + new Vector3(
							Mathf.Abs(_x) > Mathf.Abs(_z) ? Mathf.Sign(_x) : 0,
							0,
							Mathf.Abs(_z) > Mathf.Abs(_x) ? Mathf.Sign(_z) : 0);

						if (TestPosition(temp.x, temp.z)) {
							dragLock = false;
						} else {
							dragLock = true;
						}

						if (dragLock == false) {
							pivotOffset = new Vector3(0, 0, 0);

							if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) {
								dragX = dir.x > 0 ? 1 : -1;
								if (dir.x > 0) {
									pivotOffset = parent.transform.right * cubeHalfSize;
								}
								else {
									pivotOffset = parent.transform.right * -cubeHalfSize;
								}
							}
							else {
								dragZ = dir.z > 0 ? 1 : -1;
								if (dir.z > 0) {
									pivotOffset = parent.transform.forward * cubeHalfSize;
								}
								else {
									pivotOffset = -parent.transform.forward * cubeHalfSize;
								}
							}

							pivotOffset += -parent.transform.up * cubeHalfSize;
							pivot.transform.position += pivotOffset;
							target.transform.position -= pivotOffset;
						}

					} else {
						if (dragLock == false) {
							float rx = mx * cos - my * sin;
							float ry = mx * sin + my * cos;

							if (mx != 0 || my != 0) {
								if (dragX != 0) {
									zRotation += rx * 10f;
									zRotation = dragX > 0 ? Mathf.Clamp(zRotation, 0, 90) : Mathf.Clamp(zRotation, -90, 0);
									pivot.transform.localEulerAngles = new Vector3(0, 0, -zRotation);
								}
								else if (dragZ != 0) {
									xRotation += ry * 10f;
									xRotation = dragZ > 0 ? Mathf.Clamp(xRotation, 0, 90) : Mathf.Clamp(xRotation, -90, 0);
									pivot.transform.localEulerAngles = new Vector3(xRotation, 0, 0);
								}
							}
						}
					}
				}
			} else {
				// If nothing is targeted rotate the view
				cameraRotation += -mx * 8;
				RotateCameraBy(mx * 8);
			}

			lastX = Input.mousePosition.x;
			lastY = Input.mousePosition.y;
		}

		// When the left button is released, test whether the token can move in the chosen direction
		if (Input.GetMouseButtonUp(0)) {
			dragLock = false;

			if (target) {
				bool moved = false;
				// Vector3 offset;
				float pX = posX, pY = posY;

				pivot.transform.rotation = rotSave;

				if (zRotation > 45) {
					pX++;
					if (TestPosition(pX, pY)) {
						posX = pX;
						pivot.transform.position += parent.right;
						target.transform.Rotate(-parent.forward, 90, Space.World);
						moved = true;
					}
				}
				else if (zRotation < -45) {
					pX--;
					if (TestPosition(pX, pY)) {
						posX = pX;
						pivot.transform.position -= parent.right;
						target.transform.Rotate(parent.forward, 90, Space.World);
						moved = true;
					}
				}
				else if (xRotation > 45) {
					pY++;
					if (TestPosition(pX, pY)) {
						posY = pY;
						pivot.transform.position += parent.forward;
						target.transform.Rotate(parent.right, 90, Space.World);
						moved = true;
					}
				}
				else if (xRotation < -45) {
					pY--;
					if (TestPosition(pX, pY)) {
						posY = pY;
						pivot.transform.position -= parent.forward;
						target.transform.Rotate(-parent.right, 90, Space.World);
						moved = true;
					}
				}

				pivot.transform.position -= pivotOffset;
				target.transform.position += pivotOffset;
				pivot.transform.localPosition = new Vector3(posX, 0, posY);
				target.transform.localPosition = new Vector3(0, 0, 0);

				if (moved) {
					SetPosition(pivot.transform.position.x, pivot.transform.position.z);
					// sphere.transform.position = new Vector3(pivot.transform.position.x, 0, pivot.transform.position.z);
					bool done = true;
					GameObject[] trigs = GameObject.FindGameObjectsWithTag("Trigger");
					foreach (GameObject element in trigs) {
						if (!element.GetComponent<Trigger>().active) {
							done = false;
							break;
						}
					}
					if (done) {
						finished = true;
						showMessage("CLEAR!!!");
					}
				}
			}
			target = null;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!finished) {
			Step();
		}
	}

	void LateUpdate() {
	}
}
