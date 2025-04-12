using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTag.GuidedRefs.Internal;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD6 RID: 3030
	[DefaultExecutionOrder(-2147483648)]
	public class GuidedRefHub : MonoBehaviour, IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004C5A RID: 19546 RVA: 0x0006141B File Offset: 0x0005F61B
		protected void Awake()
		{
			this.GuidedRefInitialize();
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x001A4F28 File Offset: 0x001A3128
		protected void OnDestroy()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			if (this.isRootInstance)
			{
				GuidedRefHub.hasRootInstance = false;
				GuidedRefHub.rootInstance = null;
			}
			List<int> list;
			if (GuidedRefHub.globalLookupRefInstIDsByHub.TryGetValue(this, out list))
			{
				foreach (int key in list)
				{
					GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId[key].Remove(this);
				}
				GuidedRefHub.globalLookupRefInstIDsByHub.Remove(this);
			}
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x001A4FB8 File Offset: 0x001A31B8
		public void GuidedRefInitialize()
		{
			if (this.isRootInstance)
			{
				if (GuidedRefHub.hasRootInstance)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"GuidedRefHub: Attempted to assign global instance when one was already assigned:\n- This path: ",
						base.transform.GetPath(),
						"\n- Global instance: ",
						GuidedRefHub.rootInstance.transform.GetPath(),
						"\n"
					}), this);
					UnityEngine.Object.Destroy(this);
					return;
				}
				GuidedRefHub.hasRootInstance = true;
				GuidedRefHub.rootInstance = this;
			}
			GuidedRefHub.globalLookupRefInstIDsByHub[this] = new List<int>(2);
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x00061423 File Offset: 0x0005F623
		public static bool IsInstanceIDRegisteredWithAnyHub(int instanceID)
		{
			return GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId.ContainsKey(instanceID);
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x001A5044 File Offset: 0x001A3244
		private void RegisterTarget_Internal<TIGuidedRefTargetMono>(TIGuidedRefTargetMono targetMono) where TIGuidedRefTargetMono : IGuidedRefTargetMono
		{
			RelayInfo orAddRelayInfoByTargetId = this.GetOrAddRelayInfoByTargetId(targetMono.GRefTargetInfo.targetId);
			if (orAddRelayInfoByTargetId == null)
			{
				return;
			}
			IGuidedRefTargetMono guidedRefTargetMono = targetMono;
			if (orAddRelayInfoByTargetId.targetMono != null && orAddRelayInfoByTargetId.targetMono != guidedRefTargetMono)
			{
				if (targetMono.GRefTargetInfo.hackIgnoreDuplicateRegistration)
				{
					return;
				}
				Debug.LogError(string.Concat(new string[]
				{
					"GuidedRefHub: Multiple targets registering with the same Hub. Maybe look at the HubID you are using:- hub=\"",
					base.transform.GetPath(),
					"\"\n- target1=\"",
					orAddRelayInfoByTargetId.targetMono.transform.GetPath(),
					"\",\n- target2=\"",
					targetMono.transform.GetPath(),
					"\""
				}), this);
				return;
			}
			else
			{
				int instanceID = targetMono.GetInstanceID();
				GuidedRefHub.GetHubsThatHaveRegisteredInstId(instanceID).Add(this);
				List<int> list;
				if (!GuidedRefHub.globalLookupRefInstIDsByHub.TryGetValue(this, out list))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"GuidedRefHub: It appears hub was not registered before `RegisterTarget` was called on it: - hub: \"",
						base.transform.GetPath(),
						"\"\n- target: \"",
						targetMono.transform.GetPath(),
						"\""
					}), this);
					return;
				}
				list.Add(instanceID);
				orAddRelayInfoByTargetId.targetMono = targetMono;
				GuidedRefHub.ResolveReferences(orAddRelayInfoByTargetId);
				return;
			}
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x001A5198 File Offset: 0x001A3398
		public static void RegisterTarget<TIGuidedRefTargetMono>(TIGuidedRefTargetMono targetMono, GuidedRefHubIdSO[] hubIds = null, Component debugCaller = null) where TIGuidedRefTargetMono : IGuidedRefTargetMono
		{
			if (targetMono == null)
			{
				string str = (debugCaller == null) ? "UNSUPPLIED_CALLER_NAME" : debugCaller.name;
				Debug.LogError("GuidedRefHub: Cannot register null target from \"" + str + "\".", debugCaller);
				return;
			}
			if (targetMono.GRefTargetInfo.targetId == null)
			{
				return;
			}
			GuidedRefHub.globalHubsTransientList.Clear();
			targetMono.transform.GetComponentsInParent<GuidedRefHub>(true, GuidedRefHub.globalHubsTransientList);
			if (GuidedRefHub.hasRootInstance)
			{
				GuidedRefHub.globalHubsTransientList.Add(GuidedRefHub.rootInstance);
			}
			bool flag = false;
			foreach (GuidedRefHub guidedRefHub in GuidedRefHub.globalHubsTransientList)
			{
				if (hubIds == null || hubIds.Length <= 0 || Array.IndexOf<GuidedRefHubIdSO>(hubIds, guidedRefHub.hubId) != -1)
				{
					flag = true;
					guidedRefHub.RegisterTarget_Internal<TIGuidedRefTargetMono>(targetMono);
				}
			}
			if (!flag && Application.isPlaying)
			{
				Debug.LogError("GuidedRefHub: Could not find hub for target: \"" + targetMono.transform.GetPath() + "\"", targetMono.transform);
			}
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x001A52CC File Offset: 0x001A34CC
		public static void UnregisterTarget<TIGuidedRefTargetMono>(TIGuidedRefTargetMono targetMono, bool destroyed = true) where TIGuidedRefTargetMono : IGuidedRefTargetMono
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			if (targetMono == null)
			{
				Debug.LogError("GuidedRefHub: Cannot unregister null target.");
				return;
			}
			int instanceID = targetMono.GetInstanceID();
			List<GuidedRefHub> list;
			if (!GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId.TryGetValue(instanceID, out list))
			{
				return;
			}
			using (List<GuidedRefHub>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RelayInfo relayInfo;
					if (enumerator.Current.lookupRelayInfoByTargetId.TryGetValue(targetMono.GRefTargetInfo.targetId, out relayInfo))
					{
						foreach (RegisteredReceiverFieldInfo registeredReceiverFieldInfo in relayInfo.registeredFields)
						{
							if (registeredReceiverFieldInfo.receiverMono != null)
							{
								registeredReceiverFieldInfo.receiverMono.OnGuidedRefTargetDestroyed(registeredReceiverFieldInfo.fieldId);
								GuidedRefHub.kReceiversWaitingToFullyResolve.Remove(registeredReceiverFieldInfo.receiverMono);
								relayInfo.resolvedFields.Remove(registeredReceiverFieldInfo);
								relayInfo.registeredFields.Add(registeredReceiverFieldInfo);
								IGuidedRefReceiverMono receiverMono = registeredReceiverFieldInfo.receiverMono;
								int guidedRefsWaitingToResolveCount = receiverMono.GuidedRefsWaitingToResolveCount;
								receiverMono.GuidedRefsWaitingToResolveCount = guidedRefsWaitingToResolveCount + 1;
							}
						}
					}
				}
			}
			foreach (GuidedRefHub guidedRefHub in list)
			{
				RelayInfo relayInfo2;
				if (guidedRefHub.lookupRelayInfoByTargetId.TryGetValue(targetMono.GRefTargetInfo.targetId, out relayInfo2))
				{
					relayInfo2.targetMono = null;
				}
				GuidedRefHub.globalLookupRefInstIDsByHub[guidedRefHub].Remove(instanceID);
			}
			GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId.Remove(instanceID);
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x00061430 File Offset: 0x0005F630
		public static void ReceiverFullyRegistered<TIGuidedRefReceiverMono>(TIGuidedRefReceiverMono receiverMono) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono
		{
			GuidedRefHub.kReceiversFullyRegistered.Add(receiverMono);
			GuidedRefHub.kReceiversWaitingToFullyResolve.Add(receiverMono);
			GuidedRefHub.CheckAndNotifyIfReceiverFullyResolved<TIGuidedRefReceiverMono>(receiverMono);
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x001A5494 File Offset: 0x001A3694
		private static void CheckAndNotifyIfReceiverFullyResolved<TIGuidedRefReceiverMono>(TIGuidedRefReceiverMono receiverMono) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono
		{
			if (receiverMono.GuidedRefsWaitingToResolveCount == 0 && GuidedRefHub.kReceiversFullyRegistered.Contains(receiverMono))
			{
				GuidedRefHub.kReceiversWaitingToFullyResolve.Remove(receiverMono);
				receiverMono.OnAllGuidedRefsResolved();
			}
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x001A54E0 File Offset: 0x001A36E0
		private void RegisterReceiverField(RegisteredReceiverFieldInfo registeredReceiverFieldInfo, GuidedRefTargetIdSO targetId)
		{
			GuidedRefHub.globalLookupRefInstIDsByHub[this].Add(registeredReceiverFieldInfo.receiverMono.GetInstanceID());
			GuidedRefHub.GetHubsThatHaveRegisteredInstId(registeredReceiverFieldInfo.receiverMono.GetInstanceID()).Add(this);
			RelayInfo orAddRelayInfoByTargetId = this.GetOrAddRelayInfoByTargetId(targetId);
			orAddRelayInfoByTargetId.registeredFields.Add(registeredReceiverFieldInfo);
			GuidedRefHub.ResolveReferences(orAddRelayInfoByTargetId);
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x001A5538 File Offset: 0x001A3738
		private static void RegisterReceiverField_Internal<TIGuidedRefReceiverMono>(GuidedRefHubIdSO hubId, TIGuidedRefReceiverMono receiverMono, int fieldId, GuidedRefTargetIdSO targetId, int index) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono
		{
			if (receiverMono == null)
			{
				Debug.LogError("GuidedRefHub: Cannot register null receiver.");
				return;
			}
			GuidedRefHub.globalHubsTransientList.Clear();
			receiverMono.transform.GetComponentsInParent<GuidedRefHub>(true, GuidedRefHub.globalHubsTransientList);
			if (GuidedRefHub.hasRootInstance)
			{
				GuidedRefHub.globalHubsTransientList.Add(GuidedRefHub.rootInstance);
			}
			RegisteredReceiverFieldInfo registeredReceiverFieldInfo = new RegisteredReceiverFieldInfo
			{
				receiverMono = receiverMono,
				fieldId = fieldId,
				index = index
			};
			bool flag = false;
			foreach (GuidedRefHub guidedRefHub in GuidedRefHub.globalHubsTransientList)
			{
				if (!(hubId != null) || !(guidedRefHub.hubId != hubId))
				{
					flag = true;
					guidedRefHub.RegisterReceiverField(registeredReceiverFieldInfo, targetId);
					break;
				}
			}
			if (flag)
			{
				int guidedRefsWaitingToResolveCount = receiverMono.GuidedRefsWaitingToResolveCount;
				receiverMono.GuidedRefsWaitingToResolveCount = guidedRefsWaitingToResolveCount + 1;
				return;
			}
			Debug.LogError("Could not find matching GuidedRefHub to register with for receiver at: " + receiverMono.transform.GetPath(), receiverMono.transform);
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x0006145A File Offset: 0x0005F65A
		public static void RegisterReceiverField<TIGuidedRefReceiverMono>(TIGuidedRefReceiverMono receiverMono, string fieldIdName, ref GuidedRefReceiverFieldInfo fieldInfo) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono
		{
			if (!GRef.ShouldResolveNow(fieldInfo.resolveModes))
			{
				return;
			}
			fieldInfo.fieldId = Shader.PropertyToID(fieldIdName);
			GuidedRefHub.RegisterReceiverField_Internal<TIGuidedRefReceiverMono>(fieldInfo.hubId, receiverMono, fieldInfo.fieldId, fieldInfo.targetId, -1);
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x001A5670 File Offset: 0x001A3870
		public static void RegisterReceiverArray<TIGuidedRefReceiverMono, T>(TIGuidedRefReceiverMono receiverMono, string fieldIdName, ref T[] receiverArray, ref GuidedRefReceiverArrayInfo arrayInfo) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono where T : UnityEngine.Object
		{
			if (!GRef.ShouldResolveNow(arrayInfo.resolveModes))
			{
				return;
			}
			if (receiverArray == null)
			{
				receiverArray = new T[arrayInfo.targets.Length];
			}
			else if (receiverArray.Length != arrayInfo.targets.Length)
			{
				Array.Resize<T>(ref receiverArray, arrayInfo.targets.Length);
			}
			arrayInfo.fieldId = Shader.PropertyToID(fieldIdName);
			for (int i = 0; i < arrayInfo.targets.Length; i++)
			{
				GuidedRefHub.RegisterReceiverField_Internal<TIGuidedRefReceiverMono>(arrayInfo.hubId, receiverMono, arrayInfo.fieldId, arrayInfo.targets[i], i);
			}
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x001A56F8 File Offset: 0x001A38F8
		public static void UnregisterReceiver<TIGuidedRefReceiverMono>(TIGuidedRefReceiverMono receiverMono) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono
		{
			if (receiverMono == null)
			{
				Debug.LogError("GuidedRefHub: Cannot unregister null receiver.");
				return;
			}
			int instanceID = receiverMono.GetInstanceID();
			List<GuidedRefHub> list;
			if (!GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId.TryGetValue(instanceID, out list))
			{
				Debug.LogError("Tried to unregister a receiver before it was registered.");
				return;
			}
			IGuidedRefReceiverMono iReceiverMono = receiverMono;
			Predicate<RegisteredReceiverFieldInfo> <>9__0;
			foreach (GuidedRefHub guidedRefHub in list)
			{
				foreach (RelayInfo relayInfo in guidedRefHub.lookupRelayInfoByTargetId.Values)
				{
					List<RegisteredReceiverFieldInfo> registeredFields = relayInfo.registeredFields;
					Predicate<RegisteredReceiverFieldInfo> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((RegisteredReceiverFieldInfo fieldInfo) => fieldInfo.receiverMono == iReceiverMono));
					}
					registeredFields.RemoveAll(match);
				}
				GuidedRefHub.globalLookupRefInstIDsByHub[guidedRefHub].Remove(instanceID);
			}
			GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId.Remove(instanceID);
			receiverMono.GuidedRefsWaitingToResolveCount = 0;
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x001A582C File Offset: 0x001A3A2C
		private RelayInfo GetOrAddRelayInfoByTargetId(GuidedRefTargetIdSO targetId)
		{
			if (targetId == null)
			{
				Debug.LogError("GetOrAddRelayInfoByTargetId cannot register null target id");
				return null;
			}
			RelayInfo relayInfo;
			if (!this.lookupRelayInfoByTargetId.TryGetValue(targetId, out relayInfo))
			{
				relayInfo = new RelayInfo
				{
					targetMono = null,
					registeredFields = new List<RegisteredReceiverFieldInfo>(1),
					resolvedFields = new List<RegisteredReceiverFieldInfo>(1)
				};
				this.lookupRelayInfoByTargetId[targetId] = relayInfo;
				GuidedRefHub.static_relayInfo_to_targetId[relayInfo] = targetId;
			}
			return relayInfo;
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x001A58A0 File Offset: 0x001A3AA0
		public static List<GuidedRefHub> GetHubsThatHaveRegisteredInstId(int instanceId)
		{
			List<GuidedRefHub> list;
			if (!GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId.TryGetValue(instanceId, out list))
			{
				list = new List<GuidedRefHub>(1);
				GuidedRefHub.globalLookupHubsThatHaveRegisteredInstId[instanceId] = list;
			}
			return list;
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x001A58D0 File Offset: 0x001A3AD0
		private static void ResolveReferences(RelayInfo relayInfo)
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			if (relayInfo == null)
			{
				Debug.LogError("GuidedRefHub.ResolveReferences: (this should never happen) relayInfo is null.");
				return;
			}
			if (relayInfo.registeredFields == null)
			{
				GuidedRefTargetIdSO guidedRefTargetIdSO = GuidedRefHub.static_relayInfo_to_targetId[relayInfo];
				string str = (guidedRefTargetIdSO != null) ? guidedRefTargetIdSO.name : "NULL";
				Debug.LogError("GuidedRefHub.ResolveReferences: (this should never happen) \"" + str + "\"relayInfo.registeredFields is null.");
				return;
			}
			if (relayInfo.targetMono == null)
			{
				return;
			}
			for (int i = relayInfo.registeredFields.Count - 1; i >= 0; i--)
			{
				RegisteredReceiverFieldInfo registeredReceiverFieldInfo = relayInfo.registeredFields[i];
				if (registeredReceiverFieldInfo.receiverMono.GuidedRefTryResolveReference(new GuidedRefTryResolveInfo
				{
					fieldId = registeredReceiverFieldInfo.fieldId,
					index = registeredReceiverFieldInfo.index,
					targetMono = relayInfo.targetMono
				}))
				{
					relayInfo.registeredFields.RemoveAt(i);
					GuidedRefHub.CheckAndNotifyIfReceiverFullyResolved<IGuidedRefReceiverMono>(registeredReceiverFieldInfo.receiverMono);
					relayInfo.resolvedFields.Add(registeredReceiverFieldInfo);
				}
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x001A59C4 File Offset: 0x001A3BC4
		public static bool TryResolveField<TIGuidedRefReceiverMono, T>(TIGuidedRefReceiverMono receiverMono, ref T refReceiverObj, GuidedRefReceiverFieldInfo receiverFieldInfo, GuidedRefTryResolveInfo tryResolveInfo) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono where T : UnityEngine.Object
		{
			if (tryResolveInfo.index > -1 || tryResolveInfo.fieldId != receiverFieldInfo.fieldId || refReceiverObj != null)
			{
				return false;
			}
			bool flag = tryResolveInfo.targetMono != null && tryResolveInfo.targetMono.GuidedRefTargetObject != null;
			T t = flag ? (tryResolveInfo.targetMono.GuidedRefTargetObject as T) : default(T);
			if (!flag)
			{
				string fieldNameByID = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveField: Receiver \"",
					receiverMono.transform.name,
					"\" with field \"",
					fieldNameByID,
					"\": was already assigned to something other than matching target id! Assigning to found target anyway. Make the receiving field null before attempting to resolve to prevent this message. ",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
			}
			else if (refReceiverObj != null && refReceiverObj != t)
			{
				Debug.LogError("was assigned didn't match assigning anyway");
				string fieldNameByID2 = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveField: Receiver \"",
					receiverMono.transform.name,
					"\" with field \"",
					fieldNameByID2,
					"\" was already assigned to something other than matching target id! Assigning to found target anyway. Make the receiving field null before attempting to resolve to prevent this message. ",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
			}
			refReceiverObj = t;
			int guidedRefsWaitingToResolveCount = receiverMono.GuidedRefsWaitingToResolveCount;
			receiverMono.GuidedRefsWaitingToResolveCount = guidedRefsWaitingToResolveCount - 1;
			return true;
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001A5B98 File Offset: 0x001A3D98
		public static bool TryResolveArrayItem<TIGuidedRefReceiverMono, T>(TIGuidedRefReceiverMono receiverMono, IList<T> receivingArray, GuidedRefReceiverArrayInfo receiverArrayInfo, GuidedRefTryResolveInfo tryResolveInfo) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono where T : UnityEngine.Object
		{
			bool flag;
			return GuidedRefHub.TryResolveArrayItem<TIGuidedRefReceiverMono, T>(receiverMono, receivingArray, receiverArrayInfo, tryResolveInfo, out flag);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x001A5BB0 File Offset: 0x001A3DB0
		public static bool TryResolveArrayItem<TIGuidedRefReceiverMono, T>(TIGuidedRefReceiverMono receiverMono, IList<T> receivingArray, GuidedRefReceiverArrayInfo receiverArrayInfo, GuidedRefTryResolveInfo tryResolveInfo, out bool arrayResolved) where TIGuidedRefReceiverMono : IGuidedRefReceiverMono where T : UnityEngine.Object
		{
			arrayResolved = false;
			if (tryResolveInfo.index <= -1 && receiverArrayInfo.fieldId != tryResolveInfo.fieldId)
			{
				return false;
			}
			if (receivingArray == null)
			{
				string fieldNameByID = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveArrayItem: Receiver \"",
					receiverMono.transform.name,
					"\" with array \"",
					fieldNameByID,
					"\": Receiving array cannot be null!",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
				return false;
			}
			if (receiverArrayInfo.targets == null)
			{
				string fieldNameByID2 = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveArrayItem: Receiver component \"",
					receiverMono.transform.name,
					"\" with array \"",
					fieldNameByID2,
					"\": Targets array is null! It must have been set to null after registering. If this intentional than the you need to unregister first.",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
				return false;
			}
			int num = receiverArrayInfo.targets.Length;
			if (num <= receiverArrayInfo.resolveCount)
			{
				string fieldNameByID3 = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveArrayItem: Receiver component \"",
					receiverMono.transform.name,
					"\" with array \"",
					fieldNameByID3,
					"\": Targets array size is equal or smaller than resolve count. Did you change the size of the array before it finished resolving or before unregistering?",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
				return false;
			}
			if (num != receivingArray.Count)
			{
				string fieldNameByID4 = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveArrayItem: Receiver component \"",
					receiverMono.transform.name,
					"\" with array \"",
					fieldNameByID4,
					"\": The sizes of `receivingList` and `receiverArrayInfo.fieldInfos` are not equal. They must be the same length before calling.",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
				return false;
			}
			T t = tryResolveInfo.targetMono.GuidedRefTargetObject as T;
			if (t == null)
			{
				string fieldNameByID5 = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveArrayItem: Receiver \"",
					receiverMono.transform.name,
					"\" with field \"",
					fieldNameByID5,
					"\" found a matching target id but target object was null! Was it destroyed without unregistering? ",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
			}
			if (receivingArray[tryResolveInfo.index] != null && receivingArray[tryResolveInfo.index] != t)
			{
				string fieldNameByID6 = GuidedRefHub.GetFieldNameByID(tryResolveInfo.fieldId);
				Debug.LogError(string.Concat(new string[]
				{
					"TryResolveArrayItem: Receiver \"",
					receiverMono.transform.name,
					"\" with array \"",
					fieldNameByID6,
					"\" ",
					string.Format("at index {0}: Already assigned to something other than matching target id! ", tryResolveInfo.index),
					"Assigning to found target anyway. Make the receiving field null before attempting to resolve to prevent this message. ",
					string.Format("fieldId={0}, receiver path=\"{1}\"", tryResolveInfo.fieldId, receiverMono.transform.GetPath())
				}));
			}
			int num2 = receiverArrayInfo.resolveCount + 1;
			receiverArrayInfo.resolveCount = num2;
			arrayResolved = (num2 >= num);
			num2 = receiverMono.GuidedRefsWaitingToResolveCount;
			receiverMono.GuidedRefsWaitingToResolveCount = num2 - 1;
			receivingArray[tryResolveInfo.index] = t;
			return true;
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x0006148F File Offset: 0x0005F68F
		public static string GetFieldNameByID(int fieldId)
		{
			return "FieldNameOnlyAvailableInEditor";
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x00037F83 File Offset: 0x00036183
		Transform IGuidedRefMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x00031B4B File Offset: 0x0002FD4B
		int IGuidedRefObject.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04004E49 RID: 20041
		[SerializeField]
		private bool isRootInstance;

		// Token: 0x04004E4A RID: 20042
		public GuidedRefHubIdSO hubId;

		// Token: 0x04004E4B RID: 20043
		[OnEnterPlay_SetNull]
		[NonSerialized]
		public static GuidedRefHub rootInstance;

		// Token: 0x04004E4C RID: 20044
		[OnEnterPlay_Set(false)]
		[NonSerialized]
		public static bool hasRootInstance;

		// Token: 0x04004E4D RID: 20045
		[DebugReadout]
		private readonly Dictionary<GuidedRefTargetIdSO, RelayInfo> lookupRelayInfoByTargetId = new Dictionary<GuidedRefTargetIdSO, RelayInfo>(256);

		// Token: 0x04004E4E RID: 20046
		private static readonly Dictionary<RelayInfo, GuidedRefTargetIdSO> static_relayInfo_to_targetId = new Dictionary<RelayInfo, GuidedRefTargetIdSO>(256);

		// Token: 0x04004E4F RID: 20047
		[OnEnterPlay_Clear]
		private static readonly Dictionary<int, List<GuidedRefHub>> globalLookupHubsThatHaveRegisteredInstId = new Dictionary<int, List<GuidedRefHub>>(256);

		// Token: 0x04004E50 RID: 20048
		[OnEnterPlay_Clear]
		private static readonly Dictionary<GuidedRefHub, List<int>> globalLookupRefInstIDsByHub = new Dictionary<GuidedRefHub, List<int>>(256);

		// Token: 0x04004E51 RID: 20049
		[OnEnterPlay_Clear]
		private static readonly List<GuidedRefHub> globalHubsTransientList = new List<GuidedRefHub>(32);

		// Token: 0x04004E52 RID: 20050
		private const string kUnsuppliedCallerName = "UNSUPPLIED_CALLER_NAME";

		// Token: 0x04004E53 RID: 20051
		[DebugReadout]
		[OnEnterPlay_Clear]
		internal static readonly HashSet<IGuidedRefReceiverMono> kReceiversWaitingToFullyResolve = new HashSet<IGuidedRefReceiverMono>(256);

		// Token: 0x04004E54 RID: 20052
		[DebugReadout]
		[OnEnterPlay_Clear]
		internal static readonly HashSet<IGuidedRefReceiverMono> kReceiversFullyRegistered = new HashSet<IGuidedRefReceiverMono>(256);
	}
}
