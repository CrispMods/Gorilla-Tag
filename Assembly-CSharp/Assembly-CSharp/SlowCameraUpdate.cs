using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class SlowCameraUpdate : MonoBehaviour
{
	// Token: 0x06002781 RID: 10113 RVA: 0x000C19B7 File Offset: 0x000BFBB7
	public void Awake()
	{
		this.frameRate = 30f;
		this.timeToNextFrame = 1f / this.frameRate;
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000C19E2 File Offset: 0x000BFBE2
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdateMirror());
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x00019C59 File Offset: 0x00017E59
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000C19F1 File Offset: 0x000BFBF1
	public IEnumerator UpdateMirror()
	{
		for (;;)
		{
			if (base.gameObject.activeSelf)
			{
				Debug.Log("rendering camera!");
				this.myCamera.Render();
			}
			yield return new WaitForSeconds(this.timeToNextFrame);
		}
		yield break;
	}

	// Token: 0x04002B42 RID: 11074
	private Camera myCamera;

	// Token: 0x04002B43 RID: 11075
	private float frameRate;

	// Token: 0x04002B44 RID: 11076
	private float timeToNextFrame;
}
