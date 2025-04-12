using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CD8 RID: 3288
	public class BoingReactorFieldGPUSampler : MonoBehaviour
	{
		// Token: 0x060052DA RID: 21210 RVA: 0x00064EC2 File Offset: 0x000630C2
		public void OnEnable()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x00064ECA File Offset: 0x000630CA
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001C4C60 File Offset: 0x001C2E60
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

		// Token: 0x04005552 RID: 21842
		public BoingReactorField ReactorField;

		// Token: 0x04005553 RID: 21843
		[Range(0f, 10f)]
		[Tooltip("Multiplier on positional samples from reactor field.\n1.0 means 100%.")]
		public float PositionSampleMultiplier = 1f;

		// Token: 0x04005554 RID: 21844
		[Range(0f, 10f)]
		[Tooltip("Multiplier on rotational samples from reactor field.\n1.0 means 100%.")]
		public float RotationSampleMultiplier = 1f;

		// Token: 0x04005555 RID: 21845
		private MaterialPropertyBlock m_matProps;

		// Token: 0x04005556 RID: 21846
		private int m_fieldResourceSetId = -1;
	}
}
