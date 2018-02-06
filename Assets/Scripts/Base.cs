using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour {

	public GameObject startPosition;
	public GameObject pivot;
	public Condition[] conditions;
	
	GameObject cameraFocus;
	Vector3 cameraFocalPoint;
	
	GameObject token;

	GameObject position;

	GameObject area;


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

	int moves = 0;

	float posX = 0;
	float posY = 0;
	float lastX = 0;
	float lastY = 0;
	float dx = 0;
	float dy = 0;
	float xRotation = 0;
	float zRotation = 0;
	float dragX = 0;
	float dragZ = 0;
	bool dragLock = false;

	// GameObject target;
	Vector3 targetPos;
	Quaternion targetRotation;
	Quaternion rotSave;

	GameObject message;

	// Transform parent;

	Vector3 pivotOffset;


	bool finished = false;

	GameObject cameraPivot;
	float cameraRotation = -45;
	Vector3 cameraMoveVelocity = Vector3.zero;

	GameObject canvas;
	GameObject banner;

	float t = 0;

	void RotateCameraBy(float deg) {
		cameraPivot.transform.Rotate(Vector3.up, deg);
		Camera.main.transform.LookAt(cameraFocus.transform.position);
	}

	public void SetCameraFocus(GameObject go) {
		cameraFocus = go;
		cameraFocalPoint = cameraFocus.transform.position - new Vector3(0, -0.6f, 0);
	}

	void ResetCamera() {
		// focus = new Vector3(0, -0.6f, 0);
		SetCameraFocus(startPosition.transform.Find("focus").gameObject);
		cameraPivot.transform.position = cameraFocalPoint;
		cameraRotation = -45;
		cameraPivot.transform.rotation = Quaternion.identity;
		Camera.main.transform.rotation = Quaternion.identity;
		Camera.main.transform.position = new Vector3(0, 12, -12);
		RotateCameraBy(-cameraRotation);
		// cameraMoving = false;
	}

	// Test 2d grid coordinate position for valid move
	bool TestPosition(float x, float y) {
		RaycastHit hit;
		Vector3 p = new Vector3(x, 0, y);
		if (Physics.Raycast(p, -Vector3.up, out hit, 1f)) {
			if (hit.transform.CompareTag("Tile")) {

			} else if (hit.transform.CompareTag("TileSymbol")) {
				
			} else if (hit.transform.CompareTag("TileExit")) {

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
			if (hit.transform.CompareTag("Tile")) {

			} else if (hit.transform.CompareTag("TileSymbol")) {
				GameObject go = hit.transform.gameObject;
				int value = go.GetComponent<TileSymbol>().value;
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
					if (go.GetComponent<TileSymbol>().lit == false) {
						go.GetComponent<TileSymbol>().SetLight(true);
						token.GetComponent<Token>().Flash();
						// activated = true;
					}
				}
			} else if (hit.transform.CompareTag("TileExit")) {
				Won();
			}
			// token.GetComponent<Token>().SetGlow(activated);
			// if (activated) {
			// 	token.GetComponent<Token>().LightOn();
			// } else {
			// 	token.GetComponent<Token>().LightOff();
			// }
			position.transform.position = new Vector3(x, 0, y);
			if (area != hit.transform.parent.gameObject) {
				area = hit.transform.parent.gameObject;
				var focus = area.transform.Find("focus");
				if (focus) {
					SetCameraFocus(focus.gameObject);
				}
			}

			// SetCameraFocus(pivot);
			TestConditions();
		}
	}


	public void SetMoves(int value) {
		moves = value;
		GameObject go = canvas.transform.Find("MovesText").gameObject;
		go.GetComponent<Text>().text = "" + moves;
	}

	public void Reset() {
		GameObject[] trigs = GameObject.FindGameObjectsWithTag("TileSymbol");
		foreach (GameObject element in trigs) {
			element.GetComponent<TileSymbol>().SetLight(false);
		}

		area = startPosition;

		pivot.transform.position = startPosition.transform.position;
		pivot.transform.rotation = Quaternion.identity;
		token.transform.rotation = Quaternion.identity;

		posX = pivot.transform.localPosition.x;
		posY = pivot.transform.localPosition.z;
		// position.transform.position = new Vector3(posX, 0, posY);
		token.GetComponent<Token>().Spawn();

		SetPosition(0, 0);
		// SetCameraFocus(pivot);

		ResetCamera();
		// SetMoves(0);
		finished = false;
		SetMoves(0);

		DisplayNotice("STAGE");
	}

	void Won() {
		finished = true;
		DisplayNotice("STAGE CLEAR!", false);
		t = 1;
	}

	void TestWin() {
		// bool done = true;
		// GameObject[] triggers = GameObject.FindGameObjectsWithTag("Trigger");
		// foreach (GameObject element in triggers) {
		// 	if (!element.GetComponent<Trigger>().lit) {
		// 		done = false;
		// 		break;
		// 	}
		// }
		// if (done) Won();
	}

	public void TestConditions() {
		foreach(Condition condition in conditions) {
			if (condition.IsMet()) {
				// condition.target.SetActive(true);
				condition.target.SendMessage("Show");
			} else {
				condition.target.SendMessage("Hide");
				// condition.target.SetActive(false);
			}
		}
	}

	void DisplayNotice(string msg, bool fade=true) {
		StartCoroutine (DisplayNoticeNow(msg));
	}

	public IEnumerator DisplayNoticeNow(string msg) {
		GameObject goBackground = canvas.transform.Find("Background").gameObject;
		GameObject goText = canvas.transform.Find("Text").gameObject;
		Graphic image = goBackground.GetComponent<Image>();
		Text text = goText.GetComponent<Text>();
		text.text = msg;
		image.CrossFadeAlpha(0.5f, 0, true);
		text.CrossFadeAlpha(1, 0, true);

        yield return new WaitForSeconds(1);

        // Code here will be executed after 3 secs
		image.CrossFadeAlpha(0, 1, true);
		text.CrossFadeAlpha(0, 1, true);
    }

	void CreateCanvas() {
		GameObject go = new GameObject("MyCanvas");
		go.layer = 5;

		Canvas canvas = go.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;

		CanvasScaler cs = go.AddComponent<CanvasScaler>();
		cs.scaleFactor = 1.0f;
		cs.dynamicPixelsPerUnit = 100f;

		go.AddComponent<GraphicRaycaster>();

		go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3.0f);
		go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3.0f);

		GameObject goBackground = new GameObject("Background");
		goBackground.transform.parent = go.transform;
		Image backgroundImage = goBackground.AddComponent<Image>();
		backgroundImage.color = Color.black;
		RectTransform rt = goBackground.GetComponent<RectTransform>();
		rt.anchoredPosition = new Vector2(0, 200f);
		rt.anchorMin = new Vector2(0.5f, 0.5f);
		rt.anchorMax = new Vector2(0.5f, 0.5f);
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1024);
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

		GameObject goText = new GameObject("Text");
		goText.transform.parent = go.transform;
		Text text = goText.AddComponent<Text>();

		rt = goText.GetComponent<RectTransform>();
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3.0f);
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3.0f);
		rt.anchoredPosition = new Vector2(0, 200f);
		rt.anchorMax = new Vector2(0.5f, 0.5f);
		rt.anchorMin = new Vector2(0.5f, 0.5f);

		text.color = Color.white;
		text.alignment = TextAnchor.MiddleCenter;
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.verticalOverflow = VerticalWrapMode.Overflow;
		text.font = (Font) Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
		text.fontSize = 36;
		text.fontStyle = FontStyle.Bold;
		text.enabled = true;

		GameObject goMovesText = new GameObject("MovesText");
		goMovesText.transform.parent = go.transform;
		Text movesText = goMovesText.AddComponent<Text>();
		goMovesText.AddComponent<Outline>();
		goMovesText.AddComponent<Shadow>();

		rt = goMovesText.GetComponent<RectTransform>();
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3.0f);
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3.0f);
		rt.anchoredPosition = new Vector2(-75, 65);
		rt.anchorMax = new Vector2(1f, 0f);
		rt.anchorMin = new Vector2(1f, 0f);

		movesText.color = Color.white;
		movesText.alignment = TextAnchor.MiddleCenter;
		movesText.horizontalOverflow = HorizontalWrapMode.Overflow;
		movesText.verticalOverflow = VerticalWrapMode.Overflow;
		movesText.font = (Font) Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
		movesText.fontSize = 48;
		movesText.fontStyle = FontStyle.Bold;
		movesText.enabled = true;

		this.canvas = go;
	}

	// Use this for initialization
	void Start () {
		token = pivot.transform.GetChild(0).gameObject;
		position = new GameObject("Position");
		position.transform.position = new Vector3(0, 0, 0);
		cameraPivot = Camera.main.transform.parent.gameObject;
		CreateCanvas();
		Reset();
	}

	void Step() {
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		// Test whether user has clicked token
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			bool hitSomething = Physics.Raycast(ray, out hit, 100);
			if (hitSomething) {
				// if (hit.transform.CompareTag("Token")) {
				// 	target = token;
				// } else {
				// 	target = null;
				// }
			}
			lastX = Input.mousePosition.x;
			lastY = Input.mousePosition.y;
		}

		// If the left mouse button is depressed, either update the token (if selected) or rotate the view around the board
		if (Input.GetMouseButton(0)) {
			// Vector3 v1 = (Camera.main.transform.position - new Vector3(0, Camera.main.transform.position.y, 0)).normalized;
			float angle = cameraRotation;
			float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
			float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

			dx = dx + Input.mousePosition.x - lastX;
			dy = dy + Input.mousePosition.y - lastY;

			// if (target) {
				if (dragLock == false) {
					float cx = dx * cos - dy * sin;
					float cy = dx * sin + dy * cos;
					float d = Mathf.Sqrt(dx * dx + dy * dy);

					if (dragX == 0 && dragZ == 0 && Mathf.Abs(d) > 5) {
						Vector3 dir = new Vector3(cx, 0, cy).normalized;
						float _x = dir.x;
						float _z = dir.z;

						Vector3 temp = position.transform.position + new Vector3(
							Mathf.Abs(_x) >= Mathf.Abs(_z) ? Mathf.Sign(_x) : 0,
							0,
							Mathf.Abs(_z) >= Mathf.Abs(_x) ? Mathf.Sign(_z) : 0);

						if (TestPosition(temp.x, temp.z)) {
							dragLock = false;
						} else {
							dragLock = true;
						}

						// GameObject.Find("Canvas/Text2").GetComponent<Text>().text = "" + dir;

						if (dragLock == false) {
							pivotOffset = new Vector3(0, 0, 0);
							if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) {
								dragX = dir.x > 0 ? 1 : -1;
								if (dir.x > 0) {
									pivotOffset = transform.right * cubeHalfSize;
								}
								else {
									pivotOffset = transform.right * -cubeHalfSize;
								}
							}
							else {
								dragZ = dir.z > 0 ? 1 : -1;
								if (dir.z > 0) {
									pivotOffset = transform.forward * cubeHalfSize;
								}
								else {
									pivotOffset = -transform.forward * cubeHalfSize;
								}
							}

							pivotOffset += -transform.up * cubeHalfSize;
							pivot.transform.position += pivotOffset;
							// target.transform.position -= pivotOffset;
							token.transform.position -= pivotOffset;
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
			// } else {
				// If nothing is targeted, rotate the view
				// if (!cameraMoving) {
					// cameraRotation += -mx * 8;
					// RotateCameraBy(mx * 8);
				// }
			// }

			lastX = Input.mousePosition.x;
			lastY = Input.mousePosition.y;
		}

		// When the left button is released, test whether the token can move in the chosen direction
		if (Input.GetMouseButtonUp(0)) {
			dragLock = false;

			// if (target) {
				bool moved = false;
				// Vector3 offset;
				float pX = posX, pY = posY;

				pivot.transform.rotation = rotSave;

				if (zRotation > 45) {
					pX++;
					if (TestPosition(pX, pY)) {
						posX = pX;
						pivot.transform.position += transform.right;
						token.transform.Rotate(-transform.forward, 90, Space.World);
						moved = true;
					}
				}
				else if (zRotation < -45) {
					pX--;
					if (TestPosition(pX, pY)) {
						posX = pX;
						pivot.transform.position -= transform.right;
						token.transform.Rotate(transform.forward, 90, Space.World);
						moved = true;
					}
				}
				else if (xRotation > 45) {
					pY++;
					if (TestPosition(pX, pY)) {
						posY = pY;
						pivot.transform.position += transform.forward;
						token.transform.Rotate(transform.right, 90, Space.World);
						moved = true;
					}
				}
				else if (xRotation < -45) {
					pY--;
					if (TestPosition(pX, pY)) {
						posY = pY;
						pivot.transform.position -= transform.forward;
						token.transform.Rotate(-transform.right, 90, Space.World);
						moved = true;
					}
				}

				pivot.transform.position -= pivotOffset;
				// target.transform.position += pivotOffset;
				pivot.transform.localPosition = new Vector3(posX, 0, posY);
				token.transform.localPosition = new Vector3(0, 0, 0);

				if (moved) {
					SetPosition(pivot.transform.position.x, pivot.transform.position.z);
					SetMoves(moves + 1);

					// TestWin();
				}
			// }
			// target = null;
			dx = 0;
			dy = 0;
			xRotation = 0;
			zRotation = 0;
			dragX = 0;
			dragZ = 0;
			pivotOffset = new Vector3(0, 0, 0);
			rotSave = pivot.transform.rotation;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!finished) {
			Step();
		} else {
			if (t != 0) {
				t -= 2f * Time.deltaTime;
				if (t <= 0) {
					t = 0;
					token.GetComponent<Token>().Gone();
				}
			}
		}
		if (cameraPivot.transform.position != cameraFocalPoint) {
			
			// float step = 1 * Time.deltaTime;
			// cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, cameraFocalPoint, step);
			var pos = Vector3.SmoothDamp(cameraPivot.transform.position, cameraFocalPoint, ref cameraMoveVelocity, 0.5f);
			cameraPivot.transform.position = pos;
			// if (cameraPivot.transform.position == cameraFocalPoint) cameraMoving = false;
		}
	}

	void LateUpdate() {
	}
}
