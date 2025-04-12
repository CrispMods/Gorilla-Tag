using System;

// Token: 0x02000691 RID: 1681
public class SoundPostMuteButton : GorillaPressableButton
{
	// Token: 0x060029D2 RID: 10706 RVA: 0x00117774 File Offset: 0x00115974
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		SynchedMusicController[] array = this.musicControllers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MuteAudio(this);
		}
	}

	// Token: 0x04002F40 RID: 12096
	public SynchedMusicController[] musicControllers;
}
