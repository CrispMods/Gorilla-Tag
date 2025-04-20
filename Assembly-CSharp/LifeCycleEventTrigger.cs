using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006D6 RID: 1750
public class LifeCycleEventTrigger : MonoBehaviour
{
	// Token: 0x06002B67 RID: 11111 RVA: 0x0004D476 File Offset: 0x0004B676
	private void Awake()
	{
		UnityEvent onAwake = this._onAwake;
		if (onAwake == null)
		{
			return;
		}
		onAwake.Invoke();
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x0004D488 File Offset: 0x0004B688
	private void Start()
	{
		UnityEvent onStart = this._onStart;
		if (onStart == null)
		{
			return;
		}
		onStart.Invoke();
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x0004D49A File Offset: 0x0004B69A
	private void OnEnable()
	{
		UnityEvent onEnable = this._onEnable;
		if (onEnable == null)
		{
			return;
		}
		onEnable.Invoke();
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x0004D4AC File Offset: 0x0004B6AC
	private void OnDisable()
	{
		UnityEvent onDisable = this._onDisable;
		if (onDisable == null)
		{
			return;
		}
		onDisable.Invoke();
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x0004D4BE File Offset: 0x0004B6BE
	private void OnDestroy()
	{
		UnityEvent onDestroy = this._onDestroy;
		if (onDestroy == null)
		{
			return;
		}
		onDestroy.Invoke();
	}

	// Token: 0x040030CE RID: 12494
	[SerializeField]
	private UnityEvent _onAwake;

	// Token: 0x040030CF RID: 12495
	[SerializeField]
	private UnityEvent _onStart;

	// Token: 0x040030D0 RID: 12496
	[SerializeField]
	private UnityEvent _onEnable;

	// Token: 0x040030D1 RID: 12497
	[SerializeField]
	private UnityEvent _onDisable;

	// Token: 0x040030D2 RID: 12498
	[SerializeField]
	private UnityEvent _onDestroy;
}
