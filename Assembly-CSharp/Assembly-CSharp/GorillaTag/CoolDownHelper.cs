using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB6 RID: 2998
	[Serializable]
	public class CoolDownHelper
	{
		// Token: 0x06004BCB RID: 19403 RVA: 0x0017153C File Offset: 0x0016F73C
		public CoolDownHelper()
		{
			this.coolDown = 1f;
			this.checkTime = 0f;
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x0017155A File Offset: 0x0016F75A
		public CoolDownHelper(float cd)
		{
			this.coolDown = cd;
			this.checkTime = 0f;
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x00171574 File Offset: 0x0016F774
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

		// Token: 0x06004BCE RID: 19406 RVA: 0x0017159E File Offset: 0x0016F79E
		public virtual void Start()
		{
			this.checkTime = Time.time + this.coolDown;
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x001715B2 File Offset: 0x0016F7B2
		public virtual void Stop()
		{
			this.checkTime = float.MaxValue;
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x000023F4 File Offset: 0x000005F4
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
