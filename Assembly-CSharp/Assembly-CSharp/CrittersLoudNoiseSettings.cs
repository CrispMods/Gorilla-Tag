using System;

// Token: 0x0200004B RID: 75
public class CrittersLoudNoiseSettings : CrittersActorSettings
{
	// Token: 0x06000179 RID: 377 RVA: 0x00009B50 File Offset: 0x00007D50
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

	// Token: 0x040001C1 RID: 449
	public float _soundVolume;

	// Token: 0x040001C2 RID: 450
	public float _soundDuration;

	// Token: 0x040001C3 RID: 451
	public bool _soundEnabled;

	// Token: 0x040001C4 RID: 452
	public bool _disableWhenSoundDisabled;

	// Token: 0x040001C5 RID: 453
	public float _volumeFearAttractionMultiplier = 1f;
}
