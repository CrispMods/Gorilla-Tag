using System;
using System.Collections.Generic;
using System.Text;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BF1 RID: 3057
	public class GorillaMaterialReaction : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x06004D62 RID: 19810 RVA: 0x001AAFB8 File Offset: 0x001A91B8
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

		// Token: 0x06004D63 RID: 19811 RVA: 0x00062C4F File Offset: 0x00060E4F
		protected void Awake()
		{
			this.RemoveAndReportNulls();
			this.PopulateRuntimeLookupArrays();
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x001AB1E8 File Offset: 0x001A93E8
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

		// Token: 0x06004D65 RID: 19813 RVA: 0x0003368B File Offset: 0x0003188B
		protected void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06004D66 RID: 19814 RVA: 0x00062C5D File Offset: 0x00060E5D
		// (set) Token: 0x06004D67 RID: 19815 RVA: 0x00062C65 File Offset: 0x00060E65
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004D68 RID: 19816 RVA: 0x001AB290 File Offset: 0x001A9490
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

		// Token: 0x06004D69 RID: 19817 RVA: 0x001AB448 File Offset: 0x001A9648
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

		// Token: 0x04004ED8 RID: 20184
		[SerializeField]
		private GorillaMaterialReaction.ReactionEntry[] _statusEffectReactions;

		// Token: 0x04004ED9 RID: 20185
		private int _previousMatIndex;

		// Token: 0x04004EDA RID: 20186
		private GorillaMaterialReaction.EMomentInState _currentMomentInState;

		// Token: 0x04004EDB RID: 20187
		private double _currentMatIndexStartTime;

		// Token: 0x04004EDC RID: 20188
		private double _currentMomentDuration;

		// Token: 0x04004EDD RID: 20189
		private int _reactionsRemaining;

		// Token: 0x04004EDE RID: 20190
		private int _momentEnumCount;

		// Token: 0x04004EDF RID: 20191
		private int _matCount;

		// Token: 0x04004EE0 RID: 20192
		private GameObject[][] _mat_x_moment_x_activeBool_to_gObjs;

		// Token: 0x04004EE1 RID: 20193
		private VRRig _ownerVRRig;

		// Token: 0x02000BF2 RID: 3058
		[Serializable]
		public struct ReactionEntry
		{
			// Token: 0x04004EE3 RID: 20195
			[Tooltip("If any of these statuses are true then this reaction will be executed.")]
			public int[] statusMaterialIndexes;

			// Token: 0x04004EE4 RID: 20196
			public GorillaMaterialReaction.GameObjectStates[] gameObjectStates;
		}

		// Token: 0x02000BF3 RID: 3059
		[Serializable]
		public struct GameObjectStates
		{
			// Token: 0x04004EE5 RID: 20197
			public GameObject gameObject;

			// Token: 0x04004EE6 RID: 20198
			[GorillaMaterialReaction.MomentInStateAttribute]
			public GorillaMaterialReaction.MomentInStateActiveOption onEnter;

			// Token: 0x04004EE7 RID: 20199
			[GorillaMaterialReaction.MomentInStateAttribute]
			public GorillaMaterialReaction.MomentInStateActiveOption onStay;

			// Token: 0x04004EE8 RID: 20200
			[GorillaMaterialReaction.MomentInStateAttribute]
			public GorillaMaterialReaction.MomentInStateActiveOption onExit;
		}

		// Token: 0x02000BF4 RID: 3060
		[Serializable]
		public struct MomentInStateActiveOption
		{
			// Token: 0x04004EE9 RID: 20201
			public bool change;

			// Token: 0x04004EEA RID: 20202
			public bool activeState;
		}

		// Token: 0x02000BF5 RID: 3061
		public enum EMomentInState
		{
			// Token: 0x04004EEC RID: 20204
			OnEnter,
			// Token: 0x04004EED RID: 20205
			OnStay,
			// Token: 0x04004EEE RID: 20206
			OnExit
		}

		// Token: 0x02000BF6 RID: 3062
		public class MomentInStateAttribute : Attribute
		{
		}
	}
}
