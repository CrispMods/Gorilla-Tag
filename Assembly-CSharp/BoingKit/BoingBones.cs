using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CAF RID: 3247
	public class BoingBones : BoingReactor
	{
		// Token: 0x060051F7 RID: 20983 RVA: 0x00192223 File Offset: 0x00190423
		protected override void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x0019222B File Offset: 0x0019042B
		protected override void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x00192233 File Offset: 0x00190433
		protected override void OnUpgrade(Version oldVersion, Version newVersion)
		{
			base.OnUpgrade(oldVersion, newVersion);
			if (oldVersion.Revision < 33)
			{
				this.TwistPropagation = false;
			}
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x0019224F File Offset: 0x0019044F
		public void OnValidate()
		{
			this.RescanBoneChains();
			this.UpdateCollisionRadius();
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x0019225D File Offset: 0x0019045D
		public override void OnEnable()
		{
			base.OnEnable();
			this.RescanBoneChains();
			this.Reboot();
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x00192271 File Offset: 0x00190471
		public override void OnDisable()
		{
			base.OnDisable();
			this.Restore();
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x00192280 File Offset: 0x00190480
		public void RescanBoneChains()
		{
			if (this.BoneChains == null)
			{
				return;
			}
			int num = this.BoneChains.Length;
			if (this.BoneData == null || this.BoneData.Length != num)
			{
				BoingBones.Bone[][] array = new BoingBones.Bone[num][];
				if (this.BoneData != null)
				{
					int i = 0;
					int num2 = Mathf.Min(this.BoneData.Length, num);
					while (i < num2)
					{
						array[i] = this.BoneData[i];
						i++;
					}
				}
				this.BoneData = array;
			}
			Queue<BoingBones.RescanEntry> queue = new Queue<BoingBones.RescanEntry>();
			for (int j = 0; j < num; j++)
			{
				BoingBones.Chain chain = this.BoneChains[j];
				bool flag = false;
				if (this.BoneData[j] == null)
				{
					flag = true;
				}
				if (!flag && chain.m_scannedRoot == null)
				{
					flag = true;
				}
				if (!flag && chain.m_scannedRoot != chain.Root)
				{
					flag = true;
				}
				if (!flag && chain.m_scannedExclusion != null != (chain.Exclusion != null))
				{
					flag = true;
				}
				if (!flag && chain.Exclusion != null)
				{
					if (chain.m_scannedExclusion.Length != chain.Exclusion.Length)
					{
						flag = true;
					}
					else
					{
						for (int k = 0; k < chain.m_scannedExclusion.Length; k++)
						{
							if (!(chain.m_scannedExclusion[k] == chain.Exclusion[k]))
							{
								flag = true;
								break;
							}
						}
					}
				}
				Transform transform = (chain != null) ? chain.Root : null;
				int num3 = (transform != null) ? Codec.HashTransformHierarchy(transform) : -1;
				if (!flag && transform != null && chain.m_hierarchyHash != num3)
				{
					flag = true;
				}
				if (flag)
				{
					if (transform == null)
					{
						this.BoneData[j] = null;
					}
					else
					{
						chain.m_scannedRoot = chain.Root;
						chain.m_scannedExclusion = chain.Exclusion.ToArray<Transform>();
						chain.m_hierarchyHash = num3;
						chain.MaxLengthFromRoot = 0f;
						List<BoingBones.Bone> list = new List<BoingBones.Bone>();
						queue.Enqueue(new BoingBones.RescanEntry(transform, -1, 0f));
						while (queue.Count > 0)
						{
							BoingBones.RescanEntry rescanEntry = queue.Dequeue();
							if (!chain.Exclusion.Contains(rescanEntry.Transform))
							{
								int count = list.Count;
								Transform transform2 = rescanEntry.Transform;
								int[] array2 = new int[transform2.childCount];
								for (int l = 0; l < array2.Length; l++)
								{
									array2[l] = -1;
								}
								int num4 = 0;
								int m = 0;
								int childCount = transform2.childCount;
								while (m < childCount)
								{
									Transform child = transform2.GetChild(m);
									if (!chain.Exclusion.Contains(child))
									{
										float num5 = Vector3.Distance(rescanEntry.Transform.position, child.position);
										float lengthFromRoot = rescanEntry.LengthFromRoot + num5;
										queue.Enqueue(new BoingBones.RescanEntry(child, count, lengthFromRoot));
										num4++;
									}
									m++;
								}
								chain.MaxLengthFromRoot = Mathf.Max(rescanEntry.LengthFromRoot, chain.MaxLengthFromRoot);
								BoingBones.Bone bone = new BoingBones.Bone(transform2, rescanEntry.ParentIndex, rescanEntry.LengthFromRoot);
								if (num4 > 0)
								{
									bone.ChildIndices = array2;
								}
								list.Add(bone);
							}
						}
						for (int n = 0; n < list.Count; n++)
						{
							BoingBones.Bone bone2 = list[n];
							if (bone2.ParentIndex >= 0)
							{
								BoingBones.Bone bone3 = list[bone2.ParentIndex];
								int num6 = 0;
								while (bone3.ChildIndices[num6] >= 0)
								{
									num6++;
								}
								if (num6 < bone3.ChildIndices.Length)
								{
									bone3.ChildIndices[num6] = n;
								}
							}
						}
						if (list.Count != 0)
						{
							float num7 = MathUtil.InvSafe(chain.MaxLengthFromRoot);
							for (int num8 = 0; num8 < list.Count; num8++)
							{
								BoingBones.Bone bone4 = list[num8];
								float t = Mathf.Clamp01(bone4.LengthFromRoot * num7);
								bone4.CollisionRadius = chain.MaxCollisionRadius * BoingBones.Chain.EvaluateCurve(chain.CollisionRadiusCurveType, t, chain.CollisionRadiusCustomCurve);
							}
							this.BoneData[j] = list.ToArray();
							this.Reboot(j);
						}
					}
				}
			}
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x0019269C File Offset: 0x0019089C
		private void UpdateCollisionRadius()
		{
			for (int i = 0; i < this.BoneData.Length; i++)
			{
				BoingBones.Chain chain = this.BoneChains[i];
				BoingBones.Bone[] array = this.BoneData[i];
				if (array != null)
				{
					float num = MathUtil.InvSafe(chain.MaxLengthFromRoot);
					foreach (BoingBones.Bone bone in array)
					{
						float t = Mathf.Clamp01(bone.LengthFromRoot * num);
						bone.CollisionRadius = chain.MaxCollisionRadius * BoingBones.Chain.EvaluateCurve(chain.CollisionRadiusCurveType, t, chain.CollisionRadiusCustomCurve);
					}
				}
			}
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x00192724 File Offset: 0x00190924
		public override void Reboot()
		{
			base.Reboot();
			for (int i = 0; i < this.BoneData.Length; i++)
			{
				this.Reboot(i);
			}
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x00192754 File Offset: 0x00190954
		public void Reboot(int iChain)
		{
			BoingBones.Bone[] array = this.BoneData[iChain];
			if (array == null)
			{
				return;
			}
			foreach (BoingBones.Bone bone in array)
			{
				bone.Instance.PositionSpring.Reset(bone.Transform.position);
				bone.Instance.RotationSpring.Reset(bone.Transform.rotation);
				bone.CachedPositionWs = bone.Transform.position;
				bone.CachedPositionLs = bone.Transform.localPosition;
				bone.CachedRotationWs = bone.Transform.rotation;
				bone.CachedRotationLs = bone.Transform.localRotation;
				bone.CachedScaleLs = bone.Transform.localScale;
			}
			this.CachedTransformValid = true;
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06005201 RID: 20993 RVA: 0x00192818 File Offset: 0x00190A18
		internal float MinScale
		{
			get
			{
				return this.m_minScale;
			}
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x00192820 File Offset: 0x00190A20
		public override void PrepareExecute()
		{
			base.PrepareExecute();
			this.Params.Bits.SetBit(4, false);
			float fixedDeltaTime = Time.fixedDeltaTime;
			float d = (this.UpdateMode == BoingManager.UpdateMode.FixedUpdate) ? fixedDeltaTime : Time.deltaTime;
			this.m_minScale = Mathf.Min(base.transform.localScale.x, Mathf.Min(base.transform.localScale.y, base.transform.localScale.z));
			for (int i = 0; i < this.BoneData.Length; i++)
			{
				BoingBones.Chain chain = this.BoneChains[i];
				BoingBones.Bone[] array = this.BoneData[i];
				if (array != null && !(chain.Root == null) && array.Length != 0)
				{
					Vector3 b = chain.Gravity * d;
					float num = 0f;
					foreach (BoingBones.Bone bone in array)
					{
						if (bone.ParentIndex < 0)
						{
							if (!chain.LooseRoot)
							{
								bone.Instance.PositionSpring.Reset(bone.Transform.position);
								bone.Instance.RotationSpring.Reset(bone.Transform.rotation);
							}
							bone.LengthFromRoot = 0f;
						}
						else
						{
							BoingBones.Bone bone2 = array[bone.ParentIndex];
							float num2 = Vector3.Distance(bone.Transform.position, bone2.Transform.position);
							bone.LengthFromRoot = bone2.LengthFromRoot + num2;
							num = Mathf.Max(num, bone.LengthFromRoot);
						}
					}
					float num3 = MathUtil.InvSafe(num);
					foreach (BoingBones.Bone bone3 in array)
					{
						float t = bone3.LengthFromRoot * num3;
						bone3.AnimationBlend = BoingBones.Chain.EvaluateCurve(chain.AnimationBlendCurveType, t, chain.AnimationBlendCustomCurve);
						bone3.LengthStiffness = BoingBones.Chain.EvaluateCurve(chain.LengthStiffnessCurveType, t, chain.LengthStiffnessCustomCurve);
						bone3.LengthStiffnessT = 1f - Mathf.Pow(1f - bone3.LengthStiffness, 30f * fixedDeltaTime);
						bone3.FullyStiffToParentLength = ((bone3.ParentIndex >= 0) ? Vector3.Distance(array[bone3.ParentIndex].Transform.position, bone3.Transform.position) : 0f);
						bone3.PoseStiffness = BoingBones.Chain.EvaluateCurve(chain.PoseStiffnessCurveType, t, chain.PoseStiffnessCustomCurve);
						bone3.BendAngleCap = chain.MaxBendAngleCap * MathUtil.Deg2Rad * BoingBones.Chain.EvaluateCurve(chain.BendAngleCapCurveType, t, chain.BendAngleCapCustomCurve);
						bone3.CollisionRadius = chain.MaxCollisionRadius * BoingBones.Chain.EvaluateCurve(chain.CollisionRadiusCurveType, t, chain.CollisionRadiusCustomCurve);
						bone3.SquashAndStretch = BoingBones.Chain.EvaluateCurve(chain.SquashAndStretchCurveType, t, chain.SquashAndStretchCustomCurve);
					}
					Vector3 position = array[0].Transform.position;
					for (int l = 0; l < array.Length; l++)
					{
						BoingBones.Bone bone4 = array[l];
						float t2 = bone4.LengthFromRoot * num3;
						bone4.AnimationBlend = BoingBones.Chain.EvaluateCurve(chain.AnimationBlendCurveType, t2, chain.AnimationBlendCustomCurve);
						bone4.LengthStiffness = BoingBones.Chain.EvaluateCurve(chain.LengthStiffnessCurveType, t2, chain.LengthStiffnessCustomCurve);
						bone4.PoseStiffness = BoingBones.Chain.EvaluateCurve(chain.PoseStiffnessCurveType, t2, chain.PoseStiffnessCustomCurve);
						bone4.BendAngleCap = chain.MaxBendAngleCap * MathUtil.Deg2Rad * BoingBones.Chain.EvaluateCurve(chain.BendAngleCapCurveType, t2, chain.BendAngleCapCustomCurve);
						bone4.CollisionRadius = chain.MaxCollisionRadius * BoingBones.Chain.EvaluateCurve(chain.CollisionRadiusCurveType, t2, chain.CollisionRadiusCustomCurve);
						bone4.SquashAndStretch = BoingBones.Chain.EvaluateCurve(chain.SquashAndStretchCurveType, t2, chain.SquashAndStretchCustomCurve);
						if (l > 0)
						{
							BoingBones.Bone bone5 = bone4;
							bone5.Instance.PositionSpring.Velocity = bone5.Instance.PositionSpring.Velocity + b;
						}
						bone4.RotationInverseWs = Quaternion.Inverse(bone4.Transform.rotation);
						bone4.SpringRotationWs = bone4.Instance.RotationSpring.ValueQuat;
						bone4.SpringRotationInverseWs = Quaternion.Inverse(bone4.SpringRotationWs);
						Vector3 vector = bone4.Transform.position;
						Quaternion rotation = bone4.Transform.rotation;
						Vector3 localScale = bone4.Transform.localScale;
						if (bone4.ParentIndex >= 0)
						{
							BoingBones.Bone bone6 = array[bone4.ParentIndex];
							Vector3 position2 = bone6.Transform.position;
							Vector3 value = bone6.Instance.PositionSpring.Value;
							Vector3 a = bone6.SpringRotationInverseWs * (bone4.Instance.PositionSpring.Value - value);
							Quaternion a2 = bone6.SpringRotationInverseWs * bone4.Instance.RotationSpring.ValueQuat;
							Vector3 position3 = bone4.Transform.position;
							Quaternion rotation2 = bone4.Transform.rotation;
							Vector3 b2 = bone6.RotationInverseWs * (position3 - position2);
							Quaternion b3 = bone6.RotationInverseWs * rotation2;
							float poseStiffness = bone4.PoseStiffness;
							Vector3 point = Vector3.Lerp(a, b2, poseStiffness);
							Quaternion rhs = Quaternion.Slerp(a2, b3, poseStiffness);
							vector = value + bone6.SpringRotationWs * point;
							rotation = bone6.SpringRotationWs * rhs;
							if (bone4.BendAngleCap < MathUtil.Pi - MathUtil.Epsilon)
							{
								Vector3 vector2 = vector - position;
								vector2 = VectorUtil.ClampBend(vector2, position3 - position, bone4.BendAngleCap);
								vector = position + vector2;
							}
						}
						if (chain.ParamsOverride == null)
						{
							bone4.Instance.PrepareExecute(ref this.Params, vector, rotation, localScale, true);
						}
						else
						{
							bone4.Instance.PrepareExecute(ref chain.ParamsOverride.Params, vector, rotation, localScale, true);
						}
					}
				}
			}
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x00192E20 File Offset: 0x00191020
		public void AccumulateTarget(ref BoingEffector.Params effector, float dt)
		{
			for (int i = 0; i < this.BoneData.Length; i++)
			{
				BoingBones.Chain chain = this.BoneChains[i];
				BoingBones.Bone[] array = this.BoneData[i];
				if (array != null && chain.EffectorReaction)
				{
					foreach (BoingBones.Bone bone in array)
					{
						if (chain.ParamsOverride == null)
						{
							bone.Instance.AccumulateTarget(ref this.Params, ref effector, dt);
						}
						else
						{
							Bits32 bits = chain.ParamsOverride.Params.Bits;
							chain.ParamsOverride.Params.Bits = this.Params.Bits;
							bone.Instance.AccumulateTarget(ref chain.ParamsOverride.Params, ref effector, dt);
							chain.ParamsOverride.Params.Bits = bits;
						}
					}
				}
			}
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x00192F08 File Offset: 0x00191108
		public void EndAccumulateTargets()
		{
			for (int i = 0; i < this.BoneData.Length; i++)
			{
				BoingBones.Chain chain = this.BoneChains[i];
				BoingBones.Bone[] array = this.BoneData[i];
				if (array != null)
				{
					foreach (BoingBones.Bone bone in array)
					{
						if (chain.ParamsOverride == null)
						{
							bone.Instance.EndAccumulateTargets(ref this.Params);
						}
						else
						{
							bone.Instance.EndAccumulateTargets(ref chain.ParamsOverride.Params);
						}
					}
				}
			}
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x00192F8C File Offset: 0x0019118C
		public override void Restore()
		{
			if (!this.CachedTransformValid)
			{
				return;
			}
			for (int i = 0; i < this.BoneData.Length; i++)
			{
				BoingBones.Chain chain = this.BoneChains[i];
				BoingBones.Bone[] array = this.BoneData[i];
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						BoingBones.Bone bone = array[j];
						if (j != 0 || chain.LooseRoot)
						{
							bone.Transform.localPosition = bone.CachedPositionLs;
							bone.Transform.localRotation = bone.CachedRotationLs;
							bone.Transform.localScale = bone.CachedScaleLs;
						}
					}
				}
			}
		}

		// Token: 0x04005434 RID: 21556
		[SerializeField]
		internal BoingBones.Bone[][] BoneData;

		// Token: 0x04005435 RID: 21557
		public BoingBones.Chain[] BoneChains = new BoingBones.Chain[1];

		// Token: 0x04005436 RID: 21558
		public bool TwistPropagation = true;

		// Token: 0x04005437 RID: 21559
		[Range(0.1f, 20f)]
		public float MaxCollisionResolutionSpeed = 3f;

		// Token: 0x04005438 RID: 21560
		public BoingBoneCollider[] BoingColliders = new BoingBoneCollider[0];

		// Token: 0x04005439 RID: 21561
		public Collider[] UnityColliders = new Collider[0];

		// Token: 0x0400543A RID: 21562
		public bool DebugDrawRawBones;

		// Token: 0x0400543B RID: 21563
		public bool DebugDrawTargetBones;

		// Token: 0x0400543C RID: 21564
		public bool DebugDrawBoingBones;

		// Token: 0x0400543D RID: 21565
		public bool DebugDrawFinalBones;

		// Token: 0x0400543E RID: 21566
		public bool DebugDrawColliders;

		// Token: 0x0400543F RID: 21567
		public bool DebugDrawChainBounds;

		// Token: 0x04005440 RID: 21568
		public bool DebugDrawBoneNames;

		// Token: 0x04005441 RID: 21569
		public bool DebugDrawLengthFromRoot;

		// Token: 0x04005442 RID: 21570
		private float m_minScale = 1f;

		// Token: 0x02000CB0 RID: 3248
		[Serializable]
		public class Bone
		{
			// Token: 0x06005207 RID: 20999 RVA: 0x00193078 File Offset: 0x00191278
			internal void UpdateBounds()
			{
				this.Bounds = new Bounds(this.Instance.PositionSpring.Value, 2f * this.CollisionRadius * Vector3.one);
			}

			// Token: 0x06005208 RID: 21000 RVA: 0x001930AC File Offset: 0x001912AC
			internal Bone(Transform transform, int iParent, float lengthFromRoot)
			{
				this.Transform = transform;
				this.RotationInverseWs = Quaternion.identity;
				this.ParentIndex = iParent;
				this.LengthFromRoot = lengthFromRoot;
				this.Instance.Reset();
				this.CachedPositionWs = transform.position;
				this.CachedPositionLs = transform.localPosition;
				this.CachedRotationWs = transform.rotation;
				this.CachedRotationLs = transform.localRotation;
				this.CachedScaleLs = transform.localScale;
				this.AnimationBlend = 0f;
				this.LengthStiffness = 0f;
				this.PoseStiffness = 0f;
				this.BendAngleCap = 180f;
				this.CollisionRadius = 0f;
			}

			// Token: 0x04005443 RID: 21571
			internal BoingWork.Params.InstanceData Instance;

			// Token: 0x04005444 RID: 21572
			internal Transform Transform;

			// Token: 0x04005445 RID: 21573
			internal Vector3 ScaleWs;

			// Token: 0x04005446 RID: 21574
			internal Vector3 CachedScaleLs;

			// Token: 0x04005447 RID: 21575
			internal Vector3 BlendedPositionWs;

			// Token: 0x04005448 RID: 21576
			internal Vector3 BlendedScaleLs;

			// Token: 0x04005449 RID: 21577
			internal Vector3 CachedPositionWs;

			// Token: 0x0400544A RID: 21578
			internal Vector3 CachedPositionLs;

			// Token: 0x0400544B RID: 21579
			internal Bounds Bounds;

			// Token: 0x0400544C RID: 21580
			internal Quaternion RotationInverseWs;

			// Token: 0x0400544D RID: 21581
			internal Quaternion SpringRotationWs;

			// Token: 0x0400544E RID: 21582
			internal Quaternion SpringRotationInverseWs;

			// Token: 0x0400544F RID: 21583
			internal Quaternion CachedRotationWs;

			// Token: 0x04005450 RID: 21584
			internal Quaternion CachedRotationLs;

			// Token: 0x04005451 RID: 21585
			internal Quaternion BlendedRotationWs;

			// Token: 0x04005452 RID: 21586
			internal Quaternion RotationBackPropDeltaPs;

			// Token: 0x04005453 RID: 21587
			internal int ParentIndex;

			// Token: 0x04005454 RID: 21588
			internal int[] ChildIndices;

			// Token: 0x04005455 RID: 21589
			internal float LengthFromRoot;

			// Token: 0x04005456 RID: 21590
			internal float AnimationBlend;

			// Token: 0x04005457 RID: 21591
			internal float LengthStiffness;

			// Token: 0x04005458 RID: 21592
			internal float LengthStiffnessT;

			// Token: 0x04005459 RID: 21593
			internal float FullyStiffToParentLength;

			// Token: 0x0400545A RID: 21594
			internal float PoseStiffness;

			// Token: 0x0400545B RID: 21595
			internal float BendAngleCap;

			// Token: 0x0400545C RID: 21596
			internal float CollisionRadius;

			// Token: 0x0400545D RID: 21597
			internal float SquashAndStretch;
		}

		// Token: 0x02000CB1 RID: 3249
		[Serializable]
		public class Chain
		{
			// Token: 0x06005209 RID: 21001 RVA: 0x00193160 File Offset: 0x00191360
			public static float EvaluateCurve(BoingBones.Chain.CurveType type, float t, AnimationCurve curve)
			{
				switch (type)
				{
				case BoingBones.Chain.CurveType.ConstantOne:
					return 1f;
				case BoingBones.Chain.CurveType.ConstantHalf:
					return 0.5f;
				case BoingBones.Chain.CurveType.ConstantZero:
					return 0f;
				case BoingBones.Chain.CurveType.RootOneTailHalf:
					return 1f - 0.5f * Mathf.Clamp01(t);
				case BoingBones.Chain.CurveType.RootOneTailZero:
					return 1f - Mathf.Clamp01(t);
				case BoingBones.Chain.CurveType.RootHalfTailOne:
					return 0.5f + 0.5f * Mathf.Clamp01(t);
				case BoingBones.Chain.CurveType.RootZeroTailOne:
					return Mathf.Clamp01(t);
				case BoingBones.Chain.CurveType.Custom:
					return curve.Evaluate(t);
				default:
					return 0f;
				}
			}

			// Token: 0x0400545E RID: 21598
			[Tooltip("Root Transform object from which to build a chain (or tree if a bone has multiple children) of bouncy boing bones.")]
			public Transform Root;

			// Token: 0x0400545F RID: 21599
			[Tooltip("List of Transform objects to exclude from chain building.")]
			public Transform[] Exclusion;

			// Token: 0x04005460 RID: 21600
			[Tooltip("Enable to allow reaction to boing effectors.")]
			public bool EffectorReaction = true;

			// Token: 0x04005461 RID: 21601
			[Tooltip("Enable to allow root Transform object to be sprung around as well. Otherwise, no effects will be applied to the root Transform object.")]
			public bool LooseRoot;

			// Token: 0x04005462 RID: 21602
			[Tooltip("Assign a SharedParamsOverride asset to override the parameters for this chain. Useful for chains using different parameters than that of the BoingBones component.")]
			public SharedBoingParams ParamsOverride;

			// Token: 0x04005463 RID: 21603
			[ConditionalField(null, null, null, null, null, null, null, Label = "Animation Blend", Tooltip = "Animation blend determines each bone's final transform between the original raw transform and its corresponding boing bone. 1.0 means 100% contribution from raw (or animated) transform. 0.0 means 100% contribution from boing bone.\n\nEach curve type provides a type of mapping for each bone's percentage down the chain (0.0 at root & 1.0 at maximum chain length) to the bone's animation blend:\n\n - Constant One: 1.0 all the way.\n - Constant Half: 0.5 all the way.\n - Constant Zero: 0.0 all the way.\n - Root One Tail Half: 1.0 at 0% chain length and 0.5 at 100% chain length.\n - Root One Tail Zero: 1.0 at 0% chain length and 0.0 at 100% chain length.\n - Root Half Tail One: 0.5 at 0% chain length and 1.0 at 100% chain length.\n - Root Zero Tail One: 0.0 at 0% chain length and 1.0 at 100% chain length.\n - Custom: Custom curve.")]
			public BoingBones.Chain.CurveType AnimationBlendCurveType = BoingBones.Chain.CurveType.RootOneTailZero;

			// Token: 0x04005464 RID: 21604
			[ConditionalField("AnimationBlendCurveType", BoingBones.Chain.CurveType.Custom, null, null, null, null, null, Label = "  Custom Curve")]
			public AnimationCurve AnimationBlendCustomCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

			// Token: 0x04005465 RID: 21605
			[ConditionalField(null, null, null, null, null, null, null, Label = "Length Stiffness", Tooltip = "Length stiffness determines how much each target bone (target transform each boing bone is sprung towards) tries to maintain original distance from its parent. 1.0 means 100% distance maintenance. 0.0 means 0% distance maintenance.\n\nEach curve type provides a type of mapping for each bone's percentage down the chain (0.0 at root & 1.0 at maximum chain length) to the bone's length stiffness:\n\n - Constant One: 1.0 all the way.\n - Constant Half: 0.5 all the way.\n - Constant Zero: 0.0 all the way.\n - Root One Tail Half: 1.0 at 0% chain length and 0.5 at 100% chain length.\n - Root One Tail Zero: 1.0 at 0% chain length and 0.0 at 100% chain length.\n - Root Half Tail One: 0.5 at 0% chain length and 1.0 at 100% chain length.\n - Root Zero Tail One: 0.0 at 0% chain length and 1.0 at 100% chain length.\n - Custom: Custom curve.")]
			public BoingBones.Chain.CurveType LengthStiffnessCurveType;

			// Token: 0x04005466 RID: 21606
			[ConditionalField("LengthStiffnessCurveType", BoingBones.Chain.CurveType.Custom, null, null, null, null, null, Label = "  Custom Curve")]
			public AnimationCurve LengthStiffnessCustomCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

			// Token: 0x04005467 RID: 21607
			[ConditionalField(null, null, null, null, null, null, null, Label = "Pose Stiffness", Tooltip = "Pose stiffness determines how much each target bone (target transform each boing bone is sprung towards) tries to maintain original transform. 1.0 means 100% original transform maintenance. 0.0 means 0% original transform maintenance.\n\nEach curve type provides a type of mapping for each bone's percentage down the chain (0.0 at root & 1.0 at maximum chain length) to the bone's pose stiffness:\n\n - Constant One: 1.0 all the way.\n - Constant Half: 0.5 all the way.\n - Constant Zero: 0.0 all the way.\n - Root One Tail Half: 1.0 at 0% chain length and 0.5 at 100% chain length.\n - Root One Tail Zero: 1.0 at 0% chain length and 0.0 at 100% chain length.\n - Root Half Tail One: 0.5 at 0% chain length and 1.0 at 100% chain length.\n - Root Zero Tail One: 0.0 at 0% chain length and 1.0 at 100% chain length.\n - Custom: Custom curve.")]
			public BoingBones.Chain.CurveType PoseStiffnessCurveType;

			// Token: 0x04005468 RID: 21608
			[ConditionalField("PoseStiffnessCurveType", BoingBones.Chain.CurveType.Custom, null, null, null, null, null, Label = "  Custom Curve")]
			public AnimationCurve PoseStiffnessCustomCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

			// Token: 0x04005469 RID: 21609
			[ConditionalField(null, null, null, null, null, null, null, Label = "Bend Angle Cap", Tooltip = "Maximum bone bend angle cap.", Min = 0f, Max = 180f)]
			public float MaxBendAngleCap = 180f;

			// Token: 0x0400546A RID: 21610
			[ConditionalField(null, null, null, null, null, null, null, Label = "  Curve Type", Tooltip = "Percentage(0.0 = 0 %; 1.0 = 100 %) of maximum bone bend angle cap.Bend angle cap limits how much each bone can bend relative to the root (in degrees). 1.0 means 100% maximum bend angle cap. 0.0 means 0% maximum bend angle cap.\n\nEach curve type provides a type of mapping for each bone's percentage down the chain (0.0 at root & 1.0 at maximum chain length) to the bone's pose stiffness:\n\n - Constant One: 1.0 all the way.\n - Constant Half: 0.5 all the way.\n - Constant Zero: 0.0 all the way.\n - Root One Tail Half: 1.0 at 0% chain length and 0.5 at 100% chain length.\n - Root One Tail Zero: 1.0 at 0% chain length and 0.0 at 100% chain length.\n - Root Half Tail One: 0.5 at 0% chain length and 1.0 at 100% chain length.\n - Root Zero Tail One: 0.0 at 0% chain length and 1.0 at 100% chain length.\n - Custom: Custom curve.")]
			public BoingBones.Chain.CurveType BendAngleCapCurveType;

			// Token: 0x0400546B RID: 21611
			[ConditionalField("BendAngleCapCurveType", BoingBones.Chain.CurveType.Custom, null, null, null, null, null, Label = "    Custom Curve")]
			public AnimationCurve BendAngleCapCustomCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

			// Token: 0x0400546C RID: 21612
			[ConditionalField(null, null, null, null, null, null, null, Label = "Collision Radius", Tooltip = "Maximum bone collision radius.")]
			public float MaxCollisionRadius = 0.1f;

			// Token: 0x0400546D RID: 21613
			[ConditionalField(null, null, null, null, null, null, null, Label = "  Curve Type", Tooltip = "Percentage (0.0 = 0%; 1.0 = 100%) of maximum bone collision radius.\n\nEach curve type provides a type of mapping for each bone's percentage down the chain (0.0 at root & 1.0 at maximum chain length) to the bone's collision radius:\n\n - Constant One: 1.0 all the way.\n - Constant Half: 0.5 all the way.\n - Constant Zero: 0.0 all the way.\n - Root One Tail Half: 1.0 at 0% chain length and 0.5 at 100% chain length.\n - Root One Tail Zero: 1.0 at 0% chain length and 0.0 at 100% chain length.\n - Root Half Tail One: 0.5 at 0% chain length and 1.0 at 100% chain length.\n - Root Zero Tail One: 0.0 at 0% chain length and 1.0 at 100% chain length.\n - Custom: Custom curve.")]
			public BoingBones.Chain.CurveType CollisionRadiusCurveType;

			// Token: 0x0400546E RID: 21614
			[ConditionalField("CollisionRadiusCurveType", BoingBones.Chain.CurveType.Custom, null, null, null, null, null, Label = "    Custom Curve")]
			public AnimationCurve CollisionRadiusCustomCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

			// Token: 0x0400546F RID: 21615
			[ConditionalField(null, null, null, null, null, null, null, Label = "Boing Kit Collision", Tooltip = "Enable to allow this chain to collide with Boing Kit's own implementation of lightweight colliders")]
			public bool EnableBoingKitCollision;

			// Token: 0x04005470 RID: 21616
			[ConditionalField(null, null, null, null, null, null, null, Label = "Unity Collision", Tooltip = "Enable to allow this chain to collide with Unity colliders.")]
			public bool EnableUnityCollision;

			// Token: 0x04005471 RID: 21617
			[ConditionalField(null, null, null, null, null, null, null, Label = "Inter-Chain Collision", Tooltip = "Enable to allow this chain to collide with other chain (under the same BoingBones component) with inter-chain collision enabled.")]
			public bool EnableInterChainCollision;

			// Token: 0x04005472 RID: 21618
			public Vector3 Gravity = Vector3.zero;

			// Token: 0x04005473 RID: 21619
			internal Bounds Bounds;

			// Token: 0x04005474 RID: 21620
			[ConditionalField(null, null, null, null, null, null, null, Label = "Squash & Stretch", Tooltip = "Percentage (0.0 = 0%; 1.0 = 100%) of each bone's squash & stretch effect. Squash & stretch is the effect of volume preservation by scaling bones based on how compressed or stretched the distances between bones become.\n\nEach curve type provides a type of mapping for each bone's percentage down the chain (0.0 at root & 1.0 at maximum chain length) to the bone's squash & stretch effect amount:\n\n - Constant One: 1.0 all the way.\n - Constant Half: 0.5 all the way.\n - Constant Zero: 0.0 all the way.\n - Root One Tail Half: 1.0 at 0% chain length and 0.5 at 100% chain length.\n - Root One Tail Zero: 1.0 at 0% chain length and 0.0 at 100% chain length.\n - Root Half Tail One: 0.5 at 0% chain length and 1.0 at 100% chain length.\n - Root Zero Tail One: 0.0 at 0% chain length and 1.0 at 100% chain length.\n - Custom: Custom curve.")]
			public BoingBones.Chain.CurveType SquashAndStretchCurveType = BoingBones.Chain.CurveType.ConstantZero;

			// Token: 0x04005475 RID: 21621
			[ConditionalField("SquashAndStretchCurveType", BoingBones.Chain.CurveType.Custom, null, null, null, null, null, Label = "  Custom Curve")]
			public AnimationCurve SquashAndStretchCustomCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

			// Token: 0x04005476 RID: 21622
			[ConditionalField(null, null, null, null, null, null, null, Label = "  Max Squash", Tooltip = "Maximum squash amount. For example, 2.0 means a maximum scale of 200% when squashed.", Min = 1f, Max = 5f)]
			public float MaxSquash = 1.1f;

			// Token: 0x04005477 RID: 21623
			[ConditionalField(null, null, null, null, null, null, null, Label = "  Max Stretch", Tooltip = "Maximum stretch amount. For example, 2.0 means a minimum scale of 50% when stretched (200% stretched).", Min = 1f, Max = 5f)]
			public float MaxStretch = 2f;

			// Token: 0x04005478 RID: 21624
			internal Transform m_scannedRoot;

			// Token: 0x04005479 RID: 21625
			internal Transform[] m_scannedExclusion;

			// Token: 0x0400547A RID: 21626
			internal int m_hierarchyHash = -1;

			// Token: 0x0400547B RID: 21627
			internal float MaxLengthFromRoot;

			// Token: 0x02000CB2 RID: 3250
			public enum CurveType
			{
				// Token: 0x0400547D RID: 21629
				ConstantOne,
				// Token: 0x0400547E RID: 21630
				ConstantHalf,
				// Token: 0x0400547F RID: 21631
				ConstantZero,
				// Token: 0x04005480 RID: 21632
				RootOneTailHalf,
				// Token: 0x04005481 RID: 21633
				RootOneTailZero,
				// Token: 0x04005482 RID: 21634
				RootHalfTailOne,
				// Token: 0x04005483 RID: 21635
				RootZeroTailOne,
				// Token: 0x04005484 RID: 21636
				Custom
			}
		}

		// Token: 0x02000CB3 RID: 3251
		private class RescanEntry
		{
			// Token: 0x0600520B RID: 21003 RVA: 0x00193310 File Offset: 0x00191510
			internal RescanEntry(Transform transform, int iParent, float lengthFromRoot)
			{
				this.Transform = transform;
				this.ParentIndex = iParent;
				this.LengthFromRoot = lengthFromRoot;
			}

			// Token: 0x04005485 RID: 21637
			internal Transform Transform;

			// Token: 0x04005486 RID: 21638
			internal int ParentIndex;

			// Token: 0x04005487 RID: 21639
			internal float LengthFromRoot;
		}
	}
}
