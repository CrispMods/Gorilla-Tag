using System;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class PushableSlider : MonoBehaviour
{
	// Token: 0x06001A74 RID: 6772 RVA: 0x00082B0C File Offset: 0x00080D0C
	public void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x00082B30 File Offset: 0x00080D30
	private void OnTriggerStay(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		GorillaTriggerColliderHandIndicator componentInParent = other.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
		if (componentInParent == null)
		{
			return;
		}
		Vector3 b = this.localSpace.MultiplyPoint3x4(other.transform.position);
		Vector3 vector = base.transform.localPosition - this.startingPos - b;
		float num = Mathf.Abs(vector.x);
		if (num < this.farPushDist)
		{
			Vector3 currentVelocity = componentInParent.currentVelocity;
			if (Mathf.Sign(vector.x) != Mathf.Sign((this.localSpace.rotation * currentVelocity).x))
			{
				return;
			}
			vector.x = Mathf.Sign(vector.x) * (this.farPushDist - num);
			vector.y = 0f;
			vector.z = 0f;
			Vector3 vector2 = base.transform.localPosition - this.startingPos + vector;
			vector2.x = Mathf.Clamp(vector2.x, this.minXOffset, this.maxXOffset);
			base.transform.localPosition = vector2 + this.startingPos;
			GorillaTagger.Instance.StartVibration(componentInParent.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x00082C88 File Offset: 0x00080E88
	public void SetProgress(float value)
	{
		value = Mathf.Clamp(value, 0f, 1f);
		Vector3 vector = this.startingPos;
		vector.x += Mathf.Lerp(this.minXOffset, this.maxXOffset, value);
		base.transform.localPosition = vector;
		this._previousLocalPosition = vector - this.startingPos;
		this._cachedProgress = value;
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x00082CF0 File Offset: 0x00080EF0
	public float GetProgress()
	{
		Vector3 vector = base.transform.localPosition - this.startingPos;
		if (vector == this._previousLocalPosition)
		{
			return this._cachedProgress;
		}
		this._previousLocalPosition = vector;
		this._cachedProgress = (vector.x - this.minXOffset) / (this.maxXOffset - this.minXOffset);
		return this._cachedProgress;
	}

	// Token: 0x04001D40 RID: 7488
	[SerializeField]
	private float farPushDist = 0.015f;

	// Token: 0x04001D41 RID: 7489
	[SerializeField]
	private float maxXOffset;

	// Token: 0x04001D42 RID: 7490
	[SerializeField]
	private float minXOffset;

	// Token: 0x04001D43 RID: 7491
	private Matrix4x4 localSpace;

	// Token: 0x04001D44 RID: 7492
	private Vector3 startingPos;

	// Token: 0x04001D45 RID: 7493
	private Vector3 _previousLocalPosition;

	// Token: 0x04001D46 RID: 7494
	private float _cachedProgress;
}
