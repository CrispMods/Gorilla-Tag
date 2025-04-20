using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BE2 RID: 3042
	internal abstract class TickSystemTimerAbstract : CoolDownHelper, ITickSystemPre
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06004D0E RID: 19726 RVA: 0x00062987 File Offset: 0x00060B87
		// (set) Token: 0x06004D0F RID: 19727 RVA: 0x0006298F File Offset: 0x00060B8F
		bool ITickSystemPre.PreTickRunning
		{
			get
			{
				return this.registered;
			}
			set
			{
				this.registered = value;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06004D10 RID: 19728 RVA: 0x00062987 File Offset: 0x00060B87
		public bool Running
		{
			get
			{
				return this.registered;
			}
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x00062998 File Offset: 0x00060B98
		protected TickSystemTimerAbstract()
		{
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x000629A0 File Offset: 0x00060BA0
		protected TickSystemTimerAbstract(float cd) : base(cd)
		{
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x000629A9 File Offset: 0x00060BA9
		public override void Start()
		{
			base.Start();
			TickSystem<object>.AddPreTickCallback(this);
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x000629B7 File Offset: 0x00060BB7
		public override void Stop()
		{
			base.Stop();
			TickSystem<object>.RemovePreTickCallback(this);
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x000629C5 File Offset: 0x00060BC5
		public override void OnCheckPass()
		{
			this.OnTimedEvent();
		}

		// Token: 0x06004D16 RID: 19734
		public abstract void OnTimedEvent();

		// Token: 0x06004D17 RID: 19735 RVA: 0x000629CD File Offset: 0x00060BCD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITickSystemPre.PreTick()
		{
			base.CheckCooldown();
		}

		// Token: 0x04004E91 RID: 20113
		[NonSerialized]
		internal bool registered;
	}
}
