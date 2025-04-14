using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000633 RID: 1587
public class SodaBubble : MonoBehaviour
{
	// Token: 0x06002770 RID: 10096 RVA: 0x000C1457 File Offset: 0x000BF657
	public void Pop()
	{
		base.StartCoroutine(this.PopCoroutine());
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x000C1466 File Offset: 0x000BF666
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

	// Token: 0x04002B35 RID: 11061
	public MeshRenderer bubbleMesh;

	// Token: 0x04002B36 RID: 11062
	public Rigidbody body;

	// Token: 0x04002B37 RID: 11063
	public MeshCollider bubbleCollider;

	// Token: 0x04002B38 RID: 11064
	public AudioSource audioSource;
}
