using System;
using UnityEngine;

// Token: 0x020001C5 RID: 453
[CreateAssetMenu(fileName = "JoinTriggerUITemplate", menuName = "ScriptableObjects/JoinTriggerUITemplate")]
public class JoinTriggerUITemplate : ScriptableObject
{
	// Token: 0x04000CEC RID: 3308
	public Material Milestone_Error;

	// Token: 0x04000CED RID: 3309
	public Material Milestone_AlreadyInRoom;

	// Token: 0x04000CEE RID: 3310
	public Material Milestone_InPrivateRoom;

	// Token: 0x04000CEF RID: 3311
	public Material Milestone_NotConnectedSoloJoin;

	// Token: 0x04000CF0 RID: 3312
	public Material Milestone_LeaveRoomAndSoloJoin;

	// Token: 0x04000CF1 RID: 3313
	public Material Milestone_LeaveRoomAndGroupJoin;

	// Token: 0x04000CF2 RID: 3314
	public Material Milestone_AbandonPartyAndSoloJoin;

	// Token: 0x04000CF3 RID: 3315
	public Material Milestone_ChangingGameModeSoloJoin;

	// Token: 0x04000CF4 RID: 3316
	public Material ScreenBG_Error;

	// Token: 0x04000CF5 RID: 3317
	public Material ScreenBG_AlreadyInRoom;

	// Token: 0x04000CF6 RID: 3318
	public Material ScreenBG_InPrivateRoom;

	// Token: 0x04000CF7 RID: 3319
	public Material ScreenBG_NotConnectedSoloJoin;

	// Token: 0x04000CF8 RID: 3320
	public Material ScreenBG_LeaveRoomAndSoloJoin;

	// Token: 0x04000CF9 RID: 3321
	public Material ScreenBG_LeaveRoomAndGroupJoin;

	// Token: 0x04000CFA RID: 3322
	public Material ScreenBG_AbandonPartyAndSoloJoin;

	// Token: 0x04000CFB RID: 3323
	public Material ScreenBG_ChangingGameModeSoloJoin;

	// Token: 0x04000CFC RID: 3324
	public string ScreenText_Error;

	// Token: 0x04000CFD RID: 3325
	public bool showFullErrorMessages;

	// Token: 0x04000CFE RID: 3326
	public JoinTriggerUITemplate.FormattedString ScreenText_AlreadyInRoom;

	// Token: 0x04000CFF RID: 3327
	public JoinTriggerUITemplate.FormattedString ScreenText_InPrivateRoom;

	// Token: 0x04000D00 RID: 3328
	public JoinTriggerUITemplate.FormattedString ScreenText_NotConnectedSoloJoin;

	// Token: 0x04000D01 RID: 3329
	public JoinTriggerUITemplate.FormattedString ScreenText_LeaveRoomAndSoloJoin;

	// Token: 0x04000D02 RID: 3330
	public JoinTriggerUITemplate.FormattedString ScreenText_LeaveRoomAndGroupJoin;

	// Token: 0x04000D03 RID: 3331
	public JoinTriggerUITemplate.FormattedString ScreenText_AbandonPartyAndSoloJoin;

	// Token: 0x04000D04 RID: 3332
	public JoinTriggerUITemplate.FormattedString ScreenText_ChangingGameModeSoloJoin;

	// Token: 0x020001C6 RID: 454
	[Serializable]
	public struct FormattedString
	{
		// Token: 0x06000A9C RID: 2716 RVA: 0x00039C3D File Offset: 0x00037E3D
		public string GetText(string oldZone, string newZone, string oldGameType, string newGameType)
		{
			if (this.formatter == null)
			{
				this.formatter = StringFormatter.Parse(this.formatText);
			}
			return this.formatter.Format(new string[]
			{
				oldZone,
				newZone,
				oldGameType,
				newGameType
			});
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00039C7A File Offset: 0x00037E7A
		public string GetText(Func<string> oldZone, Func<string> newZone, Func<string> oldGameType, Func<string> newGameType)
		{
			if (this.formatter == null)
			{
				this.formatter = StringFormatter.Parse(this.formatText);
			}
			return this.formatter.Format(oldZone, newZone, oldGameType, newGameType);
		}

		// Token: 0x04000D05 RID: 3333
		[TextArea]
		[SerializeField]
		private string formatText;

		// Token: 0x04000D06 RID: 3334
		[NonSerialized]
		private StringFormatter formatter;
	}
}
