using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CD4 RID: 3284
	public class BoingReactorFieldCPUSampler : MonoBehaviour
	{
		// Token: 0x060052C9 RID: 21193 RVA: 0x0019701E File Offset: 0x0019521E
		public void OnEnable()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x00197026 File Offset: 0x00195226
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x00197030 File Offset: 0x00195230
		public void SampleFromField()
		{
			this.m_objPosition = base.transform.position;
			this.m_objRotation = base.transform.rotation;
			if (this.ReactorField == null)
			{
				return;
			}
			BoingReactorField component = this.ReactorField.GetComponent<BoingReactorField>();
			if (component == null)
			{
				return;
			}
			if (component.HardwareMode != BoingReactorField.HardwareModeEnum.CPU)
			{
				return;
			}
			Vector3 a;
			Vector4 v;
			if (!component.SampleCpuGrid(base.transform.position, out a, out v))
			{
				return;
			}
			base.transform.position = this.m_objPosition + a * this.PositionSampleMultiplier;
			base.transform.rotation = QuaternionUtil.Pow(QuaternionUtil.FromVector4(v, true), this.RotationSampleMultiplier) * this.m_objRotation;
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x001970EF File Offset: 0x001952EF
		public void Restore()
		{
			base.transform.position = this.m_objPosition;
			base.transform.rotation = this.m_objRotation;
		}

		// Token: 0x0400553A RID: 21818
		public BoingReactorField ReactorField;

		// Token: 0x0400553B RID: 21819
		[Tooltip("Match this mode with how you update your object's transform.\n\nUpdate - Use this mode if you update your object's transform in Update(). This uses variable Time.detalTime. Use FixedUpdate if physics simulation becomes unstable.\n\nFixed Update - Use this mode if you update your object's transform in FixedUpdate(). This uses fixed Time.fixedDeltaTime. Also, use this mode if the game object is affected by Unity physics (i.e. has a rigid body component), which uses fixed updates.")]
		public BoingManager.UpdateMode UpdateMode = BoingManager.UpdateMode.LateUpdate;

		// Token: 0x0400553C RID: 21820
		[Range(0f, 10f)]
		[Tooltip("Multiplier on positional samples from reactor field.\n1.0 means 100%.")]
		public float PositionSampleMultiplier = 1f;

		// Token: 0x0400553D RID: 21821
		[Range(0f, 10f)]
		[Tooltip("Multiplier on rotational samples from reactor field.\n1.0 means 100%.")]
		public float RotationSampleMultiplier = 1f;

		// Token: 0x0400553E RID: 21822
		private Vector3 m_objPosition;

		// Token: 0x0400553F RID: 21823
		private Quaternion m_objRotation;
	}
}
