using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200048B RID: 1163
[Serializable]
public class AudioMixVar
{
	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001C15 RID: 7189 RVA: 0x000886C0 File Offset: 0x000868C0
	// (set) Token: 0x06001C16 RID: 7190 RVA: 0x0008870F File Offset: 0x0008690F
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

	// Token: 0x06001C17 RID: 7191 RVA: 0x00088731 File Offset: 0x00086931
	public void ReturnToPool()
	{
		if (this._pool != null)
		{
			this._pool.Return(this);
		}
	}

	// Token: 0x04001F18 RID: 7960
	public AudioMixerGroup group;

	// Token: 0x04001F19 RID: 7961
	public AudioMixer mixer;

	// Token: 0x04001F1A RID: 7962
	public string name;

	// Token: 0x04001F1B RID: 7963
	[NonSerialized]
	public bool taken;

	// Token: 0x04001F1C RID: 7964
	[SerializeField]
	private AudioMixVarPool _pool;
}
