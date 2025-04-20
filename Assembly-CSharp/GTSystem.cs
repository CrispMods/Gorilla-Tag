using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
[DisallowMultipleComponent]
public abstract class GTSystem<T> : MonoBehaviour, IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : MonoBehaviour
{
	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06002583 RID: 9603 RVA: 0x000496E2 File Offset: 0x000478E2
	public PhotonView photonView
	{
		get
		{
			return this._photonView;
		}
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x000496EA File Offset: 0x000478EA
	protected virtual void Awake()
	{
		GTSystem<T>.SetSingleton(this);
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x0010651C File Offset: 0x0010471C
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

	// Token: 0x06002586 RID: 9606 RVA: 0x000496F2 File Offset: 0x000478F2
	protected virtual void OnApplicationQuit()
	{
		GTSystem<T>.gAppQuitting = true;
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnTick(float dt, T instance)
	{
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x00106568 File Offset: 0x00104768
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

	// Token: 0x06002589 RID: 9609 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnRegister(T instance)
	{
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x001065CC File Offset: 0x001047CC
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

	// Token: 0x0600258B RID: 9611 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnUnregister(T instance)
	{
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x000496FA File Offset: 0x000478FA
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return ((IEnumerable<!0>)this._instances).GetEnumerator();
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x000496FA File Offset: 0x000478FA
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<!0>)this._instances).GetEnumerator();
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x0600258E RID: 9614 RVA: 0x00049707 File Offset: 0x00047907
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return this._instances.Count;
		}
	}

	// Token: 0x170003DD RID: 989
	T IReadOnlyList<!0>.this[int index]
	{
		get
		{
			return this._instances[index];
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06002590 RID: 9616 RVA: 0x00049722 File Offset: 0x00047922
	public static PhotonView PhotonView
	{
		get
		{
			return GTSystem<T>.gSingleton._photonView;
		}
	}

	// Token: 0x06002591 RID: 9617 RVA: 0x00106630 File Offset: 0x00104830
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

	// Token: 0x06002592 RID: 9618 RVA: 0x00106720 File Offset: 0x00104920
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

	// Token: 0x06002593 RID: 9619 RVA: 0x0010678C File Offset: 0x0010498C
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

	// Token: 0x040029AC RID: 10668
	[SerializeField]
	protected List<T> _instances = new List<T>();

	// Token: 0x040029AD RID: 10669
	[SerializeField]
	private bool _networked;

	// Token: 0x040029AE RID: 10670
	[SerializeField]
	private PhotonView _photonView;

	// Token: 0x040029AF RID: 10671
	private static GTSystem<T> gSingleton;

	// Token: 0x040029B0 RID: 10672
	private static bool gInitializing = false;

	// Token: 0x040029B1 RID: 10673
	private static bool gAppQuitting = false;

	// Token: 0x040029B2 RID: 10674
	private static HashSet<T> gQueueRegister = new HashSet<T>();
}
