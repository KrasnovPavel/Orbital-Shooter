using UnityEngine;
using System.Collections;

public class Gravitation : MonoBehaviour {
	public float mu = 1f;

	// Update is called once per frame
	void FixedUpdate () {
		Collider[] objects = Physics.OverlapSphere(transform.position, 30);
		for (int i = 0; i < objects.Length; i++) {
			if (objects[i].name == "Sphere") {
				Rigidbody objRb = objects[i].gameObject.GetComponent<Rigidbody>();
				objRb.AddForce(-mu*objRb.position.normalized / 
							(objRb.position.magnitude*objRb.position.magnitude));
			}
		}
	}

	//public Vector3 GravityInPoint(Vector3 point) {

	//}
}
