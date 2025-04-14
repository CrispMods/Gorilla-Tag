using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003F3 RID: 1011
public class OwlLook : MonoBehaviour
{
	// Token: 0x060018C1 RID: 6337 RVA: 0x0007875F File Offset: 0x0007695F
	private void Awake()
	{
		this.overlapRigs = new VRRig[(int)PhotonNetworkController.Instance.GetRoomSize(null)];
		if (this.myRig == null)
		{
			this.myRig = base.GetComponentInParent<VRRig>();
		}
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x00078794 File Offset: 0x00076994
	private void LateUpdate()
	{
		if (NetworkSystem.Instance.InRoom)
		{
			if (this.rigs.Length != NetworkSystem.Instance.RoomPlayerCount)
			{
				this.rigs = VRRigCache.Instance.GetAllRigs();
			}
		}
		else if (this.rigs.Length != 1)
		{
			this.rigs = new VRRig[1];
			this.rigs[0] = VRRig.LocalRig;
		}
		float num = -1f;
		float num2 = Mathf.Cos(this.lookAtAngleDegrees / 180f * 3.1415927f);
		int num3 = 0;
		for (int i = 0; i < this.rigs.Length; i++)
		{
			if (!(this.rigs[i] == this.myRig))
			{
				Vector3 rhs = this.rigs[i].tagSound.transform.position - base.transform.position;
				if (rhs.magnitude <= this.lookRadius)
				{
					float num4 = Vector3.Dot(-base.transform.up, rhs.normalized);
					if (num4 > num2)
					{
						this.overlapRigs[num3++] = this.rigs[i];
					}
				}
			}
		}
		this.lookTarget = null;
		for (int j = 0; j < num3; j++)
		{
			Vector3 rhs = (this.overlapRigs[j].tagSound.transform.position - base.transform.position).normalized;
			float num4 = Vector3.Dot(base.transform.forward, rhs);
			if (num4 > num)
			{
				num = num4;
				this.lookTarget = this.overlapRigs[j].tagSound.transform;
			}
		}
		Vector3 vector = this.neck.forward;
		if (this.lookTarget != null)
		{
			vector = (this.lookTarget.position - this.head.position).normalized;
		}
		Vector3 vector2 = this.neck.InverseTransformDirection(vector);
		vector2.y = Mathf.Clamp(vector2.y, this.minNeckY, this.maxNeckY);
		vector = this.neck.TransformDirection(vector2.normalized);
		Vector3 forward = Vector3.RotateTowards(this.head.forward, vector, this.rotSpeed * 0.017453292f * Time.deltaTime, 0f);
		this.head.rotation = Quaternion.LookRotation(forward, this.neck.up);
	}

	// Token: 0x04001B7A RID: 7034
	public Transform head;

	// Token: 0x04001B7B RID: 7035
	public Transform lookTarget;

	// Token: 0x04001B7C RID: 7036
	public Transform neck;

	// Token: 0x04001B7D RID: 7037
	public float lookRadius = 0.5f;

	// Token: 0x04001B7E RID: 7038
	public Collider[] overlapColliders;

	// Token: 0x04001B7F RID: 7039
	public VRRig[] rigs = new VRRig[10];

	// Token: 0x04001B80 RID: 7040
	public VRRig[] overlapRigs;

	// Token: 0x04001B81 RID: 7041
	public float rotSpeed = 1f;

	// Token: 0x04001B82 RID: 7042
	public float lookAtAngleDegrees = 60f;

	// Token: 0x04001B83 RID: 7043
	public float maxNeckY;

	// Token: 0x04001B84 RID: 7044
	public float minNeckY;

	// Token: 0x04001B85 RID: 7045
	public VRRig myRig;
}
