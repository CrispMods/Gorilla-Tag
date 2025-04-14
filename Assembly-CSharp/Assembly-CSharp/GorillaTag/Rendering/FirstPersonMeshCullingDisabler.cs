using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C09 RID: 3081
	public class FirstPersonMeshCullingDisabler : MonoBehaviour
	{
		// Token: 0x06004D0B RID: 19723 RVA: 0x00176E00 File Offset: 0x00175000
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

		// Token: 0x06004D0C RID: 19724 RVA: 0x00176E64 File Offset: 0x00175064
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

		// Token: 0x04004F5E RID: 20318
		private Mesh[] meshes;

		// Token: 0x04004F5F RID: 20319
		private Transform[] xforms;
	}
}
