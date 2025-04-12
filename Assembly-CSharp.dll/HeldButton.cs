using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020005BC RID: 1468
public class HeldButton : MonoBehaviour
{
	// Token: 0x06002474 RID: 9332 RVA: 0x00101060 File Offset: 0x000FF260
	private void OnTriggerEnter(Collider other)
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
		if ((componentInParent.isLeftHand && !this.leftHandPressable) || (!componentInParent.isLeftHand && !this.rightHandPressable))
		{
			return;
		}
		if (!this.pendingPress || other != this.pendingPressCollider)
		{
			this.touchTime = Time.time;
			this.pendingPressCollider = other;
			this.pressingHand = componentInParent;
			this.pendingPress = true;
			this.SetOn(true);
			GorillaTagger.Instance.StartVibration(componentInParent.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		}
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x00101110 File Offset: 0x000FF310
	private void LateUpdate()
	{
		if (!this.pendingPress)
		{
			return;
		}
		if (this.touchTime < this.releaseTime && this.releaseTime + this.debounceTime < Time.time)
		{
			this.pendingPress = false;
			this.pendingPressCollider = null;
			this.pressingHand = null;
			this.SetOn(false);
			return;
		}
		if (this.touchTime + this.pressDuration < Time.time)
		{
			this.onPressButton.Invoke();
			if (this.pressingHand != null)
			{
				GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, this.pressingHand.isLeftHand, 0.1f);
				GorillaTagger.Instance.StartVibration(this.pressingHand.isLeftHand, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);
			}
			this.pendingPress = false;
			this.pendingPressCollider = null;
			this.pressingHand = null;
			this.releaseTime = Time.time;
			this.SetOn(false);
			return;
		}
		if (this.touchTime > this.releaseTime && this.pressingHand != null)
		{
			GorillaTagger.Instance.StartVibration(this.pressingHand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 4f, Time.fixedDeltaTime);
		}
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x00047B5D File Offset: 0x00045D5D
	private void OnTriggerExit(Collider other)
	{
		if (this.pendingPress && this.pendingPressCollider == other)
		{
			this.releaseTime = Time.time;
		}
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x00101250 File Offset: 0x000FF450
	public void SetOn(bool inOn)
	{
		if (inOn == this.isOn)
		{
			return;
		}
		this.isOn = inOn;
		if (this.isOn)
		{
			this.buttonRenderer.material = this.pressedMaterial;
			if (this.myText != null)
			{
				this.myText.text = this.onText;
				return;
			}
		}
		else
		{
			this.buttonRenderer.material = this.unpressedMaterial;
			if (this.myText != null)
			{
				this.myText.text = this.offText;
			}
		}
	}

	// Token: 0x04002892 RID: 10386
	public Material pressedMaterial;

	// Token: 0x04002893 RID: 10387
	public Material unpressedMaterial;

	// Token: 0x04002894 RID: 10388
	public MeshRenderer buttonRenderer;

	// Token: 0x04002895 RID: 10389
	private bool isOn;

	// Token: 0x04002896 RID: 10390
	public float debounceTime = 0.25f;

	// Token: 0x04002897 RID: 10391
	public bool leftHandPressable;

	// Token: 0x04002898 RID: 10392
	public bool rightHandPressable = true;

	// Token: 0x04002899 RID: 10393
	public float pressDuration = 0.5f;

	// Token: 0x0400289A RID: 10394
	public UnityEvent onPressButton;

	// Token: 0x0400289B RID: 10395
	[TextArea]
	public string offText;

	// Token: 0x0400289C RID: 10396
	[TextArea]
	public string onText;

	// Token: 0x0400289D RID: 10397
	public Text myText;

	// Token: 0x0400289E RID: 10398
	private float touchTime;

	// Token: 0x0400289F RID: 10399
	private float releaseTime;

	// Token: 0x040028A0 RID: 10400
	private bool pendingPress;

	// Token: 0x040028A1 RID: 10401
	private Collider pendingPressCollider;

	// Token: 0x040028A2 RID: 10402
	private GorillaTriggerColliderHandIndicator pressingHand;
}
