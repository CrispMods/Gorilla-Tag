using System;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class PushableSlider : MonoBehaviour
{
	// Token: 0x06001A77 RID: 6775 RVA: 0x00040EF8 File Offset: 0x0003F0F8
	public void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000D50FC File Offset: 0x000D32FC
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

	// Token: 0x06001A79 RID: 6777 RVA: 0x000D5254 File Offset: 0x000D3454
	public void SetProgress(float value)
	{
		value = Mathf.Clamp(value, 0f, 1f);
		Vector3 vector = this.startingPos;
		vector.x += Mathf.Lerp(this.minXOffset, this.maxXOffset, value);
		base.transform.localPosition = vector;
		this._previousLocalPosition = vector - this.startingPos;
		this._cachedProgress = value;
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x000D52BC File Offset: 0x000D34BC
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

	// Token: 0x04001D41 RID: 7489
	[SerializeField]
	private float farPushDist = 0.015f;

	// Token: 0x04001D42 RID: 7490
	[SerializeField]
	private float maxXOffset;

	// Token: 0x04001D43 RID: 7491
	[SerializeField]
	private float minXOffset;

	// Token: 0x04001D44 RID: 7492
	private Matrix4x4 localSpace;

	// Token: 0x04001D45 RID: 7493
	private Vector3 startingPos;

	// Token: 0x04001D46 RID: 7494
	private Vector3 _previousLocalPosition;

	// Token: 0x04001D47 RID: 7495
	private float _cachedProgress;
}
