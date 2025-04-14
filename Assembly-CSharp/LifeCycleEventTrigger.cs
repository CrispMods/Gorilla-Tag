using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006C1 RID: 1729
public class LifeCycleEventTrigger : MonoBehaviour
{
	// Token: 0x06002AD1 RID: 10961 RVA: 0x000D466F File Offset: 0x000D286F
	private void Awake()
	{
		UnityEvent onAwake = this._onAwake;
		if (onAwake == null)
		{
			return;
		}
		onAwake.Invoke();
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x000D4681 File Offset: 0x000D2881
	private void Start()
	{
		UnityEvent onStart = this._onStart;
		if (onStart == null)
		{
			return;
		}
		onStart.Invoke();
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x000D4693 File Offset: 0x000D2893
	private void OnEnable()
	{
		UnityEvent onEnable = this._onEnable;
		if (onEnable == null)
		{
			return;
		}
		onEnable.Invoke();
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x000D46A5 File Offset: 0x000D28A5
	private void OnDisable()
	{
		UnityEvent onDisable = this._onDisable;
		if (onDisable == null)
		{
			return;
		}
		onDisable.Invoke();
	}

	// Token: 0x06002AD5 RID: 10965 RVA: 0x000D46B7 File Offset: 0x000D28B7
	private void OnDestroy()
	{
		UnityEvent onDestroy = this._onDestroy;
		if (onDestroy == null)
		{
			return;
		}
		onDestroy.Invoke();
	}

	// Token: 0x04003031 RID: 12337
	[SerializeField]
	private UnityEvent _onAwake;

	// Token: 0x04003032 RID: 12338
	[SerializeField]
	private UnityEvent _onStart;

	// Token: 0x04003033 RID: 12339
	[SerializeField]
	private UnityEvent _onEnable;

	// Token: 0x04003034 RID: 12340
	[SerializeField]
	private UnityEvent _onDisable;

	// Token: 0x04003035 RID: 12341
	[SerializeField]
	private UnityEvent _onDestroy;
}
