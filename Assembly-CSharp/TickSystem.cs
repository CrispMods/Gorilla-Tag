using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaTag;
using UnityEngine;

// Token: 0x0200080B RID: 2059
internal abstract class TickSystem<T> : MonoBehaviour
{
	// Token: 0x060032A9 RID: 12969 RVA: 0x000F3611 File Offset: 0x000F1811
	private void Awake()
	{
		base.transform.SetParent(null, true);
		Object.DontDestroyOnLoad(this);
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x000F3626 File Offset: 0x000F1826
	private void Update()
	{
		TickSystem<T>.preTickCallbacks.TryRunCallbacks();
		TickSystem<T>.tickCallbacks.TryRunCallbacks();
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x000F363C File Offset: 0x000F183C
	private void LateUpdate()
	{
		TickSystem<T>.postTickCallbacks.TryRunCallbacks();
	}

	// Token: 0x060032AC RID: 12972 RVA: 0x000F3648 File Offset: 0x000F1848
	static TickSystem()
	{
		TickSystem<T>.preTickWrapperPool = new ObjectPool<TickSystem<T>.TickCallbackWrapperPre>(100);
		TickSystem<T>.tickWrapperPool = new ObjectPool<TickSystem<T>.TickCallbackWrapperTick>(100);
		TickSystem<T>.postTickWrapperPool = new ObjectPool<TickSystem<T>.TickCallbackWrapperPost>(100);
	}

	// Token: 0x060032AD RID: 12973 RVA: 0x000F36BB File Offset: 0x000F18BB
	private static void OnEnterPlay()
	{
		TickSystem<T>.preTickCallbacks.Clear();
		TickSystem<T>.preTickWrapperTable.Clear();
		TickSystem<T>.tickCallbacks.Clear();
		TickSystem<T>.tickWrapperTable.Clear();
		TickSystem<T>.postTickCallbacks.Clear();
		TickSystem<T>.postTickWrapperTable.Clear();
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x000F36FC File Offset: 0x000F18FC
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

	// Token: 0x060032AF RID: 12975 RVA: 0x000F3744 File Offset: 0x000F1944
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

	// Token: 0x060032B0 RID: 12976 RVA: 0x000F378C File Offset: 0x000F198C
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

	// Token: 0x060032B1 RID: 12977 RVA: 0x000F37D2 File Offset: 0x000F19D2
	public static void AddTickSystemCallBack(ITickSystem callback)
	{
		TickSystem<T>.AddPreTickCallback(callback);
		TickSystem<T>.AddTickCallback(callback);
		TickSystem<T>.AddPostTickCallback(callback);
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x000F37E8 File Offset: 0x000F19E8
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

	// Token: 0x060032B3 RID: 12979 RVA: 0x000F3838 File Offset: 0x000F1A38
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

	// Token: 0x060032B4 RID: 12980 RVA: 0x000F387C File Offset: 0x000F1A7C
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

	// Token: 0x060032B5 RID: 12981 RVA: 0x000F38C0 File Offset: 0x000F1AC0
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

	// Token: 0x060032B6 RID: 12982 RVA: 0x000F3902 File Offset: 0x000F1B02
	public static void RemoveTickSystemCallback(ITickSystem callback)
	{
		TickSystem<T>.RemovePreTickCallback(callback);
		TickSystem<T>.RemoveTickCallback(callback);
		TickSystem<T>.RemovePostTickCallback(callback);
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x000F3918 File Offset: 0x000F1B18
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

	// Token: 0x04003629 RID: 13865
	private static readonly ObjectPool<TickSystem<T>.TickCallbackWrapperPre> preTickWrapperPool;

	// Token: 0x0400362A RID: 13866
	private static readonly CallbackContainer<TickSystem<T>.TickCallbackWrapperPre> preTickCallbacks = new CallbackContainer<TickSystem<T>.TickCallbackWrapperPre>();

	// Token: 0x0400362B RID: 13867
	private static readonly Dictionary<ITickSystemPre, TickSystem<T>.TickCallbackWrapperPre> preTickWrapperTable = new Dictionary<ITickSystemPre, TickSystem<T>.TickCallbackWrapperPre>(100);

	// Token: 0x0400362C RID: 13868
	private static readonly ObjectPool<TickSystem<T>.TickCallbackWrapperTick> tickWrapperPool;

	// Token: 0x0400362D RID: 13869
	private static readonly CallbackContainer<TickSystem<T>.TickCallbackWrapperTick> tickCallbacks = new CallbackContainer<TickSystem<T>.TickCallbackWrapperTick>();

	// Token: 0x0400362E RID: 13870
	private static readonly Dictionary<ITickSystemTick, TickSystem<T>.TickCallbackWrapperTick> tickWrapperTable = new Dictionary<ITickSystemTick, TickSystem<T>.TickCallbackWrapperTick>(100);

	// Token: 0x0400362F RID: 13871
	private static readonly ObjectPool<TickSystem<T>.TickCallbackWrapperPost> postTickWrapperPool;

	// Token: 0x04003630 RID: 13872
	private static readonly CallbackContainer<TickSystem<T>.TickCallbackWrapperPost> postTickCallbacks = new CallbackContainer<TickSystem<T>.TickCallbackWrapperPost>();

	// Token: 0x04003631 RID: 13873
	private static readonly Dictionary<ITickSystemPost, TickSystem<T>.TickCallbackWrapperPost> postTickWrapperTable = new Dictionary<ITickSystemPost, TickSystem<T>.TickCallbackWrapperPost>(100);

	// Token: 0x0200080C RID: 2060
	private class TickCallbackWrapper<U> : ObjectPoolEvents, ICallBack where U : class
	{
		// Token: 0x060032B9 RID: 12985 RVA: 0x000023F4 File Offset: 0x000005F4
		public virtual void CallBack()
		{
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnTaken()
		{
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000F3966 File Offset: 0x000F1B66
		public void OnReturned()
		{
			this.target = default(U);
		}

		// Token: 0x04003632 RID: 13874
		public U target;
	}

	// Token: 0x0200080D RID: 2061
	private class TickCallbackWrapperPre : TickSystem<T>.TickCallbackWrapper<ITickSystemPre>
	{
		// Token: 0x060032BD RID: 12989 RVA: 0x000F3974 File Offset: 0x000F1B74
		public override void CallBack()
		{
			this.target.PreTick();
		}
	}

	// Token: 0x0200080E RID: 2062
	private class TickCallbackWrapperTick : TickSystem<T>.TickCallbackWrapper<ITickSystemTick>
	{
		// Token: 0x060032BF RID: 12991 RVA: 0x000F3989 File Offset: 0x000F1B89
		public override void CallBack()
		{
			this.target.Tick();
		}
	}

	// Token: 0x0200080F RID: 2063
	private class TickCallbackWrapperPost : TickSystem<T>.TickCallbackWrapper<ITickSystemPost>
	{
		// Token: 0x060032C1 RID: 12993 RVA: 0x000F399E File Offset: 0x000F1B9E
		public override void CallBack()
		{
			this.target.PostTick();
		}
	}
}
