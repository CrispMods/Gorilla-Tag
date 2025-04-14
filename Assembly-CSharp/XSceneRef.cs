using System;
using UnityEngine;

// Token: 0x0200021A RID: 538
[Serializable]
public struct XSceneRef
{
	// Token: 0x06000C75 RID: 3189 RVA: 0x00042388 File Offset: 0x00040588
	public bool TryResolve(out XSceneRefTarget result)
	{
		if (this.TargetID == 0)
		{
			result = null;
			return true;
		}
		if (this.didCache && this.cached != null)
		{
			result = this.cached;
			return true;
		}
		XSceneRefTarget xsceneRefTarget;
		if (!XSceneRefGlobalHub.TryResolve(this.TargetScene, this.TargetID, out xsceneRefTarget))
		{
			result = null;
			return false;
		}
		this.cached = xsceneRefTarget;
		this.didCache = true;
		result = xsceneRefTarget;
		return true;
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x000423F0 File Offset: 0x000405F0
	public bool TryResolve(out GameObject result)
	{
		XSceneRefTarget xsceneRefTarget;
		if (this.TryResolve(out xsceneRefTarget))
		{
			result = ((xsceneRefTarget == null) ? null : xsceneRefTarget.gameObject);
			return true;
		}
		result = null;
		return false;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00042424 File Offset: 0x00040624
	public bool TryResolve<T>(out T result) where T : Component
	{
		XSceneRefTarget xsceneRefTarget;
		if (this.TryResolve(out xsceneRefTarget))
		{
			result = ((xsceneRefTarget == null) ? default(T) : xsceneRefTarget.GetComponent<T>());
			return true;
		}
		result = default(T);
		return false;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x00042465 File Offset: 0x00040665
	public void AddCallbackOnLoad(Action callback)
	{
		this.TargetScene.AddCallbackOnSceneLoad(callback);
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x00042473 File Offset: 0x00040673
	public void RemoveCallbackOnLoad(Action callback)
	{
		this.TargetScene.RemoveCallbackOnSceneLoad(callback);
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00042481 File Offset: 0x00040681
	public void AddCallbackOnUnload(Action callback)
	{
		this.TargetScene.AddCallbackOnSceneUnload(callback);
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0004248F File Offset: 0x0004068F
	public void RemoveCallbackOnUnload(Action callback)
	{
		this.TargetScene.RemoveCallbackOnSceneUnload(callback);
	}

	// Token: 0x04000FDE RID: 4062
	public SceneIndex TargetScene;

	// Token: 0x04000FDF RID: 4063
	public int TargetID;

	// Token: 0x04000FE0 RID: 4064
	private XSceneRefTarget cached;

	// Token: 0x04000FE1 RID: 4065
	private bool didCache;
}
