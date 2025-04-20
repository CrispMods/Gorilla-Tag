using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020004A0 RID: 1184
public class GamePlayerLocal : MonoBehaviour
{
	// Token: 0x06001CB7 RID: 7351 RVA: 0x000DDA64 File Offset: 0x000DBC64
	private void Awake()
	{
		GamePlayerLocal.instance = this;
		this.hands = new GamePlayerLocal.HandData[2];
		this.inputData = new GamePlayerLocal.InputData[2];
		for (int i = 0; i < this.inputData.Length; i++)
		{
			this.inputData[i] = new GamePlayerLocal.InputData(32);
		}
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x00043BD4 File Offset: 0x00041DD4
	private void OnApplicationQuit()
	{
		MonkeBallGame.Instance.OnPlayerDestroy();
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x00043BE0 File Offset: 0x00041DE0
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			MonkeBallGame.Instance.OnPlayerDestroy();
		}
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x00043BD4 File Offset: 0x00041DD4
	private void OnDestroy()
	{
		MonkeBallGame.Instance.OnPlayerDestroy();
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x000DDAB4 File Offset: 0x000DBCB4
	public void OnUpdateInteract()
	{
		for (int i = 0; i < this.inputData.Length; i++)
		{
			this.UpdateInput(i);
		}
		for (int j = 0; j < this.hands.Length; j++)
		{
			this.UpdateHand(j);
		}
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000DDAF8 File Offset: 0x000DBCF8
	private void UpdateInput(int handIndex)
	{
		XRNode xrnode = this.GetXRNode(handIndex);
		GamePlayerLocal.InputDataMotion data = default(GamePlayerLocal.InputDataMotion);
		InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(xrnode);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.devicePosition, out data.position);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceRotation, out data.rotation);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceVelocity, out data.velocity);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out data.angVelocity);
		data.time = Time.timeAsDouble;
		this.inputData[handIndex].AddInput(data);
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000DDB84 File Offset: 0x000DBD84
	private void UpdateHand(int handIndex)
	{
		if (GameBallManager.Instance == null)
		{
			return;
		}
		if (!this.gamePlayer.GetGameBallId(handIndex).IsValid())
		{
			this.UpdateHandEmpty(handIndex);
			return;
		}
		this.UpdateHandHolding(handIndex);
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x000DDBC8 File Offset: 0x000DBDC8
	public void SetGrabbed(GameBallId gameBallId, int handIndex)
	{
		GamePlayerLocal.HandData handData = this.hands[handIndex];
		handData.gripPressedTime = 0.0;
		this.hands[handIndex] = handData;
		this.UpdateStuckState();
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x00043BEF File Offset: 0x00041DEF
	public void ClearGrabbed(int handIndex)
	{
		this.SetGrabbed(GameBallId.Invalid, handIndex);
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x000DDC08 File Offset: 0x000DBE08
	public void ClearAllGrabbed()
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			this.ClearGrabbed(i);
		}
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x000DDC30 File Offset: 0x000DBE30
	private void UpdateStuckState()
	{
		bool disableMovement = false;
		for (int i = 0; i < this.hands.Length; i++)
		{
			if (this.gamePlayer.GetGameBallId(i).IsValid())
			{
				disableMovement = true;
				break;
			}
		}
		GTPlayer.Instance.disableMovement = disableMovement;
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x000DDC78 File Offset: 0x000DBE78
	private void UpdateHandEmpty(int handIndex)
	{
		GamePlayerLocal.HandData handData = this.hands[handIndex];
		bool flag = ControllerInputPoller.GripFloat(this.GetXRNode(handIndex)) > 0.7f;
		double timeAsDouble = Time.timeAsDouble;
		if (flag && !handData.gripWasHeld)
		{
			handData.gripPressedTime = timeAsDouble;
		}
		double num = timeAsDouble - handData.gripPressedTime;
		handData.gripWasHeld = flag;
		this.hands[handIndex] = handData;
		if (flag && num < 0.15000000596046448)
		{
			Vector3 position = this.GetHandTransform(handIndex).position;
			GameBallId gameBallId = GameBallManager.Instance.TryGrabLocal(position, this.gamePlayer.teamId);
			float num2 = 0.15f;
			if (gameBallId.IsValid())
			{
				bool flag2 = GamePlayerLocal.IsLeftHand(handIndex);
				BodyDockPositions myBodyDockPositions = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions;
				object obj = flag2 ? myBodyDockPositions.leftHandTransform : myBodyDockPositions.rightHandTransform;
				GameBall gameBall = GameBallManager.Instance.GetGameBall(gameBallId);
				Vector3 position2 = gameBall.transform.position;
				Vector3 vector = gameBall.transform.position - position;
				if (vector.sqrMagnitude > num2 * num2)
				{
					position2 = position + vector.normalized * num2;
				}
				object obj2 = obj;
				Vector3 localPosition = obj2.InverseTransformPoint(position2);
				Quaternion localRotation = Quaternion.Inverse(obj2.rotation) * gameBall.transform.rotation;
				obj2.InverseTransformPoint(gameBall.transform.position);
				GameBallManager.Instance.RequestGrabBall(gameBallId, flag2, localPosition, localRotation);
			}
		}
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x000DDE04 File Offset: 0x000DC004
	private void UpdateHandHolding(int handIndex)
	{
		XRNode xrnode = this.GetXRNode(handIndex);
		if (ControllerInputPoller.GripFloat(xrnode) <= 0.7f)
		{
			InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(xrnode);
			Vector3 vector;
			deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out vector);
			Quaternion rotation;
			deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceRotation, out rotation);
			Transform transform = GorillaTagger.Instance.offlineVRRig.transform;
			Quaternion rotation2 = GTPlayer.Instance.turnParent.transform.rotation;
			GamePlayerLocal.InputData inputData = this.inputData[handIndex];
			Vector3 vector2 = inputData.GetMaxSpeed(0f, 0.05f) * inputData.GetAvgVel(0f, 0.05f).normalized;
			vector2 = rotation2 * vector2;
			vector2 *= transform.localScale.x;
			vector = rotation2 * -(Quaternion.Inverse(rotation) * vector);
			GameBallId gameBallId = this.gamePlayer.GetGameBallId(handIndex);
			GameBall gameBall = GameBallManager.Instance.GetGameBall(gameBallId);
			if (gameBall == null)
			{
				return;
			}
			if (gameBall.IsLaunched)
			{
				return;
			}
			if (gameBall.disc)
			{
				Vector3 vector3 = gameBall.transform.rotation * gameBall.localDiscUp;
				vector3.Normalize();
				float d = Vector3.Dot(vector3, vector);
				vector = vector3 * d;
				vector *= 1.25f;
				vector2 *= 1.25f;
			}
			else
			{
				vector2 *= 1.5f;
			}
			GorillaVelocityTracker bodyVelocityTracker = GTPlayer.Instance.bodyVelocityTracker;
			vector2 += bodyVelocityTracker.GetAverageVelocity(true, 0.05f, false);
			GameBallManager.Instance.RequestThrowBall(gameBallId, GamePlayerLocal.IsLeftHand(handIndex), vector2, vector);
		}
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x00043BFD File Offset: 0x00041DFD
	private XRNode GetXRNode(int handIndex)
	{
		if (handIndex != 0)
		{
			return XRNode.RightHand;
		}
		return XRNode.LeftHand;
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x000DDFBC File Offset: 0x000DC1BC
	private Transform GetHandTransform(int handIndex)
	{
		BodyDockPositions myBodyDockPositions = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions;
		return ((handIndex == 0) ? myBodyDockPositions.leftHandTransform : myBodyDockPositions.rightHandTransform).parent;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x00043BC6 File Offset: 0x00041DC6
	public static bool IsLeftHand(int handIndex)
	{
		return handIndex == 0;
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x00043BCC File Offset: 0x00041DCC
	public static int GetHandIndex(bool leftHand)
	{
		if (!leftHand)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x00043C05 File Offset: 0x00041E05
	public void PlayCatchFx(bool isLeftHand)
	{
		GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength, 0.1f);
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x00043C21 File Offset: 0x00041E21
	public void PlayThrowFx(bool isLeftHand)
	{
		GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength * 0.15f, 0.1f);
	}

	// Token: 0x04001FAD RID: 8109
	public GamePlayer gamePlayer;

	// Token: 0x04001FAE RID: 8110
	private const int MAX_INPUT_HISTORY = 32;

	// Token: 0x04001FAF RID: 8111
	private GamePlayerLocal.HandData[] hands;

	// Token: 0x04001FB0 RID: 8112
	private GamePlayerLocal.InputData[] inputData;

	// Token: 0x04001FB1 RID: 8113
	[OnEnterPlay_SetNull]
	public static volatile GamePlayerLocal instance;

	// Token: 0x020004A1 RID: 1185
	private enum HandGrabState
	{
		// Token: 0x04001FB3 RID: 8115
		Empty,
		// Token: 0x04001FB4 RID: 8116
		Holding
	}

	// Token: 0x020004A2 RID: 1186
	private struct HandData
	{
		// Token: 0x04001FB5 RID: 8117
		public GamePlayerLocal.HandGrabState grabState;

		// Token: 0x04001FB6 RID: 8118
		public bool gripWasHeld;

		// Token: 0x04001FB7 RID: 8119
		public double gripPressedTime;

		// Token: 0x04001FB8 RID: 8120
		public GameBallId grabbedGameBallId;
	}

	// Token: 0x020004A3 RID: 1187
	public struct InputDataMotion
	{
		// Token: 0x04001FB9 RID: 8121
		public double time;

		// Token: 0x04001FBA RID: 8122
		public Vector3 position;

		// Token: 0x04001FBB RID: 8123
		public Quaternion rotation;

		// Token: 0x04001FBC RID: 8124
		public Vector3 velocity;

		// Token: 0x04001FBD RID: 8125
		public Vector3 angVelocity;
	}

	// Token: 0x020004A4 RID: 1188
	public class InputData
	{
		// Token: 0x06001CCB RID: 7371 RVA: 0x00043C43 File Offset: 0x00041E43
		public InputData(int maxInputs)
		{
			this.maxInputs = maxInputs;
			this.inputMotionHistory = new List<GamePlayerLocal.InputDataMotion>(maxInputs);
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x00043C5E File Offset: 0x00041E5E
		public void AddInput(GamePlayerLocal.InputDataMotion data)
		{
			if (this.inputMotionHistory.Count >= this.maxInputs)
			{
				this.inputMotionHistory.RemoveAt(0);
			}
			this.inputMotionHistory.Add(data);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000DDFF0 File Offset: 0x000DC1F0
		public float GetMaxSpeed(float ignoreRecent, float window)
		{
			double timeAsDouble = Time.timeAsDouble;
			double num = timeAsDouble - (double)ignoreRecent - (double)window;
			double num2 = timeAsDouble - (double)ignoreRecent;
			float num3 = 0f;
			for (int i = this.inputMotionHistory.Count - 1; i >= 0; i--)
			{
				GamePlayerLocal.InputDataMotion inputDataMotion = this.inputMotionHistory[i];
				if (inputDataMotion.time <= num2)
				{
					if (inputDataMotion.time < num)
					{
						break;
					}
					float sqrMagnitude = inputDataMotion.velocity.sqrMagnitude;
					if (sqrMagnitude > num3)
					{
						num3 = sqrMagnitude;
					}
				}
			}
			return Mathf.Sqrt(num3);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000DE06C File Offset: 0x000DC26C
		public Vector3 GetAvgVel(float ignoreRecent, float window)
		{
			double timeAsDouble = Time.timeAsDouble;
			double num = timeAsDouble - (double)ignoreRecent - (double)window;
			double num2 = timeAsDouble - (double)ignoreRecent;
			Vector3 a = Vector3.zero;
			int num3 = 0;
			for (int i = this.inputMotionHistory.Count - 1; i >= 0; i--)
			{
				GamePlayerLocal.InputDataMotion inputDataMotion = this.inputMotionHistory[i];
				if (inputDataMotion.time <= num2)
				{
					if (inputDataMotion.time < num)
					{
						break;
					}
					a += inputDataMotion.velocity;
					num3++;
				}
			}
			if (num3 == 0)
			{
				return Vector3.zero;
			}
			return a / (float)num3;
		}

		// Token: 0x04001FBE RID: 8126
		public int maxInputs;

		// Token: 0x04001FBF RID: 8127
		public List<GamePlayerLocal.InputDataMotion> inputMotionHistory;
	}
}
