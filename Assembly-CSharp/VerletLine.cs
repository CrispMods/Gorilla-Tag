using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200065C RID: 1628
[DisallowMultipleComponent]
public class VerletLine : MonoBehaviour
{
	// Token: 0x0600284F RID: 10319 RVA: 0x00110C08 File Offset: 0x0010EE08
	private void Awake()
	{
		this._nodes = new VerletLine.LineNode[this.segmentNumber];
		this._positions = new Vector3[this.segmentNumber];
		for (int i = 0; i < this.segmentNumber; i++)
		{
			float t = (float)i / (float)(this.segmentNumber - 1);
			Vector3 vector = Vector3.Lerp(this.lineStart.position, this.lineEnd.position, t);
			this._nodes[i] = new VerletLine.LineNode
			{
				position = vector,
				lastPosition = vector,
				acceleration = this.gravity
			};
		}
		this.line.positionCount = this._nodes.Length;
		this.endRigidbody = this.lineEnd.GetComponent<Rigidbody>();
		if (this.endRigidbody)
		{
			this.endRigidbody.maxLinearVelocity = this.endMaxSpeed;
			this.endRigidbodyParent = this.endRigidbody.transform.parent;
			this.rigidBodyStartingLocalPosition = this.endRigidbody.transform.localPosition;
			this.endRigidbody.transform.parent = null;
			this.endRigidbody.gameObject.SetActive(false);
		}
		this.totalLineLength = this.segmentLength * (float)this.segmentNumber;
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x00110D48 File Offset: 0x0010EF48
	private void OnEnable()
	{
		if (this.endRigidbody)
		{
			this.endRigidbody.gameObject.SetActive(true);
			this.endRigidbody.transform.localPosition = this.endRigidbodyParent.TransformPoint(this.rigidBodyStartingLocalPosition);
		}
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x0004B6A0 File Offset: 0x000498A0
	private void OnDisable()
	{
		if (this.endRigidbody)
		{
			this.endRigidbody.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x00110D94 File Offset: 0x0010EF94
	public void SetLength(float total, float delay = 0f)
	{
		this.segmentTargetLength = total / (float)this.segmentNumber;
		if (this.segmentTargetLength < this.segmentMinLength)
		{
			this.segmentTargetLength = this.segmentMinLength;
		}
		if (this.segmentTargetLength > this.segmentMaxLength)
		{
			this.segmentTargetLength = this.segmentMaxLength;
		}
		if (delay >= 0.01f)
		{
			base.StartCoroutine(this.ResizeAfterDelay(delay));
		}
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x00110DFC File Offset: 0x0010EFFC
	public void AddSegmentLength(float amount, float delay = 0f)
	{
		this.segmentTargetLength = this.segmentLength + amount;
		if (this.segmentTargetLength <= 0f)
		{
			return;
		}
		if (this.segmentTargetLength > this.segmentMaxLength)
		{
			this.segmentTargetLength = this.segmentMaxLength;
		}
		if (delay >= 0.01f)
		{
			base.StartCoroutine(this.ResizeAfterDelay(delay));
		}
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x00110E58 File Offset: 0x0010F058
	public void RemoveSegmentLength(float amount, float delay = 0f)
	{
		this.segmentTargetLength = this.segmentLength - amount;
		if (this.segmentTargetLength <= this.segmentMinLength)
		{
			this.segmentTargetLength = (this.segmentLength = this.segmentMinLength);
			return;
		}
		if (delay >= 0.01f)
		{
			base.StartCoroutine(this.ResizeAfterDelay(delay));
		}
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x0004B6C0 File Offset: 0x000498C0
	private IEnumerator ResizeAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		yield break;
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x00110EB0 File Offset: 0x0010F0B0
	private void Update()
	{
		if (this.segmentLength.Approx(this.segmentTargetLength, 0.1f))
		{
			this.segmentLength = this.segmentTargetLength;
			return;
		}
		this.segmentLength = Mathf.Lerp(this.segmentLength, this.segmentTargetLength, this.resizeSpeed * this.resizeScale * Time.deltaTime);
		if (this.scaleLineWidth)
		{
			this.line.widthMultiplier = base.transform.lossyScale.x;
		}
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x00110F30 File Offset: 0x0010F130
	public void ForceTotalLength(float totalLength)
	{
		float num = totalLength / (float)((this.segmentNumber < 1) ? 1 : this.segmentNumber);
		this.segmentLength = (this.segmentTargetLength = num);
		this.totalLineLength = this.segmentLength * (float)this.segmentNumber;
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x00110F78 File Offset: 0x0010F178
	private void FixedUpdate()
	{
		for (int i = 0; i < this._nodes.Length; i++)
		{
			VerletLine.Simulate(ref this._nodes[i], Time.fixedDeltaTime);
		}
		for (int j = 0; j < this.simIterations; j++)
		{
			for (int k = 0; k < this._nodes.Length - 1; k++)
			{
				VerletLine.LimitDistance(ref this._nodes[k], ref this._nodes[k + 1], this.segmentLength);
			}
		}
		this._nodes[0].position = this.lineStart.position;
		if (this.endRigidbody)
		{
			if (this.onlyPullAtEdges)
			{
				if ((this.endRigidbody.transform.position - this.lineStart.position).IsLongerThan(this.totalLineLength))
				{
					Vector3 a = this.lineStart.position + (this.endRigidbody.transform.position - this.lineStart.position).normalized * this.totalLineLength;
					this.endRigidbody.velocity += (a - this.endRigidbody.transform.position) / Time.fixedDeltaTime;
					if (this.endRigidbody.velocity.IsLongerThan(this.endMaxSpeed))
					{
						this.endRigidbody.velocity = this.endRigidbody.velocity.normalized * this.endMaxSpeed;
					}
				}
			}
			else
			{
				VerletLine.LineNode[] nodes = this._nodes;
				Vector3 force = (nodes[nodes.Length - 1].position - this.lineEnd.position) * (this.tension * this.tensionScale);
				Quaternion rotation = this.endRigidbody.rotation;
				VerletLine.LineNode[] nodes2 = this._nodes;
				Vector3 position = nodes2[nodes2.Length - 1].position;
				VerletLine.LineNode[] nodes3 = this._nodes;
				Quaternion.LookRotation(position - nodes3[nodes3.Length - 2].position);
				if (!this.endRigidbody.isKinematic)
				{
					this.endRigidbody.AddForceAtPosition(force, this.endRigidbody.transform.TransformPoint(this.endLineAnchorLocalPosition));
				}
			}
		}
		VerletLine.LineNode[] nodes4 = this._nodes;
		nodes4[nodes4.Length - 1].position = this.lineEnd.position;
		for (int l = 0; l < this._nodes.Length; l++)
		{
			this._positions[l] = this._nodes[l].position;
		}
		this.line.SetPositions(this._positions);
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x00111238 File Offset: 0x0010F438
	private static void Simulate(ref VerletLine.LineNode p, float dt)
	{
		Vector3 position = p.position;
		p.position += p.position - p.lastPosition + p.acceleration * (dt * dt);
		p.lastPosition = position;
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x00111290 File Offset: 0x0010F490
	private static void LimitDistance(ref VerletLine.LineNode p1, ref VerletLine.LineNode p2, float restLength)
	{
		Vector3 a = p2.position - p1.position;
		float num = a.magnitude + 1E-05f;
		float num2 = (num - restLength) / num;
		p1.position += a * (num2 * 0.5f);
		p2.position -= a * (num2 * 0.5f);
	}

	// Token: 0x04002DAD RID: 11693
	public Transform lineStart;

	// Token: 0x04002DAE RID: 11694
	public Transform lineEnd;

	// Token: 0x04002DAF RID: 11695
	[Space]
	public LineRenderer line;

	// Token: 0x04002DB0 RID: 11696
	public Rigidbody endRigidbody;

	// Token: 0x04002DB1 RID: 11697
	public Transform endRigidbodyParent;

	// Token: 0x04002DB2 RID: 11698
	public Vector3 endLineAnchorLocalPosition;

	// Token: 0x04002DB3 RID: 11699
	private Vector3 rigidBodyStartingLocalPosition;

	// Token: 0x04002DB4 RID: 11700
	[Space]
	public int segmentNumber = 10;

	// Token: 0x04002DB5 RID: 11701
	public float segmentLength = 0.03f;

	// Token: 0x04002DB6 RID: 11702
	public float segmentTargetLength = 0.03f;

	// Token: 0x04002DB7 RID: 11703
	public float segmentMaxLength = 0.03f;

	// Token: 0x04002DB8 RID: 11704
	public float segmentMinLength = 0.03f;

	// Token: 0x04002DB9 RID: 11705
	[Space]
	public Vector3 gravity = new Vector3(0f, -9.81f, 0f);

	// Token: 0x04002DBA RID: 11706
	public int simIterations = 6;

	// Token: 0x04002DBB RID: 11707
	public float tension = 10f;

	// Token: 0x04002DBC RID: 11708
	public float tensionScale = 1f;

	// Token: 0x04002DBD RID: 11709
	public float endMaxSpeed = 48f;

	// Token: 0x04002DBE RID: 11710
	[FormerlySerializedAs("lerpSpeed")]
	[Space]
	public float resizeSpeed = 1f;

	// Token: 0x04002DBF RID: 11711
	public float resizeScale = 1f;

	// Token: 0x04002DC0 RID: 11712
	[NonSerialized]
	private VerletLine.LineNode[] _nodes = new VerletLine.LineNode[0];

	// Token: 0x04002DC1 RID: 11713
	[NonSerialized]
	private Vector3[] _positions = new Vector3[0];

	// Token: 0x04002DC2 RID: 11714
	private float totalLineLength;

	// Token: 0x04002DC3 RID: 11715
	[SerializeField]
	private bool onlyPullAtEdges;

	// Token: 0x04002DC4 RID: 11716
	[SerializeField]
	private bool scaleLineWidth = true;

	// Token: 0x0200065D RID: 1629
	[Serializable]
	public struct LineNode
	{
		// Token: 0x04002DC5 RID: 11717
		public Vector3 position;

		// Token: 0x04002DC6 RID: 11718
		public Vector3 lastPosition;

		// Token: 0x04002DC7 RID: 11719
		public Vector3 acceleration;
	}
}
