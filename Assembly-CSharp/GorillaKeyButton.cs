using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000641 RID: 1601
public abstract class GorillaKeyButton<TBinding> : MonoBehaviour where TBinding : Enum
{
	// Token: 0x060027A0 RID: 10144 RVA: 0x0004AF9A File Offset: 0x0004919A
	private void Start()
	{
		if (this.ButtonRenderer == null)
		{
			this.ButtonRenderer = base.GetComponent<Renderer>();
		}
		this.propBlock = new MaterialPropertyBlock();
		this.pressTime = 0f;
	}

	// Token: 0x060027A1 RID: 10145 RVA: 0x0010DA78 File Offset: 0x0010BC78
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
		{
			GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
			this.OnButtonPressedEvent();
			this.PressButtonColourUpdate();
			if (component != null)
			{
				GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
				GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, component.isLeftHand, 0.1f);
				if (NetworkSystem.Instance.InRoom && GorillaTagger.Instance.myVRRig != null)
				{
					GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
					{
						66,
						component.isLeftHand,
						0.1f
					});
				}
			}
		}
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x0010DB5C File Offset: 0x0010BD5C
	public void PressButtonColourUpdate()
	{
		this.propBlock.SetColor("_BaseColor", this.ButtonColorSettings.PressedColor);
		this.propBlock.SetColor("_Color", this.ButtonColorSettings.PressedColor);
		this.ButtonRenderer.SetPropertyBlock(this.propBlock);
		this.pressTime = Time.time;
		base.StartCoroutine(this.<PressButtonColourUpdate>g__ButtonColorUpdate_Local|13_0());
	}

	// Token: 0x060027A3 RID: 10147
	public abstract void OnButtonPressedEvent();

	// Token: 0x060027A5 RID: 10149 RVA: 0x0004AFDF File Offset: 0x000491DF
	[CompilerGenerated]
	private IEnumerator <PressButtonColourUpdate>g__ButtonColorUpdate_Local|13_0()
	{
		yield return new WaitForSeconds(this.ButtonColorSettings.PressedTime);
		if (this.pressTime != 0f && Time.time > this.ButtonColorSettings.PressedTime + this.pressTime)
		{
			this.propBlock.SetColor("_BaseColor", this.ButtonColorSettings.UnpressedColor);
			this.propBlock.SetColor("_Color", this.ButtonColorSettings.UnpressedColor);
			this.ButtonRenderer.SetPropertyBlock(this.propBlock);
			this.pressTime = 0f;
		}
		yield break;
	}

	// Token: 0x04002CC2 RID: 11458
	public string characterString;

	// Token: 0x04002CC3 RID: 11459
	public TBinding Binding;

	// Token: 0x04002CC4 RID: 11460
	public float pressTime;

	// Token: 0x04002CC5 RID: 11461
	public bool functionKey;

	// Token: 0x04002CC6 RID: 11462
	public bool testClick;

	// Token: 0x04002CC7 RID: 11463
	public bool repeatTestClick;

	// Token: 0x04002CC8 RID: 11464
	public float repeatCooldown = 2f;

	// Token: 0x04002CC9 RID: 11465
	public Renderer ButtonRenderer;

	// Token: 0x04002CCA RID: 11466
	public ButtonColorSettings ButtonColorSettings;

	// Token: 0x04002CCB RID: 11467
	private float lastTestClick;

	// Token: 0x04002CCC RID: 11468
	private MaterialPropertyBlock propBlock;
}
