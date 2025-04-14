using System;

// Token: 0x02000690 RID: 1680
public class SoundPostMuteButton : GorillaPressableButton
{
	// Token: 0x060029CA RID: 10698 RVA: 0x000CF9BC File Offset: 0x000CDBBC
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		SynchedMusicController[] array = this.musicControllers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MuteAudio(this);
		}
	}

	// Token: 0x04002F3A RID: 12090
	public SynchedMusicController[] musicControllers;
}
