using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CD5 RID: 3285
	public class BoingReactorFieldGPUSampler : MonoBehaviour
	{
		// Token: 0x060052CE RID: 21198 RVA: 0x00197138 File Offset: 0x00195338
		public void OnEnable()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x00197140 File Offset: 0x00195340
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x00197148 File Offset: 0x00195348
		public void Update()
		{
			if (this.ReactorField == null)
			{
				return;
			}
			BoingReactorField component = this.ReactorField.GetComponent<BoingReactorField>();
			if (component == null)
			{
				return;
			}
			if (component.HardwareMode != BoingReactorField.HardwareModeEnum.GPU)
			{
				return;
			}
			if (this.m_fieldResourceSetId != component.GpuResourceSetId)
			{
				if (this.m_matProps == null)
				{
					this.m_matProps = new MaterialPropertyBlock();
				}
				if (component.UpdateShaderConstants(this.m_matProps, this.PositionSampleMultiplier, this.RotationSampleMultiplier))
				{
					this.m_fieldResourceSetId = component.GpuResourceSetId;
					foreach (Renderer renderer in new Renderer[]
					{
						base.GetComponent<MeshRenderer>(),
						base.GetComponent<SkinnedMeshRenderer>()
					})
					{
						if (!(renderer == null))
						{
							renderer.SetPropertyBlock(this.m_matProps);
						}
					}
				}
			}
		}

		// Token: 0x04005540 RID: 21824
		public BoingReactorField ReactorField;

		// Token: 0x04005541 RID: 21825
		[Range(0f, 10f)]
		[Tooltip("Multiplier on positional samples from reactor field.\n1.0 means 100%.")]
		public float PositionSampleMultiplier = 1f;

		// Token: 0x04005542 RID: 21826
		[Range(0f, 10f)]
		[Tooltip("Multiplier on rotational samples from reactor field.\n1.0 means 100%.")]
		public float RotationSampleMultiplier = 1f;

		// Token: 0x04005543 RID: 21827
		private MaterialPropertyBlock m_matProps;

		// Token: 0x04005544 RID: 21828
		private int m_fieldResourceSetId = -1;
	}
}
