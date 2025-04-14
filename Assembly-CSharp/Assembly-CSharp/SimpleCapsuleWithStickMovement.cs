using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class SimpleCapsuleWithStickMovement : MonoBehaviour
{
	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06001164 RID: 4452 RVA: 0x00053540 File Offset: 0x00051740
	// (remove) Token: 0x06001165 RID: 4453 RVA: 0x00053578 File Offset: 0x00051778
	public event Action CameraUpdated;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x06001166 RID: 4454 RVA: 0x000535B0 File Offset: 0x000517B0
	// (remove) Token: 0x06001167 RID: 4455 RVA: 0x000535E8 File Offset: 0x000517E8
	public event Action PreCharacterMove;

	// Token: 0x06001168 RID: 4456 RVA: 0x0005361D File Offset: 0x0005181D
	private void Awake()
	{
		this._rigidbody = base.GetComponent<Rigidbody>();
		if (this.CameraRig == null)
		{
			this.CameraRig = base.GetComponentInChildren<OVRCameraRig>();
		}
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00053648 File Offset: 0x00051848
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

	// Token: 0x0600116B RID: 4459 RVA: 0x000536A8 File Offset: 0x000518A8
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

	// Token: 0x0600116C RID: 4460 RVA: 0x00053714 File Offset: 0x00051914
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

	// Token: 0x0600116D RID: 4461 RVA: 0x000537D8 File Offset: 0x000519D8
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

	// Token: 0x04001350 RID: 4944
	public bool EnableLinearMovement = true;

	// Token: 0x04001351 RID: 4945
	public bool EnableRotation = true;

	// Token: 0x04001352 RID: 4946
	public bool HMDRotatesPlayer = true;

	// Token: 0x04001353 RID: 4947
	public bool RotationEitherThumbstick;

	// Token: 0x04001354 RID: 4948
	public float RotationAngle = 45f;

	// Token: 0x04001355 RID: 4949
	public float Speed;

	// Token: 0x04001356 RID: 4950
	public OVRCameraRig CameraRig;

	// Token: 0x04001357 RID: 4951
	private bool ReadyToSnapTurn;

	// Token: 0x04001358 RID: 4952
	private Rigidbody _rigidbody;
}
