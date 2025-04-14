using System;
using System.Collections.Generic;
using System.Text;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BC3 RID: 3011
	public class GorillaMaterialReaction : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x06004C16 RID: 19478 RVA: 0x00172514 File Offset: 0x00170714
		public void PopulateRuntimeLookupArrays()
		{
			this._momentEnumCount = ((GorillaMaterialReaction.EMomentInState[])Enum.GetValues(typeof(GorillaMaterialReaction.EMomentInState))).Length;
			this._matCount = this._ownerVRRig.materialsToChangeTo.Length;
			this._mat_x_moment_x_activeBool_to_gObjs = new GameObject[this._momentEnumCount * this._matCount * 2][];
			for (int i = 0; i < this._matCount; i++)
			{
				for (int j = 0; j < this._momentEnumCount; j++)
				{
					GorillaMaterialReaction.EMomentInState emomentInState = (GorillaMaterialReaction.EMomentInState)j;
					List<GameObject> list = new List<GameObject>();
					List<GameObject> list2 = new List<GameObject>();
					foreach (GorillaMaterialReaction.ReactionEntry reactionEntry in this._statusEffectReactions)
					{
						int[] statusMaterialIndexes = reactionEntry.statusMaterialIndexes;
						for (int l = 0; l < statusMaterialIndexes.Length; l++)
						{
							if (statusMaterialIndexes[l] == i)
							{
								foreach (GorillaMaterialReaction.GameObjectStates gameObjectStates2 in reactionEntry.gameObjectStates)
								{
									switch (emomentInState)
									{
									case GorillaMaterialReaction.EMomentInState.OnEnter:
										if (gameObjectStates2.onEnter.change)
										{
											if (gameObjectStates2.onEnter.activeState)
											{
												list.Add(base.gameObject);
											}
											else
											{
												list2.Add(base.gameObject);
											}
										}
										break;
									case GorillaMaterialReaction.EMomentInState.OnStay:
										if (gameObjectStates2.onStay.change)
										{
											if (gameObjectStates2.onEnter.activeState)
											{
												list.Add(base.gameObject);
											}
											else
											{
												list2.Add(base.gameObject);
											}
										}
										break;
									case GorillaMaterialReaction.EMomentInState.OnExit:
										if (gameObjectStates2.onExit.change)
										{
											if (gameObjectStates2.onEnter.activeState)
											{
												list.Add(base.gameObject);
											}
											else
											{
												list2.Add(base.gameObject);
											}
										}
										break;
									default:
										Debug.LogError(string.Format("Unhandled enum value for {0}: {1}", "EMomentInState", emomentInState));
										break;
									}
								}
							}
						}
					}
					int num = i * this._momentEnumCount * 2 + j * 2;
					this._mat_x_moment_x_activeBool_to_gObjs[num] = list2.ToArray();
					this._mat_x_moment_x_activeBool_to_gObjs[num + 1] = list.ToArray();
				}
			}
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x00172743 File Offset: 0x00170943
		protected void Awake()
		{
			this.RemoveAndReportNulls();
			this.PopulateRuntimeLookupArrays();
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x00172754 File Offset: 0x00170954
		protected void OnEnable()
		{
			if (this._ownerVRRig == null)
			{
				this._ownerVRRig = base.GetComponentInParent<VRRig>(true);
			}
			if (this._ownerVRRig == null)
			{
				Debug.LogError("GorillaMaterialReaction: Disabling because could not find VRRig! Hierarchy path: " + base.transform.GetPath(), this);
				base.enabled = false;
				return;
			}
			this._reactionsRemaining = 0;
			for (int i = 0; i < this._statusEffectReactions.Length; i++)
			{
				this._reactionsRemaining += this._statusEffectReactions[i].gameObjectStates.Length;
			}
			this._currentMatIndexStartTime = 0.0;
			TickSystem<object>.AddCallbackTarget(this);
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x00019B13 File Offset: 0x00017D13
		protected void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x001727FC File Offset: 0x001709FC
		// (set) Token: 0x06004C1B RID: 19483 RVA: 0x00172804 File Offset: 0x00170A04
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004C1C RID: 19484 RVA: 0x00172810 File Offset: 0x00170A10
		void ITickSystemPost.PostTick()
		{
			if (!GorillaComputer.hasInstance || this._ownerVRRig == null)
			{
				return;
			}
			GorillaComputer instance = GorillaComputer.instance;
			int num = (GorillaGameManager.instance == null) ? 0 : GorillaGameManager.instance.MyMatIndex(this._ownerVRRig.creator);
			if (this._previousMatIndex == num && this._reactionsRemaining <= 0)
			{
				return;
			}
			double num2 = (double)instance.startupMillis / 1000.0 + Time.realtimeSinceStartupAsDouble;
			bool flag = false;
			if (this._currentMomentInState == GorillaMaterialReaction.EMomentInState.OnExit && this._previousMatIndex != num)
			{
				this._currentMomentInState = GorillaMaterialReaction.EMomentInState.OnEnter;
				flag = true;
				this._currentMatIndexStartTime = num2;
				this._currentMomentDuration = -1.0;
				GorillaGameManager instance2 = GorillaGameManager.instance;
				if (instance2 != null)
				{
					GorillaTagManager gorillaTagManager = instance2 as GorillaTagManager;
					if (gorillaTagManager != null)
					{
						this._currentMomentDuration = (double)gorillaTagManager.tagCoolDown;
					}
				}
			}
			else if (this._currentMomentInState == GorillaMaterialReaction.EMomentInState.OnEnter && this._previousMatIndex == num && (this._currentMomentDuration < 0.0 || this._currentMomentDuration < num2 - this._currentMatIndexStartTime))
			{
				this._currentMomentInState = GorillaMaterialReaction.EMomentInState.OnStay;
				flag = true;
				this._currentMomentDuration = -1.0;
			}
			else if (this._currentMomentInState == GorillaMaterialReaction.EMomentInState.OnStay && this._previousMatIndex != num)
			{
				this._currentMomentInState = GorillaMaterialReaction.EMomentInState.OnExit;
				flag = true;
				this._currentMomentDuration = -1.0;
			}
			this._previousMatIndex = num;
			if (!flag)
			{
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				GameObject[] array = this._mat_x_moment_x_activeBool_to_gObjs[(int)(num * this._momentEnumCount * 2 + this._currentMomentInState * GorillaMaterialReaction.EMomentInState.OnExit + i)];
				for (int j = 0; j < array.Length; j++)
				{
					array[j].SetActive(i == 1);
				}
			}
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x001729C8 File Offset: 0x00170BC8
		private void RemoveAndReportNulls()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			if (this._statusEffectReactions == null)
			{
				Debug.Log(string.Format("{0}: The array `{1}` is null. ", "GorillaMaterialReaction", this._statusEffectReactions) + "(this should never happen)");
				this._statusEffectReactions = Array.Empty<GorillaMaterialReaction.ReactionEntry>();
			}
			for (int i = 0; i < this._statusEffectReactions.Length; i++)
			{
				GorillaMaterialReaction.GameObjectStates[] gameObjectStates = this._statusEffectReactions[i].gameObjectStates;
				if (gameObjectStates == null)
				{
					this._statusEffectReactions[i].gameObjectStates = Array.Empty<GorillaMaterialReaction.GameObjectStates>();
				}
				else
				{
					int num = 0;
					int[] array = new int[gameObjectStates.Length];
					for (int j = 0; j < gameObjectStates.Length; j++)
					{
						if (gameObjectStates[j].gameObject == null)
						{
							array[num] = j;
							num++;
						}
						else
						{
							array[num] = -1;
						}
					}
					if (num == 0)
					{
						return;
					}
					stringBuilder.Clear();
					stringBuilder.Append("GorillaMaterialReaction");
					stringBuilder.Append(": Removed null references in array `");
					stringBuilder.Append("_statusEffectReactions");
					stringBuilder.Append("[").Append(i).Append("].").Append("gameObjectStates");
					stringBuilder.Append("' at indexes: ");
					stringBuilder.AppendJoin<int>(", ", array);
					stringBuilder.Append(".");
					Debug.LogError(stringBuilder.ToString(), this);
					GorillaMaterialReaction.GameObjectStates[] array2 = new GorillaMaterialReaction.GameObjectStates[gameObjectStates.Length - num];
					int num2 = 0;
					for (int k = 0; k < gameObjectStates.Length; k++)
					{
						if (!(gameObjectStates[k].gameObject == null))
						{
							array2[num2++] = gameObjectStates[k];
						}
					}
					this._statusEffectReactions[i].gameObjectStates = array2;
				}
			}
		}

		// Token: 0x04004DE2 RID: 19938
		[SerializeField]
		private GorillaMaterialReaction.ReactionEntry[] _statusEffectReactions;

		// Token: 0x04004DE3 RID: 19939
		private int _previousMatIndex;

		// Token: 0x04004DE4 RID: 19940
		private GorillaMaterialReaction.EMomentInState _currentMomentInState;

		// Token: 0x04004DE5 RID: 19941
		private double _currentMatIndexStartTime;

		// Token: 0x04004DE6 RID: 19942
		private double _currentMomentDuration;

		// Token: 0x04004DE7 RID: 19943
		private int _reactionsRemaining;

		// Token: 0x04004DE8 RID: 19944
		private int _momentEnumCount;

		// Token: 0x04004DE9 RID: 19945
		private int _matCount;

		// Token: 0x04004DEA RID: 19946
		private GameObject[][] _mat_x_moment_x_activeBool_to_gObjs;

		// Token: 0x04004DEB RID: 19947
		private VRRig _ownerVRRig;

		// Token: 0x02000BC4 RID: 3012
		[Serializable]
		public struct ReactionEntry
		{
			// Token: 0x04004DED RID: 19949
			[Tooltip("If any of these statuses are true then this reaction will be executed.")]
			public int[] statusMaterialIndexes;

			// Token: 0x04004DEE RID: 19950
			public GorillaMaterialReaction.GameObjectStates[] gameObjectStates;
		}

		// Token: 0x02000BC5 RID: 3013
		[Serializable]
		public struct GameObjectStates
		{
			// Token: 0x04004DEF RID: 19951
			public GameObject gameObject;

			// Token: 0x04004DF0 RID: 19952
			[GorillaMaterialReaction.MomentInStateAttribute]
			public GorillaMaterialReaction.MomentInStateActiveOption onEnter;

			// Token: 0x04004DF1 RID: 19953
			[GorillaMaterialReaction.MomentInStateAttribute]
			public GorillaMaterialReaction.MomentInStateActiveOption onStay;

			// Token: 0x04004DF2 RID: 19954
			[GorillaMaterialReaction.MomentInStateAttribute]
			public GorillaMaterialReaction.MomentInStateActiveOption onExit;
		}

		// Token: 0x02000BC6 RID: 3014
		[Serializable]
		public struct MomentInStateActiveOption
		{
			// Token: 0x04004DF3 RID: 19955
			public bool change;

			// Token: 0x04004DF4 RID: 19956
			public bool activeState;
		}

		// Token: 0x02000BC7 RID: 3015
		public enum EMomentInState
		{
			// Token: 0x04004DF6 RID: 19958
			OnEnter,
			// Token: 0x04004DF7 RID: 19959
			OnStay,
			// Token: 0x04004DF8 RID: 19960
			OnExit
		}

		// Token: 0x02000BC8 RID: 3016
		public class MomentInStateAttribute : Attribute
		{
		}
	}
}
