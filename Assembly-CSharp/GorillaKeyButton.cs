using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public abstract class GorillaKeyButton<TBinding> : MonoBehaviour where TBinding : Enum
{
	// Token: 0x06002875 RID: 10357 RVA: 0x000C6B19 File Offset: 0x000C4D19
	private void Start()
	{
		if (this.ButtonRenderer == null)
		{
			this.ButtonRenderer = base.GetComponent<Renderer>();
		}
		this.propBlock = new MaterialPropertyBlock();
		this.pressTime = 0f;
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x000C6B4C File Offset: 0x000C4D4C
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

	// Token: 0x06002877 RID: 10359 RVA: 0x000C6C30 File Offset: 0x000C4E30
	public void PressButtonColourUpdate()
	{
		this.propBlock.SetColor("_BaseColor", this.ButtonColorSettings.PressedColor);
		this.propBlock.SetColor("_Color", this.ButtonColorSettings.PressedColor);
		this.ButtonRenderer.SetPropertyBlock(this.propBlock);
		this.pressTime = Time.time;
		base.StartCoroutine(this.<PressButtonColourUpdate>g__ButtonColorUpdate_Local|13_0());
	}

	// Token: 0x06002878 RID: 10360
	public abstract void OnButtonPressedEvent();

	// Token: 0x0600287A RID: 10362 RVA: 0x000C6CAF File Offset: 0x000C4EAF
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

	// Token: 0x04002D5C RID: 11612
	public string characterString;

	// Token: 0x04002D5D RID: 11613
	public TBinding Binding;

	// Token: 0x04002D5E RID: 11614
	public float pressTime;

	// Token: 0x04002D5F RID: 11615
	public bool functionKey;

	// Token: 0x04002D60 RID: 11616
	public bool testClick;

	// Token: 0x04002D61 RID: 11617
	public bool repeatTestClick;

	// Token: 0x04002D62 RID: 11618
	public float repeatCooldown = 2f;

	// Token: 0x04002D63 RID: 11619
	public Renderer ButtonRenderer;

	// Token: 0x04002D64 RID: 11620
	public ButtonColorSettings ButtonColorSettings;

	// Token: 0x04002D65 RID: 11621
	private float lastTestClick;

	// Token: 0x04002D66 RID: 11622
	private MaterialPropertyBlock propBlock;
}
