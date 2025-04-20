using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D05 RID: 3333
	public class BoingReactorFieldCPUSampler : MonoBehaviour
	{
		// Token: 0x0600542B RID: 21547 RVA: 0x000668DF File Offset: 0x00064ADF
		public void OnEnable()
		{
			BoingManager.Register(this);
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x000668E7 File Offset: 0x00064AE7
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x001CCC84 File Offset: 0x001CAE84
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

		// Token: 0x0600542E RID: 21550 RVA: 0x000668EF File Offset: 0x00064AEF
		public void Restore()
		{
			base.transform.position = this.m_objPosition;
			base.transform.rotation = this.m_objRotation;
		}

		// Token: 0x04005646 RID: 22086
		public BoingReactorField ReactorField;

		// Token: 0x04005647 RID: 22087
		[Tooltip("Match this mode with how you update your object's transform.\n\nUpdate - Use this mode if you update your object's transform in Update(). This uses variable Time.detalTime. Use FixedUpdate if physics simulation becomes unstable.\n\nFixed Update - Use this mode if you update your object's transform in FixedUpdate(). This uses fixed Time.fixedDeltaTime. Also, use this mode if the game object is affected by Unity physics (i.e. has a rigid body component), which uses fixed updates.")]
		public BoingManager.UpdateMode UpdateMode = BoingManager.UpdateMode.LateUpdate;

		// Token: 0x04005648 RID: 22088
		[Range(0f, 10f)]
		[Tooltip("Multiplier on positional samples from reactor field.\n1.0 means 100%.")]
		public float PositionSampleMultiplier = 1f;

		// Token: 0x04005649 RID: 22089
		[Range(0f, 10f)]
		[Tooltip("Multiplier on rotational samples from reactor field.\n1.0 means 100%.")]
		public float RotationSampleMultiplier = 1f;

		// Token: 0x0400564A RID: 22090
		private Vector3 m_objPosition;

		// Token: 0x0400564B RID: 22091
		private Quaternion m_objRotation;
	}
}
