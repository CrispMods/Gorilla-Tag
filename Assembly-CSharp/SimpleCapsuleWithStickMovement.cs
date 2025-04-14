using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class SimpleCapsuleWithStickMovement : MonoBehaviour
{
	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06001161 RID: 4449 RVA: 0x000531BC File Offset: 0x000513BC
	// (remove) Token: 0x06001162 RID: 4450 RVA: 0x000531F4 File Offset: 0x000513F4
	public event Action CameraUpdated;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x06001163 RID: 4451 RVA: 0x0005322C File Offset: 0x0005142C
	// (remove) Token: 0x06001164 RID: 4452 RVA: 0x00053264 File Offset: 0x00051464
	public event Action PreCharacterMove;

	// Token: 0x06001165 RID: 4453 RVA: 0x00053299 File Offset: 0x00051499
	private void Awake()
	{
		this._rigidbody = base.GetComponent<Rigidbody>();
		if (this.CameraRig == null)
		{
			this.CameraRig = base.GetComponentInChildren<OVRCameraRig>();
		}
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x000532C4 File Offset: 0x000514C4
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

	// Token: 0x06001168 RID: 4456 RVA: 0x00053324 File Offset: 0x00051524
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

	// Token: 0x06001169 RID: 4457 RVA: 0x00053390 File Offset: 0x00051590
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

	// Token: 0x0600116A RID: 4458 RVA: 0x00053454 File Offset: 0x00051654
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

	// Token: 0x0400134F RID: 4943
	public bool EnableLinearMovement = true;

	// Token: 0x04001350 RID: 4944
	public bool EnableRotation = true;

	// Token: 0x04001351 RID: 4945
	public bool HMDRotatesPlayer = true;

	// Token: 0x04001352 RID: 4946
	public bool RotationEitherThumbstick;

	// Token: 0x04001353 RID: 4947
	public float RotationAngle = 45f;

	// Token: 0x04001354 RID: 4948
	public float Speed;

	// Token: 0x04001355 RID: 4949
	public OVRCameraRig CameraRig;

	// Token: 0x04001356 RID: 4950
	private bool ReadyToSnapTurn;

	// Token: 0x04001357 RID: 4951
	private Rigidbody _rigidbody;
}
