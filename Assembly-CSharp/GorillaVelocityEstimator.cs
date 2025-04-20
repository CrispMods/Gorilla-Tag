using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class GorillaVelocityEstimator : MonoBehaviour
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x00033DF9 File Offset: 0x00031FF9
	// (set) Token: 0x0600053A RID: 1338 RVA: 0x00033E01 File Offset: 0x00032001
	public Vector3 linearVelocity { get; private set; }

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x00033E0A File Offset: 0x0003200A
	// (set) Token: 0x0600053C RID: 1340 RVA: 0x00033E12 File Offset: 0x00032012
	public Vector3 angularVelocity { get; private set; }

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x00033E1B File Offset: 0x0003201B
	// (set) Token: 0x0600053E RID: 1342 RVA: 0x00033E23 File Offset: 0x00032023
	public Vector3 handPos { get; private set; }

	// Token: 0x0600053F RID: 1343 RVA: 0x00033E2C File Offset: 0x0003202C
	private void Awake()
	{
		this.history = new GorillaVelocityEstimator.VelocityHistorySample[this.numFrames];
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00080DB4 File Offset: 0x0007EFB4
	private void OnEnable()
	{
		this.currentFrame = 0;
		for (int i = 0; i < this.history.Length; i++)
		{
			this.history[i] = default(GorillaVelocityEstimator.VelocityHistorySample);
		}
		this.lastPos = base.transform.position;
		this.lastRotation = base.transform.rotation;
		GorillaVelocityEstimatorManager.Register(this);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00033E3F File Offset: 0x0003203F
	private void OnDisable()
	{
		GorillaVelocityEstimatorManager.Unregister(this);
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00033E3F File Offset: 0x0003203F
	private void OnDestroy()
	{
		GorillaVelocityEstimatorManager.Unregister(this);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00080E18 File Offset: 0x0007F018
	public void TriggeredLateUpdate()
	{
		Vector3 position = base.transform.position;
		Vector3 b = Vector3.zero;
		if (!this.useGlobalSpace)
		{
			b = GTPlayer.Instance.InstantaneousVelocity;
		}
		Vector3 linear = (position - this.lastPos) / Time.deltaTime - b;
		Quaternion rotation = base.transform.rotation;
		Vector3 vector = (rotation * Quaternion.Inverse(this.lastRotation)).eulerAngles;
		if (vector.x > 180f)
		{
			vector.x -= 360f;
		}
		if (vector.y > 180f)
		{
			vector.y -= 360f;
		}
		if (vector.z > 180f)
		{
			vector.z -= 360f;
		}
		vector *= 0.017453292f / Time.fixedDeltaTime;
		this.history[this.currentFrame % this.numFrames] = new GorillaVelocityEstimator.VelocityHistorySample
		{
			linear = linear,
			angular = vector
		};
		this.linearVelocity = this.history[0].linear;
		this.angularVelocity = this.history[0].angular;
		for (int i = 1; i < this.numFrames; i++)
		{
			this.linearVelocity += this.history[i].linear;
			this.angularVelocity += this.history[i].angular;
		}
		this.linearVelocity /= (float)this.numFrames;
		this.angularVelocity /= (float)this.numFrames;
		this.handPos = position;
		this.currentFrame = (this.currentFrame + 1) % this.numFrames;
		this.lastPos = position;
		this.lastRotation = rotation;
	}

	// Token: 0x04000617 RID: 1559
	private int numFrames = 8;

	// Token: 0x0400061B RID: 1563
	private GorillaVelocityEstimator.VelocityHistorySample[] history;

	// Token: 0x0400061C RID: 1564
	private int currentFrame;

	// Token: 0x0400061D RID: 1565
	private Vector3 lastPos;

	// Token: 0x0400061E RID: 1566
	private Quaternion lastRotation;

	// Token: 0x0400061F RID: 1567
	private Vector3 lastRotationVec;

	// Token: 0x04000620 RID: 1568
	public bool useGlobalSpace;

	// Token: 0x020000CD RID: 205
	public struct VelocityHistorySample
	{
		// Token: 0x04000621 RID: 1569
		public Vector3 linear;

		// Token: 0x04000622 RID: 1570
		public Vector3 angular;
	}
}
