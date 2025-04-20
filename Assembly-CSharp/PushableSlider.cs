using System;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class PushableSlider : MonoBehaviour
{
	// Token: 0x06001AC8 RID: 6856 RVA: 0x00042231 File Offset: 0x00040431
	public void Awake()
	{
		this.localSpace = base.transform.worldToLocalMatrix;
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000D7DB4 File Offset: 0x000D5FB4
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

	// Token: 0x06001ACA RID: 6858 RVA: 0x000D7F0C File Offset: 0x000D610C
	public void SetProgress(float value)
	{
		value = Mathf.Clamp(value, 0f, 1f);
		Vector3 vector = this.startingPos;
		vector.x += Mathf.Lerp(this.minXOffset, this.maxXOffset, value);
		base.transform.localPosition = vector;
		this._previousLocalPosition = vector - this.startingPos;
		this._cachedProgress = value;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000D7F74 File Offset: 0x000D6174
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

	// Token: 0x04001D8F RID: 7567
	[SerializeField]
	private float farPushDist = 0.015f;

	// Token: 0x04001D90 RID: 7568
	[SerializeField]
	private float maxXOffset;

	// Token: 0x04001D91 RID: 7569
	[SerializeField]
	private float minXOffset;

	// Token: 0x04001D92 RID: 7570
	private Matrix4x4 localSpace;

	// Token: 0x04001D93 RID: 7571
	private Vector3 startingPos;

	// Token: 0x04001D94 RID: 7572
	private Vector3 _previousLocalPosition;

	// Token: 0x04001D95 RID: 7573
	private float _cachedProgress;
}
