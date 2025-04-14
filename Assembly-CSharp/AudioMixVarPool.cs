using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
[CreateAssetMenu(fileName = "New AudioMixVarPool", menuName = "ScriptableObjects/AudioMixVarPool", order = 0)]
public class AudioMixVarPool : ScriptableObject
{
	// Token: 0x06001C12 RID: 7186 RVA: 0x00088628 File Offset: 0x00086828
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

	// Token: 0x06001C13 RID: 7187 RVA: 0x00088678 File Offset: 0x00086878
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

	// Token: 0x04001F17 RID: 7959
	[SerializeField]
	private AudioMixVar[] _vars = new AudioMixVar[0];
}
