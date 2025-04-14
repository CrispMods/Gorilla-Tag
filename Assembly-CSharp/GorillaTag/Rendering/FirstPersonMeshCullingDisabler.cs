using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C06 RID: 3078
	public class FirstPersonMeshCullingDisabler : MonoBehaviour
	{
		// Token: 0x06004CFF RID: 19711 RVA: 0x00176838 File Offset: 0x00174A38
		protected void Awake()
		{
			MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
			if (componentsInChildren == null)
			{
				return;
			}
			this.meshes = new Mesh[componentsInChildren.Length];
			this.xforms = new Transform[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.meshes[i] = componentsInChildren[i].mesh;
				this.xforms[i] = componentsInChildren[i].transform;
			}
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0017689C File Offset: 0x00174A9C
		protected void OnEnable()
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			Transform transform = main.transform;
			Vector3 position = transform.position;
			Vector3 a = Vector3.Normalize(transform.forward);
			float nearClipPlane = main.nearClipPlane;
			float d = (main.farClipPlane - nearClipPlane) / 2f + nearClipPlane;
			Vector3 position2 = position + a * d;
			for (int i = 0; i < this.meshes.Length; i++)
			{
				Vector3 center = this.xforms[i].InverseTransformPoint(position2);
				this.meshes[i].bounds = new Bounds(center, Vector3.one);
			}
		}

		// Token: 0x04004F4C RID: 20300
		private Mesh[] meshes;

		// Token: 0x04004F4D RID: 20301
		private Transform[] xforms;
	}
}
