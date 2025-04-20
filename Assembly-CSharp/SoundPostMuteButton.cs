using System;

// Token: 0x0200065A RID: 1626
public class SoundPostMuteButton : GorillaPressableButton
{
	// Token: 0x06002849 RID: 10313 RVA: 0x00110B7C File Offset: 0x0010ED7C
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		SynchedMusicController[] array = this.musicControllers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MuteAudio(this);
		}
	}

	// Token: 0x04002DAA RID: 11690
	public SynchedMusicController[] musicControllers;
}
