using System;
using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class DebugSpawnPointChanger : MonoBehaviour
{
	// Token: 0x06001B47 RID: 6983 RVA: 0x00086508 File Offset: 0x00084708
	private void AttachSpawnPoint(VRRig rig, Transform[] spawnPts, int locationIndex)
	{
		if (spawnPts == null)
		{
			return;
		}
		GTPlayer gtplayer = Object.FindObjectOfType<GTPlayer>();
		if (gtplayer == null)
		{
			return;
		}
		this.lastLocationIndex = locationIndex;
		int i = 0;
		while (i < spawnPts.Length)
		{
			Transform transform = spawnPts[i];
			if (transform.name == this.levelTriggers[locationIndex].levelName)
			{
				rig.transform.position = transform.position;
				rig.transform.rotation = transform.rotation;
				gtplayer.transform.position = transform.position;
				gtplayer.transform.rotation = transform.rotation;
				gtplayer.InitializeValues();
				SpawnPoint component = transform.GetComponent<SpawnPoint>();
				if (component != null)
				{
					gtplayer.SetScaleMultiplier(component.startSize);
					ZoneManagement.SetActiveZone(component.startZone);
					return;
				}
				Debug.LogWarning("Attempt to spawn at transform that does not have SpawnPoint component will be ignored: " + transform.name);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x000865FC File Offset: 0x000847FC
	private void ChangePoint(int index)
	{
		SpawnManager spawnManager = Object.FindObjectOfType<SpawnManager>();
		if (spawnManager != null)
		{
			Transform[] spawnPts = spawnManager.ChildrenXfs();
			foreach (VRRig rig in (VRRig[])Object.FindObjectsOfType(typeof(VRRig)))
			{
				this.AttachSpawnPoint(rig, spawnPts, index);
			}
		}
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x00086651 File Offset: 0x00084851
	public List<string> GetPlausibleJumpLocation()
	{
		return (from index in this.levelTriggers[this.lastLocationIndex].canJumpToIndex
		select this.levelTriggers[index].levelName).ToList<string>();
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x00086680 File Offset: 0x00084880
	public void JumpTo(int canJumpIndex)
	{
		DebugSpawnPointChanger.GeoTriggersGroup geoTriggersGroup = this.levelTriggers[this.lastLocationIndex];
		this.ChangePoint(geoTriggersGroup.canJumpToIndex[canJumpIndex]);
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000866B0 File Offset: 0x000848B0
	public void SetLastLocation(string levelName)
	{
		for (int i = 0; i < this.levelTriggers.Length; i++)
		{
			if (!(this.levelTriggers[i].levelName != levelName))
			{
				this.lastLocationIndex = i;
				return;
			}
		}
	}

	// Token: 0x04001E36 RID: 7734
	[SerializeField]
	private DebugSpawnPointChanger.GeoTriggersGroup[] levelTriggers;

	// Token: 0x04001E37 RID: 7735
	private int lastLocationIndex;

	// Token: 0x02000454 RID: 1108
	[Serializable]
	private struct GeoTriggersGroup
	{
		// Token: 0x04001E38 RID: 7736
		public string levelName;

		// Token: 0x04001E39 RID: 7737
		public GorillaGeoHideShowTrigger enterTrigger;

		// Token: 0x04001E3A RID: 7738
		public GorillaGeoHideShowTrigger[] leaveTrigger;

		// Token: 0x04001E3B RID: 7739
		public int[] canJumpToIndex;
	}
}
