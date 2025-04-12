using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020003B9 RID: 953
[Serializable]
internal class HandEffectContext : IFXEffectContextObject
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060016FE RID: 5886 RVA: 0x0003E9FE File Offset: 0x0003CBFE
	public int[] PrefabPoolIds
	{
		get
		{
			return this.prefabHashes;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060016FF RID: 5887 RVA: 0x0003EA06 File Offset: 0x0003CC06
	public Vector3 Positon
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06001700 RID: 5888 RVA: 0x0003EA0E File Offset: 0x0003CC0E
	public Quaternion Rotation
	{
		get
		{
			return this.rotation;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06001701 RID: 5889 RVA: 0x0003EA16 File Offset: 0x0003CC16
	public AudioSource SoundSource
	{
		get
		{
			return this.handSoundSource;
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06001702 RID: 5890 RVA: 0x0003EA1E File Offset: 0x0003CC1E
	public AudioClip Sound
	{
		get
		{
			return this.soundFX;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06001703 RID: 5891 RVA: 0x0003EA26 File Offset: 0x0003CC26
	public float Volume
	{
		get
		{
			return this.clipVolume;
		}
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x000C5600 File Offset: 0x000C3800
	public void OnPlayVisualFX(int fxID, GameObject fx)
	{
		FXModifier fxmodifier;
		if (fxID == GorillaAmbushManager.HandEffectHash && fx.TryGetComponent<FXModifier>(out fxmodifier))
		{
			fxmodifier.UpdateScale(this.handSpeed * GorillaAmbushManager.HandFXScaleModifier);
		}
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnPlaySoundFX(AudioSource audioSource)
	{
	}

	// Token: 0x040019A9 RID: 6569
	internal int[] prefabHashes = new int[]
	{
		-1,
		-1
	};

	// Token: 0x040019AA RID: 6570
	internal Vector3 position;

	// Token: 0x040019AB RID: 6571
	internal Quaternion rotation;

	// Token: 0x040019AC RID: 6572
	[SerializeField]
	internal AudioSource handSoundSource;

	// Token: 0x040019AD RID: 6573
	internal AudioClip soundFX;

	// Token: 0x040019AE RID: 6574
	internal float clipVolume;

	// Token: 0x040019AF RID: 6575
	internal float handSpeed;
}
