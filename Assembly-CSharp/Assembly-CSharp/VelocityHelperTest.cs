using System;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
public class VelocityHelperTest : MonoBehaviour
{
	// Token: 0x06002BBB RID: 11195 RVA: 0x000D6F19 File Offset: 0x000D5119
	private void Setup()
	{
		this.lastPosition = base.transform.position;
		this.lastVelocity = Vector3.zero;
		this.velocity = Vector3.zero;
		this.speed = 0f;
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x000D6F4D File Offset: 0x000D514D
	private void Start()
	{
		this.Setup();
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000D6F58 File Offset: 0x000D5158
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

	// Token: 0x06002BBE RID: 11198 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x040030CF RID: 12495
	public Vector3 velocity;

	// Token: 0x040030D0 RID: 12496
	public float speed;

	// Token: 0x040030D1 RID: 12497
	[Space]
	public Vector3 lastVelocity;

	// Token: 0x040030D2 RID: 12498
	public Vector3 lastPosition;

	// Token: 0x040030D3 RID: 12499
	[Space]
	[SerializeField]
	private float[] _deltaTimes = new float[5];
}
