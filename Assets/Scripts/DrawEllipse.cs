using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
public class DrawEllipse : MonoBehaviour {
	public int resolution = 1000;
	public float mu;

	LineRenderer lineRenderer;
	Rigidbody rigidBody;

	void Start () {
		rigidBody = GameObject.Find("MainBody").GetComponent<Rigidbody>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount (resolution+1);
	}

	void Update() {
		CalculateEllipse();
	}

	public void CalculateEllipse() {
		Vector3 Rv = rigidBody.position;
		Vector3 Vv = rigidBody.velocity;
		Vector3 Hv = Vector3.Cross(Rv, Vv);
		Vector3 Nv = Vector3.Cross(Vector3.up, Hv);
		float V2 = Vv.magnitude*Vv.magnitude;
		Vector3 ev = ((V2 - 1/Rv.magnitude)*Rv - Vector3.Dot(Rv, Vv)*Vv)/mu;
		float e = ev.magnitude;
		float a;
		if (!Mathf.Approximately(e, 1f)) {
				a = -1/(V2-2*mu/Rv.magnitude);
			} else {
				a = Mathf.Infinity;
			}
		float i = Mathf.Acos(Hv.y / Hv.magnitude)  * Mathf.Rad2Deg;
		float theta = Mathf.Acos(Nv.z / Nv.magnitude)  * Mathf.Rad2Deg;
		if (Mathf.Approximately (i, 0f)) {
			theta = 0f;
		}
		if (Nv.x < 0f) {
			theta = 360f - theta;
		}
		float w =180f + Mathf.Acos(Vector3.Dot(Nv, ev) / (Nv.magnitude * e)) * Mathf.Rad2Deg;
		if (ev.y < 0f){
			w = 360f - w;
		}
		NewEllipse(a, e, i, theta, w);
	}

	void NewEllipse (float a, float e, float i, float theta, float w) {
		Vector3 center = new Vector3 (0f, 0f, a * e);
		Quaternion roti = Quaternion.Euler(new Vector3 (0f, 0f, i));
		Quaternion rotw = Quaternion.Euler(new Vector3 (0f, w, 0f));
		Quaternion rottheta = Quaternion.Euler(new Vector3 (0f, theta, 0f));
		Quaternion rotation;
		rotation = rottheta;
		rotation *= roti;
		rotation *= rotw;

		center = rotation * center;

		float b = a * Mathf.Sqrt(1 - e*e);
		Vector3 position;
		for (int j = 0; j <= resolution; j++) {
			float angle = (float)j / (float)resolution * 2.0f * Mathf.PI;
			position = new Vector3(b * Mathf.Cos (angle), 0f , a * Mathf.Sin (angle));
			lineRenderer.SetPosition(j, rotation * position + center);
		}
	}

}