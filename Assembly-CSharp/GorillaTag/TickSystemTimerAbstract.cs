using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BB4 RID: 2996
	internal abstract class TickSystemTimerAbstract : CoolDownHelper, ITickSystemPre
	{
		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06004BC5 RID: 19397 RVA: 0x00170FF7 File Offset: 0x0016F1F7
		// (set) Token: 0x06004BC6 RID: 19398 RVA: 0x00170FFF File Offset: 0x0016F1FF
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

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06004BC7 RID: 19399 RVA: 0x00170FF7 File Offset: 0x0016F1F7
		public bool Running
		{
			get
			{
				return this.registered;
			}
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x00171008 File Offset: 0x0016F208
		protected TickSystemTimerAbstract()
		{
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x00171010 File Offset: 0x0016F210
		protected TickSystemTimerAbstract(float cd) : base(cd)
		{
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x00171019 File Offset: 0x0016F219
		public override void Start()
		{
			base.Start();
			TickSystem<object>.AddPreTickCallback(this);
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x00171027 File Offset: 0x0016F227
		public override void Stop()
		{
			base.Stop();
			TickSystem<object>.RemovePreTickCallback(this);
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x00171035 File Offset: 0x0016F235
		public override void OnCheckPass()
		{
			this.OnTimedEvent();
		}

		// Token: 0x06004BCD RID: 19405
		public abstract void OnTimedEvent();

		// Token: 0x06004BCE RID: 19406 RVA: 0x0017103D File Offset: 0x0016F23D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITickSystemPre.PreTick()
		{
			base.CheckCooldown();
		}

		// Token: 0x04004D9B RID: 19867
		[NonSerialized]
		internal bool registered;
	}
}
