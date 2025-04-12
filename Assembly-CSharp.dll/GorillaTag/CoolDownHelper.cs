using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB6 RID: 2998
	[Serializable]
	public class CoolDownHelper
	{
		// Token: 0x06004BCB RID: 19403 RVA: 0x00060F62 File Offset: 0x0005F162
		public CoolDownHelper()
		{
			this.coolDown = 1f;
			this.checkTime = 0f;
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x00060F80 File Offset: 0x0005F180
		public CoolDownHelper(float cd)
		{
			this.coolDown = cd;
			this.checkTime = 0f;
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x00060F9A File Offset: 0x0005F19A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CheckCooldown()
		{
			if (Time.time < this.checkTime)
			{
				return false;
			}
			this.OnCheckPass();
			this.checkTime = Time.unscaledTime + this.coolDown;
			return true;
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00060FC4 File Offset: 0x0005F1C4
		public virtual void Start()
		{
			this.checkTime = Time.time + this.coolDown;
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x00060FD8 File Offset: 0x0005F1D8
		public virtual void Stop()
		{
			this.checkTime = float.MaxValue;
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x0002F75F File Offset: 0x0002D95F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual void OnCheckPass()
		{
		}

		// Token: 0x04004DAB RID: 19883
		public float coolDown;

		// Token: 0x04004DAC RID: 19884
		[NonSerialized]
		public float checkTime;
	}
}
