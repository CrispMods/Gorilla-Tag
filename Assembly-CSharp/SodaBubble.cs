using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000612 RID: 1554
public class SodaBubble : MonoBehaviour
{
	// Token: 0x0600269B RID: 9883 RVA: 0x0004A5A6 File Offset: 0x000487A6
	public void Pop()
	{
		base.StartCoroutine(this.PopCoroutine());
	}

	// Token: 0x0600269C RID: 9884 RVA: 0x0004A5B5 File Offset: 0x000487B5
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

	// Token: 0x04002A9B RID: 10907
	public MeshRenderer bubbleMesh;

	// Token: 0x04002A9C RID: 10908
	public Rigidbody body;

	// Token: 0x04002A9D RID: 10909
	public MeshCollider bubbleCollider;

	// Token: 0x04002A9E RID: 10910
	public AudioSource audioSource;
}
