using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CD7 RID: 3287
	public class BoingReactorFieldCPUSampler : MonoBehaviour
	{
		// Token: 0x060052D5 RID: 21205 RVA: 0x00064E69 File Offset: 0x00063069
		public void OnEnable()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x00064E71 File Offset: 0x00063071
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001C4BA0 File Offset: 0x001C2DA0
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

		// Token: 0x060052D8 RID: 21208 RVA: 0x00064E79 File Offset: 0x00063079
		public void Restore()
		{
			base.transform.position = this.m_objPosition;
			base.transform.rotation = this.m_objRotation;
		}

		// Token: 0x0400554C RID: 21836
		public BoingReactorField ReactorField;

		// Token: 0x0400554D RID: 21837
		[Tooltip("Match this mode with how you update your object's transform.\n\nUpdate - Use this mode if you update your object's transform in Update(). This uses variable Time.detalTime. Use FixedUpdate if physics simulation becomes unstable.\n\nFixed Update - Use this mode if you update your object's transform in FixedUpdate(). This uses fixed Time.fixedDeltaTime. Also, use this mode if the game object is affected by Unity physics (i.e. has a rigid body component), which uses fixed updates.")]
		public BoingManager.UpdateMode UpdateMode = BoingManager.UpdateMode.LateUpdate;

		// Token: 0x0400554E RID: 21838
		[Range(0f, 10f)]
		[Tooltip("Multiplier on positional samples from reactor field.\n1.0 means 100%.")]
		public float PositionSampleMultiplier = 1f;

		// Token: 0x0400554F RID: 21839
		[Range(0f, 10f)]
		[Tooltip("Multiplier on rotational samples from reactor field.\n1.0 means 100%.")]
		public float RotationSampleMultiplier = 1f;

		// Token: 0x04005550 RID: 21840
		private Vector3 m_objPosition;

		// Token: 0x04005551 RID: 21841
		private Quaternion m_objRotation;
	}
}
