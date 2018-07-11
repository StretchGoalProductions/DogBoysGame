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

	void Update () {
		if (!toggle) {
			// Current possition of the Camera
			Vector3 pos = transform.position;

			// Get new pos based on input
			if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panborderThickness) {
				pos.z += panSpeed * dir * Time.deltaTime;
			}
			if (Input.GetKey("s") || Input.mousePosition.y <= panborderThickness) {
				pos.z -= panSpeed * dir * Time.deltaTime;
			}
			if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panborderThickness) {
				pos.x += panSpeed * dir * Time.deltaTime;
			}
			if (Input.GetKey("a") || Input.mousePosition.x <= panborderThickness) {
				pos.x -= panSpeed * dir * Time.deltaTime;
			}

			// Check Scroll for zoom
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

			// Clamp camera movement
			pos.y = Mathf.Clamp(pos.y, minY, maxY);

			if (p1)
				pos.z = Mathf.Clamp(pos.z, panLimit.z, panLimit.w);
			else
				pos.z = Mathf.Clamp(pos.z, panLimit.z + 23f, panLimit.w + 23f);
			pos.x = Mathf.Clamp(pos.x, panLimit.x, panLimit.y);

			// Transform camera
			transform.position = pos;
		} else {
			if (p1) {
				float smoothTime = Mathf.Sin(progress) * rotateSpeed * Time.deltaTime;
				float newPosX = Mathf.Lerp(transform.position.x, 13f, smoothTime);
				float newPosY = Mathf.Lerp(transform.position.y, 13f, smoothTime);
				float newPosZ = Mathf.Lerp(transform.position.z, 40f, smoothTime);
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, 180, smoothTime);
				transform.position = new Vector3(newPosX, newPosY, newPosZ);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				progress += 0.01f * Time.deltaTime;
				if (Vector3.Distance(transform.position, new Vector3(13f, 13f, 40f)) <= 0.1f) {
					dir = -1;
					p1 = false;
					toggle = false;
					progress = 0f;
				}
			} else {
				float smoothTime = Mathf.Sin(progress) * rotateSpeed * Time.deltaTime;
				float newPosX = Mathf.Lerp(transform.position.x, 13f, smoothTime);
				float newPosY = Mathf.Lerp(transform.position.y, 13f, smoothTime);
				float newPosZ = Mathf.Lerp(transform.position.z, -10f, smoothTime);
				float newRotY = Mathf.Lerp(transform.eulerAngles.y, 0, smoothTime);
				transform.position = new Vector3(newPosX, newPosY, newPosZ);
				transform.localEulerAngles = new Vector3(transform.eulerAngles.x, newRotY, transform.eulerAngles.z);
				progress += 0.01f * Time.deltaTime;
				if (Vector3.Distance(transform.position, new Vector3(13f, 13f, -10f)) <= 0.1f) {
					dir = 1;
					p1 = true;
					toggle = false;
					progress = 0f;
				}
			}
		}
	}
}
