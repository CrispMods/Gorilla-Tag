using System;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class SceneSettings : MonoBehaviour
{
	// Token: 0x0600145E RID: 5214 RVA: 0x0003DB31 File Offset: 0x0003BD31
	private void Awake()
	{
		Time.fixedDeltaTime = this.m_fixedTimeStep;
		Physics.gravity = Vector3.down * 9.81f * this.m_gravityScalar;
		Physics.defaultContactOffset = this.m_defaultContactOffset;
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x0003DB68 File Offset: 0x0003BD68
	private void Start()
	{
		SceneSettings.CollidersSetContactOffset(this.m_defaultContactOffset);
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000BBB38 File Offset: 0x000B9D38
	private static void CollidersSetContactOffset(float contactOffset)
	{
		Collider[] array = UnityEngine.Object.FindObjectsOfType<Collider>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].contactOffset = contactOffset;
		}
	}

	// Token: 0x04001678 RID: 5752
	[Header("Time")]
	[SerializeField]
	private float m_fixedTimeStep = 0.01f;

	// Token: 0x04001679 RID: 5753
	[Header("Physics")]
	[SerializeField]
	private float m_gravityScalar = 0.75f;

	// Token: 0x0400167A RID: 5754
	[SerializeField]
	private float m_defaultContactOffset = 0.001f;
}
