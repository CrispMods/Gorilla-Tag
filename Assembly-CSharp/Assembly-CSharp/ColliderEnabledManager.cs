using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class ColliderEnabledManager : MonoBehaviour
{
	// Token: 0x06000088 RID: 136 RVA: 0x00004B9E File Offset: 0x00002D9E
	private void Start()
	{
		this.floorEnabled = true;
		this.floorCollidersEnabled = true;
		ColliderEnabledManager.instance = this;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00004BB4 File Offset: 0x00002DB4
	private void OnDestroy()
	{
		ColliderEnabledManager.instance = null;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00004BBC File Offset: 0x00002DBC
	public void DisableFloorForFrame()
	{
		this.floorEnabled = false;
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00004BC8 File Offset: 0x00002DC8
	private void LateUpdate()
	{
		if (!this.floorEnabled && this.floorCollidersEnabled)
		{
			this.DisableFloor();
		}
		if (!this.floorCollidersEnabled && Time.time > this.timeDisabled + this.disableLength)
		{
			this.floorCollidersEnabled = true;
		}
		Collider[] array = this.floorCollider;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = this.floorCollidersEnabled;
		}
		if (this.floorCollidersEnabled)
		{
			GorillaSurfaceOverride[] array2 = this.walls;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].overrideIndex = this.wallsBeforeMaterial;
			}
		}
		else
		{
			GorillaSurfaceOverride[] array2 = this.walls;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].overrideIndex = this.wallsAfterMaterial;
			}
		}
		this.floorEnabled = true;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00004C88 File Offset: 0x00002E88
	private void DisableFloor()
	{
		this.floorCollidersEnabled = false;
		this.timeDisabled = Time.time;
	}

	// Token: 0x040000A5 RID: 165
	public static ColliderEnabledManager instance;

	// Token: 0x040000A6 RID: 166
	public Collider[] floorCollider;

	// Token: 0x040000A7 RID: 167
	public bool floorEnabled;

	// Token: 0x040000A8 RID: 168
	public bool wasFloorEnabled;

	// Token: 0x040000A9 RID: 169
	public bool floorCollidersEnabled;

	// Token: 0x040000AA RID: 170
	[GorillaSoundLookup]
	public int wallsBeforeMaterial;

	// Token: 0x040000AB RID: 171
	[GorillaSoundLookup]
	public int wallsAfterMaterial;

	// Token: 0x040000AC RID: 172
	public GorillaSurfaceOverride[] walls;

	// Token: 0x040000AD RID: 173
	public float timeDisabled;

	// Token: 0x040000AE RID: 174
	public float disableLength;
}
