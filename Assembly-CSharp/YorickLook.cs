using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200040F RID: 1039
public class YorickLook : MonoBehaviour
{
	// Token: 0x060019BD RID: 6589 RVA: 0x0007ED05 File Offset: 0x0007CF05
	private void Awake()
	{
		this.overlapRigs = new VRRig[(int)PhotonNetworkController.Instance.GetRoomSize(null)];
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x0007ED20 File Offset: 0x0007CF20
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
		Vector3 target = -base.transform.up;
		Vector3 target2 = -base.transform.up;
		if (this.lookTarget != null)
		{
			target = (this.lookTarget.position - this.leftEye.position).normalized;
			target2 = (this.lookTarget.position - this.rightEye.position).normalized;
		}
		Vector3 forward = Vector3.RotateTowards(this.leftEye.rotation * Vector3.forward, target, this.rotSpeed * 3.1415927f, 0f);
		Vector3 forward2 = Vector3.RotateTowards(this.rightEye.rotation * Vector3.forward, target2, this.rotSpeed * 3.1415927f, 0f);
		this.leftEye.rotation = Quaternion.LookRotation(forward);
		this.rightEye.rotation = Quaternion.LookRotation(forward2);
	}

	// Token: 0x04001CA7 RID: 7335
	public Transform leftEye;

	// Token: 0x04001CA8 RID: 7336
	public Transform rightEye;

	// Token: 0x04001CA9 RID: 7337
	public Transform lookTarget;

	// Token: 0x04001CAA RID: 7338
	public float lookRadius = 0.5f;

	// Token: 0x04001CAB RID: 7339
	public VRRig[] rigs = new VRRig[10];

	// Token: 0x04001CAC RID: 7340
	public VRRig[] overlapRigs;

	// Token: 0x04001CAD RID: 7341
	public float rotSpeed = 1f;

	// Token: 0x04001CAE RID: 7342
	public float lookAtAngleDegrees = 60f;
}
