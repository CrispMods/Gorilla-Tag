using System;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
[Serializable]
public class MonkeBallTeam
{
	// Token: 0x04001FD9 RID: 8153
	public Color color;

	// Token: 0x04001FDA RID: 8154
	public int score;

	// Token: 0x04001FDB RID: 8155
	public Transform ballStartLocation;

	// Token: 0x04001FDC RID: 8156
	public Transform ballLaunchPosition;

	// Token: 0x04001FDD RID: 8157
	[Tooltip("The min/max random velocity of the ball when launched.")]
	public Vector2 ballLaunchVelocityRange = new Vector2(8f, 15f);

	// Token: 0x04001FDE RID: 8158
	[Tooltip("The min/max random x-angle of the ball when launched.")]
	public Vector2 ballLaunchAngleXRange = new Vector2(0f, 0f);

	// Token: 0x04001FDF RID: 8159
	[Tooltip("The min/max random y-angle of the ball when launched.")]
	public Vector2 ballLaunchAngleYRange = new Vector2(0f, 0f);
}
