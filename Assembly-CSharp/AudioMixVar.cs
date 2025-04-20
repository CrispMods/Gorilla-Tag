using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000497 RID: 1175
[Serializable]
public class AudioMixVar
{
	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06001C69 RID: 7273 RVA: 0x000DC1F8 File Offset: 0x000DA3F8
	// (set) Token: 0x06001C6A RID: 7274 RVA: 0x0004395E File Offset: 0x00041B5E
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

	// Token: 0x06001C6B RID: 7275 RVA: 0x00043980 File Offset: 0x00041B80
	public void ReturnToPool()
	{
		if (this._pool != null)
		{
			this._pool.Return(this);
		}
	}

	// Token: 0x04001F67 RID: 8039
	public AudioMixerGroup group;

	// Token: 0x04001F68 RID: 8040
	public AudioMixer mixer;

	// Token: 0x04001F69 RID: 8041
	public string name;

	// Token: 0x04001F6A RID: 8042
	[NonSerialized]
	public bool taken;

	// Token: 0x04001F6B RID: 8043
	[SerializeField]
	private AudioMixVarPool _pool;
}
