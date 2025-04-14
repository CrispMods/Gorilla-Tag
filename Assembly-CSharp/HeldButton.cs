using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020005BB RID: 1467
public class HeldButton : MonoBehaviour
{
	// Token: 0x0600246C RID: 9324 RVA: 0x000B567C File Offset: 0x000B387C
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

	// Token: 0x0600246D RID: 9325 RVA: 0x000B572C File Offset: 0x000B392C
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

	// Token: 0x0600246E RID: 9326 RVA: 0x000B5869 File Offset: 0x000B3A69
	private void OnTriggerExit(Collider other)
	{
		if (this.pendingPress && this.pendingPressCollider == other)
		{
			this.releaseTime = Time.time;
		}
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000B588C File Offset: 0x000B3A8C
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

	// Token: 0x0400288C RID: 10380
	public Material pressedMaterial;

	// Token: 0x0400288D RID: 10381
	public Material unpressedMaterial;

	// Token: 0x0400288E RID: 10382
	public MeshRenderer buttonRenderer;

	// Token: 0x0400288F RID: 10383
	private bool isOn;

	// Token: 0x04002890 RID: 10384
	public float debounceTime = 0.25f;

	// Token: 0x04002891 RID: 10385
	public bool leftHandPressable;

	// Token: 0x04002892 RID: 10386
	public bool rightHandPressable = true;

	// Token: 0x04002893 RID: 10387
	public float pressDuration = 0.5f;

	// Token: 0x04002894 RID: 10388
	public UnityEvent onPressButton;

	// Token: 0x04002895 RID: 10389
	[TextArea]
	public string offText;

	// Token: 0x04002896 RID: 10390
	[TextArea]
	public string onText;

	// Token: 0x04002897 RID: 10391
	public Text myText;

	// Token: 0x04002898 RID: 10392
	private float touchTime;

	// Token: 0x04002899 RID: 10393
	private float releaseTime;

	// Token: 0x0400289A RID: 10394
	private bool pendingPress;

	// Token: 0x0400289B RID: 10395
	private Collider pendingPressCollider;

	// Token: 0x0400289C RID: 10396
	private GorillaTriggerColliderHandIndicator pressingHand;
}
