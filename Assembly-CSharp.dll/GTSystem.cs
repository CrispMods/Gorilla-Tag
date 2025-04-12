using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005DB RID: 1499
[DisallowMultipleComponent]
public abstract class GTSystem<T> : MonoBehaviour, IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : MonoBehaviour
{
	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06002529 RID: 9513 RVA: 0x0004830B File Offset: 0x0004650B
	public PhotonView photonView
	{
		get
		{
			return this._photonView;
		}
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x00048313 File Offset: 0x00046513
	protected virtual void Awake()
	{
		GTSystem<T>.SetSingleton(this);
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x001035E0 File Offset: 0x001017E0
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

	// Token: 0x0600252C RID: 9516 RVA: 0x0004831B File Offset: 0x0004651B
	protected virtual void OnApplicationQuit()
	{
		GTSystem<T>.gAppQuitting = true;
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnTick(float dt, T instance)
	{
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x0010362C File Offset: 0x0010182C
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

	// Token: 0x0600252F RID: 9519 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnRegister(T instance)
	{
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x00103690 File Offset: 0x00101890
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

	// Token: 0x06002531 RID: 9521 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnUnregister(T instance)
	{
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x00048323 File Offset: 0x00046523
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return ((IEnumerable<!0>)this._instances).GetEnumerator();
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x00048323 File Offset: 0x00046523
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<!0>)this._instances).GetEnumerator();
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06002534 RID: 9524 RVA: 0x00048330 File Offset: 0x00046530
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return this._instances.Count;
		}
	}

	// Token: 0x170003D6 RID: 982
	T IReadOnlyList<!0>.this[int index]
	{
		get
		{
			return this._instances[index];
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06002536 RID: 9526 RVA: 0x0004834B File Offset: 0x0004654B
	public static PhotonView PhotonView
	{
		get
		{
			return GTSystem<T>.gSingleton._photonView;
		}
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x001036F4 File Offset: 0x001018F4
	protected static void SetSingleton(GTSystem<T> system)
	{
		if (GTSystem<T>.gAppQuitting)
		{
			return;
		}
		if (GTSystem<T>.gSingleton != null && GTSystem<T>.gSingleton != system)
		{
			UnityEngine.Object.Destroy(system);
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

	// Token: 0x06002538 RID: 9528 RVA: 0x001037E4 File Offset: 0x001019E4
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

	// Token: 0x06002539 RID: 9529 RVA: 0x00103850 File Offset: 0x00101A50
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

	// Token: 0x04002953 RID: 10579
	[SerializeField]
	protected List<T> _instances = new List<T>();

	// Token: 0x04002954 RID: 10580
	[SerializeField]
	private bool _networked;

	// Token: 0x04002955 RID: 10581
	[SerializeField]
	private PhotonView _photonView;

	// Token: 0x04002956 RID: 10582
	private static GTSystem<T> gSingleton;

	// Token: 0x04002957 RID: 10583
	private static bool gInitializing = false;

	// Token: 0x04002958 RID: 10584
	private static bool gAppQuitting = false;

	// Token: 0x04002959 RID: 10585
	private static HashSet<T> gQueueRegister = new HashSet<T>();
}
