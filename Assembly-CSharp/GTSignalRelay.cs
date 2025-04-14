using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005AE RID: 1454
public class GTSignalRelay : MonoBehaviourStatic<GTSignalRelay>, IOnEventCallback
{
	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06002418 RID: 9240 RVA: 0x000B382A File Offset: 0x000B1A2A
	public static IReadOnlyList<GTSignalListener> ActiveListeners
	{
		get
		{
			return GTSignalRelay.gActiveListeners;
		}
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000B3831 File Offset: 0x000B1A31
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			PhotonNetwork.AddCallbackTarget(this);
		}
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000B3840 File Offset: 0x000B1A40
	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			PhotonNetwork.RemoveCallbackTarget(this);
		}
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000B3850 File Offset: 0x000B1A50
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

	// Token: 0x0600241C RID: 9244 RVA: 0x000B38BC File Offset: 0x000B1ABC
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

	// Token: 0x0600241D RID: 9245 RVA: 0x000B390C File Offset: 0x000B1B0C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeOnLoad()
	{
		Object.DontDestroyOnLoad(new GameObject("GTSignalRelay").AddComponent<GTSignalRelay>());
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000B3924 File Offset: 0x000B1B24
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

	// Token: 0x04002803 RID: 10243
	private static List<GTSignalListener> gActiveListeners = new List<GTSignalListener>(128);

	// Token: 0x04002804 RID: 10244
	private static HashSet<GTSignalListener> gListenerSet = new HashSet<GTSignalListener>(128);

	// Token: 0x04002805 RID: 10245
	private static Dictionary<int, List<GTSignalListener>> gSignalIdToListeners = new Dictionary<int, List<GTSignalListener>>(128);
}
