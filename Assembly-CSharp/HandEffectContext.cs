using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020003C4 RID: 964
[Serializable]
internal class HandEffectContext : IFXEffectContextObject
{
	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06001748 RID: 5960 RVA: 0x0003FCE8 File Offset: 0x0003DEE8
	public int[] PrefabPoolIds
	{
		get
		{
			return this.prefabHashes;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06001749 RID: 5961 RVA: 0x0003FCF0 File Offset: 0x0003DEF0
	public Vector3 Positon
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x0600174A RID: 5962 RVA: 0x0003FCF8 File Offset: 0x0003DEF8
	public Quaternion Rotation
	{
		get
		{
			return this.rotation;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x0600174B RID: 5963 RVA: 0x0003FD00 File Offset: 0x0003DF00
	public AudioSource SoundSource
	{
		get
		{
			return this.handSoundSource;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x0600174C RID: 5964 RVA: 0x0003FD08 File Offset: 0x0003DF08
	public AudioClip Sound
	{
		get
		{
			return this.soundFX;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x0600174D RID: 5965 RVA: 0x0003FD10 File Offset: 0x0003DF10
	public float Volume
	{
		get
		{
			return this.clipVolume;
		}
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000C7E28 File Offset: 0x000C6028
	public void OnPlayVisualFX(int fxID, GameObject fx)
	{
		FXModifier fxmodifier;
		if (fxID == GorillaAmbushManager.HandEffectHash && fx.TryGetComponent<FXModifier>(out fxmodifier))
		{
			fxmodifier.UpdateScale(this.handSpeed * GorillaAmbushManager.HandFXScaleModifier);
		}
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlaySoundFX(AudioSource audioSource)
	{
	}

	// Token: 0x040019F1 RID: 6641
	internal int[] prefabHashes = new int[]
	{
		-1,
		-1
	};

	// Token: 0x040019F2 RID: 6642
	internal Vector3 position;

	// Token: 0x040019F3 RID: 6643
	internal Quaternion rotation;

	// Token: 0x040019F4 RID: 6644
	[SerializeField]
	internal AudioSource handSoundSource;

	// Token: 0x040019F5 RID: 6645
	internal AudioClip soundFX;

	// Token: 0x040019F6 RID: 6646
	internal float clipVolume;

	// Token: 0x040019F7 RID: 6647
	internal float handSpeed;
}
