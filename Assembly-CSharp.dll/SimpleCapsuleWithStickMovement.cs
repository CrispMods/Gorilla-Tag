using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class SimpleCapsuleWithStickMovement : MonoBehaviour
{
	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06001164 RID: 4452 RVA: 0x000ABAD0 File Offset: 0x000A9CD0
	// (remove) Token: 0x06001165 RID: 4453 RVA: 0x000ABB08 File Offset: 0x000A9D08
	public event Action CameraUpdated;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x06001166 RID: 4454 RVA: 0x000ABB40 File Offset: 0x000A9D40
	// (remove) Token: 0x06001167 RID: 4455 RVA: 0x000ABB78 File Offset: 0x000A9D78
	public event Action PreCharacterMove;

	// Token: 0x06001168 RID: 4456 RVA: 0x0003AD2A File Offset: 0x00038F2A
	private void Awake()
	{
		this._rigidbody = base.GetComponent<Rigidbody>();
		if (this.CameraRig == null)
		{
			this.CameraRig = base.GetComponentInChildren<OVRCameraRig>();
		}
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Start()
	{
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x000ABBB0 File Offset: 0x000A9DB0
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

	// Token: 0x0600116B RID: 4459 RVA: 0x000ABC10 File Offset: 0x000A9E10
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

	// Token: 0x0600116C RID: 4460 RVA: 0x000ABC7C File Offset: 0x000A9E7C
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

	// Token: 0x0600116D RID: 4461 RVA: 0x000ABD40 File Offset: 0x000A9F40
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
