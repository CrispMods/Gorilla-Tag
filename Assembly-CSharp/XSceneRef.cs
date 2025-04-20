using System;
using UnityEngine;

// Token: 0x02000225 RID: 549
[Serializable]
public struct XSceneRef
{
	// Token: 0x06000CC0 RID: 3264 RVA: 0x000A064C File Offset: 0x0009E84C
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

	// Token: 0x06000CC1 RID: 3265 RVA: 0x000A06B4 File Offset: 0x0009E8B4
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

	// Token: 0x06000CC2 RID: 3266 RVA: 0x000A06E8 File Offset: 0x0009E8E8
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

	// Token: 0x06000CC3 RID: 3267 RVA: 0x00038EB4 File Offset: 0x000370B4
	public void AddCallbackOnLoad(Action callback)
	{
		this.TargetScene.AddCallbackOnSceneLoad(callback);
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x00038EC2 File Offset: 0x000370C2
	public void RemoveCallbackOnLoad(Action callback)
	{
		this.TargetScene.RemoveCallbackOnSceneLoad(callback);
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x00038ED0 File Offset: 0x000370D0
	public void AddCallbackOnUnload(Action callback)
	{
		this.TargetScene.AddCallbackOnSceneUnload(callback);
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x00038EDE File Offset: 0x000370DE
	public void RemoveCallbackOnUnload(Action callback)
	{
		this.TargetScene.RemoveCallbackOnSceneUnload(callback);
	}

	// Token: 0x04001024 RID: 4132
	public SceneIndex TargetScene;

	// Token: 0x04001025 RID: 4133
	public int TargetID;

	// Token: 0x04001026 RID: 4134
	private XSceneRefTarget cached;

	// Token: 0x04001027 RID: 4135
	private bool didCache;
}
