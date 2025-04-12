using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200066B RID: 1643
public class GorillaPressableButton : MonoBehaviour
{
	// Token: 0x14000053 RID: 83
	// (add) Token: 0x060028AA RID: 10410 RVA: 0x001105EC File Offset: 0x0010E7EC
	// (remove) Token: 0x060028AB RID: 10411 RVA: 0x00110624 File Offset: 0x0010E824
	public event Action<GorillaPressableButton, bool> onPressed;

	// Token: 0x060028AC RID: 10412 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void Start()
	{
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnEnable()
	{
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnDisable()
	{
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x0011065C File Offset: 0x0010E85C
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

	// Token: 0x060028B0 RID: 10416 RVA: 0x00110798 File Offset: 0x0010E998
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

	// Token: 0x060028B1 RID: 10417 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void ButtonActivation()
	{
	}

	// Token: 0x060028B2 RID: 10418 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void ButtonActivationWithHand(bool isLeftHand)
	{
	}

	// Token: 0x060028B3 RID: 10419 RVA: 0x0004AB84 File Offset: 0x00048D84
	public virtual void ResetState()
	{
		this.isOn = false;
		this.UpdateColor();
	}

	// Token: 0x04002DB3 RID: 11699
	public Material pressedMaterial;

	// Token: 0x04002DB4 RID: 11700
	public Material unpressedMaterial;

	// Token: 0x04002DB5 RID: 11701
	public MeshRenderer buttonRenderer;

	// Token: 0x04002DB6 RID: 11702
	public int pressButtonSoundIndex = 67;

	// Token: 0x04002DB7 RID: 11703
	public bool isOn;

	// Token: 0x04002DB8 RID: 11704
	public float debounceTime = 0.25f;

	// Token: 0x04002DB9 RID: 11705
	public float touchTime;

	// Token: 0x04002DBA RID: 11706
	public bool testPress;

	// Token: 0x04002DBB RID: 11707
	public bool testHandLeft;

	// Token: 0x04002DBC RID: 11708
	[TextArea]
	public string offText;

	// Token: 0x04002DBD RID: 11709
	[TextArea]
	public string onText;

	// Token: 0x04002DBE RID: 11710
	[SerializeField]
	[Tooltip("Use this one when you can. Don't use MyText if you can help it!")]
	public TMP_Text myTmpText;

	// Token: 0x04002DBF RID: 11711
	[SerializeField]
	[Tooltip("Use this one when you can. Don't use MyText if you can help it!")]
	public TMP_Text myTmpText2;

	// Token: 0x04002DC0 RID: 11712
	public Text myText;

	// Token: 0x04002DC1 RID: 11713
	[Space]
	public UnityEvent onPressButton;
}
