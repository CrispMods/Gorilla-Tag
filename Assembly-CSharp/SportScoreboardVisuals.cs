using System;
using GorillaTag.Sports;
using UnityEngine;

// Token: 0x02000619 RID: 1561
public class SportScoreboardVisuals : MonoBehaviour
{
	// Token: 0x060026C5 RID: 9925 RVA: 0x0004A76B File Offset: 0x0004896B
	private void Awake()
	{
		SportScoreboard.Instance.RegisterTeamVisual(this.TeamIndex, this);
	}

	// Token: 0x04002ACC RID: 10956
	[SerializeField]
	public MaterialUVOffsetListSetter score1s;

	// Token: 0x04002ACD RID: 10957
	[SerializeField]
	public MaterialUVOffsetListSetter score10s;

	// Token: 0x04002ACE RID: 10958
	[SerializeField]
	private int TeamIndex;
}
