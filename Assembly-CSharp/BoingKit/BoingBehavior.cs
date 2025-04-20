using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CDD RID: 3293
	public class BoingBehavior : BoingBase
	{
		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x0600533E RID: 21310 RVA: 0x00065F83 File Offset: 0x00064183
		// (set) Token: 0x0600533F RID: 21311 RVA: 0x00065F95 File Offset: 0x00064195
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

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06005340 RID: 21312 RVA: 0x00065FAF File Offset: 0x000641AF
		// (set) Token: 0x06005341 RID: 21313 RVA: 0x00065FC1 File Offset: 0x000641C1
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

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06005342 RID: 21314 RVA: 0x00065FDB File Offset: 0x000641DB
		// (set) Token: 0x06005343 RID: 21315 RVA: 0x00065FED File Offset: 0x000641ED
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

		// Token: 0x06005344 RID: 21316 RVA: 0x00066007 File Offset: 0x00064207
		public BoingBehavior()
		{
			this.Params.Init();
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x001C7C80 File Offset: 0x001C5E80
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

		// Token: 0x06005346 RID: 21318 RVA: 0x0006602F File Offset: 0x0006422F
		public virtual void OnEnable()
		{
			this.CachedTransformValid = false;
			this.InitRebooted = false;
			this.Register();
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x00066045 File Offset: 0x00064245
		public void Start()
		{
			this.InitRebooted = false;
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x0006604E File Offset: 0x0006424E
		public virtual void OnDisable()
		{
			this.Unregister();
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x00066056 File Offset: 0x00064256
		protected virtual void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x0006605E File Offset: 0x0006425E
		protected virtual void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x001C7D4C File Offset: 0x001C5F4C
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

		// Token: 0x0600534C RID: 21324 RVA: 0x00066066 File Offset: 0x00064266
		public virtual void PrepareExecute()
		{
			this.PrepareExecute(false);
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x001C7E4C File Offset: 0x001C604C
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

		// Token: 0x0600534E RID: 21326 RVA: 0x0006606F File Offset: 0x0006426F
		public void Execute(float dt)
		{
			this.Params.Execute(dt);
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x0006607D File Offset: 0x0006427D
		public void PullResults()
		{
			this.PullResults(ref this.Params);
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x001C7EC4 File Offset: 0x001C60C4
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

		// Token: 0x06005351 RID: 21329 RVA: 0x001C7F90 File Offset: 0x001C6190
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

		// Token: 0x06005352 RID: 21330 RVA: 0x001C8088 File Offset: 0x001C6288
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

		// Token: 0x0400551D RID: 21789
		public BoingManager.UpdateMode UpdateMode = BoingManager.UpdateMode.LateUpdate;

		// Token: 0x0400551E RID: 21790
		public bool TwoDDistanceCheck;

		// Token: 0x0400551F RID: 21791
		public bool TwoDPositionInfluence;

		// Token: 0x04005520 RID: 21792
		public bool TwoDRotationInfluence;

		// Token: 0x04005521 RID: 21793
		public bool EnablePositionEffect = true;

		// Token: 0x04005522 RID: 21794
		public bool EnableRotationEffect = true;

		// Token: 0x04005523 RID: 21795
		public bool EnableScaleEffect;

		// Token: 0x04005524 RID: 21796
		public bool GlobalReactionUpVector;

		// Token: 0x04005525 RID: 21797
		public BoingManager.TranslationLockSpace TranslationLockSpace;

		// Token: 0x04005526 RID: 21798
		public bool LockTranslationX;

		// Token: 0x04005527 RID: 21799
		public bool LockTranslationY;

		// Token: 0x04005528 RID: 21800
		public bool LockTranslationZ;

		// Token: 0x04005529 RID: 21801
		public BoingWork.Params Params;

		// Token: 0x0400552A RID: 21802
		public SharedBoingParams SharedParams;

		// Token: 0x0400552B RID: 21803
		internal bool PositionSpringDirty;

		// Token: 0x0400552C RID: 21804
		internal bool RotationSpringDirty;

		// Token: 0x0400552D RID: 21805
		internal bool ScaleSpringDirty;

		// Token: 0x0400552E RID: 21806
		internal bool CachedTransformValid;

		// Token: 0x0400552F RID: 21807
		internal Vector3 CachedPositionLs;

		// Token: 0x04005530 RID: 21808
		internal Vector3 CachedPositionWs;

		// Token: 0x04005531 RID: 21809
		internal Vector3 RenderPositionWs;

		// Token: 0x04005532 RID: 21810
		internal Quaternion CachedRotationLs;

		// Token: 0x04005533 RID: 21811
		internal Quaternion CachedRotationWs;

		// Token: 0x04005534 RID: 21812
		internal Quaternion RenderRotationWs;

		// Token: 0x04005535 RID: 21813
		internal Vector3 CachedScaleLs;

		// Token: 0x04005536 RID: 21814
		internal Vector3 RenderScaleLs;

		// Token: 0x04005537 RID: 21815
		internal bool InitRebooted;
	}
}
