using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB3 RID: 2995
	[Serializable]
	public class CoolDownHelper
	{
		// Token: 0x06004BBF RID: 19391 RVA: 0x00170F74 File Offset: 0x0016F174
		public CoolDownHelper()
		{
			this.coolDown = 1f;
			this.checkTime = 0f;
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x00170F92 File Offset: 0x0016F192
		public CoolDownHelper(float cd)
		{
			this.coolDown = cd;
			this.checkTime = 0f;
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x00170FAC File Offset: 0x0016F1AC
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

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00170FD6 File Offset: 0x0016F1D6
		public virtual void Start()
		{
			this.checkTime = Time.time + this.coolDown;
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x00170FEA File Offset: 0x0016F1EA
		public virtual void Stop()
		{
			this.checkTime = float.MaxValue;
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x000023F4 File Offset: 0x000005F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual void OnCheckPass()
		{
		}

		// Token: 0x04004D99 RID: 19865
		public float coolDown;

		// Token: 0x04004D9A RID: 19866
		[NonSerialized]
		public float checkTime;
	}
}
