using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200066A RID: 1642
public class GorillaPressableButton : MonoBehaviour
{
	// Token: 0x14000053 RID: 83
	// (add) Token: 0x060028A2 RID: 10402 RVA: 0x000C7C3C File Offset: 0x000C5E3C
	// (remove) Token: 0x060028A3 RID: 10403 RVA: 0x000C7C74 File Offset: 0x000C5E74
	public event Action<GorillaPressableButton, bool> onPressed;

	// Token: 0x060028A4 RID: 10404 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void Start()
	{
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnEnable()
	{
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnDisable()
	{
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x000C7CAC File Offset: 0x000C5EAC
	protected void OnTriggerEnter(Collider collider)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.touchTime + this.debounceTime >= Time.time)
		{
			return;
		}
		if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
		{
			return;
		}
		this.touchTime = Time.time;
		GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
		UnityEvent unityEvent = this.onPressButton;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		Action<GorillaPressableButton, bool> action = this.onPressed;
		if (action != null)
		{
			action(this, component.isLeftHand);
		}
		this.ButtonActivation();
		this.ButtonActivationWithHand(component.isLeftHand);
		if (component == null)
		{
			return;
		}
		GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(this.pressButtonSoundIndex, component.isLeftHand, 0.05f);
		GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		if (NetworkSystem.Instance.InRoom && GorillaTagger.Instance.myVRRig != null)
		{
			GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
			{
				67,
				component.isLeftHand,
				0.05f
			});
		}
	}

	// Token: 0x060028A8 RID: 10408 RVA: 0x000C7DE8 File Offset: 0x000C5FE8
	public virtual void UpdateColor()
	{
		if (this.isOn)
		{
			this.buttonRenderer.material = this.pressedMaterial;
			if (this.myTmpText != null)
			{
				this.myTmpText.text = this.onText;
			}
			if (this.myTmpText2 != null)
			{
				this.myTmpText2.text = this.onText;
				return;
			}
			if (this.myText != null)
			{
				this.myText.text = this.onText;
				return;
			}
		}
		else
		{
			this.buttonRenderer.material = this.unpressedMaterial;
			if (this.myTmpText != null)
			{
				this.myTmpText.text = this.offText;
			}
			if (this.myTmpText2 != null)
			{
				this.myTmpText2.text = this.offText;
				return;
			}
			if (this.myText != null)
			{
				this.myText.text = this.offText;
			}
		}
	}

	// Token: 0x060028A9 RID: 10409 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void ButtonActivation()
	{
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void ButtonActivationWithHand(bool isLeftHand)
	{
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x000C7EDF File Offset: 0x000C60DF
	public virtual void ResetState()
	{
		this.isOn = false;
		this.UpdateColor();
	}

	// Token: 0x04002DAD RID: 11693
	public Material pressedMaterial;

	// Token: 0x04002DAE RID: 11694
	public Material unpressedMaterial;

	// Token: 0x04002DAF RID: 11695
	public MeshRenderer buttonRenderer;

	// Token: 0x04002DB0 RID: 11696
	public int pressButtonSoundIndex = 67;

	// Token: 0x04002DB1 RID: 11697
	public bool isOn;

	// Token: 0x04002DB2 RID: 11698
	public float debounceTime = 0.25f;

	// Token: 0x04002DB3 RID: 11699
	public float touchTime;

	// Token: 0x04002DB4 RID: 11700
	public bool testPress;

	// Token: 0x04002DB5 RID: 11701
	public bool testHandLeft;

	// Token: 0x04002DB6 RID: 11702
	[TextArea]
	public string offText;

	// Token: 0x04002DB7 RID: 11703
	[TextArea]
	public string onText;

	// Token: 0x04002DB8 RID: 11704
	[SerializeField]
	[Tooltip("Use this one when you can. Don't use MyText if you can help it!")]
	public TMP_Text myTmpText;

	// Token: 0x04002DB9 RID: 11705
	[SerializeField]
	[Tooltip("Use this one when you can. Don't use MyText if you can help it!")]
	public TMP_Text myTmpText2;

	// Token: 0x04002DBA RID: 11706
	public Text myText;

	// Token: 0x04002DBB RID: 11707
	[Space]
	public UnityEvent onPressButton;
}
