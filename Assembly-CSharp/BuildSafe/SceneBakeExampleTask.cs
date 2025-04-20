using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildSafe
{
	// Token: 0x02000A5A RID: 2650
	public class SceneBakeExampleTask : SceneBakeTask
	{
		// Token: 0x06004263 RID: 16995 RVA: 0x0017515C File Offset: 0x0017335C
		public override void OnSceneBake(Scene scene, SceneBakeMode mode)
		{
			for (int i = 0; i < 10; i++)
			{
				SceneBakeExampleTask.DuplicateAndRecolor(base.gameObject);
			}
			if (mode != SceneBakeMode.OnBuildPlayer)
			{
			}
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0017518C File Offset: 0x0017338C
		private static void DuplicateAndRecolor(GameObject target)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target);
			gameObject.transform.position = UnityEngine.Random.insideUnitSphere * 4f;
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			component.material = new Material(component.sharedMaterial)
			{
				color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f)
			};
		}
	}
}
