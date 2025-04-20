using System;
using UnityEngine;

// Token: 0x020002CB RID: 715
public class CharacterCameraConstraint : MonoBehaviour
{
	// Token: 0x06001142 RID: 4418 RVA: 0x00030758 File Offset: 0x0002E958
	private CharacterCameraConstraint()
	{
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x0003BCC0 File Offset: 0x00039EC0
	private void Awake()
	{
		this._character = base.GetComponent<CapsuleCollider>();
		this._simplePlayerController = base.GetComponent<SimpleCapsuleWithStickMovement>();
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x0003BCDA File Offset: 0x00039EDA
	private void OnEnable()
	{
		this._simplePlayerController.CameraUpdated += this.CameraUpdate;
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x0003BCF3 File Offset: 0x00039EF3
	private void OnDisable()
	{
		this._simplePlayerController.CameraUpdated -= this.CameraUpdate;
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x000AD2D0 File Offset: 0x000AB4D0
	private void CameraUpdate()
	{
		float value = 0f;
		if (this.CheckCameraOverlapped())
		{
			OVRScreenFade.instance.SetExplicitFade(1f);
		}
		else if (this.CheckCameraNearClipping(out value))
		{
			float t = Mathf.InverseLerp(0f, 0.1f, value);
			float explicitFade = Mathf.Lerp(0f, 1f, t);
			OVRScreenFade.instance.SetExplicitFade(explicitFade);
		}
		else
		{
			OVRScreenFade.instance.SetExplicitFade(0f);
		}
		float num = 0.25f;
		float value2 = this.CameraRig.centerEyeAnchor.localPosition.y + this.HeightOffset + num;
		float num2 = this.MinimumHeight;
		num2 = Mathf.Min(this._character.height, num2);
		float num3 = this.MaximumHeight;
		RaycastHit raycastHit;
		if (Physics.SphereCast(this._character.transform.position, this._character.radius * 0.2f, Vector3.up, out raycastHit, this.MaximumHeight - this._character.transform.position.y, this.CollideLayers, QueryTriggerInteraction.Ignore))
		{
			num3 = raycastHit.point.y;
		}
		num3 = Mathf.Max(this._character.height, num3);
		this._character.height = Mathf.Clamp(value2, num2, num3);
		float y = this.HeightOffset - this._character.height * 0.5f - num;
		this.CameraRig.transform.localPosition = new Vector3(0f, y, 0f);
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x000AD458 File Offset: 0x000AB658
	private bool CheckCameraOverlapped()
	{
		Camera component = this.CameraRig.centerEyeAnchor.GetComponent<Camera>();
		Vector3 position = this._character.transform.position;
		float num = Mathf.Max(0f, this._character.height * 0.5f - component.nearClipPlane - 0.01f);
		position.y = Mathf.Clamp(this.CameraRig.centerEyeAnchor.position.y, this._character.transform.position.y - num, this._character.transform.position.y + num);
		Vector3 a = this.CameraRig.centerEyeAnchor.position - position;
		float magnitude = a.magnitude;
		Vector3 direction = a / magnitude;
		RaycastHit raycastHit;
		return Physics.SphereCast(position, component.nearClipPlane, direction, out raycastHit, magnitude, this.CollideLayers, QueryTriggerInteraction.Ignore);
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x000AD548 File Offset: 0x000AB748
	private bool CheckCameraNearClipping(out float result)
	{
		Camera component = this.CameraRig.centerEyeAnchor.GetComponent<Camera>();
		Vector3[] array = new Vector3[4];
		component.CalculateFrustumCorners(new Rect(0f, 0f, 1f, 1f), component.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, array);
		Vector3 vector = this.CameraRig.centerEyeAnchor.position + Vector3.Normalize(this.CameraRig.centerEyeAnchor.TransformVector(array[0])) * 0.25f;
		Vector3 vector2 = this.CameraRig.centerEyeAnchor.position + Vector3.Normalize(this.CameraRig.centerEyeAnchor.TransformVector(array[1])) * 0.25f;
		Vector3 vector3 = this.CameraRig.centerEyeAnchor.position + Vector3.Normalize(this.CameraRig.centerEyeAnchor.TransformVector(array[2])) * 0.25f;
		Vector3 vector4 = this.CameraRig.centerEyeAnchor.position + Vector3.Normalize(this.CameraRig.centerEyeAnchor.TransformVector(array[3])) * 0.25f;
		Vector3 vector5 = (vector2 + vector4) / 2f;
		bool result2 = false;
		result = 0f;
		foreach (Vector3 vector6 in new Vector3[]
		{
			vector,
			vector2,
			vector3,
			vector4,
			vector5
		})
		{
			RaycastHit raycastHit;
			if (Physics.Linecast(this.CameraRig.centerEyeAnchor.position, vector6, out raycastHit, this.CollideLayers, QueryTriggerInteraction.Ignore))
			{
				result2 = true;
				result = Mathf.Max(result, Vector3.Distance(raycastHit.point, vector6));
			}
		}
		return result2;
	}

	// Token: 0x0400133D RID: 4925
	private const float FADE_RAY_LENGTH = 0.25f;

	// Token: 0x0400133E RID: 4926
	private const float FADE_OVERLAP_MAXIMUM = 0.1f;

	// Token: 0x0400133F RID: 4927
	private const float FADE_AMOUNT_MAXIMUM = 1f;

	// Token: 0x04001340 RID: 4928
	[Tooltip("This should be a reference to the OVRCameraRig that is usually a child of the PlayerController.")]
	public OVRCameraRig CameraRig;

	// Token: 0x04001341 RID: 4929
	[Tooltip("Collision layers to be used for the purposes of fading out the screen when the HMD is inside world geometry and adjusting the capsule height.")]
	public LayerMask CollideLayers;

	// Token: 0x04001342 RID: 4930
	[Tooltip("Offset is added to camera's real world height, effectively treating it as though the player was taller/standing higher.")]
	public float HeightOffset;

	// Token: 0x04001343 RID: 4931
	[Tooltip("Minimum height that the character capsule can shrink to.  To disable, set to capsule's height.")]
	public float MinimumHeight;

	// Token: 0x04001344 RID: 4932
	[Tooltip("Maximum height that the character capsule can grow to.  To disable, set to capsule's height.")]
	public float MaximumHeight;

	// Token: 0x04001345 RID: 4933
	private CapsuleCollider _character;

	// Token: 0x04001346 RID: 4934
	private SimpleCapsuleWithStickMovement _simplePlayerController;
}
