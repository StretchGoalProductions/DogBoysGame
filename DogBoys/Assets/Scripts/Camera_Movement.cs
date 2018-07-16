using UnityEngine;

public class Camera_Movement : MonoBehaviour {

	// Public Variables
	public int dir = 1;							// Direction of Camera. 1 = P1, -1 = P2

	public float panSpeed = 20f;				// Camera pan speed
	public float panborderThickness = 10f;		// Border Thickness for panning camera with mouse
	
	public Vector4 panLimit;					// Clamps for camera pan. X = -pos.x, Y = pos.x, Z = -pos.z, W = pos.z

	public float scrollSpeed = 5f;				// Scroll speed for zoom
	public float minY = 10f;					// Min zoom of camera
	public float maxY = 16f;					// Max zoom of camera

	public float rotateSpeed = 75f;

	public bool toggle = false;
	public bool p1 = true;

	// Private Variables
	private float progress = 0f;
	private float ang;

	private bool vert = true;
	private bool rotating = false;

	void Update () {
		if (!toggle) {
			// Current possition of the Camera
			Vector3 pos = Vector3.zero;
				
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
					vert = !vert;
					ang = transform.localEulerAngles.y + 90f;
					rotating = true;
				} 
			}

			// Check Scroll for zoom
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

			
			// Clamp camera movement
			pos.y = Mathf.Clamp(pos.y, minY, maxY);

			if (vert) {
				if (transform.position.z + pos.z > panLimit.w || transform.position.z + pos.z < panLimit.z) {
					pos.z = 0f;
				}
				if (transform.position.x + pos.x > panLimit.y || transform.position.x + pos.x < panLimit.x) {
					pos.x = 0f;
				}
			} else {
				if (transform.position.z + pos.z > panLimit.w || transform.position.z + pos.z < panLimit.z) {
					pos.x = 0f;
				}
				if (transform.position.x + pos.x > panLimit.y || transform.position.x + pos.x < panLimit.x) {
					pos.z = 0f;
				}
			}

			// Transform camera
			transform.Translate(pos.x, 0, pos.z);

			// Rotate camera
			if (rotating) {
				float smoothTime = Mathf.Sin(progress) * rotateSpeed * Time.deltaTime;
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, ang, smoothTime);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				if (transform.localEulerAngles.y > ang - 0.1f && transform.localEulerAngles.y < ang + 0.1f) {
						progress = 0f;
						rotating = false;
						Debug.Log(ang);
				}
			}
		} else {
			if (p1) {
				float smoothTime = Mathf.Sin(progress) * rotateSpeed * Time.deltaTime;
				float newPosX = Mathf.Lerp(transform.position.x, 13f, smoothTime);
				float newPosY = Mathf.Lerp(transform.position.y, 13f, smoothTime);
				float newPosZ = Mathf.Lerp(transform.position.z, 53f, smoothTime);
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, 180, smoothTime);
				transform.position = new Vector3(newPosX, newPosY, newPosZ);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				progress += 0.01f * Time.deltaTime;
				if (Vector3.Distance(transform.position, new Vector3(13f, 13f, 53f)) <= 0.1f) {
					dir = -1;
					p1 = false;
					toggle = false;
					progress = 0f;
				}
			} else {
				float smoothTime = Mathf.Sin(progress) * rotateSpeed * Time.deltaTime;
				float newPosX = Mathf.Lerp(transform.position.x, 13f, smoothTime);
				float newPosY = Mathf.Lerp(transform.position.y, 13f, smoothTime);
				float newPosZ = Mathf.Lerp(transform.position.z, 3f, smoothTime);
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, 0, smoothTime);
				transform.position = new Vector3(newPosX, newPosY, newPosZ);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				progress += 0.01f * Time.deltaTime;
				if (Vector3.Distance(transform.position, new Vector3(13f, 13f, 3f)) <= 0.1f) {
					dir = 1;
					p1 = true;
					toggle = false;
					progress = 0f;
				}
			}
		}
	}
}
