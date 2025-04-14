using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000663 RID: 1635
public abstract class GorillaKeyButton<TBinding> : MonoBehaviour where TBinding : Enum
{
	// Token: 0x0600287D RID: 10365 RVA: 0x000C6F99 File Offset: 0x000C5199
	private void Start()
	{
		if (this.ButtonRenderer == null)
		{
			this.ButtonRenderer = base.GetComponent<Renderer>();
		}
		this.propBlock = new MaterialPropertyBlock();
		this.pressTime = 0f;
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x000C6FCC File Offset: 0x000C51CC
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

	// Token: 0x0600287F RID: 10367 RVA: 0x000C70B0 File Offset: 0x000C52B0
	public void PressButtonColourUpdate()
	{
		this.propBlock.SetColor("_BaseColor", this.ButtonColorSettings.PressedColor);
		this.propBlock.SetColor("_Color", this.ButtonColorSettings.PressedColor);
		this.ButtonRenderer.SetPropertyBlock(this.propBlock);
		this.pressTime = Time.time;
		base.StartCoroutine(this.<PressButtonColourUpdate>g__ButtonColorUpdate_Local|13_0());
	}

	// Token: 0x06002880 RID: 10368
	public abstract void OnButtonPressedEvent();

	// Token: 0x06002882 RID: 10370 RVA: 0x000C712F File Offset: 0x000C532F
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

	// Token: 0x04002D62 RID: 11618
	public string characterString;

	// Token: 0x04002D63 RID: 11619
	public TBinding Binding;

	// Token: 0x04002D64 RID: 11620
	public float pressTime;

	// Token: 0x04002D65 RID: 11621
	public bool functionKey;

	// Token: 0x04002D66 RID: 11622
	public bool testClick;

	// Token: 0x04002D67 RID: 11623
	public bool repeatTestClick;

	// Token: 0x04002D68 RID: 11624
	public float repeatCooldown = 2f;

	// Token: 0x04002D69 RID: 11625
	public Renderer ButtonRenderer;

	// Token: 0x04002D6A RID: 11626
	public ButtonColorSettings ButtonColorSettings;

	// Token: 0x04002D6B RID: 11627
	private float lastTestClick;

	// Token: 0x04002D6C RID: 11628
	private MaterialPropertyBlock propBlock;
}
