using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006C2 RID: 1730
public class LifeCycleEventTrigger : MonoBehaviour
{
	// Token: 0x06002AD9 RID: 10969 RVA: 0x0004C131 File Offset: 0x0004A331
	private void Awake()
	{
		UnityEvent onAwake = this._onAwake;
		if (onAwake == null)
		{
			return;
		}
		onAwake.Invoke();
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x0004C143 File Offset: 0x0004A343
	private void Start()
	{
		UnityEvent onStart = this._onStart;
		if (onStart == null)
		{
			return;
		}
		onStart.Invoke();
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x0004C155 File Offset: 0x0004A355
	private void OnEnable()
	{
		UnityEvent onEnable = this._onEnable;
		if (onEnable == null)
		{
			return;
		}
		onEnable.Invoke();
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x0004C167 File Offset: 0x0004A367
	private void OnDisable()
	{
		UnityEvent onDisable = this._onDisable;
		if (onDisable == null)
		{
			return;
		}
		onDisable.Invoke();
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x0004C179 File Offset: 0x0004A379
	private void OnDestroy()
	{
		UnityEvent onDestroy = this._onDestroy;
		if (onDestroy == null)
		{
			return;
		}
		onDestroy.Invoke();
	}

	// Token: 0x04003037 RID: 12343
	[SerializeField]
	private UnityEvent _onAwake;

	// Token: 0x04003038 RID: 12344
	[SerializeField]
	private UnityEvent _onStart;

	// Token: 0x04003039 RID: 12345
	[SerializeField]
	private UnityEvent _onEnable;

	// Token: 0x0400303A RID: 12346
	[SerializeField]
	private UnityEvent _onDisable;

	// Token: 0x0400303B RID: 12347
	[SerializeField]
	private UnityEvent _onDestroy;
}
