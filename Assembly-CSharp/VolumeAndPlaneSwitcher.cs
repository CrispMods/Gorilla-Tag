using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200035F RID: 863
[RequireComponent(typeof(OVRSceneAnchor))]
public class VolumeAndPlaneSwitcher : MonoBehaviour
{
	// Token: 0x06001417 RID: 5143 RVA: 0x000BAD74 File Offset: 0x000B8F74
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

	// Token: 0x06001418 RID: 5144 RVA: 0x000BAE08 File Offset: 0x000B9008
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

	// Token: 0x06001419 RID: 5145 RVA: 0x0003D7D0 File Offset: 0x0003B9D0
	private void GetVolumeFromTopPlane(Transform plane, Vector2 dimensions, float height, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		position = plane.position;
		rotation = plane.rotation;
		localScale = new Vector3(dimensions.x, dimensions.y, height);
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x000BB014 File Offset: 0x000B9214
	private void GetTopPlaneFromVolume(Transform volume, Vector3 dimensions, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		float d = dimensions.y / 2f;
		position = volume.position + Vector3.up * d;
		rotation = Quaternion.LookRotation(Vector3.up, -volume.forward);
		localScale = new Vector3(dimensions.x, dimensions.z, dimensions.y);
	}

	// Token: 0x0400163C RID: 5692
	public OVRSceneAnchor planePrefab;

	// Token: 0x0400163D RID: 5693
	public OVRSceneAnchor volumePrefab;

	// Token: 0x0400163E RID: 5694
	public List<VolumeAndPlaneSwitcher.LabelGeometryPair> desiredSwitches;

	// Token: 0x02000360 RID: 864
	public enum GeometryType
	{
		// Token: 0x04001640 RID: 5696
		Plane,
		// Token: 0x04001641 RID: 5697
		Volume
	}

	// Token: 0x02000361 RID: 865
	[Serializable]
	public struct LabelGeometryPair
	{
		// Token: 0x04001642 RID: 5698
		public string label;

		// Token: 0x04001643 RID: 5699
		public VolumeAndPlaneSwitcher.GeometryType desiredGeometryType;
	}
}
