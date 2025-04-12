using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace DynamicSceneManagerHelper
{
	// Token: 0x02000A3D RID: 2621
	internal class UnityObjectUpdater
	{
		// Token: 0x06004158 RID: 16728 RVA: 0x0016E8F0 File Offset: 0x0016CAF0
		public Task<GameObject> CreateUnityObject(OVRAnchor anchor, GameObject parent)
		{
			UnityObjectUpdater.<CreateUnityObject>d__0 <CreateUnityObject>d__;
			<CreateUnityObject>d__.<>t__builder = AsyncTaskMethodBuilder<GameObject>.Create();
			<CreateUnityObject>d__.anchor = anchor;
			<CreateUnityObject>d__.parent = parent;
			<CreateUnityObject>d__.<>1__state = -1;
			<CreateUnityObject>d__.<>t__builder.Start<UnityObjectUpdater.<CreateUnityObject>d__0>(ref <CreateUnityObject>d__);
			return <CreateUnityObject>d__.<>t__builder.Task;
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x0016E93C File Offset: 0x0016CB3C
		public void UpdateUnityObject(OVRAnchor anchor, GameObject gameObject)
		{
			SceneManagerHelper sceneManagerHelper = new SceneManagerHelper(gameObject);
			OVRLocatable locatable;
			if (anchor.TryGetComponent<OVRLocatable>(out locatable))
			{
				sceneManagerHelper.SetLocation(locatable, null);
			}
			OVRBounded2D bounds;
			if (anchor.TryGetComponent<OVRBounded2D>(out bounds) && bounds.IsEnabled)
			{
				sceneManagerHelper.UpdatePlane(bounds);
			}
			OVRBounded3D bounds2;
			if (anchor.TryGetComponent<OVRBounded3D>(out bounds2) && bounds2.IsEnabled)
			{
				sceneManagerHelper.UpdateVolume(bounds2);
			}
			OVRTriangleMesh mesh;
			if (anchor.TryGetComponent<OVRTriangleMesh>(out mesh) && mesh.IsEnabled)
			{
				sceneManagerHelper.UpdateMesh(mesh);
			}
		}
	}
}
