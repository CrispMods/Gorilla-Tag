using System;
using GorillaTag.Sports;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class SportScoreboardVisuals : MonoBehaviour
{
	// Token: 0x0600279A RID: 10138 RVA: 0x000C1C30 File Offset: 0x000BFE30
	private void Awake()
	{
		SportScoreboard.Instance.RegisterTeamVisual(this.TeamIndex, this);
	}

	// Token: 0x04002B66 RID: 11110
	[SerializeField]
	public MaterialUVOffsetListSetter score1s;

	// Token: 0x04002B67 RID: 11111
	[SerializeField]
	public MaterialUVOffsetListSetter score10s;

	// Token: 0x04002B68 RID: 11112
	[SerializeField]
	private int TeamIndex;
}
