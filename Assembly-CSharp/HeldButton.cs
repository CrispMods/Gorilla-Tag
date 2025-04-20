using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020005C9 RID: 1481
public class HeldButton : MonoBehaviour
{
	// Token: 0x060024CE RID: 9422 RVA: 0x00103F14 File Offset: 0x00102114
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
			UnityEvent unityEvent = this.onStartPressingButton;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.touchTime = Time.time;
			this.pendingPressCollider = other;
			this.pressingHand = componentInParent;
			this.pendingPress = true;
			this.SetOn(true);
			GorillaTagger.Instance.StartVibration(componentInParent.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		}
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x00103FD4 File Offset: 0x001021D4
	private void LateUpdate()
	{
		if (!this.pendingPress)
		{
			return;
		}
		if (this.touchTime < this.releaseTime && this.releaseTime + this.debounceTime < Time.time)
		{
			UnityEvent unityEvent = this.onStopPressingButton;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
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
			UnityEvent unityEvent2 = this.onStopPressingButton;
			if (unityEvent2 != null)
			{
				unityEvent2.Invoke();
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

	// Token: 0x060024D0 RID: 9424 RVA: 0x00048F66 File Offset: 0x00047166
	private void OnTriggerExit(Collider other)
	{
		if (this.pendingPress && this.pendingPressCollider == other)
		{
			this.releaseTime = Time.time;
			UnityEvent unityEvent = this.onStopPressingButton;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x00104134 File Offset: 0x00102334
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

	// Token: 0x040028E9 RID: 10473
	public Material pressedMaterial;

	// Token: 0x040028EA RID: 10474
	public Material unpressedMaterial;

	// Token: 0x040028EB RID: 10475
	public MeshRenderer buttonRenderer;

	// Token: 0x040028EC RID: 10476
	private bool isOn;

	// Token: 0x040028ED RID: 10477
	public float debounceTime = 0.25f;

	// Token: 0x040028EE RID: 10478
	public bool leftHandPressable;

	// Token: 0x040028EF RID: 10479
	public bool rightHandPressable = true;

	// Token: 0x040028F0 RID: 10480
	public float pressDuration = 0.5f;

	// Token: 0x040028F1 RID: 10481
	public UnityEvent onStartPressingButton;

	// Token: 0x040028F2 RID: 10482
	public UnityEvent onStopPressingButton;

	// Token: 0x040028F3 RID: 10483
	public UnityEvent onPressButton;

	// Token: 0x040028F4 RID: 10484
	[TextArea]
	public string offText;

	// Token: 0x040028F5 RID: 10485
	[TextArea]
	public string onText;

	// Token: 0x040028F6 RID: 10486
	public Text myText;

	// Token: 0x040028F7 RID: 10487
	private float touchTime;

	// Token: 0x040028F8 RID: 10488
	private float releaseTime;

	// Token: 0x040028F9 RID: 10489
	private bool pendingPress;

	// Token: 0x040028FA RID: 10490
	private Collider pendingPressCollider;

	// Token: 0x040028FB RID: 10491
	private GorillaTriggerColliderHandIndicator pressingHand;
}
