using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class BouncingBallLogic : MonoBehaviour
{
	// Token: 0x0600131B RID: 4891 RVA: 0x0005D22E File Offset: 0x0005B42E
	private void OnCollisionEnter()
	{
		this.audioSource.PlayOneShot(this.bounce);
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x0005D241 File Offset: 0x0005B441
	private void Start()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		this.audioSource.PlayOneShot(this.loadball);
		this.centerEyeCamera = OVRManager.instance.GetComponentInChildren<OVRCameraRig>().centerEyeAnchor;
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x0005D278 File Offset: 0x0005B478
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

	// Token: 0x0600131E RID: 4894 RVA: 0x0005D2F0 File Offset: 0x0005B4F0
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

	// Token: 0x0600131F RID: 4895 RVA: 0x0005D364 File Offset: 0x0005B564
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

	// Token: 0x06001320 RID: 4896 RVA: 0x0005D3B9 File Offset: 0x0005B5B9
	public void Release(Vector3 pos, Vector3 vel, Vector3 angVel)
	{
		this.isReleased = true;
		base.transform.position = pos;
		base.GetComponent<Rigidbody>().isKinematic = false;
		base.GetComponent<Rigidbody>().velocity = vel;
		base.GetComponent<Rigidbody>().angularVelocity = angVel;
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x0005D3F2 File Offset: 0x0005B5F2
	private IEnumerator PlayPopCallback(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400151B RID: 5403
	[SerializeField]
	private float TTL = 5f;

	// Token: 0x0400151C RID: 5404
	[SerializeField]
	private AudioClip pop;

	// Token: 0x0400151D RID: 5405
	[SerializeField]
	private AudioClip bounce;

	// Token: 0x0400151E RID: 5406
	[SerializeField]
	private AudioClip loadball;

	// Token: 0x0400151F RID: 5407
	[SerializeField]
	private Material visibleMat;

	// Token: 0x04001520 RID: 5408
	[SerializeField]
	private Material hiddenMat;

	// Token: 0x04001521 RID: 5409
	private AudioSource audioSource;

	// Token: 0x04001522 RID: 5410
	private Transform centerEyeCamera;

	// Token: 0x04001523 RID: 5411
	private bool isVisible = true;

	// Token: 0x04001524 RID: 5412
	private float timer;

	// Token: 0x04001525 RID: 5413
	private bool isReleased;

	// Token: 0x04001526 RID: 5414
	private bool isReadyForDestroy;
}
