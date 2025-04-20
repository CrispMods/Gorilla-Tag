using System;
using UnityEngine;

// Token: 0x02000496 RID: 1174
[CreateAssetMenu(fileName = "New AudioMixVarPool", menuName = "ScriptableObjects/AudioMixVarPool", order = 0)]
public class AudioMixVarPool : ScriptableObject
{
	// Token: 0x06001C66 RID: 7270 RVA: 0x000DC174 File Offset: 0x000DA374
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

	// Token: 0x06001C67 RID: 7271 RVA: 0x000DC1C4 File Offset: 0x000DA3C4
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

	// Token: 0x04001F66 RID: 8038
	[SerializeField]
	private AudioMixVar[] _vars = new AudioMixVar[0];
}
