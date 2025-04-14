using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005DA RID: 1498
[DisallowMultipleComponent]
public abstract class GTSystem<T> : MonoBehaviour, IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : MonoBehaviour
{
	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06002521 RID: 9505 RVA: 0x000B83A2 File Offset: 0x000B65A2
	public PhotonView photonView
	{
		get
		{
			return this._photonView;
		}
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x000B83AA File Offset: 0x000B65AA
	protected virtual void Awake()
	{
		GTSystem<T>.SetSingleton(this);
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x000B83B4 File Offset: 0x000B65B4
	protected virtual void Tick()
	{
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < this._instances.Count; i++)
		{
			T t = this._instances[i];
			if (t)
			{
				this.OnTick(deltaTime, t);
			}
		}
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x000B83FF File Offset: 0x000B65FF
	protected virtual void OnApplicationQuit()
	{
		GTSystem<T>.gAppQuitting = true;
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnTick(float dt, T instance)
	{
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000B8408 File Offset: 0x000B6608
	private bool RegisterInstance(T instance)
	{
		if (instance == null)
		{
			GTDev.LogError<string>("[" + base.GetType().Name + "::Register] Instance is null.", null);
			return false;
		}
		if (this._instances.Contains(instance))
		{
			return false;
		}
		this._instances.Add(instance);
		this.OnRegister(instance);
		return true;
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnRegister(T instance)
	{
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000B846C File Offset: 0x000B666C
	private bool UnregisterInstance(T instance)
	{
		if (instance == null)
		{
			GTDev.LogError<string>("[" + base.GetType().Name + "::Unregister] Instance is null.", null);
			return false;
		}
		if (!this._instances.Contains(instance))
		{
			return false;
		}
		this._instances.Remove(instance);
		this.OnUnregister(instance);
		return true;
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnUnregister(T instance)
	{
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x000B84CE File Offset: 0x000B66CE
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return ((IEnumerable<!0>)this._instances).GetEnumerator();
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x000B84CE File Offset: 0x000B66CE
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<!0>)this._instances).GetEnumerator();
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x0600252C RID: 9516 RVA: 0x000B84DB File Offset: 0x000B66DB
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return this._instances.Count;
		}
	}

	// Token: 0x170003D5 RID: 981
	T IReadOnlyList<!0>.this[int index]
	{
		get
		{
			return this._instances[index];
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x0600252E RID: 9518 RVA: 0x000B84F6 File Offset: 0x000B66F6
	public static PhotonView PhotonView
	{
		get
		{
			return GTSystem<T>.gSingleton._photonView;
		}
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000B8504 File Offset: 0x000B6704
	protected static void SetSingleton(GTSystem<T> system)
	{
		if (GTSystem<T>.gAppQuitting)
		{
			return;
		}
		if (GTSystem<T>.gSingleton != null && GTSystem<T>.gSingleton != system)
		{
			Object.Destroy(system);
			GTDev.LogWarning<string>("Singleton of type " + GTSystem<T>.gSingleton.GetType().Name + " already exists.", null);
			return;
		}
		GTSystem<T>.gSingleton = system;
		if (!GTSystem<T>.gInitializing)
		{
			return;
		}
		GTSystem<T>.gSingleton._instances.Clear();
		T[] collection = (from x in GTSystem<T>.gQueueRegister
		where x != null
		select x).ToArray<T>();
		GTSystem<T>.gSingleton._instances.AddRange(collection);
		GTSystem<T>.gQueueRegister.Clear();
		PhotonView component = GTSystem<T>.gSingleton.GetComponent<PhotonView>();
		if (component != null)
		{
			GTSystem<T>.gSingleton._photonView = component;
			GTSystem<T>.gSingleton._networked = true;
		}
		GTSystem<T>.gInitializing = false;
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000B85F4 File Offset: 0x000B67F4
	public static void Register(T instance)
	{
		if (GTSystem<T>.gAppQuitting)
		{
			return;
		}
		if (instance == null)
		{
			return;
		}
		if (GTSystem<T>.gInitializing)
		{
			GTSystem<T>.gQueueRegister.Add(instance);
			return;
		}
		if (GTSystem<T>.gSingleton == null && !GTSystem<T>.gInitializing)
		{
			GTSystem<T>.gInitializing = true;
			GTSystem<T>.gQueueRegister.Add(instance);
			return;
		}
		GTSystem<T>.gSingleton.RegisterInstance(instance);
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000B8660 File Offset: 0x000B6860
	public static void Unregister(T instance)
	{
		if (GTSystem<T>.gAppQuitting)
		{
			return;
		}
		if (instance == null)
		{
			return;
		}
		if (GTSystem<T>.gInitializing)
		{
			GTSystem<T>.gQueueRegister.Remove(instance);
			return;
		}
		if (GTSystem<T>.gSingleton == null && !GTSystem<T>.gInitializing)
		{
			GTSystem<T>.gInitializing = true;
			GTSystem<T>.gQueueRegister.Remove(instance);
			return;
		}
		GTSystem<T>.gSingleton.UnregisterInstance(instance);
	}

	// Token: 0x0400294D RID: 10573
	[SerializeField]
	protected List<T> _instances = new List<T>();

	// Token: 0x0400294E RID: 10574
	[SerializeField]
	private bool _networked;

	// Token: 0x0400294F RID: 10575
	[SerializeField]
	private PhotonView _photonView;

	// Token: 0x04002950 RID: 10576
	private static GTSystem<T> gSingleton;

	// Token: 0x04002951 RID: 10577
	private static bool gInitializing = false;

	// Token: 0x04002952 RID: 10578
	private static bool gAppQuitting = false;

	// Token: 0x04002953 RID: 10579
	private static HashSet<T> gQueueRegister = new HashSet<T>();
}
