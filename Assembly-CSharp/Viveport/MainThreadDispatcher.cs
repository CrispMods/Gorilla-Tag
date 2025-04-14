using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viveport
{
	// Token: 0x020008F6 RID: 2294
	public class MainThreadDispatcher : MonoBehaviour
	{
		// Token: 0x0600371E RID: 14110 RVA: 0x00104F49 File Offset: 0x00103149
		private void Awake()
		{
			if (MainThreadDispatcher.instance == null)
			{
				MainThreadDispatcher.instance = this;
				Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x00104F6C File Offset: 0x0010316C
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

		// Token: 0x06003720 RID: 14112 RVA: 0x00104FC4 File Offset: 0x001031C4
		public static MainThreadDispatcher Instance()
		{
			if (MainThreadDispatcher.instance == null)
			{
				throw new Exception("Could not find the MainThreadDispatcher GameObject. Please ensure you have added this script to an empty GameObject in your scene.");
			}
			return MainThreadDispatcher.instance;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x00104FE3 File Offset: 0x001031E3
		private void OnDestroy()
		{
			MainThreadDispatcher.instance = null;
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x00104FEC File Offset: 0x001031EC
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

		// Token: 0x06003723 RID: 14115 RVA: 0x00105050 File Offset: 0x00103250
		public void Enqueue(Action action)
		{
			this.Enqueue(this.ActionWrapper(action));
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x0010505F File Offset: 0x0010325F
		public void Enqueue<T1>(Action<T1> action, T1 param1)
		{
			this.Enqueue(this.ActionWrapper<T1>(action, param1));
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0010506F File Offset: 0x0010326F
		public void Enqueue<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
		{
			this.Enqueue(this.ActionWrapper<T1, T2>(action, param1, param2));
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x00105080 File Offset: 0x00103280
		public void Enqueue<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
		{
			this.Enqueue(this.ActionWrapper<T1, T2, T3>(action, param1, param2, param3));
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x00105093 File Offset: 0x00103293
		public void Enqueue<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			this.Enqueue(this.ActionWrapper<T1, T2, T3, T4>(action, param1, param2, param3, param4));
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x001050A8 File Offset: 0x001032A8
		private IEnumerator ActionWrapper(Action action)
		{
			action();
			yield return null;
			yield break;
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x001050B7 File Offset: 0x001032B7
		private IEnumerator ActionWrapper<T1>(Action<T1> action, T1 param1)
		{
			action(param1);
			yield return null;
			yield break;
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x001050CD File Offset: 0x001032CD
		private IEnumerator ActionWrapper<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
		{
			action(param1, param2);
			yield return null;
			yield break;
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x001050EA File Offset: 0x001032EA
		private IEnumerator ActionWrapper<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
		{
			action(param1, param2, param3);
			yield return null;
			yield break;
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x0010510F File Offset: 0x0010330F
		private IEnumerator ActionWrapper<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			action(param1, param2, param3, param4);
			yield return null;
			yield break;
		}

		// Token: 0x04003A1F RID: 14879
		private static readonly Queue<Action> actions = new Queue<Action>();

		// Token: 0x04003A20 RID: 14880
		private static MainThreadDispatcher instance = null;
	}
}
