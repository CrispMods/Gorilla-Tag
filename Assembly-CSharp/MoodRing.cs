using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class MoodRing : MonoBehaviour, ISpawnable
{
	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06001B05 RID: 6917 RVA: 0x000425AC File Offset: 0x000407AC
	// (set) Token: 0x06001B06 RID: 6918 RVA: 0x000425B4 File Offset: 0x000407B4
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06001B07 RID: 6919 RVA: 0x000425BD File Offset: 0x000407BD
	// (set) Token: 0x06001B08 RID: 6920 RVA: 0x000425C5 File Offset: 0x000407C5
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001B09 RID: 6921 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000425CE File Offset: 0x000407CE
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000D8500 File Offset: 0x000D6700
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

	// Token: 0x06001B0C RID: 6924 RVA: 0x000D86E4 File Offset: 0x000D68E4
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

	// Token: 0x04001DCE RID: 7630
	[SerializeField]
	private bool attachedToLeftHand;

	// Token: 0x04001DCF RID: 7631
	private VRRig myRig;

	// Token: 0x04001DD0 RID: 7632
	[SerializeField]
	private float rotationSpeed;

	// Token: 0x04001DD1 RID: 7633
	[SerializeField]
	private float furCycleSpeed;

	// Token: 0x04001DD2 RID: 7634
	private float nextFurCycleTimestamp;

	// Token: 0x04001DD3 RID: 7635
	private float animRedValue;

	// Token: 0x04001DD4 RID: 7636
	private float animGreenValue;

	// Token: 0x04001DD5 RID: 7637
	private float animBlueValue;

	// Token: 0x04001DD6 RID: 7638
	private bool isCycling;
}
