using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class OwlLook : MonoBehaviour
{
	// Token: 0x0600190E RID: 6414 RVA: 0x00040F1A File Offset: 0x0003F11A
	private void Awake()
	{
		this.overlapRigs = new VRRig[(int)PhotonNetworkController.Instance.GetRoomSize(null)];
		if (this.myRig == null)
		{
			this.myRig = base.GetComponentInParent<VRRig>();
		}
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x000CE87C File Offset: 0x000CCA7C
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

	// Token: 0x04001BC3 RID: 7107
	public Transform head;

	// Token: 0x04001BC4 RID: 7108
	public Transform lookTarget;

	// Token: 0x04001BC5 RID: 7109
	public Transform neck;

	// Token: 0x04001BC6 RID: 7110
	public float lookRadius = 0.5f;

	// Token: 0x04001BC7 RID: 7111
	public Collider[] overlapColliders;

	// Token: 0x04001BC8 RID: 7112
	public VRRig[] rigs = new VRRig[10];

	// Token: 0x04001BC9 RID: 7113
	public VRRig[] overlapRigs;

	// Token: 0x04001BCA RID: 7114
	public float rotSpeed = 1f;

	// Token: 0x04001BCB RID: 7115
	public float lookAtAngleDegrees = 60f;

	// Token: 0x04001BCC RID: 7116
	public float maxNeckY;

	// Token: 0x04001BCD RID: 7117
	public float minNeckY;

	// Token: 0x04001BCE RID: 7118
	public VRRig myRig;
}
