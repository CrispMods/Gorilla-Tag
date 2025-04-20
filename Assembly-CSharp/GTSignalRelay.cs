using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005BC RID: 1468
public class GTSignalRelay : MonoBehaviourStatic<GTSignalRelay>, IOnEventCallback
{
	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06002478 RID: 9336 RVA: 0x00048BC7 File Offset: 0x00046DC7
	public static IReadOnlyList<GTSignalListener> ActiveListeners
	{
		get
		{
			return GTSignalRelay.gActiveListeners;
		}
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x00048BCE File Offset: 0x00046DCE
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			PhotonNetwork.AddCallbackTarget(this);
		}
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x00048BDD File Offset: 0x00046DDD
	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			PhotonNetwork.RemoveCallbackTarget(this);
		}
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x001023B0 File Offset: 0x001005B0
	public static void Register(GTSignalListener listener)
	{
		if (listener == null)
		{
			return;
		}
		int num = listener.signal;
		if (num == 0)
		{
			return;
		}
		if (!GTSignalRelay.gListenerSet.Add(listener))
		{
			return;
		}
		GTSignalRelay.gActiveListeners.Add(listener);
		List<GTSignalListener> list;
		if (!GTSignalRelay.gSignalIdToListeners.TryGetValue(num, out list))
		{
			list = new List<GTSignalListener>(64);
			GTSignalRelay.gSignalIdToListeners.Add(num, list);
		}
		list.Add(listener);
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x0010241C File Offset: 0x0010061C
	public static void Unregister(GTSignalListener listener)
	{
		if (listener == null)
		{
			return;
		}
		GTSignalRelay.gListenerSet.Remove(listener);
		GTSignalRelay.gActiveListeners.Remove(listener);
		List<GTSignalListener> list;
		if (GTSignalRelay.gSignalIdToListeners.TryGetValue(listener.signal, out list))
		{
			list.Remove(listener);
		}
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x00048BEC File Offset: 0x00046DEC
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeOnLoad()
	{
		UnityEngine.Object.DontDestroyOnLoad(new GameObject("GTSignalRelay").AddComponent<GTSignalRelay>());
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x0010246C File Offset: 0x0010066C
	void IOnEventCallback.OnEvent(EventData eventData)
	{
		if (eventData.Code != 186)
		{
			return;
		}
		object[] array = (object[])eventData.CustomData;
		int key = (int)array[0];
		List<GTSignalListener> list;
		if (!GTSignalRelay.gSignalIdToListeners.TryGetValue(key, out list))
		{
			return;
		}
		int sender = eventData.Sender;
		for (int i = 0; i < list.Count; i++)
		{
			try
			{
				GTSignalListener gtsignalListener = list[i];
				if (!gtsignalListener.deafen)
				{
					if (gtsignalListener.IsReady())
					{
						if (!gtsignalListener.ignoreSelf || sender != gtsignalListener.rigActorID)
						{
							if (!gtsignalListener.listenToSelfOnly || sender == gtsignalListener.rigActorID)
							{
								gtsignalListener.HandleSignalReceived(sender, array);
								if (gtsignalListener.callUnityEvent)
								{
									UnityEvent onSignalReceived = gtsignalListener.onSignalReceived;
									if (onSignalReceived != null)
									{
										onSignalReceived.Invoke();
									}
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x0400285F RID: 10335
	private static List<GTSignalListener> gActiveListeners = new List<GTSignalListener>(128);

	// Token: 0x04002860 RID: 10336
	private static HashSet<GTSignalListener> gListenerSet = new HashSet<GTSignalListener>(128);

	// Token: 0x04002861 RID: 10337
	private static Dictionary<int, List<GTSignalListener>> gSignalIdToListeners = new Dictionary<int, List<GTSignalListener>>(128);
}
