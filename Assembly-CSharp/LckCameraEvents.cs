using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x02000243 RID: 579
public class LckCameraEvents : MonoBehaviour
{
	// Token: 0x06000D58 RID: 3416 RVA: 0x00039721 File Offset: 0x00037921
	private void OnEnable()
	{
		RenderPipelineManager.beginCameraRendering += this.RenderPipelineManagerOnbeginCameraRendering;
		RenderPipelineManager.endCameraRendering += this.RenderPipelineManagerOnendCameraRendering;
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x00039745 File Offset: 0x00037945
	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= this.RenderPipelineManagerOnbeginCameraRendering;
		RenderPipelineManager.endCameraRendering -= this.RenderPipelineManagerOnendCameraRendering;
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x00039769 File Offset: 0x00037969
	private void RenderPipelineManagerOnbeginCameraRendering(ScriptableRenderContext scriptableRenderContext, Camera camera)
	{
		if (this._camera != camera)
		{
			return;
		}
		UnityEvent unityEvent = this.onPreRender;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x0003978A File Offset: 0x0003798A
	private void RenderPipelineManagerOnendCameraRendering(ScriptableRenderContext scriptableRenderContext, Camera camera)
	{
		if (this._camera != camera)
		{
			return;
		}
		UnityEvent unityEvent = this.onPostRender;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040010A6 RID: 4262
	[SerializeField]
	private Camera _camera;

	// Token: 0x040010A7 RID: 4263
	public UnityEvent onPreRender;

	// Token: 0x040010A8 RID: 4264
	public UnityEvent onPostRender;
}
