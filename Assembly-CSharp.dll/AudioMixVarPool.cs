using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
[CreateAssetMenu(fileName = "New AudioMixVarPool", menuName = "ScriptableObjects/AudioMixVarPool", order = 0)]
public class AudioMixVarPool : ScriptableObject
{
	// Token: 0x06001C15 RID: 7189 RVA: 0x000D94C4 File Offset: 0x000D76C4
	public bool Rent(out AudioMixVar mixVar)
	{
		for (int i = 0; i < this._vars.Length; i++)
		{
			if (!this._vars[i].taken)
			{
				this._vars[i].taken = true;
				mixVar = this._vars[i];
				return true;
			}
		}
		mixVar = null;
		return false;
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x000D9514 File Offset: 0x000D7714
	public void Return(AudioMixVar mixVar)
	{
		if (mixVar == null)
		{
			return;
		}
		int num = this._vars.IndexOfRef(mixVar);
		if (num == -1)
		{
			return;
		}
		this._vars[num].taken = false;
	}

	// Token: 0x04001F18 RID: 7960
	[SerializeField]
	private AudioMixVar[] _vars = new AudioMixVar[0];
}
