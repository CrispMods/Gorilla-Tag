using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BB7 RID: 2999
	internal abstract class TickSystemTimerAbstract : CoolDownHelper, ITickSystemPre
	{
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06004BD1 RID: 19409 RVA: 0x00060FE5 File Offset: 0x0005F1E5
		// (set) Token: 0x06004BD2 RID: 19410 RVA: 0x00060FED File Offset: 0x0005F1ED
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

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06004BD3 RID: 19411 RVA: 0x00060FE5 File Offset: 0x0005F1E5
		public bool Running
		{
			get
			{
				return this.registered;
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x00060FF6 File Offset: 0x0005F1F6
		protected TickSystemTimerAbstract()
		{
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x00060FFE File Offset: 0x0005F1FE
		protected TickSystemTimerAbstract(float cd) : base(cd)
		{
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x00061007 File Offset: 0x0005F207
		public override void Start()
		{
			base.Start();
			TickSystem<object>.AddPreTickCallback(this);
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00061015 File Offset: 0x0005F215
		public override void Stop()
		{
			base.Stop();
			TickSystem<object>.RemovePreTickCallback(this);
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00061023 File Offset: 0x0005F223
		public override void OnCheckPass()
		{
			this.OnTimedEvent();
		}

		// Token: 0x06004BD9 RID: 19417
		public abstract void OnTimedEvent();

		// Token: 0x06004BDA RID: 19418 RVA: 0x0006102B File Offset: 0x0005F22B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITickSystemPre.PreTick()
		{
			base.CheckCooldown();
		}

		// Token: 0x04004DAD RID: 19885
		[NonSerialized]
		internal bool registered;
	}
}
