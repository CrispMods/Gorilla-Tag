using System;
using GTMathUtil;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005DD RID: 1501
public class MovingPlatform : BasePlatform
{
	// Token: 0x0600253F RID: 9535 RVA: 0x0004839C File Offset: 0x0004659C
	public float InitTimeOffset()
	{
		return this.startPercentage * this.cycleLength;
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x000483AB File Offset: 0x000465AB
	private long InitTimeOffsetMs()
	{
		return (long)(this.InitTimeOffset() * 1000f);
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000483BA File Offset: 0x000465BA
	private long NetworkTimeMs()
	{
		if (PhotonNetwork.InRoom)
		{
			return (long)((ulong)(PhotonNetwork.ServerTimestamp + int.MinValue) + (ulong)this.InitTimeOffsetMs());
		}
		return (long)(Time.time * 1000f);
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x000483E3 File Offset: 0x000465E3
	private long CycleLengthMs()
	{
		return (long)(this.cycleLength * 1000f);
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x001038BC File Offset: 0x00101ABC
	public double PlatformTime()
	{
		long num = this.NetworkTimeMs();
		long num2 = this.CycleLengthMs();
		return (double)(num - num / num2 * num2) / 1000.0;
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000483F2 File Offset: 0x000465F2
	public int CycleCount()
	{
		return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x001038E8 File Offset: 0x00101AE8
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

	// Token: 0x06002546 RID: 9542 RVA: 0x00048402 File Offset: 0x00046602
	public bool CycleForward()
	{
		return (this.CycleCount() + (this.startNextCycle ? 1 : 0)) % 2 == 0;
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x0010394C File Offset: 0x00101B4C
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

	// Token: 0x06002548 RID: 9544 RVA: 0x00103A1C File Offset: 0x00101C1C
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

	// Token: 0x06002549 RID: 9545 RVA: 0x0004841C File Offset: 0x0004661C
	private Vector3 UpdatePointToPoint()
	{
		return Vector3.Lerp(this.startPos, this.endPos, this.smoothedPercent);
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x00103AA8 File Offset: 0x00101CA8
	private Vector3 UpdateArc()
	{
		float angle = Mathf.Lerp(this.rotateStartAmt, this.rotateStartAmt + this.rotateAmt, this.smoothedPercent);
		Quaternion quaternion = this.initLocalRotation;
		Vector3 b = Quaternion.AngleAxis(angle, Vector3.forward) * this.initOffset;
		return this.pivot.transform.position + b;
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x00048435 File Offset: 0x00046635
	private Quaternion UpdateRotation()
	{
		return Quaternion.Slerp(this.startRot, this.endRot, this.smoothedPercent);
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x0004844E File Offset: 0x0004664E
	private Quaternion UpdateContinuousRotation()
	{
		return Quaternion.AngleAxis(this.smoothedPercent * 360f, Vector3.up) * base.transform.parent.rotation;
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x00103B08 File Offset: 0x00101D08
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

	// Token: 0x0600254E RID: 9550 RVA: 0x00103BD8 File Offset: 0x00101DD8
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

	// Token: 0x0600254F RID: 9551 RVA: 0x0004847B File Offset: 0x0004667B
	public Vector3 ThisFrameMovement()
	{
		return this.deltaPosition;
	}

	// Token: 0x0400295C RID: 10588
	public MovingPlatform.PlatformType platformType;

	// Token: 0x0400295D RID: 10589
	public float cycleLength;

	// Token: 0x0400295E RID: 10590
	public float smoothingHalflife = 0.1f;

	// Token: 0x0400295F RID: 10591
	public float rotateStartAmt;

	// Token: 0x04002960 RID: 10592
	public float rotateAmt;

	// Token: 0x04002961 RID: 10593
	public bool reverseDirOnCycle = true;

	// Token: 0x04002962 RID: 10594
	public bool reverseDir;

	// Token: 0x04002963 RID: 10595
	private CriticalSpringDamper springCD = new CriticalSpringDamper();

	// Token: 0x04002964 RID: 10596
	private Rigidbody rb;

	// Token: 0x04002965 RID: 10597
	public Transform startXf;

	// Token: 0x04002966 RID: 10598
	public Transform endXf;

	// Token: 0x04002967 RID: 10599
	public Vector3 platformInitLocalPos;

	// Token: 0x04002968 RID: 10600
	private Vector3 startPos;

	// Token: 0x04002969 RID: 10601
	private Vector3 endPos;

	// Token: 0x0400296A RID: 10602
	private Quaternion startRot;

	// Token: 0x0400296B RID: 10603
	private Quaternion endRot;

	// Token: 0x0400296C RID: 10604
	public float startPercentage;

	// Token: 0x0400296D RID: 10605
	public float startDelay;

	// Token: 0x0400296E RID: 10606
	public bool startNextCycle;

	// Token: 0x0400296F RID: 10607
	public Transform pivot;

	// Token: 0x04002970 RID: 10608
	private Quaternion initLocalRotation;

	// Token: 0x04002971 RID: 10609
	private Vector3 initOffset;

	// Token: 0x04002972 RID: 10610
	private float currT;

	// Token: 0x04002973 RID: 10611
	private float percent;

	// Token: 0x04002974 RID: 10612
	private float smoothedPercent = -1f;

	// Token: 0x04002975 RID: 10613
	private bool currForward;

	// Token: 0x04002976 RID: 10614
	private float dtSinceServerUpdate;

	// Token: 0x04002977 RID: 10615
	private double lastServerTime;

	// Token: 0x04002978 RID: 10616
	public Vector3 currentVelocity;

	// Token: 0x04002979 RID: 10617
	public Vector3 rotationalAxis;

	// Token: 0x0400297A RID: 10618
	public float angularVelocity;

	// Token: 0x0400297B RID: 10619
	public Vector3 rotationPivot;

	// Token: 0x0400297C RID: 10620
	public Vector3 lastPos;

	// Token: 0x0400297D RID: 10621
	public Quaternion lastRot;

	// Token: 0x0400297E RID: 10622
	public Vector3 deltaPosition;

	// Token: 0x0400297F RID: 10623
	public bool debugMovement;

	// Token: 0x04002980 RID: 10624
	private double lastNT;

	// Token: 0x04002981 RID: 10625
	private float lastT;

	// Token: 0x020005DE RID: 1502
	public enum PlatformType
	{
		// Token: 0x04002983 RID: 10627
		PointToPoint,
		// Token: 0x04002984 RID: 10628
		Arc,
		// Token: 0x04002985 RID: 10629
		Rotation,
		// Token: 0x04002986 RID: 10630
		Child,
		// Token: 0x04002987 RID: 10631
		ContinuousRotation
	}
}
