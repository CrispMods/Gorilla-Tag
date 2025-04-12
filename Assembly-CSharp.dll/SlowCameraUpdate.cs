using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class SlowCameraUpdate : MonoBehaviour
{
	// Token: 0x06002781 RID: 10113 RVA: 0x0004A046 File Offset: 0x00048246
	public void Awake()
	{
		this.frameRate = 30f;
		this.timeToNextFrame = 1f / this.frameRate;
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x0004A071 File Offset: 0x00048271
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdateMirror());
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x0003242F File Offset: 0x0003062F
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x0004A080 File Offset: 0x00048280
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
