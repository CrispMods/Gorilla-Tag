using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viveport
{
	// Token: 0x020008F9 RID: 2297
	public class MainThreadDispatcher : MonoBehaviour
	{
		// Token: 0x0600372A RID: 14122 RVA: 0x00105511 File Offset: 0x00103711
		private void Awake()
		{
			if (MainThreadDispatcher.instance == null)
			{
				MainThreadDispatcher.instance = this;
				Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x00105534 File Offset: 0x00103734
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

		// Token: 0x0600372C RID: 14124 RVA: 0x0010558C File Offset: 0x0010378C
		public static MainThreadDispatcher Instance()
		{
			if (MainThreadDispatcher.instance == null)
			{
				throw new Exception("Could not find the MainThreadDispatcher GameObject. Please ensure you have added this script to an empty GameObject in your scene.");
			}
			return MainThreadDispatcher.instance;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x001055AB File Offset: 0x001037AB
		private void OnDestroy()
		{
			MainThreadDispatcher.instance = null;
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x001055B4 File Offset: 0x001037B4
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

		// Token: 0x0600372F RID: 14127 RVA: 0x00105618 File Offset: 0x00103818
		public void Enqueue(Action action)
		{
			this.Enqueue(this.ActionWrapper(action));
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x00105627 File Offset: 0x00103827
		public void Enqueue<T1>(Action<T1> action, T1 param1)
		{
			this.Enqueue(this.ActionWrapper<T1>(action, param1));
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x00105637 File Offset: 0x00103837
		public void Enqueue<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
		{
			this.Enqueue(this.ActionWrapper<T1, T2>(action, param1, param2));
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x00105648 File Offset: 0x00103848
		public void Enqueue<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
		{
			this.Enqueue(this.ActionWrapper<T1, T2, T3>(action, param1, param2, param3));
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x0010565B File Offset: 0x0010385B
		public void Enqueue<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			this.Enqueue(this.ActionWrapper<T1, T2, T3, T4>(action, param1, param2, param3, param4));
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x00105670 File Offset: 0x00103870
		private IEnumerator ActionWrapper(Action action)
		{
			action();
			yield return null;
			yield break;
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x0010567F File Offset: 0x0010387F
		private IEnumerator ActionWrapper<T1>(Action<T1> action, T1 param1)
		{
			action(param1);
			yield return null;
			yield break;
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x00105695 File Offset: 0x00103895
		private IEnumerator ActionWrapper<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
		{
			action(param1, param2);
			yield return null;
			yield break;
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x001056B2 File Offset: 0x001038B2
		private IEnumerator ActionWrapper<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
		{
			action(param1, param2, param3);
			yield return null;
			yield break;
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x001056D7 File Offset: 0x001038D7
		private IEnumerator ActionWrapper<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			action(param1, param2, param3, param4);
			yield return null;
			yield break;
		}

		// Token: 0x04003A31 RID: 14897
		private static readonly Queue<Action> actions = new Queue<Action>();

		// Token: 0x04003A32 RID: 14898
		private static MainThreadDispatcher instance = null;
	}
}
