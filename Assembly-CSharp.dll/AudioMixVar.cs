using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200048B RID: 1163
[Serializable]
public class AudioMixVar
{
	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001C18 RID: 7192 RVA: 0x000D9548 File Offset: 0x000D7748
	// (set) Token: 0x06001C19 RID: 7193 RVA: 0x00042625 File Offset: 0x00040825
	public float value
	{
		get
		{
			if (!this.group)
			{
				return 0f;
			}
			if (!this.mixer)
			{
				return 0f;
			}
			float result;
			if (!this.mixer.GetFloat(this.name, out result))
			{
				return 0f;
			}
			return result;
		}
		set
		{
			if (this.mixer)
			{
				this.mixer.SetFloat(this.name, value);
			}
		}
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x00042647 File Offset: 0x00040847
	public void ReturnToPool()
	{
		if (this._pool != null)
		{
			this._pool.Return(this);
		}
	}

	// Token: 0x04001F19 RID: 7961
	public AudioMixerGroup group;

	// Token: 0x04001F1A RID: 7962
	public AudioMixer mixer;

	// Token: 0x04001F1B RID: 7963
	public string name;

	// Token: 0x04001F1C RID: 7964
	[NonSerialized]
	public bool taken;

	// Token: 0x04001F1D RID: 7965
	[SerializeField]
	private AudioMixVarPool _pool;
}
