using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000656 RID: 1622
public class GorillaFireball : GorillaThrowable, IPunInstantiateMagicCallback
{
	// Token: 0x0600282F RID: 10287 RVA: 0x0004B59D File Offset: 0x0004979D
	public override void Start()
	{
		base.Start();
		this.canExplode = false;
		this.explosionStartTime = 0f;
	}

	// Token: 0x06002830 RID: 10288 RVA: 0x0010FF88 File Offset: 0x0010E188
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

	// Token: 0x06002831 RID: 10289 RVA: 0x00110010 File Offset: 0x0010E210
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (this.rigidbody.useGravity)
		{
			this.rigidbody.AddForce(Physics.gravity * -this.gravityStrength * this.rigidbody.mass);
		}
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x0004B5B7 File Offset: 0x000497B7
	public override void ThrowThisThingo()
	{
		base.ThrowThisThingo();
		this.canExplode = true;
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x0004B5C6 File Offset: 0x000497C6
	private new void OnCollisionEnter(Collision collision)
	{
		if (base.photonView.IsMine && this.canExplode)
		{
			base.photonView.RPC("Explode", RpcTarget.All, null);
		}
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x0004B5EF File Offset: 0x000497EF
	public void LocalExplode()
	{
		this.rigidbody.isKinematic = true;
		this.canExplode = false;
		this.explosionStartTime = Time.time;
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x0011005C File Offset: 0x0010E25C
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

	// Token: 0x06002836 RID: 10294 RVA: 0x0004B60F File Offset: 0x0004980F
	[PunRPC]
	public void Explode()
	{
		this.LocalExplode();
	}

	// Token: 0x04002D6F RID: 11631
	public float maxExplosionScale;

	// Token: 0x04002D70 RID: 11632
	public float totalExplosionTime;

	// Token: 0x04002D71 RID: 11633
	public float gravityStrength;

	// Token: 0x04002D72 RID: 11634
	private bool canExplode;

	// Token: 0x04002D73 RID: 11635
	private float explosionStartTime;
}
