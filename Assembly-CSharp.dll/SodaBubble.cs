using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class SodaBubble : MonoBehaviour
{
	// Token: 0x06002778 RID: 10104 RVA: 0x0004A011 File Offset: 0x00048211
	public void Pop()
	{
		base.StartCoroutine(this.PopCoroutine());
	}

	// Token: 0x06002779 RID: 10105 RVA: 0x0004A020 File Offset: 0x00048220
	private IEnumerator PopCoroutine()
	{
		this.audioSource.GTPlay();
		this.bubbleMesh.gameObject.SetActive(false);
		this.bubbleCollider.gameObject.SetActive(false);
		yield return new WaitForSeconds(1f);
		this.bubbleMesh.gameObject.SetActive(true);
		this.bubbleCollider.gameObject.SetActive(true);
		ObjectPools.instance.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002B3B RID: 11067
	public MeshRenderer bubbleMesh;

	// Token: 0x04002B3C RID: 11068
	public Rigidbody body;

	// Token: 0x04002B3D RID: 11069
	public MeshCollider bubbleCollider;

	// Token: 0x04002B3E RID: 11070
	public AudioSource audioSource;
}
