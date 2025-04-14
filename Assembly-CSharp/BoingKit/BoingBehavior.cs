using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CAC RID: 3244
	public class BoingBehavior : BoingBase
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060051DC RID: 20956 RVA: 0x001916BF File Offset: 0x0018F8BF
		// (set) Token: 0x060051DD RID: 20957 RVA: 0x001916D1 File Offset: 0x0018F8D1
		public Vector3Spring PositionSpring
		{
			get
			{
				return this.Params.Instance.PositionSpring;
			}
			set
			{
				this.Params.Instance.PositionSpring = value;
				this.PositionSpringDirty = true;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x001916EB File Offset: 0x0018F8EB
		// (set) Token: 0x060051DF RID: 20959 RVA: 0x001916FD File Offset: 0x0018F8FD
		public QuaternionSpring RotationSpring
		{
			get
			{
				return this.Params.Instance.RotationSpring;
			}
			set
			{
				this.Params.Instance.RotationSpring = value;
				this.RotationSpringDirty = true;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060051E0 RID: 20960 RVA: 0x00191717 File Offset: 0x0018F917
		// (set) Token: 0x060051E1 RID: 20961 RVA: 0x00191729 File Offset: 0x0018F929
		public Vector3Spring ScaleSpring
		{
			get
			{
				return this.Params.Instance.ScaleSpring;
			}
			set
			{
				this.Params.Instance.ScaleSpring = value;
				this.ScaleSpringDirty = true;
			}
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x00191743 File Offset: 0x0018F943
		public BoingBehavior()
		{
			this.Params.Init();
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x0019176C File Offset: 0x0018F96C
		public virtual void Reboot()
		{
			this.Params.Instance.PositionSpring.Reset(base.transform.position);
			this.Params.Instance.RotationSpring.Reset(base.transform.rotation);
			this.Params.Instance.ScaleSpring.Reset(base.transform.localScale);
			this.CachedPositionLs = base.transform.localPosition;
			this.CachedRotationLs = base.transform.localRotation;
			this.CachedPositionWs = base.transform.position;
			this.CachedRotationWs = base.transform.rotation;
			this.CachedScaleLs = base.transform.localScale;
			this.CachedTransformValid = true;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x00191835 File Offset: 0x0018FA35
		public virtual void OnEnable()
		{
			this.CachedTransformValid = false;
			this.InitRebooted = false;
			this.Register();
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x0019184B File Offset: 0x0018FA4B
		public void Start()
		{
			this.InitRebooted = false;
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x00191854 File Offset: 0x0018FA54
		public virtual void OnDisable()
		{
			this.Unregister();
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x0019185C File Offset: 0x0018FA5C
		protected virtual void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x00191864 File Offset: 0x0018FA64
		protected virtual void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x0019186C File Offset: 0x0018FA6C
		public void UpdateFlags()
		{
			this.Params.Bits.SetBit(0, this.TwoDDistanceCheck);
			this.Params.Bits.SetBit(1, this.TwoDPositionInfluence);
			this.Params.Bits.SetBit(2, this.TwoDRotationInfluence);
			this.Params.Bits.SetBit(3, this.EnablePositionEffect);
			this.Params.Bits.SetBit(4, this.EnableRotationEffect);
			this.Params.Bits.SetBit(5, this.EnableScaleEffect);
			this.Params.Bits.SetBit(6, this.GlobalReactionUpVector);
			this.Params.Bits.SetBit(9, this.UpdateMode == BoingManager.UpdateMode.FixedUpdate);
			this.Params.Bits.SetBit(10, this.UpdateMode == BoingManager.UpdateMode.EarlyUpdate);
			this.Params.Bits.SetBit(11, this.UpdateMode == BoingManager.UpdateMode.LateUpdate);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x0019196B File Offset: 0x0018FB6B
		public virtual void PrepareExecute()
		{
			this.PrepareExecute(false);
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x00191974 File Offset: 0x0018FB74
		protected void PrepareExecute(bool accumulateEffectors)
		{
			if (this.SharedParams != null)
			{
				BoingWork.Params.Copy(ref this.SharedParams.Params, ref this.Params);
			}
			this.UpdateFlags();
			this.Params.InstanceID = base.GetInstanceID();
			this.Params.Instance.PrepareExecute(ref this.Params, this.CachedPositionWs, this.CachedRotationWs, base.transform.localScale, accumulateEffectors);
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001919EA File Offset: 0x0018FBEA
		public void Execute(float dt)
		{
			this.Params.Execute(dt);
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x001919F8 File Offset: 0x0018FBF8
		public void PullResults()
		{
			this.PullResults(ref this.Params);
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x00191A08 File Offset: 0x0018FC08
		public void GatherOutput(ref BoingWork.Output o)
		{
			if (!BoingManager.UseAsynchronousJobs)
			{
				this.Params.Instance.PositionSpring = o.PositionSpring;
				this.Params.Instance.RotationSpring = o.RotationSpring;
				this.Params.Instance.ScaleSpring = o.ScaleSpring;
				return;
			}
			if (this.PositionSpringDirty)
			{
				this.PositionSpringDirty = false;
			}
			else
			{
				this.Params.Instance.PositionSpring = o.PositionSpring;
			}
			if (this.RotationSpringDirty)
			{
				this.RotationSpringDirty = false;
			}
			else
			{
				this.Params.Instance.RotationSpring = o.RotationSpring;
			}
			if (this.ScaleSpringDirty)
			{
				this.ScaleSpringDirty = false;
				return;
			}
			this.Params.Instance.ScaleSpring = o.ScaleSpring;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x00191AD4 File Offset: 0x0018FCD4
		private void PullResults(ref BoingWork.Params p)
		{
			this.CachedPositionLs = base.transform.localPosition;
			this.CachedPositionWs = base.transform.position;
			this.RenderPositionWs = BoingWork.ComputeTranslationalResults(base.transform, base.transform.position, p.Instance.PositionSpring.Value, this);
			base.transform.position = this.RenderPositionWs;
			this.CachedRotationLs = base.transform.localRotation;
			this.CachedRotationWs = base.transform.rotation;
			this.RenderRotationWs = p.Instance.RotationSpring.ValueQuat;
			base.transform.rotation = this.RenderRotationWs;
			this.CachedScaleLs = base.transform.localScale;
			this.RenderScaleLs = p.Instance.ScaleSpring.Value;
			base.transform.localScale = this.RenderScaleLs;
			this.CachedTransformValid = true;
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x00191BCC File Offset: 0x0018FDCC
		public virtual void Restore()
		{
			if (!this.CachedTransformValid)
			{
				return;
			}
			if (Application.isEditor)
			{
				if ((base.transform.position - this.RenderPositionWs).sqrMagnitude < 0.0001f)
				{
					base.transform.localPosition = this.CachedPositionLs;
				}
				if (QuaternionUtil.GetAngle(base.transform.rotation * Quaternion.Inverse(this.RenderRotationWs)) < 0.01f)
				{
					base.transform.localRotation = this.CachedRotationLs;
				}
				if ((base.transform.localScale - this.RenderScaleLs).sqrMagnitude < 0.0001f)
				{
					base.transform.localScale = this.CachedScaleLs;
					return;
				}
			}
			else
			{
				base.transform.localPosition = this.CachedPositionLs;
				base.transform.localRotation = this.CachedRotationLs;
				base.transform.localScale = this.CachedScaleLs;
			}
		}

		// Token: 0x04005411 RID: 21521
		public BoingManager.UpdateMode UpdateMode = BoingManager.UpdateMode.LateUpdate;

		// Token: 0x04005412 RID: 21522
		public bool TwoDDistanceCheck;

		// Token: 0x04005413 RID: 21523
		public bool TwoDPositionInfluence;

		// Token: 0x04005414 RID: 21524
		public bool TwoDRotationInfluence;

		// Token: 0x04005415 RID: 21525
		public bool EnablePositionEffect = true;

		// Token: 0x04005416 RID: 21526
		public bool EnableRotationEffect = true;

		// Token: 0x04005417 RID: 21527
		public bool EnableScaleEffect;

		// Token: 0x04005418 RID: 21528
		public bool GlobalReactionUpVector;

		// Token: 0x04005419 RID: 21529
		public BoingManager.TranslationLockSpace TranslationLockSpace;

		// Token: 0x0400541A RID: 21530
		public bool LockTranslationX;

		// Token: 0x0400541B RID: 21531
		public bool LockTranslationY;

		// Token: 0x0400541C RID: 21532
		public bool LockTranslationZ;

		// Token: 0x0400541D RID: 21533
		public BoingWork.Params Params;

		// Token: 0x0400541E RID: 21534
		public SharedBoingParams SharedParams;

		// Token: 0x0400541F RID: 21535
		internal bool PositionSpringDirty;

		// Token: 0x04005420 RID: 21536
		internal bool RotationSpringDirty;

		// Token: 0x04005421 RID: 21537
		internal bool ScaleSpringDirty;

		// Token: 0x04005422 RID: 21538
		internal bool CachedTransformValid;

		// Token: 0x04005423 RID: 21539
		internal Vector3 CachedPositionLs;

		// Token: 0x04005424 RID: 21540
		internal Vector3 CachedPositionWs;

		// Token: 0x04005425 RID: 21541
		internal Vector3 RenderPositionWs;

		// Token: 0x04005426 RID: 21542
		internal Quaternion CachedRotationLs;

		// Token: 0x04005427 RID: 21543
		internal Quaternion CachedRotationWs;

		// Token: 0x04005428 RID: 21544
		internal Quaternion RenderRotationWs;

		// Token: 0x04005429 RID: 21545
		internal Vector3 CachedScaleLs;

		// Token: 0x0400542A RID: 21546
		internal Vector3 RenderScaleLs;

		// Token: 0x0400542B RID: 21547
		internal bool InitRebooted;
	}
}
