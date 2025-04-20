using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class UmbrellaItem : TransferrableObject
{
	// Token: 0x060019EC RID: 6636 RVA: 0x00041758 File Offset: 0x0003F958
	protected override void Start()
	{
		base.Start();
		this.itemState = TransferrableObject.ItemStates.State1;
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x000D3DF0 File Offset: 0x000D1FF0
	public override void OnActivate()
	{
		base.OnActivate();
		float hapticStrength = GorillaTagger.Instance.tapHapticStrength / 4f;
		float fixedDeltaTime = Time.fixedDeltaTime;
		float soundVolume = 0.08f;
		int soundIndex;
		if (this.itemState == TransferrableObject.ItemStates.State1)
		{
			soundIndex = this.SoundIdOpen;
			this.itemState = TransferrableObject.ItemStates.State0;
			BetterDayNightManager.instance.collidersToAddToWeatherSystems.Add(this.umbrellaRainDestroyTrigger);
		}
		else
		{
			soundIndex = this.SoundIdClose;
			this.itemState = TransferrableObject.ItemStates.State1;
			BetterDayNightManager.instance.collidersToAddToWeatherSystems.Remove(this.umbrellaRainDestroyTrigger);
		}
		base.ActivateItemFX(hapticStrength, fixedDeltaTime, soundIndex, soundVolume);
		this.OnUmbrellaStateChanged();
	}

	// Token: 0x060019EE RID: 6638 RVA: 0x00041767 File Offset: 0x0003F967
	internal override void OnEnable()
	{
		base.OnEnable();
		this.OnUmbrellaStateChanged();
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x00041775 File Offset: 0x0003F975
	internal override void OnDisable()
	{
		base.OnDisable();
		BetterDayNightManager.instance.collidersToAddToWeatherSystems.Remove(this.umbrellaRainDestroyTrigger);
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00041795 File Offset: 0x0003F995
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		BetterDayNightManager.instance.collidersToAddToWeatherSystems.Remove(this.umbrellaRainDestroyTrigger);
		this.itemState = TransferrableObject.ItemStates.State1;
		this.OnUmbrellaStateChanged();
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x000417C2 File Offset: 0x0003F9C2
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		if (base.InHand())
		{
			return false;
		}
		if (this.itemState == TransferrableObject.ItemStates.State0)
		{
			this.OnActivate();
		}
		return true;
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x000D3E88 File Offset: 0x000D2088
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		UmbrellaItem.UmbrellaStates itemState = (UmbrellaItem.UmbrellaStates)this.itemState;
		if (itemState != this.previousUmbrellaState)
		{
			this.OnUmbrellaStateChanged();
		}
		this.UpdateAngles((itemState == UmbrellaItem.UmbrellaStates.UmbrellaOpen) ? this.startingAngles : this.endingAngles, this.lerpValue);
		this.previousUmbrellaState = itemState;
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x000D3ED8 File Offset: 0x000D20D8
	protected virtual void OnUmbrellaStateChanged()
	{
		bool flag = this.itemState == TransferrableObject.ItemStates.State0;
		GameObject[] array = this.gameObjectsActivatedOnOpen;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(flag);
		}
		ParticleSystem[] array2;
		if (flag)
		{
			array2 = this.particlesEmitOnOpen;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Play();
			}
			return;
		}
		array2 = this.particlesEmitOnOpen;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Stop();
		}
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x000D3F4C File Offset: 0x000D214C
	protected virtual void UpdateAngles(Quaternion[] toAngles, float t)
	{
		for (int i = 0; i < this.umbrellaBones.Length; i++)
		{
			this.umbrellaBones[i].localRotation = Quaternion.Lerp(this.umbrellaBones[i].localRotation, toAngles[i], t);
		}
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x000D3F94 File Offset: 0x000D2194
	protected void GenerateAngles()
	{
		this.startingAngles = new Quaternion[this.umbrellaBones.Length];
		for (int i = 0; i < this.endingAngles.Length; i++)
		{
			this.startingAngles[i] = this.umbrellaToCopy.startingAngles[i];
		}
		this.endingAngles = new Quaternion[this.umbrellaBones.Length];
		for (int j = 0; j < this.endingAngles.Length; j++)
		{
			this.endingAngles[j] = this.umbrellaToCopy.endingAngles[j];
		}
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x00039846 File Offset: 0x00037A46
	public override bool CanActivate()
	{
		return true;
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x00039846 File Offset: 0x00037A46
	public override bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x04001CCE RID: 7374
	[AssignInCorePrefab]
	public Transform[] umbrellaBones;

	// Token: 0x04001CCF RID: 7375
	[AssignInCorePrefab]
	public Quaternion[] startingAngles;

	// Token: 0x04001CD0 RID: 7376
	[AssignInCorePrefab]
	public Quaternion[] endingAngles;

	// Token: 0x04001CD1 RID: 7377
	[AssignInCorePrefab]
	[Tooltip("Assign to use the 'Generate Angles' button")]
	private UmbrellaItem umbrellaToCopy;

	// Token: 0x04001CD2 RID: 7378
	[AssignInCorePrefab]
	public float lerpValue = 0.25f;

	// Token: 0x04001CD3 RID: 7379
	[AssignInCorePrefab]
	public Collider umbrellaRainDestroyTrigger;

	// Token: 0x04001CD4 RID: 7380
	[AssignInCorePrefab]
	public GameObject[] gameObjectsActivatedOnOpen;

	// Token: 0x04001CD5 RID: 7381
	[AssignInCorePrefab]
	public ParticleSystem[] particlesEmitOnOpen;

	// Token: 0x04001CD6 RID: 7382
	[GorillaSoundLookup]
	public int SoundIdOpen = 64;

	// Token: 0x04001CD7 RID: 7383
	[GorillaSoundLookup]
	public int SoundIdClose = 65;

	// Token: 0x04001CD8 RID: 7384
	private UmbrellaItem.UmbrellaStates previousUmbrellaState = UmbrellaItem.UmbrellaStates.UmbrellaOpen;

	// Token: 0x02000418 RID: 1048
	private enum UmbrellaStates
	{
		// Token: 0x04001CDA RID: 7386
		UmbrellaOpen = 1,
		// Token: 0x04001CDB RID: 7387
		UmbrellaClosed
	}
}
