using System;
using GorillaLocomotion;
using Liv.Lck.GorillaTag;
using UnityEngine;

// Token: 0x0200024D RID: 589
public class LckTabletSizeManager : MonoBehaviour
{
	// Token: 0x06000D9D RID: 3485 RVA: 0x000A22AC File Offset: 0x000A04AC
	private void Update()
	{
		if (!GTPlayer.Instance.IsDefaultScale && this.lckDirectGrabbable.isGrabbed)
		{
			if (this.tabletFollower.GetPlayerSizeModifier() != this._shrinkSize)
			{
				this.tabletFollower.SetPlayerSizeModifier(false, this._shrinkSize);
			}
		}
		else if (GTPlayer.Instance.IsDefaultScale && this.lckDirectGrabbable.isGrabbed && this.tabletFollower.GetPlayerSizeModifier() != 1f)
		{
			this.tabletFollower.SetPlayerSizeModifier(true, 1f);
		}
		if (!GTPlayer.Instance.IsDefaultScale && !this.lckDirectGrabbable.isGrabbed)
		{
			if (base.transform.localScale != this._shrinkVector)
			{
				this.tabletFollower.SetPlayerSizeModifier(false, this._shrinkSize);
				base.transform.localScale = this._shrinkVector;
				GameObject gameObject = Camera.main.transform.Find("LCKBodyCameraSpawner(Clone)").gameObject;
				if (gameObject != null)
				{
					gameObject.GetComponent<LckBodyCameraSpawner>().ManuallySetCameraOnNeck();
					return;
				}
			}
		}
		else if (GTPlayer.Instance.IsDefaultScale && !this.lckDirectGrabbable.isGrabbed && base.transform.localScale != Vector3.one)
		{
			this.tabletFollower.SetPlayerSizeModifier(true, 1f);
			base.transform.localScale = Vector3.one;
			GameObject gameObject2 = Camera.main.transform.Find("LCKBodyCameraSpawner(Clone)").gameObject;
			if (gameObject2 != null)
			{
				gameObject2.GetComponent<LckBodyCameraSpawner>().ManuallySetCameraOnNeck();
			}
		}
	}

	// Token: 0x040010CF RID: 4303
	[SerializeField]
	private LckDirectGrabbable lckDirectGrabbable;

	// Token: 0x040010D0 RID: 4304
	[SerializeField]
	private GtTabletFollower tabletFollower;

	// Token: 0x040010D1 RID: 4305
	private float _shrinkSize = 0.06f;

	// Token: 0x040010D2 RID: 4306
	private Vector3 _shrinkVector = new Vector3(0.06f, 0.06f, 0.06f);
}
