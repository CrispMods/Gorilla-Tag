using System;
using UnityEngine;

// Token: 0x020006ED RID: 1773
public class VelocityHelperTest : MonoBehaviour
{
	// Token: 0x06002C49 RID: 11337 RVA: 0x0004E159 File Offset: 0x0004C359
	private void Setup()
	{
		this.lastPosition = base.transform.position;
		this.lastVelocity = Vector3.zero;
		this.velocity = Vector3.zero;
		this.speed = 0f;
	}

	// Token: 0x06002C4A RID: 11338 RVA: 0x0004E18D File Offset: 0x0004C38D
	private void Start()
	{
		this.Setup();
	}

	// Token: 0x06002C4B RID: 11339 RVA: 0x00121D20 File Offset: 0x0011FF20
	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		Vector3 position = base.transform.position;
		Vector3 b = (position - this.lastPosition) / deltaTime;
		this.velocity = Vector3.Lerp(this.lastVelocity, b, deltaTime);
		this.speed = this.velocity.magnitude;
		this.lastPosition = position;
		this.lastVelocity = b;
	}

	// Token: 0x06002C4C RID: 11340 RVA: 0x00030607 File Offset: 0x0002E807
	private void Update()
	{
	}

	// Token: 0x04003166 RID: 12646
	public Vector3 velocity;

	// Token: 0x04003167 RID: 12647
	public float speed;

	// Token: 0x04003168 RID: 12648
	[Space]
	public Vector3 lastVelocity;

	// Token: 0x04003169 RID: 12649
	public Vector3 lastPosition;

	// Token: 0x0400316A RID: 12650
	[Space]
	[SerializeField]
	private float[] _deltaTimes = new float[5];
}
