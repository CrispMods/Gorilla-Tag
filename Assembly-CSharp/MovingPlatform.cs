using System;
using GTMathUtil;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005DC RID: 1500
public class MovingPlatform : BasePlatform
{
	// Token: 0x06002537 RID: 9527 RVA: 0x000B8710 File Offset: 0x000B6910
	public float InitTimeOffset()
	{
		return this.startPercentage * this.cycleLength;
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000B871F File Offset: 0x000B691F
	private long InitTimeOffsetMs()
	{
		return (long)(this.InitTimeOffset() * 1000f);
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000B872E File Offset: 0x000B692E
	private long NetworkTimeMs()
	{
		if (PhotonNetwork.InRoom)
		{
			return (long)((ulong)(PhotonNetwork.ServerTimestamp + int.MinValue) + (ulong)this.InitTimeOffsetMs());
		}
		return (long)(Time.time * 1000f);
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000B8757 File Offset: 0x000B6957
	private long CycleLengthMs()
	{
		return (long)(this.cycleLength * 1000f);
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x000B8768 File Offset: 0x000B6968
	public double PlatformTime()
	{
		long num = this.NetworkTimeMs();
		long num2 = this.CycleLengthMs();
		return (double)(num - num / num2 * num2) / 1000.0;
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x000B8793 File Offset: 0x000B6993
	public int CycleCount()
	{
		return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000B87A4 File Offset: 0x000B69A4
	public float CycleCompletionPercent()
	{
		float num = (float)(this.PlatformTime() / (double)this.cycleLength);
		num = Mathf.Clamp(num, 0f, 1f);
		if (this.startDelay > 0f)
		{
			float num2 = this.startDelay / this.cycleLength;
			if (num <= num2)
			{
				num = 0f;
			}
			else
			{
				num = (num - num2) / (1f - num2);
			}
		}
		return num;
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000B8806 File Offset: 0x000B6A06
	public bool CycleForward()
	{
		return (this.CycleCount() + (this.startNextCycle ? 1 : 0)) % 2 == 0;
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x000B8820 File Offset: 0x000B6A20
	private void Awake()
	{
		if (this.platformType == MovingPlatform.PlatformType.Child)
		{
			return;
		}
		this.rb = base.GetComponent<Rigidbody>();
		this.initLocalRotation = base.transform.localRotation;
		if (this.pivot != null)
		{
			this.initOffset = this.pivot.transform.position - this.startXf.transform.position;
		}
		this.startPos = this.startXf.position;
		this.endPos = this.endXf.position;
		this.startRot = this.startXf.rotation;
		this.endRot = this.endXf.rotation;
		this.platformInitLocalPos = base.transform.localPosition;
		this.currT = this.startPercentage;
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x000B88F0 File Offset: 0x000B6AF0
	private void OnEnable()
	{
		if (this.platformType == MovingPlatform.PlatformType.Child)
		{
			return;
		}
		base.transform.localRotation = this.initLocalRotation;
		this.startPos = this.startXf.position;
		this.endPos = this.endXf.position;
		this.startRot = this.startXf.rotation;
		this.endRot = this.endXf.rotation;
		this.platformInitLocalPos = base.transform.localPosition;
		this.currT = this.startPercentage;
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000B8979 File Offset: 0x000B6B79
	private Vector3 UpdatePointToPoint()
	{
		return Vector3.Lerp(this.startPos, this.endPos, this.smoothedPercent);
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x000B8994 File Offset: 0x000B6B94
	private Vector3 UpdateArc()
	{
		float angle = Mathf.Lerp(this.rotateStartAmt, this.rotateStartAmt + this.rotateAmt, this.smoothedPercent);
		Quaternion quaternion = this.initLocalRotation;
		Vector3 b = Quaternion.AngleAxis(angle, Vector3.forward) * this.initOffset;
		return this.pivot.transform.position + b;
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x000B89F2 File Offset: 0x000B6BF2
	private Quaternion UpdateRotation()
	{
		return Quaternion.Slerp(this.startRot, this.endRot, this.smoothedPercent);
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000B8A0B File Offset: 0x000B6C0B
	private Quaternion UpdateContinuousRotation()
	{
		return Quaternion.AngleAxis(this.smoothedPercent * 360f, Vector3.up) * base.transform.parent.rotation;
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x000B8A38 File Offset: 0x000B6C38
	private void SetupContext()
	{
		double time = PhotonNetwork.Time;
		if (this.lastServerTime == time)
		{
			this.dtSinceServerUpdate += Time.fixedDeltaTime;
		}
		else
		{
			this.dtSinceServerUpdate = 0f;
			this.lastServerTime = time;
		}
		float num = this.currT;
		this.currT = this.CycleCompletionPercent();
		this.currForward = this.CycleForward();
		this.percent = this.currT;
		if (this.reverseDirOnCycle)
		{
			this.percent = (this.currForward ? this.currT : (1f - this.currT));
		}
		if (this.reverseDir)
		{
			this.percent = 1f - this.percent;
		}
		this.smoothedPercent = this.percent;
		this.lastNT = time;
		this.lastT = Time.time;
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000B8B08 File Offset: 0x000B6D08
	private void Update()
	{
		if (this.platformType == MovingPlatform.PlatformType.Child)
		{
			return;
		}
		this.SetupContext();
		Vector3 vector = base.transform.position;
		Quaternion quaternion = base.transform.rotation;
		bool flag = false;
		switch (this.platformType)
		{
		case MovingPlatform.PlatformType.PointToPoint:
			vector = this.UpdatePointToPoint();
			break;
		case MovingPlatform.PlatformType.Arc:
			vector = this.UpdateArc();
			flag = true;
			break;
		case MovingPlatform.PlatformType.Rotation:
			quaternion = this.UpdateRotation();
			flag = true;
			break;
		case MovingPlatform.PlatformType.ContinuousRotation:
			quaternion = this.UpdateContinuousRotation();
			flag = true;
			break;
		}
		if (!this.debugMovement)
		{
			this.lastPos = this.rb.position;
			this.lastRot = this.rb.rotation;
			if (this.platformType != MovingPlatform.PlatformType.Rotation)
			{
				this.rb.MovePosition(vector);
			}
			if (flag)
			{
				this.rb.MoveRotation(quaternion);
			}
		}
		else
		{
			this.lastPos = base.transform.position;
			this.lastRot = base.transform.rotation;
			base.transform.position = vector;
			if (flag)
			{
				base.transform.rotation = quaternion;
			}
		}
		this.deltaPosition = vector - this.lastPos;
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x000B8C29 File Offset: 0x000B6E29
	public Vector3 ThisFrameMovement()
	{
		return this.deltaPosition;
	}

	// Token: 0x04002956 RID: 10582
	public MovingPlatform.PlatformType platformType;

	// Token: 0x04002957 RID: 10583
	public float cycleLength;

	// Token: 0x04002958 RID: 10584
	public float smoothingHalflife = 0.1f;

	// Token: 0x04002959 RID: 10585
	public float rotateStartAmt;

	// Token: 0x0400295A RID: 10586
	public float rotateAmt;

	// Token: 0x0400295B RID: 10587
	public bool reverseDirOnCycle = true;

	// Token: 0x0400295C RID: 10588
	public bool reverseDir;

	// Token: 0x0400295D RID: 10589
	private CriticalSpringDamper springCD = new CriticalSpringDamper();

	// Token: 0x0400295E RID: 10590
	private Rigidbody rb;

	// Token: 0x0400295F RID: 10591
	public Transform startXf;

	// Token: 0x04002960 RID: 10592
	public Transform endXf;

	// Token: 0x04002961 RID: 10593
	public Vector3 platformInitLocalPos;

	// Token: 0x04002962 RID: 10594
	private Vector3 startPos;

	// Token: 0x04002963 RID: 10595
	private Vector3 endPos;

	// Token: 0x04002964 RID: 10596
	private Quaternion startRot;

	// Token: 0x04002965 RID: 10597
	private Quaternion endRot;

	// Token: 0x04002966 RID: 10598
	public float startPercentage;

	// Token: 0x04002967 RID: 10599
	public float startDelay;

	// Token: 0x04002968 RID: 10600
	public bool startNextCycle;

	// Token: 0x04002969 RID: 10601
	public Transform pivot;

	// Token: 0x0400296A RID: 10602
	private Quaternion initLocalRotation;

	// Token: 0x0400296B RID: 10603
	private Vector3 initOffset;

	// Token: 0x0400296C RID: 10604
	private float currT;

	// Token: 0x0400296D RID: 10605
	private float percent;

	// Token: 0x0400296E RID: 10606
	private float smoothedPercent = -1f;

	// Token: 0x0400296F RID: 10607
	private bool currForward;

	// Token: 0x04002970 RID: 10608
	private float dtSinceServerUpdate;

	// Token: 0x04002971 RID: 10609
	private double lastServerTime;

	// Token: 0x04002972 RID: 10610
	public Vector3 currentVelocity;

	// Token: 0x04002973 RID: 10611
	public Vector3 rotationalAxis;

	// Token: 0x04002974 RID: 10612
	public float angularVelocity;

	// Token: 0x04002975 RID: 10613
	public Vector3 rotationPivot;

	// Token: 0x04002976 RID: 10614
	public Vector3 lastPos;

	// Token: 0x04002977 RID: 10615
	public Quaternion lastRot;

	// Token: 0x04002978 RID: 10616
	public Vector3 deltaPosition;

	// Token: 0x04002979 RID: 10617
	public bool debugMovement;

	// Token: 0x0400297A RID: 10618
	private double lastNT;

	// Token: 0x0400297B RID: 10619
	private float lastT;

	// Token: 0x020005DD RID: 1501
	public enum PlatformType
	{
		// Token: 0x0400297D RID: 10621
		PointToPoint,
		// Token: 0x0400297E RID: 10622
		Arc,
		// Token: 0x0400297F RID: 10623
		Rotation,
		// Token: 0x04002980 RID: 10624
		Child,
		// Token: 0x04002981 RID: 10625
		ContinuousRotation
	}
}
