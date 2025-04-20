using System;
using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class DebugSpawnPointChanger : MonoBehaviour
{
	// Token: 0x06001B9B RID: 7067 RVA: 0x000DAB48 File Offset: 0x000D8D48
	private void AttachSpawnPoint(VRRig rig, Transform[] spawnPts, int locationIndex)
	{
		if (spawnPts == null)
		{
			return;
		}
		GTPlayer gtplayer = UnityEngine.Object.FindObjectOfType<GTPlayer>();
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

	// Token: 0x06001B9C RID: 7068 RVA: 0x000DAC3C File Offset: 0x000D8E3C
	private void ChangePoint(int index)
	{
		SpawnManager spawnManager = UnityEngine.Object.FindObjectOfType<SpawnManager>();
		if (spawnManager != null)
		{
			Transform[] spawnPts = spawnManager.ChildrenXfs();
			foreach (VRRig rig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
			{
				this.AttachSpawnPoint(rig, spawnPts, index);
			}
		}
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x00042E8E File Offset: 0x0004108E
	public List<string> GetPlausibleJumpLocation()
	{
		return (from index in this.levelTriggers[this.lastLocationIndex].canJumpToIndex
		select this.levelTriggers[index].levelName).ToList<string>();
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000DAC94 File Offset: 0x000D8E94
	public void JumpTo(int canJumpIndex)
	{
		DebugSpawnPointChanger.GeoTriggersGroup geoTriggersGroup = this.levelTriggers[this.lastLocationIndex];
		this.ChangePoint(geoTriggersGroup.canJumpToIndex[canJumpIndex]);
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x000DACC4 File Offset: 0x000D8EC4
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

	// Token: 0x04001E85 RID: 7813
	[SerializeField]
	private DebugSpawnPointChanger.GeoTriggersGroup[] levelTriggers;

	// Token: 0x04001E86 RID: 7814
	private int lastLocationIndex;

	// Token: 0x02000460 RID: 1120
	[Serializable]
	private struct GeoTriggersGroup
	{
		// Token: 0x04001E87 RID: 7815
		public string levelName;

		// Token: 0x04001E88 RID: 7816
		public GorillaGeoHideShowTrigger enterTrigger;

		// Token: 0x04001E89 RID: 7817
		public GorillaGeoHideShowTrigger[] leaveTrigger;

		// Token: 0x04001E8A RID: 7818
		public int[] canJumpToIndex;
	}
}
