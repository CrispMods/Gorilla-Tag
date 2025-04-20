using System;
using UnityEngine;

// Token: 0x02000367 RID: 871
public class SpatialAnchorLoader : MonoBehaviour
{
	// Token: 0x0600144F RID: 5199 RVA: 0x000BB890 File Offset: 0x000B9A90
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

	// Token: 0x06001450 RID: 5200 RVA: 0x0003DA97 File Offset: 0x0003BC97
	private void Awake()
	{
		this._onLoadAnchor = new Action<OVRSpatialAnchor.UnboundAnchor, bool>(this.OnLocalized);
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x0003DAAB File Offset: 0x0003BCAB
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

	// Token: 0x06001452 RID: 5202 RVA: 0x000BB950 File Offset: 0x000B9B50
	private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
	{
		if (!success)
		{
			SpatialAnchorLoader.Log(string.Format("{0} Localization failed!", unboundAnchor));
			return;
		}
		Pose pose = unboundAnchor.Pose;
		OVRSpatialAnchor ovrspatialAnchor = UnityEngine.Object.Instantiate<OVRSpatialAnchor>(this._anchorPrefab, pose.position, pose.rotation);
		unboundAnchor.BindTo(ovrspatialAnchor);
		Anchor anchor;
		if (ovrspatialAnchor.TryGetComponent<Anchor>(out anchor))
		{
			anchor.ShowSaveIcon = true;
		}
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x0003DAC0 File Offset: 0x0003BCC0
	private static void Log(string message)
	{
		Debug.Log("[SpatialAnchorsUnity]: " + message);
	}

	// Token: 0x0400166F RID: 5743
	[SerializeField]
	private OVRSpatialAnchor _anchorPrefab;

	// Token: 0x04001670 RID: 5744
	private Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;
}
