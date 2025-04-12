using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class SpringyWobbler : MonoBehaviour
{
	// Token: 0x06000976 RID: 2422 RVA: 0x0008FF60 File Offset: 0x0008E160
	private void Start()
	{
		int num = 1;
		Transform transform = base.transform;
		while (transform.childCount > 0)
		{
			transform = transform.GetChild(0);
			num++;
		}
		this.children = new Transform[num];
		transform = base.transform;
		this.children[0] = transform;
		int num2 = 1;
		while (transform.childCount > 0)
		{
			transform = transform.GetChild(0);
			this.children[num2] = transform;
			num2++;
		}
		this.lastEndpointWorldPos = this.children[this.children.Length - 1].transform.position;
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0008FFEC File Offset: 0x0008E1EC
	private void Update()
	{
		float x = base.transform.lossyScale.x;
		Vector3 vector = base.transform.TransformPoint(this.idealEndpointLocalPos);
		this.endpointVelocity += (vector - this.lastEndpointWorldPos) * this.stabilizingForce * x * Time.deltaTime;
		Vector3 vector2 = this.lastEndpointWorldPos + this.endpointVelocity * Time.deltaTime;
		float num = this.maxDisplacement * x;
		if ((vector2 - vector).IsLongerThan(num))
		{
			vector2 = vector + (vector2 - vector).normalized * num;
		}
		this.endpointVelocity = (vector2 - this.lastEndpointWorldPos) * (1f - this.drag) / Time.deltaTime;
		Vector3 a = base.transform.TransformPoint(this.rotateToFaceLocalPos);
		Vector3 upwards = base.transform.TransformDirection(Vector3.up);
		Vector3 position = base.transform.position;
		Vector3 ctrl = position + base.transform.TransformDirection(this.idealEndpointLocalPos) * this.startStiffness * x;
		Vector3 vector3 = vector2;
		Vector3 ctrl2 = vector3 + (a - vector3).normalized * this.endStiffness * x;
		for (int i = 1; i < this.children.Length; i++)
		{
			float num2 = (float)i / (float)(this.children.Length - 1);
			Vector3 vector4 = BezierUtils.BezierSolve(num2, position, ctrl, ctrl2, vector3);
			Vector3 a2 = BezierUtils.BezierSolve(num2 + 0.1f, position, ctrl, ctrl2, vector3);
			this.children[i].transform.position = vector4;
			this.children[i].transform.rotation = Quaternion.LookRotation(a2 - vector4, upwards);
		}
		this.lastIdealEndpointWorldPos = vector;
		this.lastEndpointWorldPos = vector2;
	}

	// Token: 0x04000B67 RID: 2919
	[SerializeField]
	private float stabilizingForce;

	// Token: 0x04000B68 RID: 2920
	[SerializeField]
	private float drag;

	// Token: 0x04000B69 RID: 2921
	[SerializeField]
	private float maxDisplacement;

	// Token: 0x04000B6A RID: 2922
	private Transform[] children;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private Vector3 idealEndpointLocalPos;

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private Vector3 rotateToFaceLocalPos;

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	private float startStiffness;

	// Token: 0x04000B6E RID: 2926
	[SerializeField]
	private float endStiffness;

	// Token: 0x04000B6F RID: 2927
	private Vector3 lastIdealEndpointWorldPos;

	// Token: 0x04000B70 RID: 2928
	private Vector3 lastEndpointWorldPos;

	// Token: 0x04000B71 RID: 2929
	private Vector3 endpointVelocity;
}
