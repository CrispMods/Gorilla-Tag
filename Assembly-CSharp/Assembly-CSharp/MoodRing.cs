using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class MoodRing : MonoBehaviour, ISpawnable
{
	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x0008395B File Offset: 0x00081B5B
	// (set) Token: 0x06001AB5 RID: 6837 RVA: 0x00083963 File Offset: 0x00081B63
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06001AB6 RID: 6838 RVA: 0x0008396C File Offset: 0x00081B6C
	// (set) Token: 0x06001AB7 RID: 6839 RVA: 0x00083974 File Offset: 0x00081B74
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x0008397D File Offset: 0x00081B7D
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x00083988 File Offset: 0x00081B88
	private void Update()
	{
		if ((this.attachedToLeftHand ? this.myRig.leftIndex.calcT : this.myRig.rightIndex.calcT) > 0.5f)
		{
			if (!this.isCycling)
			{
				this.animRedValue = this.myRig.playerColor.r;
				this.animGreenValue = this.myRig.playerColor.g;
				this.animBlueValue = this.myRig.playerColor.b;
			}
			this.isCycling = true;
			this.RainbowCycle(ref this.animRedValue, ref this.animGreenValue, ref this.animBlueValue);
			this.myRig.InitializeNoobMaterialLocal(this.animRedValue, this.animGreenValue, this.animBlueValue);
			return;
		}
		if (this.isCycling)
		{
			this.isCycling = false;
			if (this.myRig.isOfflineVRRig)
			{
				this.animRedValue = Mathf.Round(this.animRedValue * 9f) / 9f;
				this.animGreenValue = Mathf.Round(this.animGreenValue * 9f) / 9f;
				this.animBlueValue = Mathf.Round(this.animBlueValue * 9f) / 9f;
				GorillaTagger.Instance.UpdateColor(this.animRedValue, this.animGreenValue, this.animBlueValue);
				if (NetworkSystem.Instance.InRoom)
				{
					GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
					{
						this.animRedValue,
						this.animGreenValue,
						this.animBlueValue
					});
				}
				PlayerPrefs.SetFloat("redValue", this.animRedValue);
				PlayerPrefs.SetFloat("greenValue", this.animGreenValue);
				PlayerPrefs.SetFloat("blueValue", this.animBlueValue);
				PlayerPrefs.Save();
			}
		}
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x00083B6C File Offset: 0x00081D6C
	private void RainbowCycle(ref float r, ref float g, ref float b)
	{
		float num = this.furCycleSpeed * Time.deltaTime;
		if (r == 1f)
		{
			if (b > 0f)
			{
				b = Mathf.Clamp01(b - num);
				return;
			}
			if (g < 1f)
			{
				g = Mathf.Clamp01(g + num);
				return;
			}
			r = Mathf.Clamp01(r - num);
			return;
		}
		else if (g == 1f)
		{
			if (r > 0f)
			{
				r = Mathf.Clamp01(r - num);
				return;
			}
			if (b < 1f)
			{
				b = Mathf.Clamp01(b + num);
				return;
			}
			g = Mathf.Clamp01(g - num);
			return;
		}
		else
		{
			if (b != 1f)
			{
				r = Mathf.Clamp01(r + num);
				return;
			}
			if (g > 0f)
			{
				g = Mathf.Clamp01(g - num);
				return;
			}
			if (r < 1f)
			{
				r = Mathf.Clamp01(r + num);
				return;
			}
			b = Mathf.Clamp01(b - num);
			return;
		}
	}

	// Token: 0x04001D80 RID: 7552
	[SerializeField]
	private bool attachedToLeftHand;

	// Token: 0x04001D81 RID: 7553
	private VRRig myRig;

	// Token: 0x04001D82 RID: 7554
	[SerializeField]
	private float rotationSpeed;

	// Token: 0x04001D83 RID: 7555
	[SerializeField]
	private float furCycleSpeed;

	// Token: 0x04001D84 RID: 7556
	private float nextFurCycleTimestamp;

	// Token: 0x04001D85 RID: 7557
	private float animRedValue;

	// Token: 0x04001D86 RID: 7558
	private float animGreenValue;

	// Token: 0x04001D87 RID: 7559
	private float animBlueValue;

	// Token: 0x04001D88 RID: 7560
	private bool isCycling;
}
