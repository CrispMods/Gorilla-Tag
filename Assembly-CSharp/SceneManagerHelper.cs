using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

// Token: 0x0200033D RID: 829
public class SceneManagerHelper
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x0600136D RID: 4973 RVA: 0x0005F3D8 File Offset: 0x0005D5D8
	public GameObject AnchorGameObject { get; }

	// Token: 0x0600136E RID: 4974 RVA: 0x0005F3E0 File Offset: 0x0005D5E0
	public SceneManagerHelper(GameObject gameObject)
	{
		this.AnchorGameObject = gameObject;
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x0005F3F0 File Offset: 0x0005D5F0
	public void SetLocation(OVRLocatable locatable, Camera camera = null)
	{
		OVRLocatable.TrackingSpacePose trackingSpacePose;
		if (!locatable.TryGetSceneAnchorPose(out trackingSpacePose))
		{
			return;
		}
		Camera camera2 = (camera == null) ? Camera.main : camera;
		Vector3? vector = trackingSpacePose.ComputeWorldPosition(camera2);
		Quaternion? quaternion = trackingSpacePose.ComputeWorldRotation(camera2);
		if (vector != null && quaternion != null)
		{
			this.AnchorGameObject.transform.SetPositionAndRotation(vector.Value, quaternion.Value);
		}
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x0005F460 File Offset: 0x0005D660
	public void CreatePlane(OVRBounded2D bounds)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		gameObject.name = "Plane";
		gameObject.transform.SetParent(this.AnchorGameObject.transform, false);
		gameObject.transform.localScale = new Vector3(bounds.BoundingBox.size.x, bounds.BoundingBox.size.y, 0.01f);
		gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x0005F4EC File Offset: 0x0005D6EC
	public void UpdatePlane(OVRBounded2D bounds)
	{
		Transform transform = this.AnchorGameObject.transform.Find("Plane");
		if (transform == null)
		{
			this.CreatePlane(bounds);
			return;
		}
		transform.transform.localScale = new Vector3(bounds.BoundingBox.size.x, bounds.BoundingBox.size.y, 0.01f);
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x0005F560 File Offset: 0x0005D760
	public void CreateVolume(OVRBounded3D bounds)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		gameObject.name = "Volume";
		gameObject.transform.SetParent(this.AnchorGameObject.transform, false);
		gameObject.transform.localPosition = new Vector3(0f, 0f, -bounds.BoundingBox.size.z / 2f);
		gameObject.transform.localScale = bounds.BoundingBox.size;
		gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x0005F600 File Offset: 0x0005D800
	public void UpdateVolume(OVRBounded3D bounds)
	{
		Transform transform = this.AnchorGameObject.transform.Find("Volume");
		if (transform == null)
		{
			this.CreateVolume(bounds);
			return;
		}
		transform.transform.localPosition = new Vector3(0f, 0f, -bounds.BoundingBox.size.z / 2f);
		transform.transform.localScale = bounds.BoundingBox.size;
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x0005F684 File Offset: 0x0005D884
	public void CreateMesh(OVRTriangleMesh mesh)
	{
		int length;
		int num;
		if (!mesh.TryGetCounts(out length, out num))
		{
			return;
		}
		using (NativeArray<Vector3> nativeArray = new NativeArray<Vector3>(length, Allocator.Temp, NativeArrayOptions.ClearMemory))
		{
			using (NativeArray<int> indices = new NativeArray<int>(num * 3, Allocator.Temp, NativeArrayOptions.ClearMemory))
			{
				if (mesh.TryGetMesh(nativeArray, indices))
				{
					Mesh mesh2 = new Mesh();
					mesh2.indexFormat = IndexFormat.UInt32;
					mesh2.SetVertices<Vector3>(nativeArray);
					mesh2.SetTriangles(indices.ToArray(), 0);
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
					gameObject.name = "Mesh";
					gameObject.transform.SetParent(this.AnchorGameObject.transform, false);
					gameObject.GetComponent<MeshFilter>().sharedMesh = mesh2;
					gameObject.GetComponent<MeshCollider>().sharedMesh = mesh2;
					gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
				}
			}
		}
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x0005F780 File Offset: 0x0005D980
	public void UpdateMesh(OVRTriangleMesh mesh)
	{
		Transform transform = this.AnchorGameObject.transform.Find("Mesh");
		if (transform != null)
		{
			Object.Destroy(transform);
		}
		this.CreateMesh(mesh);
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x0005F7BC File Offset: 0x0005D9BC
	public static Task<bool> RequestSceneCapture()
	{
		SceneManagerHelper.<RequestSceneCapture>d__11 <RequestSceneCapture>d__;
		<RequestSceneCapture>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<RequestSceneCapture>d__.<>1__state = -1;
		<RequestSceneCapture>d__.<>t__builder.Start<SceneManagerHelper.<RequestSceneCapture>d__11>(ref <RequestSceneCapture>d__);
		return <RequestSceneCapture>d__.<>t__builder.Task;
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x0005F7F7 File Offset: 0x0005D9F7
	public static void RequestScenePermission()
	{
		if (!Permission.HasUserAuthorizedPermission("com.oculus.permission.USE_SCENE"))
		{
			Permission.RequestUserPermission("com.oculus.permission.USE_SCENE");
		}
	}

	// Token: 0x0400159D RID: 5533
	private static bool SceneCaptureRunning;
}
