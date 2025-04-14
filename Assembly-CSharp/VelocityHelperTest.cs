using System;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
public class VelocityHelperTest : MonoBehaviour
{
	// Token: 0x06002BB3 RID: 11187 RVA: 0x000D6A99 File Offset: 0x000D4C99
	private void Setup()
	{
		this.lastPosition = base.transform.position;
		this.lastVelocity = Vector3.zero;
		this.velocity = Vector3.zero;
		this.speed = 0f;
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000D6ACD File Offset: 0x000D4CCD
	private void Start()
	{
		this.Setup();
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000D6AD8 File Offset: 0x000D4CD8
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

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x040030C9 RID: 12489
	public Vector3 velocity;

	// Token: 0x040030CA RID: 12490
	public float speed;

	// Token: 0x040030CB RID: 12491
	[Space]
	public Vector3 lastVelocity;

	// Token: 0x040030CC RID: 12492
	public Vector3 lastPosition;

	// Token: 0x040030CD RID: 12493
	[Space]
	[SerializeField]
	private float[] _deltaTimes = new float[5];
}
