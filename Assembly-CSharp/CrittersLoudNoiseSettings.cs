using System;

// Token: 0x02000050 RID: 80
public class CrittersLoudNoiseSettings : CrittersActorSettings
{
	// Token: 0x06000192 RID: 402 RVA: 0x0006EC50 File Offset: 0x0006CE50
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersLoudNoise crittersLoudNoise = (CrittersLoudNoise)this.parentActor;
		crittersLoudNoise.soundVolume = this._soundVolume;
		crittersLoudNoise.soundDuration = this._soundDuration;
		crittersLoudNoise.soundEnabled = this._soundEnabled;
		crittersLoudNoise.disableWhenSoundDisabled = this._disableWhenSoundDisabled;
		crittersLoudNoise.volumeFearAttractionMultiplier = this._volumeFearAttractionMultiplier;
	}

	// Token: 0x040001E4 RID: 484
	public float _soundVolume;

	// Token: 0x040001E5 RID: 485
	public float _soundDuration;

	// Token: 0x040001E6 RID: 486
	public bool _soundEnabled;

	// Token: 0x040001E7 RID: 487
	public bool _disableWhenSoundDisabled;

	// Token: 0x040001E8 RID: 488
	public float _volumeFearAttractionMultiplier = 1f;
}
