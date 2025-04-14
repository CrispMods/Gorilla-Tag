using System;
using UnityEngine;

// Token: 0x0200035C RID: 860
public class SpatialAnchorLoader : MonoBehaviour
{
	// Token: 0x06001406 RID: 5126 RVA: 0x000625B0 File Offset: 0x000607B0
	public void LoadAnchorsByUuid()
	{
		if (!PlayerPrefs.HasKey("numUuids"))
		{
			PlayerPrefs.SetInt("numUuids", 0);
		}
		int @int = PlayerPrefs.GetInt("numUuids");
		SpatialAnchorLoader.Log(string.Format("Attempting to load {0} saved anchors.", @int));
		if (@int == 0)
		{
			return;
		}
		Guid[] array = new Guid[@int];
		for (int i = 0; i < @int; i++)
		{
			string @string = PlayerPrefs.GetString("uuid" + i.ToString());
			SpatialAnchorLoader.Log("QueryAnchorByUuid: " + @string);
			array[i] = new Guid(@string);
		}
		this.Load(new OVRSpatialAnchor.LoadOptions
		{
			Timeout = 0.0,
			StorageLocation = OVRSpace.StorageLocation.Local,
			Uuids = array
		});
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x0006266F File Offset: 0x0006086F
	private void Awake()
	{
		this._onLoadAnchor = new Action<OVRSpatialAnchor.UnboundAnchor, bool>(this.OnLocalized);
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x00062683 File Offset: 0x00060883
	private void Load(OVRSpatialAnchor.LoadOptions options)
	{
		OVRSpatialAnchor.LoadUnboundAnchors(options, delegate(OVRSpatialAnchor.UnboundAnchor[] anchors)
		{
			if (anchors == null)
			{
				SpatialAnchorLoader.Log("Query failed.");
				return;
			}
			foreach (OVRSpatialAnchor.UnboundAnchor arg in anchors)
			{
				if (arg.Localized)
				{
					this._onLoadAnchor(arg, true);
				}
				else if (!arg.Localizing)
				{
					arg.Localize(this._onLoadAnchor, 0.0);
				}
			}
		});
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x00062698 File Offset: 0x00060898
	private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
	{
		if (!success)
		{
			SpatialAnchorLoader.Log(string.Format("{0} Localization failed!", unboundAnchor));
			return;
		}
		Pose pose = unboundAnchor.Pose;
		OVRSpatialAnchor ovrspatialAnchor = Object.Instantiate<OVRSpatialAnchor>(this._anchorPrefab, pose.position, pose.rotation);
		unboundAnchor.BindTo(ovrspatialAnchor);
		Anchor anchor;
		if (ovrspatialAnchor.TryGetComponent<Anchor>(out anchor))
		{
			anchor.ShowSaveIcon = true;
		}
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x000626F7 File Offset: 0x000608F7
	private static void Log(string message)
	{
		Debug.Log("[SpatialAnchorsUnity]: " + message);
	}

	// Token: 0x04001628 RID: 5672
	[SerializeField]
	private OVRSpatialAnchor _anchorPrefab;

	// Token: 0x04001629 RID: 5673
	private Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;
}
