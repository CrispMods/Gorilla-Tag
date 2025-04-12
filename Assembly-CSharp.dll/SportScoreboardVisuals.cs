using System;
using GorillaTag.Sports;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class SportScoreboardVisuals : MonoBehaviour
{
	// Token: 0x060027A2 RID: 10146 RVA: 0x0004A1D6 File Offset: 0x000483D6
	private void Awake()
	{
		SportScoreboard.Instance.RegisterTeamVisual(this.TeamIndex, this);
	}

	// Token: 0x04002B6C RID: 11116
	[SerializeField]
	public MaterialUVOffsetListSetter score1s;

	// Token: 0x04002B6D RID: 11117
	[SerializeField]
	public MaterialUVOffsetListSetter score10s;

	// Token: 0x04002B6E RID: 11118
	[SerializeField]
	private int TeamIndex;
}
