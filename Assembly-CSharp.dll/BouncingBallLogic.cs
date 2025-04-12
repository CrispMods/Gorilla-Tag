using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class BouncingBallLogic : MonoBehaviour
{
	// Token: 0x0600131E RID: 4894 RVA: 0x0003C04D File Offset: 0x0003A24D
	private void OnCollisionEnter()
	{
		this.audioSource.PlayOneShot(this.bounce);
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x0003C060 File Offset: 0x0003A260
	private void Start()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		this.audioSource.PlayOneShot(this.loadball);
		this.centerEyeCamera = OVRManager.instance.GetComponentInChildren<OVRCameraRig>().centerEyeAnchor;
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x000B47AC File Offset: 0x000B29AC
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

	// Token: 0x06001321 RID: 4897 RVA: 0x000B4824 File Offset: 0x000B2A24
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

	// Token: 0x06001322 RID: 4898 RVA: 0x000B4898 File Offset: 0x000B2A98
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

	// Token: 0x06001323 RID: 4899 RVA: 0x0003C094 File Offset: 0x0003A294
	public void Release(Vector3 pos, Vector3 vel, Vector3 angVel)
	{
		this.isReleased = true;
		base.transform.position = pos;
		base.GetComponent<Rigidbody>().isKinematic = false;
		base.GetComponent<Rigidbody>().velocity = vel;
		base.GetComponent<Rigidbody>().angularVelocity = angVel;
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x0003C0CD File Offset: 0x0003A2CD
	private IEnumerator PlayPopCallback(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400151C RID: 5404
	[SerializeField]
	private float TTL = 5f;

	// Token: 0x0400151D RID: 5405
	[SerializeField]
	private AudioClip pop;

	// Token: 0x0400151E RID: 5406
	[SerializeField]
	private AudioClip bounce;

	// Token: 0x0400151F RID: 5407
	[SerializeField]
	private AudioClip loadball;

	// Token: 0x04001520 RID: 5408
	[SerializeField]
	private Material visibleMat;

	// Token: 0x04001521 RID: 5409
	[SerializeField]
	private Material hiddenMat;

	// Token: 0x04001522 RID: 5410
	private AudioSource audioSource;

	// Token: 0x04001523 RID: 5411
	private Transform centerEyeCamera;

	// Token: 0x04001524 RID: 5412
	private bool isVisible = true;

	// Token: 0x04001525 RID: 5413
	private float timer;

	// Token: 0x04001526 RID: 5414
	private bool isReleased;

	// Token: 0x04001527 RID: 5415
	private bool isReadyForDestroy;
}
