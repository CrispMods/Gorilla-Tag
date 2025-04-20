using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class BouncingBallLogic : MonoBehaviour
{
	// Token: 0x06001367 RID: 4967 RVA: 0x0003D30D File Offset: 0x0003B50D
	private void OnCollisionEnter()
	{
		this.audioSource.PlayOneShot(this.bounce);
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x0003D320 File Offset: 0x0003B520
	private void Start()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		this.audioSource.PlayOneShot(this.loadball);
		this.centerEyeCamera = OVRManager.instance.GetComponentInChildren<OVRCameraRig>().centerEyeAnchor;
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x000B7044 File Offset: 0x000B5244
	private void Update()
	{
		if (!this.isReleased)
		{
			return;
		}
		this.UpdateVisibility();
		this.timer += Time.deltaTime;
		if (!this.isReadyForDestroy && this.timer >= this.TTL)
		{
			this.isReadyForDestroy = true;
			float length = this.pop.length;
			this.audioSource.PlayOneShot(this.pop);
			base.StartCoroutine(this.PlayPopCallback(length));
		}
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x000B70BC File Offset: 0x000B52BC
	private void UpdateVisibility()
	{
		Vector3 direction = this.centerEyeCamera.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(base.transform.position, direction), out raycastHit, direction.magnitude))
		{
			if (raycastHit.collider.gameObject != base.gameObject)
			{
				this.SetVisible(false);
				return;
			}
		}
		else
		{
			this.SetVisible(true);
		}
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x000B7130 File Offset: 0x000B5330
	private void SetVisible(bool setVisible)
	{
		if (this.isVisible && !setVisible)
		{
			base.GetComponent<MeshRenderer>().material = this.hiddenMat;
			this.isVisible = false;
		}
		if (!this.isVisible && setVisible)
		{
			base.GetComponent<MeshRenderer>().material = this.visibleMat;
			this.isVisible = true;
		}
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x0003D354 File Offset: 0x0003B554
	public void Release(Vector3 pos, Vector3 vel, Vector3 angVel)
	{
		this.isReleased = true;
		base.transform.position = pos;
		base.GetComponent<Rigidbody>().isKinematic = false;
		base.GetComponent<Rigidbody>().velocity = vel;
		base.GetComponent<Rigidbody>().angularVelocity = angVel;
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x0003D38D File Offset: 0x0003B58D
	private IEnumerator PlayPopCallback(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001563 RID: 5475
	[SerializeField]
	private float TTL = 5f;

	// Token: 0x04001564 RID: 5476
	[SerializeField]
	private AudioClip pop;

	// Token: 0x04001565 RID: 5477
	[SerializeField]
	private AudioClip bounce;

	// Token: 0x04001566 RID: 5478
	[SerializeField]
	private AudioClip loadball;

	// Token: 0x04001567 RID: 5479
	[SerializeField]
	private Material visibleMat;

	// Token: 0x04001568 RID: 5480
	[SerializeField]
	private Material hiddenMat;

	// Token: 0x04001569 RID: 5481
	private AudioSource audioSource;

	// Token: 0x0400156A RID: 5482
	private Transform centerEyeCamera;

	// Token: 0x0400156B RID: 5483
	private bool isVisible = true;

	// Token: 0x0400156C RID: 5484
	private float timer;

	// Token: 0x0400156D RID: 5485
	private bool isReleased;

	// Token: 0x0400156E RID: 5486
	private bool isReadyForDestroy;
}
