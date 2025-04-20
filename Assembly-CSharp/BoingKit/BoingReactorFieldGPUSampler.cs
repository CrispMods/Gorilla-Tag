using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D06 RID: 3334
	public class BoingReactorFieldGPUSampler : MonoBehaviour
	{
		// Token: 0x06005430 RID: 21552 RVA: 0x00066938 File Offset: 0x00064B38
		public void OnEnable()
		{
			BoingManager.Register(this);
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x00066940 File Offset: 0x00064B40
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x001CCD44 File Offset: 0x001CAF44
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

		// Token: 0x0400564C RID: 22092
		public BoingReactorField ReactorField;

		// Token: 0x0400564D RID: 22093
		[Range(0f, 10f)]
		[Tooltip("Multiplier on positional samples from reactor field.\n1.0 means 100%.")]
		public float PositionSampleMultiplier = 1f;

		// Token: 0x0400564E RID: 22094
		[Range(0f, 10f)]
		[Tooltip("Multiplier on rotational samples from reactor field.\n1.0 means 100%.")]
		public float RotationSampleMultiplier = 1f;

		// Token: 0x0400564F RID: 22095
		private MaterialPropertyBlock m_matProps;

		// Token: 0x04005650 RID: 22096
		private int m_fieldResourceSetId = -1;
	}
}
