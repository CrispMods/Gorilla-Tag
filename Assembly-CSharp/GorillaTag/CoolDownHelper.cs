using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BE1 RID: 3041
	[Serializable]
	public class CoolDownHelper
	{
		// Token: 0x06004D08 RID: 19720 RVA: 0x00062904 File Offset: 0x00060B04
		public CoolDownHelper()
		{
			this.coolDown = 1f;
			this.checkTime = 0f;
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x00062922 File Offset: 0x00060B22
		public CoolDownHelper(float cd)
		{
			this.coolDown = cd;
			this.checkTime = 0f;
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x0006293C File Offset: 0x00060B3C
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

		// Token: 0x06004D0B RID: 19723 RVA: 0x00062966 File Offset: 0x00060B66
		public virtual void Start()
		{
			this.checkTime = Time.time + this.coolDown;
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x0006297A File Offset: 0x00060B7A
		public virtual void Stop()
		{
			this.checkTime = float.MaxValue;
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x00030607 File Offset: 0x0002E807
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual void OnCheckPass()
		{
		}

		// Token: 0x04004E8F RID: 20111
		public float coolDown;

		// Token: 0x04004E90 RID: 20112
		[NonSerialized]
		public float checkTime;
	}
}
