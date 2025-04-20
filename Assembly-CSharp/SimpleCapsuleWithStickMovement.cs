using System;
using UnityEngine;

// Token: 0x020002D9 RID: 729
public class SimpleCapsuleWithStickMovement : MonoBehaviour
{
	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060011AD RID: 4525 RVA: 0x000AE368 File Offset: 0x000AC568
	// (remove) Token: 0x060011AE RID: 4526 RVA: 0x000AE3A0 File Offset: 0x000AC5A0
	public event Action CameraUpdated;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x060011AF RID: 4527 RVA: 0x000AE3D8 File Offset: 0x000AC5D8
	// (remove) Token: 0x060011B0 RID: 4528 RVA: 0x000AE410 File Offset: 0x000AC610
	public event Action PreCharacterMove;

	// Token: 0x060011B1 RID: 4529 RVA: 0x0003BFEA File Offset: 0x0003A1EA
	private void Awake()
	{
		this._rigidbody = base.GetComponent<Rigidbody>();
		if (this.CameraRig == null)
		{
			this.CameraRig = base.GetComponentInChildren<OVRCameraRig>();
		}
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x000AE448 File Offset: 0x000AC648
	private void FixedUpdate()
	{
		if (this.CameraUpdated != null)
		{
			this.CameraUpdated();
		}
		if (this.PreCharacterMove != null)
		{
			this.PreCharacterMove();
		}
		if (this.HMDRotatesPlayer)
		{
			this.RotatePlayerToHMD();
		}
		if (this.EnableLinearMovement)
		{
			this.StickMovement();
		}
		if (this.EnableRotation)
		{
			this.SnapTurn();
		}
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x000AE4A8 File Offset: 0x000AC6A8
	private void RotatePlayerToHMD()
	{
		Transform trackingSpace = this.CameraRig.trackingSpace;
		Transform centerEyeAnchor = this.CameraRig.centerEyeAnchor;
		Vector3 position = trackingSpace.position;
		Quaternion rotation = trackingSpace.rotation;
		base.transform.rotation = Quaternion.Euler(0f, centerEyeAnchor.rotation.eulerAngles.y, 0f);
		trackingSpace.position = position;
		trackingSpace.rotation = rotation;
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x000AE514 File Offset: 0x000AC714
	private void StickMovement()
	{
		Vector3 eulerAngles = this.CameraRig.centerEyeAnchor.rotation.eulerAngles;
		eulerAngles.z = (eulerAngles.x = 0f);
		Quaternion rotation = Quaternion.Euler(eulerAngles);
		Vector3 a = Vector3.zero;
		Vector2 vector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active);
		a += rotation * (vector.x * Vector3.right);
		a += rotation * (vector.y * Vector3.forward);
		this._rigidbody.MovePosition(this._rigidbody.position + a * this.Speed * Time.fixedDeltaTime);
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x000AE5D8 File Offset: 0x000AC7D8
	private void SnapTurn()
	{
		if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft, OVRInput.Controller.Active) || (this.RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.Active)))
		{
			if (this.ReadyToSnapTurn)
			{
				this.ReadyToSnapTurn = false;
				base.transform.RotateAround(this.CameraRig.centerEyeAnchor.position, Vector3.up, -this.RotationAngle);
				return;
			}
		}
		else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight, OVRInput.Controller.Active) || (this.RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.Active)))
		{
			if (this.ReadyToSnapTurn)
			{
				this.ReadyToSnapTurn = false;
				base.transform.RotateAround(this.CameraRig.centerEyeAnchor.position, Vector3.up, this.RotationAngle);
				return;
			}
		}
		else
		{
			this.ReadyToSnapTurn = true;
		}
	}

	// Token: 0x04001397 RID: 5015
	public bool EnableLinearMovement = true;

	// Token: 0x04001398 RID: 5016
	public bool EnableRotation = true;

	// Token: 0x04001399 RID: 5017
	public bool HMDRotatesPlayer = true;

	// Token: 0x0400139A RID: 5018
	public bool RotationEitherThumbstick;

	// Token: 0x0400139B RID: 5019
	public float RotationAngle = 45f;

	// Token: 0x0400139C RID: 5020
	public float Speed;

	// Token: 0x0400139D RID: 5021
	public OVRCameraRig CameraRig;

	// Token: 0x0400139E RID: 5022
	private bool ReadyToSnapTurn;

	// Token: 0x0400139F RID: 5023
	private Rigidbody _rigidbody;
}
