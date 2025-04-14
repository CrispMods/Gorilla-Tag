using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CAF RID: 3247
	public class BoingBehavior : BoingBase
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060051E8 RID: 20968 RVA: 0x00191C87 File Offset: 0x0018FE87
		// (set) Token: 0x060051E9 RID: 20969 RVA: 0x00191C99 File Offset: 0x0018FE99
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

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060051EA RID: 20970 RVA: 0x00191CB3 File Offset: 0x0018FEB3
		// (set) Token: 0x060051EB RID: 20971 RVA: 0x00191CC5 File Offset: 0x0018FEC5
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

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060051EC RID: 20972 RVA: 0x00191CDF File Offset: 0x0018FEDF
		// (set) Token: 0x060051ED RID: 20973 RVA: 0x00191CF1 File Offset: 0x0018FEF1
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

		// Token: 0x060051EE RID: 20974 RVA: 0x00191D0B File Offset: 0x0018FF0B
		public BoingBehavior()
		{
			this.Params.Init();
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x00191D34 File Offset: 0x0018FF34
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

		// Token: 0x060051F0 RID: 20976 RVA: 0x00191DFD File Offset: 0x0018FFFD
		public virtual void OnEnable()
		{
			this.CachedTransformValid = false;
			this.InitRebooted = false;
			this.Register();
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x00191E13 File Offset: 0x00190013
		public void Start()
		{
			this.InitRebooted = false;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x00191E1C File Offset: 0x0019001C
		public virtual void OnDisable()
		{
			this.Unregister();
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x00191E24 File Offset: 0x00190024
		protected virtual void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x00191E2C File Offset: 0x0019002C
		protected virtual void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x00191E34 File Offset: 0x00190034
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

		// Token: 0x060051F6 RID: 20982 RVA: 0x00191F33 File Offset: 0x00190133
		public virtual void PrepareExecute()
		{
			this.PrepareExecute(false);
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x00191F3C File Offset: 0x0019013C
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

		// Token: 0x060051F8 RID: 20984 RVA: 0x00191FB2 File Offset: 0x001901B2
		public void Execute(float dt)
		{
			this.Params.Execute(dt);
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x00191FC0 File Offset: 0x001901C0
		public void PullResults()
		{
			this.PullResults(ref this.Params);
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x00191FD0 File Offset: 0x001901D0
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

		// Token: 0x060051FB RID: 20987 RVA: 0x0019209C File Offset: 0x0019029C
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

		// Token: 0x060051FC RID: 20988 RVA: 0x00192194 File Offset: 0x00190394
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

		// Token: 0x04005423 RID: 21539
		public BoingManager.UpdateMode UpdateMode = BoingManager.UpdateMode.LateUpdate;

		// Token: 0x04005424 RID: 21540
		public bool TwoDDistanceCheck;

		// Token: 0x04005425 RID: 21541
		public bool TwoDPositionInfluence;

		// Token: 0x04005426 RID: 21542
		public bool TwoDRotationInfluence;

		// Token: 0x04005427 RID: 21543
		public bool EnablePositionEffect = true;

		// Token: 0x04005428 RID: 21544
		public bool EnableRotationEffect = true;

		// Token: 0x04005429 RID: 21545
		public bool EnableScaleEffect;

		// Token: 0x0400542A RID: 21546
		public bool GlobalReactionUpVector;

		// Token: 0x0400542B RID: 21547
		public BoingManager.TranslationLockSpace TranslationLockSpace;

		// Token: 0x0400542C RID: 21548
		public bool LockTranslationX;

		// Token: 0x0400542D RID: 21549
		public bool LockTranslationY;

		// Token: 0x0400542E RID: 21550
		public bool LockTranslationZ;

		// Token: 0x0400542F RID: 21551
		public BoingWork.Params Params;

		// Token: 0x04005430 RID: 21552
		public SharedBoingParams SharedParams;

		// Token: 0x04005431 RID: 21553
		internal bool PositionSpringDirty;

		// Token: 0x04005432 RID: 21554
		internal bool RotationSpringDirty;

		// Token: 0x04005433 RID: 21555
		internal bool ScaleSpringDirty;

		// Token: 0x04005434 RID: 21556
		internal bool CachedTransformValid;

		// Token: 0x04005435 RID: 21557
		internal Vector3 CachedPositionLs;

		// Token: 0x04005436 RID: 21558
		internal Vector3 CachedPositionWs;

		// Token: 0x04005437 RID: 21559
		internal Vector3 RenderPositionWs;

		// Token: 0x04005438 RID: 21560
		internal Quaternion CachedRotationLs;

		// Token: 0x04005439 RID: 21561
		internal Quaternion CachedRotationWs;

		// Token: 0x0400543A RID: 21562
		internal Quaternion RenderRotationWs;

		// Token: 0x0400543B RID: 21563
		internal Vector3 CachedScaleLs;

		// Token: 0x0400543C RID: 21564
		internal Vector3 RenderScaleLs;

		// Token: 0x0400543D RID: 21565
		internal bool InitRebooted;
	}
}
