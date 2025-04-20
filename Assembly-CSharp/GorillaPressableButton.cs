using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000649 RID: 1609
public class GorillaPressableButton : MonoBehaviour
{
	// Token: 0x14000054 RID: 84
	// (add) Token: 0x060027CD RID: 10189 RVA: 0x0010EA14 File Offset: 0x0010CC14
	// (remove) Token: 0x060027CE RID: 10190 RVA: 0x0010EA4C File Offset: 0x0010CC4C
	public event Action<GorillaPressableButton, bool> onPressed;

	// Token: 0x060027CF RID: 10191 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void Start()
	{
	}

	// Token: 0x060027D0 RID: 10192 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnEnable()
	{
	}

	// Token: 0x060027D1 RID: 10193 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnDisable()
	{
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x0010EA84 File Offset: 0x0010CC84
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

	// Token: 0x060027D3 RID: 10195 RVA: 0x0010EBC0 File Offset: 0x0010CDC0
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

	// Token: 0x060027D4 RID: 10196 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void ButtonActivation()
	{
	}

	// Token: 0x060027D5 RID: 10197 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void ButtonActivationWithHand(bool isLeftHand)
	{
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x0004B121 File Offset: 0x00049321
	public virtual void ResetState()
	{
		this.isOn = false;
		this.UpdateColor();
	}

	// Token: 0x04002D13 RID: 11539
	public Material pressedMaterial;

	// Token: 0x04002D14 RID: 11540
	public Material unpressedMaterial;

	// Token: 0x04002D15 RID: 11541
	public MeshRenderer buttonRenderer;

	// Token: 0x04002D16 RID: 11542
	public int pressButtonSoundIndex = 67;

	// Token: 0x04002D17 RID: 11543
	public bool isOn;

	// Token: 0x04002D18 RID: 11544
	public float debounceTime = 0.25f;

	// Token: 0x04002D19 RID: 11545
	public float touchTime;

	// Token: 0x04002D1A RID: 11546
	public bool testPress;

	// Token: 0x04002D1B RID: 11547
	public bool testHandLeft;

	// Token: 0x04002D1C RID: 11548
	[TextArea]
	public string offText;

	// Token: 0x04002D1D RID: 11549
	[TextArea]
	public string onText;

	// Token: 0x04002D1E RID: 11550
	[SerializeField]
	[Tooltip("Use this one when you can. Don't use MyText if you can help it!")]
	public TMP_Text myTmpText;

	// Token: 0x04002D1F RID: 11551
	[SerializeField]
	[Tooltip("Use this one when you can. Don't use MyText if you can help it!")]
	public TMP_Text myTmpText2;

	// Token: 0x04002D20 RID: 11552
	public Text myText;

	// Token: 0x04002D21 RID: 11553
	[Space]
	public UnityEvent onPressButton;
}
