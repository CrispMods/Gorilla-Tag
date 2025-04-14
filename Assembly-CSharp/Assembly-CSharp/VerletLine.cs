using System;
using System.Collections;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000693 RID: 1683
[DisallowMultipleComponent]
public class VerletLine : MonoBehaviour
{
	// Token: 0x060029D8 RID: 10712 RVA: 0x000CFEFC File Offset: 0x000CE0FC
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

	// Token: 0x060029D9 RID: 10713 RVA: 0x000D003C File Offset: 0x000CE23C
	private void OnEnable()
	{
		if (this.endRigidbody)
		{
			this.endRigidbody.gameObject.SetActive(true);
			this.endRigidbody.transform.localPosition = this.endRigidbodyParent.TransformPoint(this.rigidBodyStartingLocalPosition);
		}
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x000D0088 File Offset: 0x000CE288
	private void OnDisable()
	{
		if (this.endRigidbody)
		{
			this.endRigidbody.gameObject.SetActive(false);
		}
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x000D00A8 File Offset: 0x000CE2A8
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

	// Token: 0x060029DC RID: 10716 RVA: 0x000D0110 File Offset: 0x000CE310
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

	// Token: 0x060029DD RID: 10717 RVA: 0x000D016C File Offset: 0x000CE36C
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

	// Token: 0x060029DE RID: 10718 RVA: 0x000D01C1 File Offset: 0x000CE3C1
	private IEnumerator ResizeAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		yield break;
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x000D01D0 File Offset: 0x000CE3D0
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

	// Token: 0x060029E0 RID: 10720 RVA: 0x000D0250 File Offset: 0x000CE450
	public void ForceTotalLength(float totalLength)
	{
		float num = totalLength / (float)((this.segmentNumber < 1) ? 1 : this.segmentNumber);
		this.segmentLength = (this.segmentTargetLength = num);
		this.totalLineLength = this.segmentLength * (float)this.segmentNumber;
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x000D0298 File Offset: 0x000CE498
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

	// Token: 0x060029E2 RID: 10722 RVA: 0x000D0558 File Offset: 0x000CE758
	private static void Simulate(ref VerletLine.LineNode p, float dt)
	{
		Vector3 position = p.position;
		p.position += p.position - p.lastPosition + p.acceleration * (dt * dt);
		p.lastPosition = position;
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x000D05B0 File Offset: 0x000CE7B0
	private static void LimitDistance(ref VerletLine.LineNode p1, ref VerletLine.LineNode p2, float restLength)
	{
		Vector3 a = p2.position - p1.position;
		float num = a.magnitude + 1E-05f;
		float num2 = (num - restLength) / num;
		p1.position += a * (num2 * 0.5f);
		p2.position -= a * (num2 * 0.5f);
	}

	// Token: 0x04002F43 RID: 12099
	public Transform lineStart;

	// Token: 0x04002F44 RID: 12100
	public Transform lineEnd;

	// Token: 0x04002F45 RID: 12101
	[Space]
	public LineRenderer line;

	// Token: 0x04002F46 RID: 12102
	public Rigidbody endRigidbody;

	// Token: 0x04002F47 RID: 12103
	public Transform endRigidbodyParent;

	// Token: 0x04002F48 RID: 12104
	public Vector3 endLineAnchorLocalPosition;

	// Token: 0x04002F49 RID: 12105
	private Vector3 rigidBodyStartingLocalPosition;

	// Token: 0x04002F4A RID: 12106
	[Space]
	public int segmentNumber = 10;

	// Token: 0x04002F4B RID: 12107
	public float segmentLength = 0.03f;

	// Token: 0x04002F4C RID: 12108
	public float segmentTargetLength = 0.03f;

	// Token: 0x04002F4D RID: 12109
	public float segmentMaxLength = 0.03f;

	// Token: 0x04002F4E RID: 12110
	public float segmentMinLength = 0.03f;

	// Token: 0x04002F4F RID: 12111
	[Space]
	public Vector3 gravity = new Vector3(0f, -9.81f, 0f);

	// Token: 0x04002F50 RID: 12112
	public int simIterations = 6;

	// Token: 0x04002F51 RID: 12113
	public float tension = 10f;

	// Token: 0x04002F52 RID: 12114
	public float tensionScale = 1f;

	// Token: 0x04002F53 RID: 12115
	public float endMaxSpeed = 48f;

	// Token: 0x04002F54 RID: 12116
	[FormerlySerializedAs("lerpSpeed")]
	[Space]
	public float resizeSpeed = 1f;

	// Token: 0x04002F55 RID: 12117
	public float resizeScale = 1f;

	// Token: 0x04002F56 RID: 12118
	[NonSerialized]
	private VerletLine.LineNode[] _nodes = new VerletLine.LineNode[0];

	// Token: 0x04002F57 RID: 12119
	[NonSerialized]
	private Vector3[] _positions = new Vector3[0];

	// Token: 0x04002F58 RID: 12120
	private float totalLineLength;

	// Token: 0x04002F59 RID: 12121
	[SerializeField]
	private bool onlyPullAtEdges;

	// Token: 0x04002F5A RID: 12122
	[SerializeField]
	private bool scaleLineWidth = true;

	// Token: 0x02000694 RID: 1684
	[Serializable]
	public struct LineNode
	{
		// Token: 0x04002F5B RID: 12123
		public Vector3 position;

		// Token: 0x04002F5C RID: 12124
		public Vector3 lastPosition;

		// Token: 0x04002F5D RID: 12125
		public Vector3 acceleration;
	}
}
