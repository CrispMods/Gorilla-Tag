using System;
using UnityEngine;

// Token: 0x020001D0 RID: 464
[CreateAssetMenu(fileName = "JoinTriggerUITemplate", menuName = "ScriptableObjects/JoinTriggerUITemplate")]
public class JoinTriggerUITemplate : ScriptableObject
{
	// Token: 0x04000D31 RID: 3377
	public Material Milestone_Error;

	// Token: 0x04000D32 RID: 3378
	public Material Milestone_AlreadyInRoom;

	// Token: 0x04000D33 RID: 3379
	public Material Milestone_InPrivateRoom;

	// Token: 0x04000D34 RID: 3380
	public Material Milestone_NotConnectedSoloJoin;

	// Token: 0x04000D35 RID: 3381
	public Material Milestone_LeaveRoomAndSoloJoin;

	// Token: 0x04000D36 RID: 3382
	public Material Milestone_LeaveRoomAndGroupJoin;

	// Token: 0x04000D37 RID: 3383
	public Material Milestone_AbandonPartyAndSoloJoin;

	// Token: 0x04000D38 RID: 3384
	public Material Milestone_ChangingGameModeSoloJoin;

	// Token: 0x04000D39 RID: 3385
	public Material ScreenBG_Error;

	// Token: 0x04000D3A RID: 3386
	public Material ScreenBG_AlreadyInRoom;

	// Token: 0x04000D3B RID: 3387
	public Material ScreenBG_InPrivateRoom;

	// Token: 0x04000D3C RID: 3388
	public Material ScreenBG_NotConnectedSoloJoin;

	// Token: 0x04000D3D RID: 3389
	public Material ScreenBG_LeaveRoomAndSoloJoin;

	// Token: 0x04000D3E RID: 3390
	public Material ScreenBG_LeaveRoomAndGroupJoin;

	// Token: 0x04000D3F RID: 3391
	public Material ScreenBG_AbandonPartyAndSoloJoin;

	// Token: 0x04000D40 RID: 3392
	public Material ScreenBG_ChangingGameModeSoloJoin;

	// Token: 0x04000D41 RID: 3393
	public string ScreenText_Error;

	// Token: 0x04000D42 RID: 3394
	public bool showFullErrorMessages;

	// Token: 0x04000D43 RID: 3395
	public JoinTriggerUITemplate.FormattedString ScreenText_AlreadyInRoom;

	// Token: 0x04000D44 RID: 3396
	public JoinTriggerUITemplate.FormattedString ScreenText_InPrivateRoom;

	// Token: 0x04000D45 RID: 3397
	public JoinTriggerUITemplate.FormattedString ScreenText_NotConnectedSoloJoin;

	// Token: 0x04000D46 RID: 3398
	public JoinTriggerUITemplate.FormattedString ScreenText_LeaveRoomAndSoloJoin;

	// Token: 0x04000D47 RID: 3399
	public JoinTriggerUITemplate.FormattedString ScreenText_LeaveRoomAndGroupJoin;

	// Token: 0x04000D48 RID: 3400
	public JoinTriggerUITemplate.FormattedString ScreenText_AbandonPartyAndSoloJoin;

	// Token: 0x04000D49 RID: 3401
	public JoinTriggerUITemplate.FormattedString ScreenText_ChangingGameModeSoloJoin;

	// Token: 0x020001D1 RID: 465
	[Serializable]
	public struct FormattedString
	{
		// Token: 0x06000AE6 RID: 2790 RVA: 0x00037AA0 File Offset: 0x00035CA0
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

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00037ADD File Offset: 0x00035CDD
		public string GetText(Func<string> oldZone, Func<string> newZone, Func<string> oldGameType, Func<string> newGameType)
		{
			if (this.formatter == null)
			{
				this.formatter = StringFormatter.Parse(this.formatText);
			}
			return this.formatter.Format(oldZone, newZone, oldGameType, newGameType);
		}

		// Token: 0x04000D4A RID: 3402
		[TextArea]
		[SerializeField]
		private string formatText;

		// Token: 0x04000D4B RID: 3403
		[NonSerialized]
		private StringFormatter formatter;
	}
}
