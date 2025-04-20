using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaTag;
using UnityEngine;

// Token: 0x02000825 RID: 2085
internal abstract class TickSystem<T> : MonoBehaviour
{
	// Token: 0x06003364 RID: 13156 RVA: 0x00051E17 File Offset: 0x00050017
	private void Awake()
	{
		base.transform.SetParent(null, true);
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x00051E2C File Offset: 0x0005002C
	private void Update()
	{
		TickSystem<T>.preTickCallbacks.TryRunCallbacks();
		TickSystem<T>.tickCallbacks.TryRunCallbacks();
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x00051E42 File Offset: 0x00050042
	private void LateUpdate()
	{
		TickSystem<T>.postTickCallbacks.TryRunCallbacks();
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x0013B740 File Offset: 0x00139940
	static TickSystem()
	{
		TickSystem<T>.preTickWrapperPool = new ObjectPool<TickSystem<T>.TickCallbackWrapperPre>(100);
		TickSystem<T>.tickWrapperPool = new ObjectPool<TickSystem<T>.TickCallbackWrapperTick>(100);
		TickSystem<T>.postTickWrapperPool = new ObjectPool<TickSystem<T>.TickCallbackWrapperPost>(100);
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x00051E4E File Offset: 0x0005004E
	private static void OnEnterPlay()
	{
		TickSystem<T>.preTickCallbacks.Clear();
		TickSystem<T>.preTickWrapperTable.Clear();
		TickSystem<T>.tickCallbacks.Clear();
		TickSystem<T>.tickWrapperTable.Clear();
		TickSystem<T>.postTickCallbacks.Clear();
		TickSystem<T>.postTickWrapperTable.Clear();
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x0013B7B4 File Offset: 0x001399B4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AddPreTickCallback(ITickSystemPre callback)
	{
		if (callback.PreTickRunning)
		{
			return;
		}
		TickSystem<T>.TickCallbackWrapperPre tickCallbackWrapperPre = TickSystem<T>.preTickWrapperPool.Take();
		tickCallbackWrapperPre.target = callback;
		TickSystem<T>.preTickWrapperTable[callback] = tickCallbackWrapperPre;
		TickSystem<T>.preTickCallbacks.Add(tickCallbackWrapperPre);
		callback.PreTickRunning = true;
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x0013B7FC File Offset: 0x001399FC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AddTickCallback(ITickSystemTick callback)
	{
		if (callback.TickRunning)
		{
			return;
		}
		TickSystem<T>.TickCallbackWrapperTick tickCallbackWrapperTick = TickSystem<T>.tickWrapperPool.Take();
		tickCallbackWrapperTick.target = callback;
		TickSystem<T>.tickWrapperTable[callback] = tickCallbackWrapperTick;
		TickSystem<T>.tickCallbacks.Add(tickCallbackWrapperTick);
		callback.TickRunning = true;
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x0013B844 File Offset: 0x00139A44
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AddPostTickCallback(ITickSystemPost callback)
	{
		if (callback.PostTickRunning)
		{
			return;
		}
		TickSystem<T>.TickCallbackWrapperPost tickCallbackWrapperPost = TickSystem<T>.postTickWrapperPool.Take();
		tickCallbackWrapperPost.target = callback;
		TickSystem<T>.postTickWrapperTable[callback] = tickCallbackWrapperPost;
		TickSystem<T>.postTickCallbacks.Add(tickCallbackWrapperPost);
		callback.PostTickRunning = true;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x00051E8C File Offset: 0x0005008C
	public static void AddTickSystemCallBack(ITickSystem callback)
	{
		TickSystem<T>.AddPreTickCallback(callback);
		TickSystem<T>.AddTickCallback(callback);
		TickSystem<T>.AddPostTickCallback(callback);
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x0013B88C File Offset: 0x00139A8C
	public static void AddCallbackTarget(object target)
	{
		ITickSystem tickSystem = target as ITickSystem;
		if (tickSystem != null)
		{
			TickSystem<T>.AddTickSystemCallBack(tickSystem);
			return;
		}
		ITickSystemPre tickSystemPre = target as ITickSystemPre;
		if (tickSystemPre != null)
		{
			TickSystem<T>.AddPreTickCallback(tickSystemPre);
		}
		ITickSystemTick tickSystemTick = target as ITickSystemTick;
		if (tickSystemTick != null)
		{
			TickSystem<T>.AddTickCallback(tickSystemTick);
		}
		ITickSystemPost tickSystemPost = target as ITickSystemPost;
		if (tickSystemPost != null)
		{
			TickSystem<T>.AddPostTickCallback(tickSystemPost);
		}
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x0013B8DC File Offset: 0x00139ADC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RemovePreTickCallback(ITickSystemPre callback)
	{
		TickSystem<T>.TickCallbackWrapperPre tickCallbackWrapperPre;
		if (!callback.PreTickRunning || !TickSystem<T>.preTickWrapperTable.TryGetValue(callback, out tickCallbackWrapperPre))
		{
			return;
		}
		TickSystem<T>.preTickCallbacks.Remove(tickCallbackWrapperPre);
		callback.PreTickRunning = false;
		TickSystem<T>.preTickWrapperPool.Return(tickCallbackWrapperPre);
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x0013B920 File Offset: 0x00139B20
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RemoveTickCallback(ITickSystemTick callback)
	{
		TickSystem<T>.TickCallbackWrapperTick tickCallbackWrapperTick;
		if (!callback.TickRunning || !TickSystem<T>.tickWrapperTable.TryGetValue(callback, out tickCallbackWrapperTick))
		{
			return;
		}
		TickSystem<T>.tickCallbacks.Remove(tickCallbackWrapperTick);
		callback.TickRunning = false;
		TickSystem<T>.tickWrapperPool.Return(tickCallbackWrapperTick);
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x0013B964 File Offset: 0x00139B64
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RemovePostTickCallback(ITickSystemPost callback)
	{
		TickSystem<T>.TickCallbackWrapperPost tickCallbackWrapperPost;
		if (!callback.PostTickRunning || !TickSystem<T>.postTickWrapperTable.TryGetValue(callback, out tickCallbackWrapperPost))
		{
			return;
		}
		TickSystem<T>.postTickCallbacks.Remove(tickCallbackWrapperPost);
		callback.PostTickRunning = false;
		TickSystem<T>.postTickWrapperPool.Return(tickCallbackWrapperPost);
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x00051EA0 File Offset: 0x000500A0
	public static void RemoveTickSystemCallback(ITickSystem callback)
	{
		TickSystem<T>.RemovePreTickCallback(callback);
		TickSystem<T>.RemoveTickCallback(callback);
		TickSystem<T>.RemovePostTickCallback(callback);
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x0013B9A8 File Offset: 0x00139BA8
	public static void RemoveCallbackTarget(object target)
	{
		ITickSystem tickSystem = target as ITickSystem;
		if (tickSystem != null)
		{
			TickSystem<T>.RemoveTickSystemCallback(tickSystem);
			return;
		}
		ITickSystemPre tickSystemPre = target as ITickSystemPre;
		if (tickSystemPre != null)
		{
			TickSystem<T>.RemovePreTickCallback(tickSystemPre);
		}
		ITickSystemTick tickSystemTick = target as ITickSystemTick;
		if (tickSystemTick != null)
		{
			TickSystem<T>.RemoveTickCallback(tickSystemTick);
		}
		ITickSystemPost tickSystemPost = target as ITickSystemPost;
		if (tickSystemPost != null)
		{
			TickSystem<T>.RemovePostTickCallback(tickSystemPost);
		}
	}

	// Token: 0x040036E5 RID: 14053
	private static readonly ObjectPool<TickSystem<T>.TickCallbackWrapperPre> preTickWrapperPool;

	// Token: 0x040036E6 RID: 14054
	private static readonly CallbackContainer<TickSystem<T>.TickCallbackWrapperPre> preTickCallbacks = new CallbackContainer<TickSystem<T>.TickCallbackWrapperPre>();

	// Token: 0x040036E7 RID: 14055
	private static readonly Dictionary<ITickSystemPre, TickSystem<T>.TickCallbackWrapperPre> preTickWrapperTable = new Dictionary<ITickSystemPre, TickSystem<T>.TickCallbackWrapperPre>(100);

	// Token: 0x040036E8 RID: 14056
	private static readonly ObjectPool<TickSystem<T>.TickCallbackWrapperTick> tickWrapperPool;

	// Token: 0x040036E9 RID: 14057
	private static readonly CallbackContainer<TickSystem<T>.TickCallbackWrapperTick> tickCallbacks = new CallbackContainer<TickSystem<T>.TickCallbackWrapperTick>();

	// Token: 0x040036EA RID: 14058
	private static readonly Dictionary<ITickSystemTick, TickSystem<T>.TickCallbackWrapperTick> tickWrapperTable = new Dictionary<ITickSystemTick, TickSystem<T>.TickCallbackWrapperTick>(100);

	// Token: 0x040036EB RID: 14059
	private static readonly ObjectPool<TickSystem<T>.TickCallbackWrapperPost> postTickWrapperPool;

	// Token: 0x040036EC RID: 14060
	private static readonly CallbackContainer<TickSystem<T>.TickCallbackWrapperPost> postTickCallbacks = new CallbackContainer<TickSystem<T>.TickCallbackWrapperPost>();

	// Token: 0x040036ED RID: 14061
	private static readonly Dictionary<ITickSystemPost, TickSystem<T>.TickCallbackWrapperPost> postTickWrapperTable = new Dictionary<ITickSystemPost, TickSystem<T>.TickCallbackWrapperPost>(100);

	// Token: 0x02000826 RID: 2086
	private class TickCallbackWrapper<U> : ObjectPoolEvents, ICallBack where U : class
	{
		// Token: 0x06003374 RID: 13172 RVA: 0x00030607 File Offset: 0x0002E807
		public virtual void CallBack()
		{
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnTaken()
		{
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00051EB4 File Offset: 0x000500B4
		public void OnReturned()
		{
			this.target = default(U);
		}

		// Token: 0x040036EE RID: 14062
		public U target;
	}

	// Token: 0x02000827 RID: 2087
	private class TickCallbackWrapperPre : TickSystem<T>.TickCallbackWrapper<ITickSystemPre>
	{
		// Token: 0x06003378 RID: 13176 RVA: 0x00051EC2 File Offset: 0x000500C2
		public override void CallBack()
		{
			this.target.PreTick();
		}
	}

	// Token: 0x02000828 RID: 2088
	private class TickCallbackWrapperTick : TickSystem<T>.TickCallbackWrapper<ITickSystemTick>
	{
		// Token: 0x0600337A RID: 13178 RVA: 0x00051ED7 File Offset: 0x000500D7
		public override void CallBack()
		{
			this.target.Tick();
		}
	}

	// Token: 0x02000829 RID: 2089
	private class TickCallbackWrapperPost : TickSystem<T>.TickCallbackWrapper<ITickSystemPost>
	{
		// Token: 0x0600337C RID: 13180 RVA: 0x00051EEC File Offset: 0x000500EC
		public override void CallBack()
		{
			this.target.PostTick();
		}
	}
}
