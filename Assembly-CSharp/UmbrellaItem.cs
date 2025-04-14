using System;
using GorillaTag;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public class UmbrellaItem : TransferrableObject
{
	// Token: 0x0600199F RID: 6559 RVA: 0x0007E547 File Offset: 0x0007C747
	protected override void Start()
	{
		base.Start();
		this.itemState = TransferrableObject.ItemStates.State1;
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x0007E558 File Offset: 0x0007C758
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

	// Token: 0x060019A1 RID: 6561 RVA: 0x0007E5F0 File Offset: 0x0007C7F0
	internal override void OnEnable()
	{
		base.OnEnable();
		this.OnUmbrellaStateChanged();
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x0007E5FE File Offset: 0x0007C7FE
	internal override void OnDisable()
	{
		base.OnDisable();
		BetterDayNightManager.instance.collidersToAddToWeatherSystems.Remove(this.umbrellaRainDestroyTrigger);
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x0007E61E File Offset: 0x0007C81E
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		BetterDayNightManager.instance.collidersToAddToWeatherSystems.Remove(this.umbrellaRainDestroyTrigger);
		this.itemState = TransferrableObject.ItemStates.State1;
		this.OnUmbrellaStateChanged();
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x0007E64B File Offset: 0x0007C84B
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

	// Token: 0x060019A5 RID: 6565 RVA: 0x0007E674 File Offset: 0x0007C874
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

	// Token: 0x060019A6 RID: 6566 RVA: 0x0007E6C4 File Offset: 0x0007C8C4
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

	// Token: 0x060019A7 RID: 6567 RVA: 0x0007E738 File Offset: 0x0007C938
	protected virtual void UpdateAngles(Quaternion[] toAngles, float t)
	{
		for (int i = 0; i < this.umbrellaBones.Length; i++)
		{
			this.umbrellaBones[i].localRotation = Quaternion.Lerp(this.umbrellaBones[i].localRotation, toAngles[i], t);
		}
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x0007E780 File Offset: 0x0007C980
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

	// Token: 0x060019A9 RID: 6569 RVA: 0x000444E2 File Offset: 0x000426E2
	public override bool CanActivate()
	{
		return true;
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000444E2 File Offset: 0x000426E2
	public override bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x04001C85 RID: 7301
	[AssignInCorePrefab]
	public Transform[] umbrellaBones;

	// Token: 0x04001C86 RID: 7302
	[AssignInCorePrefab]
	public Quaternion[] startingAngles;

	// Token: 0x04001C87 RID: 7303
	[AssignInCorePrefab]
	public Quaternion[] endingAngles;

	// Token: 0x04001C88 RID: 7304
	[AssignInCorePrefab]
	[Tooltip("Assign to use the 'Generate Angles' button")]
	private UmbrellaItem umbrellaToCopy;

	// Token: 0x04001C89 RID: 7305
	[AssignInCorePrefab]
	public float lerpValue = 0.25f;

	// Token: 0x04001C8A RID: 7306
	[AssignInCorePrefab]
	public Collider umbrellaRainDestroyTrigger;

	// Token: 0x04001C8B RID: 7307
	[AssignInCorePrefab]
	public GameObject[] gameObjectsActivatedOnOpen;

	// Token: 0x04001C8C RID: 7308
	[AssignInCorePrefab]
	public ParticleSystem[] particlesEmitOnOpen;

	// Token: 0x04001C8D RID: 7309
	[GorillaSoundLookup]
	public int SoundIdOpen = 64;

	// Token: 0x04001C8E RID: 7310
	[GorillaSoundLookup]
	public int SoundIdClose = 65;

	// Token: 0x04001C8F RID: 7311
	private UmbrellaItem.UmbrellaStates previousUmbrellaState = UmbrellaItem.UmbrellaStates.UmbrellaOpen;

	// Token: 0x0200040D RID: 1037
	private enum UmbrellaStates
	{
		// Token: 0x04001C91 RID: 7313
		UmbrellaOpen = 1,
		// Token: 0x04001C92 RID: 7314
		UmbrellaClosed
	}
}
