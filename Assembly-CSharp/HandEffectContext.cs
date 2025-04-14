using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020003B9 RID: 953
[Serializable]
internal class HandEffectContext : IFXEffectContextObject
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060016FB RID: 5883 RVA: 0x00070A5F File Offset: 0x0006EC5F
	public int[] PrefabPoolIds
	{
		get
		{
			return this.prefabHashes;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060016FC RID: 5884 RVA: 0x00070A67 File Offset: 0x0006EC67
	public Vector3 Positon
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x060016FD RID: 5885 RVA: 0x00070A6F File Offset: 0x0006EC6F
	public Quaternion Rotation
	{
		get
		{
			return this.rotation;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x060016FE RID: 5886 RVA: 0x00070A77 File Offset: 0x0006EC77
	public AudioSource SoundSource
	{
		get
		{
			return this.handSoundSource;
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x060016FF RID: 5887 RVA: 0x00070A7F File Offset: 0x0006EC7F
	public AudioClip Sound
	{
		get
		{
			return this.soundFX;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06001700 RID: 5888 RVA: 0x00070A87 File Offset: 0x0006EC87
	public float Volume
	{
		get
		{
			return this.clipVolume;
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x00070A90 File Offset: 0x0006EC90
	public void OnPlayVisualFX(int fxID, GameObject fx)
	{
		FXModifier fxmodifier;
		if (fxID == GorillaAmbushManager.HandEffectHash && fx.TryGetComponent<FXModifier>(out fxmodifier))
		{
			fxmodifier.UpdateScale(this.handSpeed * GorillaAmbushManager.HandFXScaleModifier);
		}
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPlaySoundFX(AudioSource audioSource)
	{
	}

	// Token: 0x040019A8 RID: 6568
	internal int[] prefabHashes = new int[]
	{
		-1,
		-1
	};

	// Token: 0x040019A9 RID: 6569
	internal Vector3 position;

	// Token: 0x040019AA RID: 6570
	internal Quaternion rotation;

	// Token: 0x040019AB RID: 6571
	[SerializeField]
	internal AudioSource handSoundSource;

	// Token: 0x040019AC RID: 6572
	internal AudioClip soundFX;

	// Token: 0x040019AD RID: 6573
	internal float clipVolume;

	// Token: 0x040019AE RID: 6574
	internal float handSpeed;
}
