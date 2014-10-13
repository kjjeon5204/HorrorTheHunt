using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
	public GameObject player;
	public GameObject mainCamera;
	public float cameraX;
	public float cameraY;
	public float cameraZ;
	public float cameraDistance;

	protected Vector3 localTransform;
	protected Vector3 inputRotation;
	protected Vector3 inputMovement;
	private Vector3 tempvector1;
	private Vector3 tempvector2;

	public float movespeed;

	public float bulletSpeed = 1000.0f;
	public Rigidbody bulletPrefab;
	private float shootTime;
	public float shootInterval = .5f;
	public Transform bulletSpawn;
	public GameObject barrel;
	public GameObject muzzleFlash;
			
	// Use this for initialization
	void Start () {
		player = (GameObject) GameObject.FindWithTag ("Player");
		mainCamera = (GameObject) GameObject.FindWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		GetInput();
		ProcessMovement();
		HandleCamera();
		FireCheck();
		Animate();
	}

//----------------------------------------Functions---------------------------------------------

	//Sets up the mouse position on screen and rotates body accordingly
	void GetInput() {
		//Find keyboard movement vector
		inputMovement = new Vector3 (Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
		localTransform = transform.InverseTransformDirection(inputMovement);
	}
	
	void LookAtMouse() {
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// Determine the point where the cursor ray intersects the plane.
		// This will be the point that the object must look towards to be looking at the mouse.

		float hitdist = 0.0f;
		// If the ray is parallel to the plane, Raycast will return false.
		if (playerPlane.Raycast (ray, out hitdist)) 
		{
			// Get the point along the ray that hits the calculated distance.
			Vector3 targetPoint = ray.GetPoint(hitdist);
			
			// Determine the target rotation.  This is the rotation if the transform looks at the target point.
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			
			// Smoothly rotate towards the target point.
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 360f * Time.deltaTime);
		}
	}
	
	//Transforms character based on the inputs
	void ProcessMovement() {
		rigidbody.AddForce (inputMovement.normalized * movespeed * Time.deltaTime);
		LookAtMouse();
		transform.position = new Vector3 (transform.position.x, 0 , transform.position.z);
	}

	//Moves camera to follow character
	void HandleCamera() {
		mainCamera.transform.position = new Vector3 (transform.position.x, cameraDistance, transform.position.z);
		mainCamera.transform.eulerAngles = new Vector3 (cameraX, cameraY, cameraZ);
	}

	//Checks to see if left mouse is pressed down. Will fire 1 bullet every shoot interval
	void FireCheck() {
		if (Input.GetButton ("Fire1")) {
		
			barrel.transform.Rotate(0, 0, 720f * Time.deltaTime);
			if (Time.time >= shootTime)
			{ 
				//muzzleFlash.SetActive (true);
				Rigidbody bullet = (Rigidbody) Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
				bullet.rigidbody.AddForce(transform.forward * bulletSpeed);
				Physics.IgnoreCollision(bullet.collider, transform.root.collider);    //ignore hero collision
				shootTime = Time.time + shootInterval;
			}
			//muzzleFlash.SetActive (false);
		}
	}
	
	void Animate() {

		if (localTransform.z > 0) {
			animation.CrossFade("moveloop");
		}
		else if (localTransform.z < 0) {
			animation.CrossFade ("movebackloop");
		}
		else {
			animation.CrossFade ("idle");
		}  
	}
	

	void Lean() {
		if (inputMovement.x > 0) {
			transform.Rotate(0, 0, 10f * Time.deltaTime);
		}
		else if (inputMovement.x < 0) {
			transform.Rotate(0, 0, -10f * Time.deltaTime);
		}
	}
}
