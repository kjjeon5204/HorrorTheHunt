using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	public float speed = 6.0f;
	public float bulletSpeed = 1000.0f;
	public Rigidbody bulletPrefab;
	
	private float shootTime = 0.0f;
	private Vector3 moveDirection = Vector3.zero;
	private bool grounded = false;
	private Transform bulletSpawn;
	
	void Start(){
		bulletSpawn = transform.Find ("muzzle");   
	}
	
	void FixedUpdate() 
	{	
		
		CharacterController controller = (CharacterController)GetComponent(typeof(CharacterController));
		CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
		grounded = (flags & CollisionFlags.CollidedBelow) != 0;
		
		if (Input.GetButton ("Fire1"))
		{
			if (Time.time >= shootTime)
			{ 
				Rigidbody bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
				bullet.rigidbody.AddForce(transform.forward * bulletSpeed);
				shootTime = Time.time + shotInterval;
			}
		}
	}
}