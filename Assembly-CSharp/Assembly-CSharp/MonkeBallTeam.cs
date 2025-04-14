using System;
using UnityEngine;

// Token: 0x0200049D RID: 1181
[Serializable]
public class MonkeBallTeam
{
	// Token: 0x04001F8B RID: 8075
	public Color color;

	// Token: 0x04001F8C RID: 8076
	public int score;

	// Token: 0x04001F8D RID: 8077
	public Transform ballStartLocation;

	// Token: 0x04001F8E RID: 8078
	public Transform ballLaunchPosition;

	// Token: 0x04001F8F RID: 8079
	[Tooltip("The min/max random velocity of the ball when launched.")]
	public Vector2 ballLaunchVelocityRange = new Vector2(8f, 15f);

	// Token: 0x04001F90 RID: 8080
	[Tooltip("The min/max random x-angle of the ball when launched.")]
	public Vector2 ballLaunchAngleXRange = new Vector2(0f, 0f);

	// Token: 0x04001F91 RID: 8081
	[Tooltip("The min/max random y-angle of the ball when launched.")]
	public Vector2 ballLaunchAngleYRange = new Vector2(0f, 0f);
}
