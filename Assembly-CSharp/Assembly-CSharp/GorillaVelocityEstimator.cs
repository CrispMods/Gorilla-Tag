using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class GorillaVelocityEstimator : MonoBehaviour
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001DE38 File Offset: 0x0001C038
	// (set) Token: 0x06000500 RID: 1280 RVA: 0x0001DE40 File Offset: 0x0001C040
	public Vector3 linearVelocity { get; private set; }

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001DE49 File Offset: 0x0001C049
	// (set) Token: 0x06000502 RID: 1282 RVA: 0x0001DE51 File Offset: 0x0001C051
	public Vector3 angularVelocity { get; private set; }

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001DE5A File Offset: 0x0001C05A
	// (set) Token: 0x06000504 RID: 1284 RVA: 0x0001DE62 File Offset: 0x0001C062
	public Vector3 handPos { get; private set; }

	// Token: 0x06000505 RID: 1285 RVA: 0x0001DE6B File Offset: 0x0001C06B
	private void Awake()
	{
		this.history = new GorillaVelocityEstimator.VelocityHistorySample[this.numFrames];
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0001DE80 File Offset: 0x0001C080
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

	// Token: 0x06000507 RID: 1287 RVA: 0x0001DEE1 File Offset: 0x0001C0E1
	private void OnDisable()
	{
		GorillaVelocityEstimatorManager.Unregister(this);
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0001DEE1 File Offset: 0x0001C0E1
	private void OnDestroy()
	{
		GorillaVelocityEstimatorManager.Unregister(this);
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x0001DEEC File Offset: 0x0001C0EC
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

	// Token: 0x040005D8 RID: 1496
	private int numFrames = 8;

	// Token: 0x040005DC RID: 1500
	private GorillaVelocityEstimator.VelocityHistorySample[] history;

	// Token: 0x040005DD RID: 1501
	private int currentFrame;

	// Token: 0x040005DE RID: 1502
	private Vector3 lastPos;

	// Token: 0x040005DF RID: 1503
	private Quaternion lastRotation;

	// Token: 0x040005E0 RID: 1504
	private Vector3 lastRotationVec;

	// Token: 0x040005E1 RID: 1505
	public bool useGlobalSpace;

	// Token: 0x020000C3 RID: 195
	public struct VelocityHistorySample
	{
		// Token: 0x040005E2 RID: 1506
		public Vector3 linear;

		// Token: 0x040005E3 RID: 1507
		public Vector3 angular;
	}
}
