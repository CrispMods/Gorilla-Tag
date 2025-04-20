using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000354 RID: 852
[RequireComponent(typeof(OVRSceneAnchor))]
[DefaultExecutionOrder(30)]
public class FurnitureSpawner : MonoBehaviour
{
	// Token: 0x060013E5 RID: 5093 RVA: 0x0003D699 File Offset: 0x0003B899
	private void Start()
	{
		this._sceneAnchor = base.GetComponent<OVRSceneAnchor>();
		this._classification = base.GetComponent<OVRSemanticClassification>();
		this.AddRoomLight();
		this.SpawnSpawnable();
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x000B9ED4 File Offset: 0x000B80D4
	private void SpawnSpawnable()
	{
		SimpleResizable sourcePrefab;
		if (!this.FindValidSpawnable(out sourcePrefab))
		{
			return;
		}
		Vector3 position = base.transform.position;
		Quaternion rotation = base.transform.rotation;
		Vector3 localScale = base.transform.localScale;
		OVRScenePlane component = this._sceneAnchor.GetComponent<OVRScenePlane>();
		OVRSceneVolume component2 = this._sceneAnchor.GetComponent<OVRSceneVolume>();
		Vector3 newSize = component2 ? component2.Dimensions : Vector3.one;
		if (this._classification && component)
		{
			newSize = component.Dimensions;
			newSize.z = 1f;
			if (this._classification.Contains("TABLE") || this._classification.Contains("COUCH"))
			{
				this.GetVolumeFromTopPlane(base.transform, component.Dimensions, base.transform.position.y, out position, out rotation, out localScale);
				newSize = localScale;
				position.y += localScale.y / 2f;
			}
			if (this._classification.Contains("WALL_FACE") || this._classification.Contains("CEILING") || this._classification.Contains("FLOOR"))
			{
				newSize.z = 0.01f;
			}
		}
		GameObject gameObject = new GameObject("Root");
		gameObject.transform.parent = base.transform;
		gameObject.transform.SetPositionAndRotation(position, rotation);
		new SimpleResizer().CreateResizedObject(newSize, gameObject, sourcePrefab);
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x000BA060 File Offset: 0x000B8260
	private bool FindValidSpawnable(out SimpleResizable currentSpawnable)
	{
		currentSpawnable = null;
		if (!this._classification)
		{
			return false;
		}
		if (!UnityEngine.Object.FindObjectOfType<OVRSceneManager>())
		{
			return false;
		}
		foreach (Spawnable spawnable in this.SpawnablePrefabs)
		{
			if (this._classification.Contains(spawnable.ClassificationLabel))
			{
				currentSpawnable = spawnable.ResizablePrefab;
				return true;
			}
		}
		if (this.FallbackPrefab != null)
		{
			currentSpawnable = this.FallbackPrefab;
			return true;
		}
		return false;
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x000BA108 File Offset: 0x000B8308
	private void AddRoomLight()
	{
		if (!this.RoomLightPrefab)
		{
			return;
		}
		if (this._classification && this._classification.Contains("CEILING") && !FurnitureSpawner._roomLightRef)
		{
			FurnitureSpawner._roomLightRef = UnityEngine.Object.Instantiate<GameObject>(this.RoomLightPrefab, this._sceneAnchor.transform);
		}
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x000BA16C File Offset: 0x000B836C
	private void GetVolumeFromTopPlane(Transform plane, Vector2 dimensions, float height, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		float num = height / 2f;
		position = plane.position - Vector3.up * num;
		rotation = Quaternion.LookRotation(-plane.up, Vector3.up);
		localScale = new Vector3(dimensions.x, num * 2f, dimensions.y);
	}

	// Token: 0x04001613 RID: 5651
	[Tooltip("Add a point at ceiling.")]
	public GameObject RoomLightPrefab;

	// Token: 0x04001614 RID: 5652
	[Tooltip("This prefab will be used if the label is not in the SpawnablesPrefabs")]
	public SimpleResizable FallbackPrefab;

	// Token: 0x04001615 RID: 5653
	public List<Spawnable> SpawnablePrefabs;

	// Token: 0x04001616 RID: 5654
	private OVRSceneAnchor _sceneAnchor;

	// Token: 0x04001617 RID: 5655
	private OVRSemanticClassification _classification;

	// Token: 0x04001618 RID: 5656
	private static GameObject _roomLightRef;

	// Token: 0x04001619 RID: 5657
	private int _frameCounter;
}
