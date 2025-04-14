using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000354 RID: 852
[RequireComponent(typeof(OVRSceneAnchor))]
public class VolumeAndPlaneSwitcher : MonoBehaviour
{
	// Token: 0x060013CB RID: 5067 RVA: 0x00061448 File Offset: 0x0005F648
	private void ReplaceAnchor(OVRSceneAnchor prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
	{
		OVRSceneAnchor ovrsceneAnchor = Object.Instantiate<OVRSceneAnchor>(prefab, base.transform.parent);
		ovrsceneAnchor.enabled = false;
		ovrsceneAnchor.InitializeFrom(base.GetComponent<OVRSceneAnchor>());
		ovrsceneAnchor.transform.SetPositionAndRotation(position, rotation);
		foreach (object obj in ovrsceneAnchor.transform)
		{
			((Transform)obj).localScale = localScale;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x000614DC File Offset: 0x0005F6DC
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
		Object.Destroy(this);
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x000616E8 File Offset: 0x0005F8E8
	private void GetVolumeFromTopPlane(Transform plane, Vector2 dimensions, float height, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		position = plane.position;
		rotation = plane.rotation;
		localScale = new Vector3(dimensions.x, dimensions.y, height);
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00061720 File Offset: 0x0005F920
	private void GetTopPlaneFromVolume(Transform volume, Vector3 dimensions, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
	{
		float d = dimensions.y / 2f;
		position = volume.position + Vector3.up * d;
		rotation = Quaternion.LookRotation(Vector3.up, -volume.forward);
		localScale = new Vector3(dimensions.x, dimensions.z, dimensions.y);
	}

	// Token: 0x040015F4 RID: 5620
	public OVRSceneAnchor planePrefab;

	// Token: 0x040015F5 RID: 5621
	public OVRSceneAnchor volumePrefab;

	// Token: 0x040015F6 RID: 5622
	public List<VolumeAndPlaneSwitcher.LabelGeometryPair> desiredSwitches;

	// Token: 0x02000355 RID: 853
	public enum GeometryType
	{
		// Token: 0x040015F8 RID: 5624
		Plane,
		// Token: 0x040015F9 RID: 5625
		Volume
	}

	// Token: 0x02000356 RID: 854
	[Serializable]
	public struct LabelGeometryPair
	{
		// Token: 0x040015FA RID: 5626
		public string label;

		// Token: 0x040015FB RID: 5627
		public VolumeAndPlaneSwitcher.GeometryType desiredGeometryType;
	}
}
