using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viveport
{
	// Token: 0x02000913 RID: 2323
	public class MainThreadDispatcher : MonoBehaviour
	{
		// Token: 0x060037EF RID: 14319 RVA: 0x00054FE9 File Offset: 0x000531E9
		private void Awake()
		{
			if (MainThreadDispatcher.instance == null)
			{
				MainThreadDispatcher.instance = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x00149D98 File Offset: 0x00147F98
		public void Update()
		{
			Queue<Action> obj = MainThreadDispatcher.actions;
			lock (obj)
			{
				while (MainThreadDispatcher.actions.Count > 0)
				{
					MainThreadDispatcher.actions.Dequeue()();
				}
			}
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x00055009 File Offset: 0x00053209
		public static MainThreadDispatcher Instance()
		{
			if (MainThreadDispatcher.instance == null)
			{
				throw new Exception("Could not find the MainThreadDispatcher GameObject. Please ensure you have added this script to an empty GameObject in your scene.");
			}
			return MainThreadDispatcher.instance;
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x00055028 File Offset: 0x00053228
		private void OnDestroy()
		{
			MainThreadDispatcher.instance = null;
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x00149DF0 File Offset: 0x00147FF0
		public void Enqueue(IEnumerator action)
		{
			Queue<Action> obj = MainThreadDispatcher.actions;
			lock (obj)
			{
				MainThreadDispatcher.actions.Enqueue(delegate
				{
					this.StartCoroutine(action);
				});
			}
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x00055030 File Offset: 0x00053230
		public void Enqueue(Action action)
		{
			this.Enqueue(this.ActionWrapper(action));
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x0005503F File Offset: 0x0005323F
		public void Enqueue<T1>(Action<T1> action, T1 param1)
		{
			this.Enqueue(this.ActionWrapper<T1>(action, param1));
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x0005504F File Offset: 0x0005324F
		public void Enqueue<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
		{
			this.Enqueue(this.ActionWrapper<T1, T2>(action, param1, param2));
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x00055060 File Offset: 0x00053260
		public void Enqueue<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
		{
			this.Enqueue(this.ActionWrapper<T1, T2, T3>(action, param1, param2, param3));
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x00055073 File Offset: 0x00053273
		public void Enqueue<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			this.Enqueue(this.ActionWrapper<T1, T2, T3, T4>(action, param1, param2, param3, param4));
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x00055088 File Offset: 0x00053288
		private IEnumerator ActionWrapper(Action action)
		{
			action();
			yield return null;
			yield break;
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x00055097 File Offset: 0x00053297
		private IEnumerator ActionWrapper<T1>(Action<T1> action, T1 param1)
		{
			action(param1);
			yield return null;
			yield break;
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x000550AD File Offset: 0x000532AD
		private IEnumerator ActionWrapper<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
		{
			action(param1, param2);
			yield return null;
			yield break;
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x000550CA File Offset: 0x000532CA
		private IEnumerator ActionWrapper<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
		{
			action(param1, param2, param3);
			yield return null;
			yield break;
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x000550EF File Offset: 0x000532EF
		private IEnumerator ActionWrapper<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			action(param1, param2, param3, param4);
			yield return null;
			yield break;
		}

		// Token: 0x04003AE4 RID: 15076
		private static readonly Queue<Action> actions = new Queue<Action>();

		// Token: 0x04003AE5 RID: 15077
		private static MainThreadDispatcher instance = null;
	}
}
