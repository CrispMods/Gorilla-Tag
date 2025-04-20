using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

// Token: 0x02000348 RID: 840
public class SceneManagerHelper
{
	// Token: 0x17000236 RID: 566
	// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0003D582 File Offset: 0x0003B782
	public GameObject AnchorGameObject { get; }

	// Token: 0x060013BA RID: 5050 RVA: 0x0003D58A File Offset: 0x0003B78A
	public SceneManagerHelper(GameObject gameObject)
	{
		this.AnchorGameObject = gameObject;
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x000B8F60 File Offset: 0x000B7160
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

	// Token: 0x060013BC RID: 5052 RVA: 0x000B8FD0 File Offset: 0x000B71D0
	public void CreatePlane(OVRBounded2D bounds)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		gameObject.name = "Plane";
		gameObject.transform.SetParent(this.AnchorGameObject.transform, false);
		gameObject.transform.localScale = new Vector3(bounds.BoundingBox.size.x, bounds.BoundingBox.size.y, 0.01f);
		gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", UnityEngine.Random.ColorHSV());
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x000B905C File Offset: 0x000B725C
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

	// Token: 0x060013BE RID: 5054 RVA: 0x000B90D0 File Offset: 0x000B72D0
	public void CreateVolume(OVRBounded3D bounds)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		gameObject.name = "Volume";
		gameObject.transform.SetParent(this.AnchorGameObject.transform, false);
		gameObject.transform.localPosition = new Vector3(0f, 0f, -bounds.BoundingBox.size.z / 2f);
		gameObject.transform.localScale = bounds.BoundingBox.size;
		gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", UnityEngine.Random.ColorHSV());
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x000B9170 File Offset: 0x000B7370
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

	// Token: 0x060013C0 RID: 5056 RVA: 0x000B91F4 File Offset: 0x000B73F4
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
					gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", UnityEngine.Random.ColorHSV());
				}
			}
		}
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x000B92F0 File Offset: 0x000B74F0
	public void UpdateMesh(OVRTriangleMesh mesh)
	{
		Transform transform = this.AnchorGameObject.transform.Find("Mesh");
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform);
		}
		this.CreateMesh(mesh);
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x000B932C File Offset: 0x000B752C
	public static Task<bool> RequestSceneCapture()
	{
		SceneManagerHelper.<RequestSceneCapture>d__11 <RequestSceneCapture>d__;
		<RequestSceneCapture>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<RequestSceneCapture>d__.<>1__state = -1;
		<RequestSceneCapture>d__.<>t__builder.Start<SceneManagerHelper.<RequestSceneCapture>d__11>(ref <RequestSceneCapture>d__);
		return <RequestSceneCapture>d__.<>t__builder.Task;
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x0003D599 File Offset: 0x0003B799
	public static void RequestScenePermission()
	{
		if (!Permission.HasUserAuthorizedPermission("com.oculus.permission.USE_SCENE"))
		{
			Permission.RequestUserPermission("com.oculus.permission.USE_SCENE");
		}
	}

	// Token: 0x040015E5 RID: 5605
	private static bool SceneCaptureRunning;
}
