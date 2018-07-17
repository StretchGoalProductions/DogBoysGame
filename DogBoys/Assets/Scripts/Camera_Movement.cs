using UnityEngine;

public class Camera_Movement : MonoBehaviour {

	// Public Variables
	public float panSpeed = 20f;				// Camera pan speed
	public float panborderThickness = 10f;		// Border Thickness for panning camera with mouse
	
	public Vector4 panLimit;					// Clamps for camera pan. X = -pos.x, Y = pos.x, Z = -pos.z, W = pos.z

	public float scrollSpeed = 5f;				// Scroll speed for zoom
	public float minY = 10f;					// Min zoom of camera
	public float maxY = 16f;					// Max zoom of camera

	public float flipSpeed = 75f;
	public float rotateSpeed = 75f;

	public bool toggle = false;
	public bool p1 = true;

	// Private Variables
	private float progress = 0f;
	private float ang = 0;

	enum Direction {North, East, South, West};
	Direction dir = Direction.North;
	private bool rotating = false;

	void Update () {
		Debug.Log(dir);
		if (!toggle) {
			// Current possition of the Camera
			Vector3 pos = new Vector3(0, transform.position.y, 0);
				
			if (!rotating) {
				// Get new pos based on input
				if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panborderThickness) {
					pos.z = panSpeed * Time.deltaTime;
				}
				if (Input.GetKey("s") || Input.mousePosition.y <= panborderThickness) {
					pos.z = -panSpeed * Time.deltaTime;
				}
				if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panborderThickness) {
					pos.x = panSpeed * Time.deltaTime;
				}
				if (Input.GetKey("a") || Input.mousePosition.x <= panborderThickness) {
					pos.x = -panSpeed * Time.deltaTime;
				}

				// Get new rotation based on input
				if (Input.GetKeyDown("q")) {
					ang = transform.localEulerAngles.y + 90f;
					rotating = true;
				} 
				if (Input.GetKeyDown("e")) {
					if (transform.localEulerAngles.y > -1 && transform.localEulerAngles.y < 1){
						transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 359.9f, transform.eulerAngles.z);
						ang = 359.9f;
					}
					ang = transform.localEulerAngles.y - 90f;
					rotating = true;
				} 

				// Check Scroll for zoom
				float scroll = Input.GetAxis("Mouse ScrollWheel");
				pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
			}
			
			// Clamp camera movement
			pos.y = Mathf.Clamp(pos.y, minY, maxY);

			if (dir == Direction.North) {
				if (transform.position.z + pos.z >= panLimit.w) {
					pos.z = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.w - 0.0001f);
				}
				if (transform.position.z + pos.z <= panLimit.z) {
					pos.z = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.z + 0.0001f);
				}
				if (transform.position.x + pos.x >= panLimit.y) {
					pos.x = 0f;
					transform.position = new Vector3(panLimit.y + 0.0001f, transform.position.y, transform.position.z);
				}
				if (transform.position.x + pos.x <= panLimit.x) {
					pos.x = 0f;
					transform.position = new Vector3(panLimit.x - 0.0001f, transform.position.y, transform.position.z);
				}
			} else if (dir == Direction.East) {
				if (transform.position.x + pos.z >= panLimit.y) {
					pos.z = 0f;
					transform.position = new Vector3(panLimit.y - 0.0001f, transform.position.y, transform.position.z);
				}
				if (transform.position.x + pos.z <= panLimit.x) {
					pos.z = 0f;
					transform.position = new Vector3(panLimit.x + 0.0001f, transform.position.y, transform.position.z);
				}
				if (transform.position.z - pos.x <= panLimit.z) {
					pos.x = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.z + 0.0001f);
				}
				if (transform.position.z - pos.x >= panLimit.w) {
					pos.x = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.w - 0.0001f);
				}
			} else if (dir == Direction.South) {
				if (transform.position.z - pos.z >= panLimit.w) {
					pos.z = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.w - 0.0001f);
				}
				if (transform.position.z - pos.z <= panLimit.z) {
					pos.z = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.z + 0.0001f);
				}
				if (transform.position.x - pos.x >= panLimit.y) {
					pos.x = 0f;
					transform.position = new Vector3( panLimit.y + 0.0001f, transform.position.y, transform.position.z);
				}
				if (transform.position.x - pos.x <= panLimit.x) {
					pos.x = 0f;
					transform.position = new Vector3( panLimit.x - 0.0001f, transform.position.y, transform.position.z);
				}
			} else if (dir == Direction.West) {
				if (transform.position.x - pos.z >= panLimit.y) {
					pos.z = 0f;
					transform.position = new Vector3(panLimit.y - 0.0001f, transform.position.y, transform.position.z);
				}
				if (transform.position.x - pos.z <= panLimit.x) {
					pos.z = 0f;
					transform.position = new Vector3(panLimit.x + 0.0001f, transform.position.y, transform.position.z);
				}
				if (transform.position.z + pos.x <= panLimit.z) {
					pos.x = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.z + 0.0001f);
				}
				if (transform.position.z + pos.x >= panLimit.w) {
					pos.x = 0f;
					transform.position = new Vector3(transform.position.x, transform.position.y, panLimit.w - 0.0001f);
				}
			}

			// Transform camera
			transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);
			transform.Translate(pos.x, 0, pos.z);

			// Rotate camera
			if (rotating) {
				float smoothTime = Mathf.Sin(progress) * rotateSpeed * Time.deltaTime;
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, ang, smoothTime);
				Debug.Log("newRotY:" + newRotY);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				progress += 0.01f * Time.deltaTime;
				if (transform.localEulerAngles.y > ang - 0.1f && transform.localEulerAngles.y < ang + 0.1f) {
					if (transform.localEulerAngles.y > 359f && transform.localEulerAngles.y < 361f) {
						transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
						dir = Direction.North;
					}
					else if (transform.localEulerAngles.y > 269f && transform.localEulerAngles.y < 271f) {
						transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 270f, transform.eulerAngles.z);
						dir = Direction.West;
					}
					else if (transform.localEulerAngles.y > 179f && transform.localEulerAngles.y < 181f) {
						transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
						dir = Direction.South;
					}
					else if (transform.localEulerAngles.y > 89f && transform.localEulerAngles.y < 91f) {
						transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
						dir = Direction.East;
					}
					else if (transform.localEulerAngles.y > -1f && transform.localEulerAngles.y < 1f) {
						transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
						dir = Direction.North;
					}
					progress = 0f;
					rotating = false;
					Debug.Log("Rotating Complete");
				}
			}
		} else {
			if (p1) {
				float smoothTime = Mathf.Sin(progress) * flipSpeed * Time.deltaTime;
				float newPosX = Mathf.Lerp(transform.position.x, 13f, smoothTime);
				float newPosY = Mathf.Lerp(transform.position.y, 13f, smoothTime);
				float newPosZ = Mathf.Lerp(transform.position.z, 30f, smoothTime);
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, 180, smoothTime);
				transform.position = new Vector3(newPosX, newPosY, newPosZ);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				progress += 0.01f * Time.deltaTime;
				if (Vector3.Distance(transform.position, new Vector3(13f, 13f, 30f)) <= 0.1f) {
					transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
					dir = Direction.South;
					p1 = false;
					toggle = false;
					progress = 0f;
				}
			} else {
				float smoothTime = Mathf.Sin(progress) * flipSpeed * Time.deltaTime;
				float newPosX = Mathf.Lerp(transform.position.x, 13f, smoothTime);
				float newPosY = Mathf.Lerp(transform.position.y, 13f, smoothTime);
				float newPosZ = Mathf.Lerp(transform.position.z, 5f, smoothTime);
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, 0, smoothTime);
				transform.position = new Vector3(newPosX, newPosY, newPosZ);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				progress += 0.01f * Time.deltaTime;
				if (Vector3.Distance(transform.position, new Vector3(13f, 13f, 5f)) <= 0.1f) {
					transform.localEulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
					dir = Direction.North;
					p1 = true;
					toggle = false;
					progress = 0f;
				}
			}
		}
	}
}
