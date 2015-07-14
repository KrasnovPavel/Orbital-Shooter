using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
public class DrawEllipse : MonoBehaviour {
	public int resolution = 1000;
	public float mu;

	LineRenderer lineRenderer;
	Rigidbody rigidBody;

	void Start () {
		rigidBody = GameObject.Find("Sphere").GetComponent<Rigidbody>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount (resolution+1);
	}

	public void CalculateEllipse() {
		Vector3 Rv = rigidBody.position;
		Vector3 Vv = rigidBody.velocity;
		Vector3 Hv = Vector3.Cross(Rv, Vv);
		Vector3 Nv = Vector3.Cross(Vector3.forward, Hv);
		float V2 = Vv.magnitude*Vv.magnitude;
		Vector3 ev = ((V2 - 1/Rv.magnitude)*Rv - Vector3.Dot(Rv, Vv)*Vv)/mu;
		float e = ev.magnitude;
		float r = Rv.magnitude;
		float a;
		if (!Mathf.Approximately(e, 1f)) {
				a = -1/(V2-2*mu/Rv.magnitude);
			} else {
				a = Mathf.Infinity;
			}
		float i =270f - Mathf.Acos(Hv.z / Hv.magnitude)  * Mathf.Rad2Deg;
		float theta = 180f - Mathf.Acos(Nv.x / Nv.magnitude)  * Mathf.Rad2Deg;
		if (Nv.y < 0f) {
   			theta = 360f - theta;
   		}
		float w = Mathf.Acos(Vector3.Dot(Nv ,ev) / (Nv.magnitude * e)) * Mathf.Rad2Deg;
		if (ev.z < 0f){
			w = 360f - w;
		}

		NewEllipse(a, e, i, theta, w);
	}

	void NewEllipse (float a, float e, float i, float theta, float w) {
		Vector3 center = new Vector3 (a * e, 0f, 0f);
		Quaternion roti = Quaternion.Euler(new Vector3 (i, 0f, 0f));
		Quaternion rotw = Quaternion.Euler(new Vector3 (0f, 0f, w));
		Quaternion rottheta = Quaternion.Euler(new Vector3 (0f, 0f, theta));
		Quaternion rotation = new Quaternion();
		rotation = rotw;
		rotation *= roti;
		rotation *= rottheta;

		center = rotation * center;

		float b = a * Mathf.Sqrt(1 - e*e);
		Vector3 position;
		for (int j = 0; j <= resolution; j++) {
			float angle = (float)j / (float)resolution * 2.0f * Mathf.PI;
			position = new Vector3(a * Mathf.Cos (angle), 0f , b * Mathf.Sin (angle));
			lineRenderer.SetPosition(j, rotation * position + center);
		}
	}

}