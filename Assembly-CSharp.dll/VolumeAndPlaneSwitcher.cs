using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000354 RID: 852
[RequireComponent(typeof(OVRSceneAnchor))]
public class VolumeAndPlaneSwitcher : MonoBehaviour
{
	// Token: 0x060013CE RID: 5070 RVA: 0x000B84DC File Offset: 0x000B66DC
	private void ReplaceAnchor(OVRSceneAnchor prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
	{
		OVRSceneAnchor ovrsceneAnchor = UnityEngine.Object.Instantiate<OVRSceneAnchor>(prefab, base.transform.parent);
		ovrsceneAnchor.enabled = false;
		ovrsceneAnchor.InitializeFrom(base.GetComponent<OVRSceneAnchor>());
		ovrsceneAnchor.transform.SetPositionAndRotation(position, rotation);
		foreach (object obj in ovrsceneAnchor.transform)
		{
			((Transform)obj).localScale = localScale;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x000B8570 File Offset: 0x000B6770
	private void Start()
	{
		OVRSemanticClassification component = base.GetComponent<OVRSemanticClassification>();
		if (!component)
		{
			return;
		}
		foreach (VolumeAndPlaneSwitcher.LabelGeometryPair labelGeometryPair in this.desiredSwitches)
		{
			if (component.Contains(labelGeometryPair.label))
			{
				Vector3 zero = Vector3.zero;
				Quaternion identity = Quaternion.identity;
				Vector3 zero2 = Vector3.zero;
				VolumeAndPlaneSwitcher.GeometryType desiredGeometryType = labelGeometryPair.desiredGeometryType;
				if (desiredGeometryType != VolumeAndPlaneSwitcher.GeometryType.Plane)
				{
					if (desiredGeometryType == VolumeAndPlaneSwitcher.GeometryType.Volume)
					{
						OVRScenePlane component2 = base.GetComponent<OVRScenePlane>();
						if (!component2)
						{
							Debug.LogWarning("Ignoring desired plane to volume switch for " + labelGeometryPair.label + " because it is not a plane.");
						}
						else
						{
							Debug.Log(string.Format("IN Plane Position {0}, Dimensions: {1}", base.transform.position, component2.Dimensions));
							this.GetVolumeFromTopPlane(base.transform, component2.Dimensions, base.transform.position.y, out zero, out identity, out zero2);
							Debug.Log(string.Format("OUT Volume Position {0}, Dimensions: {1}", zero, zero2));
							this.ReplaceAnchor(this.volumePrefab, zero, identity, zero2);
						}
					}
				}
				else
				{
					OVRSceneVolume component3 = base.GetComponent<OVRSceneVolume>();
					if (!component3)
					{
						Debug.LogWarning("Ignoring desired volume to plane switch for " + labelGeometryPair.label + " because it is not a volume.");
					}
					else
					{
						Debug.Log(string.Format("IN Volume Position {0}, Dimensions: {1}", base.transform.position, component3.Dimensions));
						this.GetTopPlaneFromVolume(base.transform, component3.Dimensions, out zero, out identity, out zero2);
						Debug.Log(string.Format("OUT Plane Position {0}, Dimensions: {1}", zero, zero2));
						this.ReplaceAnchor(this.planePrefab, zero, identity, zero2);
					}
				}
			}
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x0003C510 File Offset: 0x0003A710
	private void GetVolumeFromTopPlane(Transform plane, Vector2 dimensions, float height, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		position = plane.position;
		rotation = plane.rotation;
		localScale = new Vector3(dimensions.x, dimensions.y, height);
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x000B877C File Offset: 0x000B697C
	private void GetTopPlaneFromVolume(Transform volume, Vector3 dimensions, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		float d = dimensions.y / 2f;
		position = volume.position + Vector3.up * d;
		rotation = Quaternion.LookRotation(Vector3.up, -volume.forward);
		localScale = new Vector3(dimensions.x, dimensions.z, dimensions.y);
	}

	// Token: 0x040015F5 RID: 5621
	public OVRSceneAnchor planePrefab;

	// Token: 0x040015F6 RID: 5622
	public OVRSceneAnchor volumePrefab;

	// Token: 0x040015F7 RID: 5623
	public List<VolumeAndPlaneSwitcher.LabelGeometryPair> desiredSwitches;

	// Token: 0x02000355 RID: 853
	public enum GeometryType
	{
		// Token: 0x040015F9 RID: 5625
		Plane,
		// Token: 0x040015FA RID: 5626
		Volume
	}

	// Token: 0x02000356 RID: 854
	[Serializable]
	public struct LabelGeometryPair
	{
		// Token: 0x040015FB RID: 5627
		public string label;

		// Token: 0x040015FC RID: 5628
		public VolumeAndPlaneSwitcher.GeometryType desiredGeometryType;
	}
}
