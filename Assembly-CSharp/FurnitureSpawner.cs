using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000349 RID: 841
[RequireComponent(typeof(OVRSceneAnchor))]
[DefaultExecutionOrder(30)]
public class FurnitureSpawner : MonoBehaviour
{
	// Token: 0x06001399 RID: 5017 RVA: 0x0006046F File Offset: 0x0005E66F
	private void Start()
	{
		this._sceneAnchor = base.GetComponent<OVRSceneAnchor>();
		this._classification = base.GetComponent<OVRSemanticClassification>();
		this.AddRoomLight();
		this.SpawnSpawnable();
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x00060498 File Offset: 0x0005E698
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

	// Token: 0x0600139B RID: 5019 RVA: 0x00060624 File Offset: 0x0005E824
	private bool FindValidSpawnable(out SimpleResizable currentSpawnable)
	{
		currentSpawnable = null;
		if (!this._classification)
		{
			return false;
		}
		if (!Object.FindObjectOfType<OVRSceneManager>())
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

	// Token: 0x0600139C RID: 5020 RVA: 0x000606CC File Offset: 0x0005E8CC
	private void AddRoomLight()
	{
		if (!this.RoomLightPrefab)
		{
			return;
		}
		if (this._classification && this._classification.Contains("CEILING") && !FurnitureSpawner._roomLightRef)
		{
			FurnitureSpawner._roomLightRef = Object.Instantiate<GameObject>(this.RoomLightPrefab, this._sceneAnchor.transform);
		}
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x00060730 File Offset: 0x0005E930
	private void GetVolumeFromTopPlane(Transform plane, Vector2 dimensions, float height, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		float num = height / 2f;
		position = plane.position - Vector3.up * num;
		rotation = Quaternion.LookRotation(-plane.up, Vector3.up);
		localScale = new Vector3(dimensions.x, num * 2f, dimensions.y);
	}

	// Token: 0x040015CB RID: 5579
	[Tooltip("Add a point at ceiling.")]
	public GameObject RoomLightPrefab;

	// Token: 0x040015CC RID: 5580
	[Tooltip("This prefab will be used if the label is not in the SpawnablesPrefabs")]
	public SimpleResizable FallbackPrefab;

	// Token: 0x040015CD RID: 5581
	public List<Spawnable> SpawnablePrefabs;

	// Token: 0x040015CE RID: 5582
	private OVRSceneAnchor _sceneAnchor;

	// Token: 0x040015CF RID: 5583
	private OVRSemanticClassification _classification;

	// Token: 0x040015D0 RID: 5584
	private static GameObject _roomLightRef;

	// Token: 0x040015D1 RID: 5585
	private int _frameCounter;
}
