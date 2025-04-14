using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200068C RID: 1676
public class GorillaFireball : GorillaThrowable, IPunInstantiateMagicCallback
{
	// Token: 0x060029B0 RID: 10672 RVA: 0x000CECF6 File Offset: 0x000CCEF6
	public override void Start()
	{
		base.Start();
		this.canExplode = false;
		this.explosionStartTime = 0f;
	}

	// Token: 0x060029B1 RID: 10673 RVA: 0x000CED10 File Offset: 0x000CCF10
	private void Update()
	{
		if (this.explosionStartTime != 0f)
		{
			float num = (Time.time - this.explosionStartTime) / this.totalExplosionTime * (this.maxExplosionScale - 0.25f) + 0.25f;
			base.gameObject.transform.localScale = new Vector3(num, num, num);
			if (base.photonView.IsMine && Time.time > this.explosionStartTime + this.totalExplosionTime)
			{
				PhotonNetwork.Destroy(PhotonView.Get(this));
			}
		}
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x000CED98 File Offset: 0x000CCF98
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (this.rigidbody.useGravity)
		{
			this.rigidbody.AddForce(Physics.gravity * -this.gravityStrength * this.rigidbody.mass);
		}
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x000CEDE4 File Offset: 0x000CCFE4
	public override void ThrowThisThingo()
	{
		base.ThrowThisThingo();
		this.canExplode = true;
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x000CEDF3 File Offset: 0x000CCFF3
	private new void OnCollisionEnter(Collision collision)
	{
		if (base.photonView.IsMine && this.canExplode)
		{
			base.photonView.RPC("Explode", RpcTarget.All, null);
		}
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x000CEE1C File Offset: 0x000CD01C
	public void LocalExplode()
	{
		this.rigidbody.isKinematic = true;
		this.canExplode = false;
		this.explosionStartTime = Time.time;
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x000CEE3C File Offset: 0x000CD03C
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (base.photonView.IsMine)
		{
			if ((bool)base.photonView.InstantiationData[0])
			{
				base.transform.parent = GorillaPlaySpace.Instance.myVRRig.leftHandTransform;
				return;
			}
			base.transform.parent = GorillaPlaySpace.Instance.myVRRig.rightHandTransform;
		}
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x000CEE9F File Offset: 0x000CD09F
	[PunRPC]
	public void Explode()
	{
		this.LocalExplode();
	}

	// Token: 0x04002EFF RID: 12031
	public float maxExplosionScale;

	// Token: 0x04002F00 RID: 12032
	public float totalExplosionTime;

	// Token: 0x04002F01 RID: 12033
	public float gravityStrength;

	// Token: 0x04002F02 RID: 12034
	private bool canExplode;

	// Token: 0x04002F03 RID: 12035
	private float explosionStartTime;
}
