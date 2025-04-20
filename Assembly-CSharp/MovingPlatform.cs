using System;
using GTMathUtil;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public class MovingPlatform : BasePlatform
{
	// Token: 0x06002599 RID: 9625 RVA: 0x00049773 File Offset: 0x00047973
	public float InitTimeOffset()
	{
		return this.startPercentage * this.cycleLength;
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x00049782 File Offset: 0x00047982
	private long InitTimeOffsetMs()
	{
		return (long)(this.InitTimeOffset() * 1000f);
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x00049791 File Offset: 0x00047991
	private long NetworkTimeMs()
	{
		if (PhotonNetwork.InRoom)
		{
			return (long)((ulong)(PhotonNetwork.ServerTimestamp + int.MinValue) + (ulong)this.InitTimeOffsetMs());
		}
		return (long)(Time.time * 1000f);
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x000497BA File Offset: 0x000479BA
	private long CycleLengthMs()
	{
		return (long)(this.cycleLength * 1000f);
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x001067F8 File Offset: 0x001049F8
	public double PlatformTime()
	{
		long num = this.NetworkTimeMs();
		long num2 = this.CycleLengthMs();
		return (double)(num - num / num2 * num2) / 1000.0;
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x000497C9 File Offset: 0x000479C9
	public int CycleCount()
	{
		return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x00106824 File Offset: 0x00104A24
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

	// Token: 0x060025A0 RID: 9632 RVA: 0x000497D9 File Offset: 0x000479D9
	public bool CycleForward()
	{
		return (this.CycleCount() + (this.startNextCycle ? 1 : 0)) % 2 == 0;
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x00106888 File Offset: 0x00104A88
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

	// Token: 0x060025A2 RID: 9634 RVA: 0x00106958 File Offset: 0x00104B58
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

	// Token: 0x060025A3 RID: 9635 RVA: 0x000497F3 File Offset: 0x000479F3
	private Vector3 UpdatePointToPoint()
	{
		return Vector3.Lerp(this.startPos, this.endPos, this.smoothedPercent);
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x001069E4 File Offset: 0x00104BE4
	private Vector3 UpdateArc()
	{
		float angle = Mathf.Lerp(this.rotateStartAmt, this.rotateStartAmt + this.rotateAmt, this.smoothedPercent);
		Quaternion quaternion = this.initLocalRotation;
		Vector3 b = Quaternion.AngleAxis(angle, Vector3.forward) * this.initOffset;
		return this.pivot.transform.position + b;
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x0004980C File Offset: 0x00047A0C
	private Quaternion UpdateRotation()
	{
		return Quaternion.Slerp(this.startRot, this.endRot, this.smoothedPercent);
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x00049825 File Offset: 0x00047A25
	private Quaternion UpdateContinuousRotation()
	{
		return Quaternion.AngleAxis(this.smoothedPercent * 360f, Vector3.up) * base.transform.parent.rotation;
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x00106A44 File Offset: 0x00104C44
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

	// Token: 0x060025A8 RID: 9640 RVA: 0x00106B14 File Offset: 0x00104D14
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

	// Token: 0x060025A9 RID: 9641 RVA: 0x00049852 File Offset: 0x00047A52
	public Vector3 ThisFrameMovement()
	{
		return this.deltaPosition;
	}

	// Token: 0x040029B5 RID: 10677
	public MovingPlatform.PlatformType platformType;

	// Token: 0x040029B6 RID: 10678
	public float cycleLength;

	// Token: 0x040029B7 RID: 10679
	public float smoothingHalflife = 0.1f;

	// Token: 0x040029B8 RID: 10680
	public float rotateStartAmt;

	// Token: 0x040029B9 RID: 10681
	public float rotateAmt;

	// Token: 0x040029BA RID: 10682
	public bool reverseDirOnCycle = true;

	// Token: 0x040029BB RID: 10683
	public bool reverseDir;

	// Token: 0x040029BC RID: 10684
	private CriticalSpringDamper springCD = new CriticalSpringDamper();

	// Token: 0x040029BD RID: 10685
	private Rigidbody rb;

	// Token: 0x040029BE RID: 10686
	public Transform startXf;

	// Token: 0x040029BF RID: 10687
	public Transform endXf;

	// Token: 0x040029C0 RID: 10688
	public Vector3 platformInitLocalPos;

	// Token: 0x040029C1 RID: 10689
	private Vector3 startPos;

	// Token: 0x040029C2 RID: 10690
	private Vector3 endPos;

	// Token: 0x040029C3 RID: 10691
	private Quaternion startRot;

	// Token: 0x040029C4 RID: 10692
	private Quaternion endRot;

	// Token: 0x040029C5 RID: 10693
	public float startPercentage;

	// Token: 0x040029C6 RID: 10694
	public float startDelay;

	// Token: 0x040029C7 RID: 10695
	public bool startNextCycle;

	// Token: 0x040029C8 RID: 10696
	public Transform pivot;

	// Token: 0x040029C9 RID: 10697
	private Quaternion initLocalRotation;

	// Token: 0x040029CA RID: 10698
	private Vector3 initOffset;

	// Token: 0x040029CB RID: 10699
	private float currT;

	// Token: 0x040029CC RID: 10700
	private float percent;

	// Token: 0x040029CD RID: 10701
	private float smoothedPercent = -1f;

	// Token: 0x040029CE RID: 10702
	private bool currForward;

	// Token: 0x040029CF RID: 10703
	private float dtSinceServerUpdate;

	// Token: 0x040029D0 RID: 10704
	private double lastServerTime;

	// Token: 0x040029D1 RID: 10705
	public Vector3 currentVelocity;

	// Token: 0x040029D2 RID: 10706
	public Vector3 rotationalAxis;

	// Token: 0x040029D3 RID: 10707
	public float angularVelocity;

	// Token: 0x040029D4 RID: 10708
	public Vector3 rotationPivot;

	// Token: 0x040029D5 RID: 10709
	public Vector3 lastPos;

	// Token: 0x040029D6 RID: 10710
	public Quaternion lastRot;

	// Token: 0x040029D7 RID: 10711
	public Vector3 deltaPosition;

	// Token: 0x040029D8 RID: 10712
	public bool debugMovement;

	// Token: 0x040029D9 RID: 10713
	private double lastNT;

	// Token: 0x040029DA RID: 10714
	private float lastT;

	// Token: 0x020005EB RID: 1515
	public enum PlatformType
	{
		// Token: 0x040029DC RID: 10716
		PointToPoint,
		// Token: 0x040029DD RID: 10717
		Arc,
		// Token: 0x040029DE RID: 10718
		Rotation,
		// Token: 0x040029DF RID: 10719
		Child,
		// Token: 0x040029E0 RID: 10720
		ContinuousRotation
	}
}
