using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BB7 RID: 2999
	internal abstract class TickSystemTimerAbstract : CoolDownHelper, ITickSystemPre
	{
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06004BD1 RID: 19409 RVA: 0x001715BF File Offset: 0x0016F7BF
		// (set) Token: 0x06004BD2 RID: 19410 RVA: 0x001715C7 File Offset: 0x0016F7C7
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
		// (get) Token: 0x06004BD3 RID: 19411 RVA: 0x001715BF File Offset: 0x0016F7BF
		public bool Running
		{
			get
			{
				return this.registered;
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x001715D0 File Offset: 0x0016F7D0
		protected TickSystemTimerAbstract()
		{
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x001715D8 File Offset: 0x0016F7D8
		protected TickSystemTimerAbstract(float cd) : base(cd)
		{
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x001715E1 File Offset: 0x0016F7E1
		public override void Start()
		{
			base.Start();
			TickSystem<object>.AddPreTickCallback(this);
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x001715EF File Offset: 0x0016F7EF
		public override void Stop()
		{
			base.Stop();
			TickSystem<object>.RemovePreTickCallback(this);
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x001715FD File Offset: 0x0016F7FD
		public override void OnCheckPass()
		{
			this.OnTimedEvent();
		}

		// Token: 0x06004BD9 RID: 19417
		public abstract void OnTimedEvent();

		// Token: 0x06004BDA RID: 19418 RVA: 0x00171605 File Offset: 0x0016F805
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
