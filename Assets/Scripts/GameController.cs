using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public float mu;
	GameObject ellipse;

	// Use this for initialization
	void Start () {
		GameObject.Find("Sun").GetComponent<Gravitation>().mu = mu;
		ellipse = GameObject.Find("Ellipse");
		ellipse.GetComponent<DrawEllipse>().mu = mu;
		GameObject.Find("Sphere").GetComponent<Rigidbody>().velocity = 
									new Vector3(0f, 0f, -0.3f);
	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1"))
			ellipse.GetComponent<DrawEllipse>().CalculateEllipse();
	}
}
