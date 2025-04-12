using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000349 RID: 841
[RequireComponent(typeof(OVRSceneAnchor))]
[DefaultExecutionOrder(30)]
public class FurnitureSpawner : MonoBehaviour
{
	// Token: 0x0600139C RID: 5020 RVA: 0x0003C3D9 File Offset: 0x0003A5D9
	private void Start()
	{
		this._sceneAnchor = base.GetComponent<OVRSceneAnchor>();
		this._classification = base.GetComponent<OVRSemanticClassification>();
		this.AddRoomLight();
		this.SpawnSpawnable();
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x000B763C File Offset: 0x000B583C
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

	// Token: 0x0600139E RID: 5022 RVA: 0x000B77C8 File Offset: 0x000B59C8
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

	// Token: 0x0600139F RID: 5023 RVA: 0x000B7870 File Offset: 0x000B5A70
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

	// Token: 0x060013A0 RID: 5024 RVA: 0x000B78D4 File Offset: 0x000B5AD4
	private void GetVolumeFromTopPlane(Transform plane, Vector2 dimensions, float height, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		float num = height / 2f;
		position = plane.position - Vector3.up * num;
		rotation = Quaternion.LookRotation(-plane.up, Vector3.up);
		localScale = new Vector3(dimensions.x, num * 2f, dimensions.y);
	}

	// Token: 0x040015CC RID: 5580
	[Tooltip("Add a point at ceiling.")]
	public GameObject RoomLightPrefab;

	// Token: 0x040015CD RID: 5581
	[Tooltip("This prefab will be used if the label is not in the SpawnablesPrefabs")]
	public SimpleResizable FallbackPrefab;

	// Token: 0x040015CE RID: 5582
	public List<Spawnable> SpawnablePrefabs;

	// Token: 0x040015CF RID: 5583
	private OVRSceneAnchor _sceneAnchor;

	// Token: 0x040015D0 RID: 5584
	private OVRSemanticClassification _classification;

	// Token: 0x040015D1 RID: 5585
	private static GameObject _roomLightRef;

	// Token: 0x040015D2 RID: 5586
	private int _frameCounter;
}
