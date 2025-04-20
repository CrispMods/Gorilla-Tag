using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Cysharp.Text;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace GorillaExtensions
{
	// Token: 0x02000B9C RID: 2972
	public static class GTExt
	{
		// Token: 0x06004A75 RID: 19061 RVA: 0x0019ED5C File Offset: 0x0019CF5C
		public static T GetComponentInHierarchy<T>(this Scene scene, bool includeInactive = true) where T : Component
		{
			if (!scene.IsValid())
			{
				return default(T);
			}
			foreach (GameObject gameObject in scene.GetRootGameObjects())
			{
				T component = gameObject.GetComponent<T>();
				if (component != null)
				{
					return component;
				}
				Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(includeInactive);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					component = componentsInChildren[j].GetComponent<T>();
					if (component != null)
					{
						return component;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x0019EDF4 File Offset: 0x0019CFF4
		public static List<T> GetComponentsInHierarchy<T>(this Scene scene, bool includeInactive = true, int capacity = 64)
		{
			List<T> list = new List<T>(capacity);
			if (!scene.IsValid())
			{
				return list;
			}
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				T[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren<T>(includeInactive);
				list.AddRange(componentsInChildren);
			}
			return list;
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0019EE3C File Offset: 0x0019D03C
		public static List<UnityEngine.Object> GetComponentsInHierarchy(this Scene scene, Type type, bool includeInactive = true, int capacity = 64)
		{
			List<UnityEngine.Object> list = new List<UnityEngine.Object>(capacity);
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				Component[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren(type, includeInactive);
				list.AddRange(componentsInChildren);
			}
			return list;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x000607AD File Offset: 0x0005E9AD
		public static List<GameObject> GetGameObjectsInHierarchy(this Scene scene, bool includeInactive = true, int capacity = 64)
		{
			return scene.GetComponentsInHierarchy(includeInactive, capacity);
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0019EE7C File Offset: 0x0019D07C
		public static List<T> GetComponentsInHierarchyUntil<T, TStop1>(this Scene scene, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component
		{
			List<T> list = new List<T>(capacity);
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				List<T> componentsInChildrenUntil = rootGameObjects[i].transform.GetComponentsInChildrenUntil(includeInactive, stopAtRoot, capacity);
				list.AddRange(componentsInChildrenUntil);
			}
			return list;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0019EEC0 File Offset: 0x0019D0C0
		public static List<T> GetComponentsInHierarchyUntil<T, TStop1, TStop2>(this Scene scene, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component where TStop2 : Component
		{
			List<T> list = new List<T>(capacity);
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				List<T> componentsInChildrenUntil = rootGameObjects[i].transform.GetComponentsInChildrenUntil(includeInactive, stopAtRoot, capacity);
				list.AddRange(componentsInChildrenUntil);
			}
			return list;
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x0019EF04 File Offset: 0x0019D104
		public static List<T> GetComponentsInHierarchyUntil<T, TStop1, TStop2, TStop3>(this Scene scene, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component where TStop2 : Component where TStop3 : Component
		{
			List<T> list = new List<T>(capacity);
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				List<T> componentsInChildrenUntil = rootGameObjects[i].transform.GetComponentsInChildrenUntil(includeInactive, stopAtRoot, capacity);
				list.AddRange(componentsInChildrenUntil);
			}
			return list;
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x0019EF48 File Offset: 0x0019D148
		public static List<T> GetComponentsInChildrenUntil<T, TStop1>(this Component root, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component
		{
			GTExt.<>c__DisplayClass7_0<T, TStop1> CS$<>8__locals1;
			CS$<>8__locals1.includeInactive = includeInactive;
			List<T> list = new List<T>(capacity);
			if (stopAtRoot && root.GetComponent<TStop1>() != null)
			{
				return list;
			}
			T component = root.GetComponent<T>();
			if (component != null)
			{
				list.Add(component);
			}
			GTExt.<GetComponentsInChildrenUntil>g__GetRecursive|7_0<T, TStop1>(root.transform, ref list, ref CS$<>8__locals1);
			return list;
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x0019EFA8 File Offset: 0x0019D1A8
		public static List<T> GetComponentsInChildrenUntil<T, TStop1, TStop2>(this Component root, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component where TStop2 : Component
		{
			GTExt.<>c__DisplayClass8_0<T, TStop1, TStop2> CS$<>8__locals1;
			CS$<>8__locals1.includeInactive = includeInactive;
			List<T> list = new List<T>(capacity);
			if (stopAtRoot && (root.GetComponent<TStop1>() != null || root.GetComponent<TStop2>() != null))
			{
				return list;
			}
			T component = root.GetComponent<T>();
			if (component != null)
			{
				list.Add(component);
			}
			GTExt.<GetComponentsInChildrenUntil>g__GetRecursive|8_0<T, TStop1, TStop2>(root.transform, ref list, ref CS$<>8__locals1);
			return list;
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x0019F01C File Offset: 0x0019D21C
		public static List<T> GetComponentsInChildrenUntil<T, TStop1, TStop2, TStop3>(this Component root, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component where TStop2 : Component where TStop3 : Component
		{
			GTExt.<>c__DisplayClass9_0<T, TStop1, TStop2, TStop3> CS$<>8__locals1;
			CS$<>8__locals1.includeInactive = includeInactive;
			List<T> list = new List<T>(capacity);
			if (stopAtRoot && (root.GetComponent<TStop1>() != null || root.GetComponent<TStop2>() != null || root.GetComponent<TStop3>() != null))
			{
				return list;
			}
			T component = root.GetComponent<T>();
			if (component != null)
			{
				list.Add(component);
			}
			GTExt.<GetComponentsInChildrenUntil>g__GetRecursive|9_0<T, TStop1, TStop2, TStop3>(root.transform, ref list, ref CS$<>8__locals1);
			return list;
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x000607B7 File Offset: 0x0005E9B7
		public static void GetComponentsInChildrenUntil<T, TStop1, TStop2, TStop3>(this Component root, out List<T> out_included, out HashSet<T> out_excluded, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component where TStop2 : Component where TStop3 : Component
		{
			out_included = root.GetComponentsInChildrenUntil(includeInactive, stopAtRoot, capacity);
			out_excluded = new HashSet<T>(root.GetComponentsInChildren<T>(includeInactive));
			out_excluded.ExceptWith(new HashSet<T>(out_included));
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x0019F0A4 File Offset: 0x0019D2A4
		private static void _GetComponentsInChildrenUntil_OutExclusions_GetRecursive<T, TStop1, TStop2, TStop3>(Transform currentTransform, List<T> included, List<Component> excluded, bool includeInactive) where T : Component where TStop1 : Component where TStop2 : Component where TStop3 : Component
		{
			foreach (object obj in currentTransform)
			{
				Transform transform = (Transform)obj;
				if (includeInactive || transform.gameObject.activeSelf)
				{
					Component item;
					if (GTExt._HasAnyComponents<TStop1, TStop2, TStop3>(transform, out item))
					{
						excluded.Add(item);
					}
					else
					{
						T component = transform.GetComponent<T>();
						if (component != null)
						{
							included.Add(component);
						}
						GTExt._GetComponentsInChildrenUntil_OutExclusions_GetRecursive<T, TStop1, TStop2, TStop3>(transform, included, excluded, includeInactive);
					}
				}
			}
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x0019F13C File Offset: 0x0019D33C
		private static bool _HasAnyComponents<TStop1, TStop2, TStop3>(Component component, out Component stopComponent) where TStop1 : Component where TStop2 : Component where TStop3 : Component
		{
			stopComponent = component.GetComponent<TStop1>();
			if (stopComponent != null)
			{
				return true;
			}
			stopComponent = component.GetComponent<TStop2>();
			if (stopComponent != null)
			{
				return true;
			}
			stopComponent = component.GetComponent<TStop3>();
			return stopComponent != null;
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x0019F198 File Offset: 0x0019D398
		public static T GetComponentWithRegex<T>(this Component root, string regexString) where T : Component
		{
			T[] componentsInChildren = root.GetComponentsInChildren<T>();
			Regex regex = new Regex(regexString);
			foreach (T t in componentsInChildren)
			{
				if (regex.IsMatch(t.name))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x0019F1E8 File Offset: 0x0019D3E8
		private static List<T> GetComponentsWithRegex_Internal<T>(IEnumerable<T> allComponents, string regexString, bool includeInactive, int capacity = 64) where T : Component
		{
			List<T> result = new List<T>(capacity);
			Regex regex = new Regex(regexString);
			GTExt.GetComponentsWithRegex_Internal<T>(allComponents, regex, ref result);
			return result;
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x0019F210 File Offset: 0x0019D410
		private static void GetComponentsWithRegex_Internal<T>(IEnumerable<T> allComponents, Regex regex, ref List<T> foundComponents) where T : Component
		{
			foreach (T t in allComponents)
			{
				string name = t.name;
				if (regex.IsMatch(name))
				{
					foundComponents.Add(t);
				}
			}
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x000607E2 File Offset: 0x0005E9E2
		public static List<T> GetComponentsWithRegex<T>(this Scene scene, string regexString, bool includeInactive, int capacity) where T : Component
		{
			return GTExt.GetComponentsWithRegex_Internal<T>(scene.GetComponentsInHierarchy(includeInactive, capacity), regexString, includeInactive, capacity);
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x000607F4 File Offset: 0x0005E9F4
		public static List<T> GetComponentsWithRegex<T>(this Component root, string regexString, bool includeInactive, int capacity) where T : Component
		{
			return GTExt.GetComponentsWithRegex_Internal<T>(root.GetComponentsInChildren<T>(includeInactive), regexString, includeInactive, capacity);
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x0019F270 File Offset: 0x0019D470
		public static List<GameObject> GetGameObjectsWithRegex(this Scene scene, string regexString, bool includeInactive = true, int capacity = 64)
		{
			List<Transform> componentsWithRegex = scene.GetComponentsWithRegex(regexString, includeInactive, capacity);
			List<GameObject> list = new List<GameObject>(componentsWithRegex.Count);
			foreach (Transform transform in componentsWithRegex)
			{
				list.Add(transform.gameObject);
			}
			return list;
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x0019F2D8 File Offset: 0x0019D4D8
		public static void GetComponentsWithRegex_Internal<T>(this List<T> allComponents, Regex[] regexes, int maxCount, ref List<T> foundComponents) where T : Component
		{
			if (maxCount == 0)
			{
				return;
			}
			int num = 0;
			foreach (T t in allComponents)
			{
				for (int i = 0; i < regexes.Length; i++)
				{
					if (regexes[i].IsMatch(t.name))
					{
						foundComponents.Add(t);
						num++;
						if (maxCount > 0 && num >= maxCount)
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x0019F368 File Offset: 0x0019D568
		public static List<T> GetComponentsWithRegex<T>(this Scene scene, string[] regexStrings, bool includeInactive = true, int maxCount = -1, int capacity = 64) where T : Component
		{
			List<T> componentsInHierarchy = scene.GetComponentsInHierarchy(includeInactive, capacity);
			List<T> result = new List<T>(componentsInHierarchy.Count);
			Regex[] array = new Regex[regexStrings.Length];
			for (int i = 0; i < regexStrings.Length; i++)
			{
				array[i] = new Regex(regexStrings[i]);
			}
			componentsInHierarchy.GetComponentsWithRegex_Internal(array, maxCount, ref result);
			return result;
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0019F3B8 File Offset: 0x0019D5B8
		public static List<T> GetComponentsWithRegex<T>(this Scene scene, string[] regexStrings, string[] excludeRegexStrings, bool includeInactive = true, int maxCount = -1) where T : Component
		{
			List<T> componentsInHierarchy = scene.GetComponentsInHierarchy(includeInactive, 64);
			List<T> list = new List<T>(componentsInHierarchy.Count);
			if (maxCount == 0)
			{
				return list;
			}
			int num = 0;
			foreach (T t in componentsInHierarchy)
			{
				bool flag = false;
				foreach (string pattern in regexStrings)
				{
					if (!flag && Regex.IsMatch(t.name, pattern))
					{
						foreach (string pattern2 in excludeRegexStrings)
						{
							if (!flag)
							{
								flag = Regex.IsMatch(t.name, pattern2);
							}
						}
						if (!flag)
						{
							list.Add(t);
							num++;
							if (maxCount > 0 && num >= maxCount)
							{
								return list;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x0019F4BC File Offset: 0x0019D6BC
		public static List<GameObject> GetGameObjectsWithRegex(this Scene scene, string[] regexStrings, bool includeInactive = true, int maxCount = -1)
		{
			List<Transform> componentsWithRegex = scene.GetComponentsWithRegex(regexStrings, includeInactive, maxCount, 64);
			List<GameObject> list = new List<GameObject>(componentsWithRegex.Count);
			foreach (Transform transform in componentsWithRegex)
			{
				list.Add(transform.gameObject);
			}
			return list;
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x0019F528 File Offset: 0x0019D728
		public static List<GameObject> GetGameObjectsWithRegex(this Scene scene, string[] regexStrings, string[] excludeRegexStrings, bool includeInactive = true, int maxCount = -1)
		{
			List<Transform> componentsWithRegex = scene.GetComponentsWithRegex(regexStrings, excludeRegexStrings, includeInactive, maxCount);
			List<GameObject> list = new List<GameObject>(componentsWithRegex.Count);
			foreach (Transform transform in componentsWithRegex)
			{
				list.Add(transform.gameObject);
			}
			return list;
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x0019F594 File Offset: 0x0019D794
		public static List<T> GetComponentsByName<T>(this Transform xform, string name, bool includeInactive = true) where T : Component
		{
			T[] componentsInChildren = xform.GetComponentsInChildren<T>(includeInactive);
			List<T> list = new List<T>(componentsInChildren.Length);
			foreach (T t in componentsInChildren)
			{
				if (t.name == name)
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x0019F5E4 File Offset: 0x0019D7E4
		public static T GetComponentByName<T>(this Transform xform, string name, bool includeInactive = true) where T : Component
		{
			foreach (T t in xform.GetComponentsInChildren<T>(includeInactive))
			{
				if (t.name == name)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0019F630 File Offset: 0x0019D830
		public static List<GameObject> GetGameObjectsInHierarchy(this Scene scene, string name, bool includeInactive = true)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in scene.GetRootGameObjects())
			{
				if (gameObject.name.Contains(name))
				{
					list.Add(gameObject);
				}
				foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(includeInactive))
				{
					if (transform.name.Contains(name))
					{
						list.Add(transform.gameObject);
					}
				}
			}
			return list;
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x00060805 File Offset: 0x0005EA05
		public static T GetOrAddComponent<T>(this GameObject gameObject, ref T component) where T : Component
		{
			if (component == null)
			{
				component = gameObject.GetOrAddComponent<T>();
			}
			return component;
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x0019F6B4 File Offset: 0x0019D8B4
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T result;
			if (!gameObject.TryGetComponent<T>(out result))
			{
				result = gameObject.AddComponent<T>();
			}
			return result;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x0019F6D4 File Offset: 0x0019D8D4
		public static void SetLossyScale(this Transform transform, Vector3 scale)
		{
			scale = transform.InverseTransformVector(scale);
			Vector3 lossyScale = transform.lossyScale;
			transform.localScale = new Vector3(scale.x / lossyScale.x, scale.y / lossyScale.y, scale.z / lossyScale.z);
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x0006082C File Offset: 0x0005EA2C
		public static Quaternion TransformRotation(this Transform transform, Quaternion localRotation)
		{
			return transform.rotation * localRotation;
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x0006083A File Offset: 0x0005EA3A
		public static Quaternion InverseTransformRotation(this Transform transform, Quaternion localRotation)
		{
			return Quaternion.Inverse(transform.rotation) * localRotation;
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x0006084D File Offset: 0x0005EA4D
		public static Vector3 ProjectOnPlane(this Vector3 point, Vector3 planeAnchorPosition, Vector3 planeNormal)
		{
			return planeAnchorPosition + Vector3.ProjectOnPlane(point - planeAnchorPosition, planeNormal);
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0019F724 File Offset: 0x0019D924
		public static void ForEachBackwards<T>(this List<T> list, Action<T> action)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				T obj = list[i];
				try
				{
					action(obj);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0019F76C File Offset: 0x0019D96C
		public static void AddSortedUnique<T>(this List<T> list, T item)
		{
			int num = list.BinarySearch(item);
			if (num < 0)
			{
				list.Insert(~num, item);
			}
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x0019F724 File Offset: 0x0019D924
		public static void SafeForEachBackwards<T>(this List<T> list, Action<T> action)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				T obj = list[i];
				try
				{
					action(obj);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x0019F790 File Offset: 0x0019D990
		public static bool CompareAs255Unclamped(this Color a, Color b)
		{
			int num = (int)(a.r * 255f);
			int num2 = (int)(a.g * 255f);
			int num3 = (int)(a.b * 255f);
			int num4 = (int)(a.a * 255f);
			int num5 = (int)(b.r * 255f);
			int num6 = (int)(b.g * 255f);
			int num7 = (int)(b.b * 255f);
			int num8 = (int)(b.a * 255f);
			return num == num5 && num2 == num6 && num3 == num7 && num4 == num8;
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x0019F824 File Offset: 0x0019DA24
		public static Quaternion QuaternionFromToVec(Vector3 toVector, Vector3 fromVector)
		{
			Vector3 vector = Vector3.Cross(fromVector, toVector);
			Debug.Log(vector);
			Debug.Log(vector.magnitude);
			Debug.Log(Vector3.Dot(fromVector, toVector) + 1f);
			Quaternion quaternion = new Quaternion(vector.x, vector.y, vector.z, 1f + Vector3.Dot(toVector, fromVector));
			Debug.Log(quaternion);
			Debug.Log(quaternion.eulerAngles);
			Debug.Log(quaternion.normalized);
			return quaternion.normalized;
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x0019F8C8 File Offset: 0x0019DAC8
		public static Vector3 Position(this Matrix4x4 matrix)
		{
			float m = matrix.m03;
			float m2 = matrix.m13;
			float m3 = matrix.m23;
			return new Vector3(m, m2, m3);
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0019F8F0 File Offset: 0x0019DAF0
		public static Vector3 Scale(this Matrix4x4 m)
		{
			Vector3 result = new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
			if (Vector3.Cross(m.GetColumn(0), m.GetColumn(1)).normalized != m.GetColumn(2).normalized)
			{
				result.x *= -1f;
			}
			return result;
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x00030607 File Offset: 0x0002E807
		public static void SetLocalRelativeToParentMatrixWithParityAxis(this Matrix4x4 matrix, GTExt.ParityOptions parity = GTExt.ParityOptions.XFlip)
		{
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00060862 File Offset: 0x0005EA62
		public static void MultiplyInPlaceWith(this Vector3 a, in Vector3 b)
		{
			a.x *= b.x;
			a.y *= b.y;
			a.z *= b.z;
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x0019F988 File Offset: 0x0019DB88
		public static void DecomposeWithXFlip(this Matrix4x4 matrix, out Vector3 transformation, out Quaternion rotation, out Vector3 scale)
		{
			Matrix4x4 matrix2 = matrix;
			bool flag = matrix2.ValidTRS();
			transformation = matrix2.Position();
			Quaternion quaternion;
			if (!flag)
			{
				quaternion = Quaternion.identity;
			}
			else
			{
				int num = 2;
				Vector3 forward = matrix2.GetColumnNoCopy(num);
				int num2 = 1;
				quaternion = Quaternion.LookRotation(forward, matrix2.GetColumnNoCopy(num2));
			}
			rotation = quaternion;
			Vector3 vector;
			if (!flag)
			{
				vector = Vector3.zero;
			}
			else
			{
				Matrix4x4 matrix4x = matrix;
				vector = matrix4x.lossyScale;
			}
			scale = vector;
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x0019FA04 File Offset: 0x0019DC04
		public static void SetLocalMatrixRelativeToParentWithXParity(this Transform transform, in Matrix4x4 matrix4X4)
		{
			Vector3 localPosition;
			Quaternion localRotation;
			Vector3 localScale;
			matrix4X4.DecomposeWithXFlip(out localPosition, out localRotation, out localScale);
			transform.localPosition = localPosition;
			transform.localRotation = localRotation;
			transform.localScale = localScale;
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x0019FA34 File Offset: 0x0019DC34
		public static Matrix4x4 Matrix4x4Scale(in Vector3 vector)
		{
			Matrix4x4 result;
			result.m00 = vector.x;
			result.m01 = 0f;
			result.m02 = 0f;
			result.m03 = 0f;
			result.m10 = 0f;
			result.m11 = vector.y;
			result.m12 = 0f;
			result.m13 = 0f;
			result.m20 = 0f;
			result.m21 = 0f;
			result.m22 = vector.z;
			result.m23 = 0f;
			result.m30 = 0f;
			result.m31 = 0f;
			result.m32 = 0f;
			result.m33 = 1f;
			return result;
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x0019FB08 File Offset: 0x0019DD08
		public static Vector4 GetColumnNoCopy(this Matrix4x4 matrix, in int index)
		{
			switch (index)
			{
			case 0:
				return new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30);
			case 1:
				return new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31);
			case 2:
				return new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32);
			case 3:
				return new Vector4(matrix.m03, matrix.m13, matrix.m23, matrix.m33);
			default:
				throw new IndexOutOfRangeException("Invalid column index!");
			}
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x0019FBB4 File Offset: 0x0019DDB4
		public static Quaternion RotationWithScaleContext(this Matrix4x4 m, in Vector3 scale)
		{
			Matrix4x4 matrix4x = m * GTExt.Matrix4x4Scale(scale);
			int num = 2;
			Vector3 forward = matrix4x.GetColumnNoCopy(num);
			int num2 = 1;
			return Quaternion.LookRotation(forward, matrix4x.GetColumnNoCopy(num2));
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x0019FBF8 File Offset: 0x0019DDF8
		public static Quaternion Rotation(this Matrix4x4 m)
		{
			int num = 2;
			Vector3 forward = m.GetColumnNoCopy(num);
			int num2 = 1;
			return Quaternion.LookRotation(forward, m.GetColumnNoCopy(num2));
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x00060894 File Offset: 0x0005EA94
		public static Vector3 x0y(this Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x000608AC File Offset: 0x0005EAAC
		public static Vector3 x0y(this Vector3 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x000608C4 File Offset: 0x0005EAC4
		public static Vector3 xy0(this Vector2 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x000608DC File Offset: 0x0005EADC
		public static Vector3 xy0(this Vector3 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x000608F4 File Offset: 0x0005EAF4
		public static Vector3 xz0(this Vector3 v)
		{
			return new Vector3(v.x, v.z, 0f);
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x000373C0 File Offset: 0x000355C0
		public static Vector3 x0z(this Vector3 v)
		{
			return new Vector3(v.x, 0f, v.z);
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0006090C File Offset: 0x0005EB0C
		public static Matrix4x4 LocalMatrixRelativeToParentNoScale(this Transform transform)
		{
			return Matrix4x4.TRS(transform.localPosition, transform.localRotation, Vector3.one);
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x00060924 File Offset: 0x0005EB24
		public static Matrix4x4 LocalMatrixRelativeToParentWithScale(this Transform transform)
		{
			if (transform.parent == null)
			{
				return transform.localToWorldMatrix;
			}
			return transform.parent.worldToLocalMatrix * transform.localToWorldMatrix;
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x00060951 File Offset: 0x0005EB51
		public static void SetLocalMatrixRelativeToParent(this Transform transform, Matrix4x4 matrix)
		{
			transform.localPosition = matrix.Position();
			transform.localRotation = matrix.Rotation();
			transform.localScale = matrix.Scale();
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x00060978 File Offset: 0x0005EB78
		public static void SetLocalMatrixRelativeToParentNoScale(this Transform transform, Matrix4x4 matrix)
		{
			transform.localPosition = matrix.Position();
			transform.localRotation = matrix.Rotation();
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x00060993 File Offset: 0x0005EB93
		public static void SetLocalToWorldMatrixNoScale(this Transform transform, Matrix4x4 matrix)
		{
			transform.position = matrix.Position();
			transform.rotation = matrix.Rotation();
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x000609AE File Offset: 0x0005EBAE
		public static Matrix4x4 localToWorldNoScale(this Transform transform)
		{
			return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x000609C6 File Offset: 0x0005EBC6
		public static void SetLocalToWorldMatrixWithScale(this Transform transform, Matrix4x4 matrix)
		{
			transform.position = matrix.Position();
			transform.rotation = matrix.rotation;
			transform.SetLossyScale(matrix.lossyScale);
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x000609EE File Offset: 0x0005EBEE
		public static Matrix4x4 Matrix4X4LerpNoScale(Matrix4x4 a, Matrix4x4 b, float t)
		{
			return Matrix4x4.TRS(Vector3.Lerp(a.Position(), b.Position(), t), Quaternion.Slerp(a.rotation, b.rotation, t), b.lossyScale);
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x00060A22 File Offset: 0x0005EC22
		public static Matrix4x4 LerpTo(this Matrix4x4 a, Matrix4x4 b, float t)
		{
			return GTExt.Matrix4X4LerpNoScale(a, b, t);
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x00060A2C File Offset: 0x0005EC2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(this Vector3 v)
		{
			return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x00060A55 File Offset: 0x0005EC55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNan(this Quaternion q)
		{
			return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x00060A8B File Offset: 0x0005EC8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInfinity(this Vector3 v)
		{
			return float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z);
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x00060AB4 File Offset: 0x0005ECB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInfinity(this Quaternion q)
		{
			return float.IsInfinity(q.x) || float.IsInfinity(q.y) || float.IsInfinity(q.z) || float.IsInfinity(q.w);
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x00060AEA File Offset: 0x0005ECEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ValuesInRange(this Vector3 v, in float maxVal)
		{
			return Mathf.Abs(v.x) < maxVal && Mathf.Abs(v.y) < maxVal && Mathf.Abs(v.z) < maxVal;
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x00060B1B File Offset: 0x0005ED1B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsValid(this Vector3 v, in float maxVal = 10000f)
		{
			return !v.IsNaN() && !v.IsInfinity() && v.ValuesInRange(maxVal);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0019FC28 File Offset: 0x0019DE28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 GetValidWithFallback(this Vector3 v, in Vector3 safeVal)
		{
			float num = 10000f;
			if (!v.IsValid(num))
			{
				return safeVal;
			}
			return v;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0019FC54 File Offset: 0x0019DE54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetValueSafe(this Vector3 v, in Vector3 newVal)
		{
			float num = 10000f;
			if (newVal.IsValid(num))
			{
				v = newVal;
			}
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x00060B36 File Offset: 0x0005ED36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsValid(this Quaternion q)
		{
			return !q.IsNan() && !q.IsInfinity();
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x00060B4B File Offset: 0x0005ED4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Quaternion GetValidWithFallback(this Quaternion q, in Quaternion safeVal)
		{
			if (!q.IsValid())
			{
				return safeVal;
			}
			return q;
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x00060B62 File Offset: 0x0005ED62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetValueSafe(this Quaternion q, in Quaternion newVal)
		{
			if (newVal.IsValid())
			{
				q = newVal;
			}
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0019FC80 File Offset: 0x0019DE80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ClampMagnitudeSafe(this Vector2 v2, float magnitude)
		{
			if (!float.IsFinite(v2.x))
			{
				v2.x = 0f;
			}
			if (!float.IsFinite(v2.y))
			{
				v2.y = 0f;
			}
			if (!float.IsFinite(magnitude))
			{
				magnitude = 0f;
			}
			return Vector2.ClampMagnitude(v2, magnitude);
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x0019FCD8 File Offset: 0x0019DED8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClampThisMagnitudeSafe(this Vector2 v2, float magnitude)
		{
			if (!float.IsFinite(v2.x))
			{
				v2.x = 0f;
			}
			if (!float.IsFinite(v2.y))
			{
				v2.y = 0f;
			}
			if (!float.IsFinite(magnitude))
			{
				magnitude = 0f;
			}
			v2 = Vector2.ClampMagnitude(v2, magnitude);
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x0019FD38 File Offset: 0x0019DF38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ClampMagnitudeSafe(this Vector3 v3, float magnitude)
		{
			if (!float.IsFinite(v3.x))
			{
				v3.x = 0f;
			}
			if (!float.IsFinite(v3.y))
			{
				v3.y = 0f;
			}
			if (!float.IsFinite(v3.z))
			{
				v3.z = 0f;
			}
			if (!float.IsFinite(magnitude))
			{
				magnitude = 0f;
			}
			return Vector3.ClampMagnitude(v3, magnitude);
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x0019FDA8 File Offset: 0x0019DFA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClampThisMagnitudeSafe(this Vector3 v3, float magnitude)
		{
			if (!float.IsFinite(v3.x))
			{
				v3.x = 0f;
			}
			if (!float.IsFinite(v3.y))
			{
				v3.y = 0f;
			}
			if (!float.IsFinite(v3.z))
			{
				v3.z = 0f;
			}
			if (!float.IsFinite(magnitude))
			{
				magnitude = 0f;
			}
			v3 = Vector3.ClampMagnitude(v3, magnitude);
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x00060B78 File Offset: 0x0005ED78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MinSafe(this float value, float min)
		{
			if (!float.IsFinite(value))
			{
				value = 0f;
			}
			if (!float.IsFinite(min))
			{
				min = 0f;
			}
			if (value >= min)
			{
				return min;
			}
			return value;
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x00060B9F File Offset: 0x0005ED9F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThisMinSafe(this float value, float min)
		{
			if (!float.IsFinite(value))
			{
				value = 0f;
			}
			if (!float.IsFinite(min))
			{
				min = 0f;
			}
			value = ((value < min) ? value : min);
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x00060BCC File Offset: 0x0005EDCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double MinSafe(this double value, float min)
		{
			if (!double.IsFinite(value))
			{
				value = 0.0;
			}
			if (!double.IsFinite((double)min))
			{
				min = 0f;
			}
			if (value >= (double)min)
			{
				return (double)min;
			}
			return value;
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x00060BFA File Offset: 0x0005EDFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThisMinSafe(this double value, float min)
		{
			if (!double.IsFinite(value))
			{
				value = 0.0;
			}
			if (!double.IsFinite((double)min))
			{
				min = 0f;
			}
			value = ((value < (double)min) ? value : ((double)min));
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x00060C2E File Offset: 0x0005EE2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MaxSafe(this float value, float max)
		{
			if (!float.IsFinite(value))
			{
				value = 0f;
			}
			if (!float.IsFinite(max))
			{
				max = 0f;
			}
			if (value <= max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x00060C55 File Offset: 0x0005EE55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThisMaxSafe(this float value, float max)
		{
			if (!float.IsFinite(value))
			{
				value = 0f;
			}
			if (!float.IsFinite(max))
			{
				max = 0f;
			}
			value = ((value > max) ? value : max);
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x00060C82 File Offset: 0x0005EE82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double MaxSafe(this double value, float max)
		{
			if (!double.IsFinite(value))
			{
				value = 0.0;
			}
			if (!double.IsFinite((double)max))
			{
				max = 0f;
			}
			if (value <= (double)max)
			{
				return (double)max;
			}
			return value;
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x00060CB0 File Offset: 0x0005EEB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThisMaxSafe(this double value, float max)
		{
			if (!double.IsFinite(value))
			{
				value = 0.0;
			}
			if (!double.IsFinite((double)max))
			{
				max = 0f;
			}
			value = ((value > (double)max) ? value : ((double)max));
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x00060CE4 File Offset: 0x0005EEE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ClampSafe(this float value, float min, float max)
		{
			if (!float.IsFinite(value))
			{
				value = 0f;
			}
			if (!float.IsFinite(min))
			{
				min = 0f;
			}
			if (!float.IsFinite(max))
			{
				max = 0f;
			}
			if (value > max)
			{
				return max;
			}
			if (value >= min)
			{
				return value;
			}
			return min;
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x0019FE20 File Offset: 0x0019E020
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double ClampSafe(this double value, double min, double max)
		{
			if (!double.IsFinite(value))
			{
				value = 0.0;
			}
			if (!double.IsFinite(min))
			{
				min = 0.0;
			}
			if (!double.IsFinite(max))
			{
				max = 0.0;
			}
			if (value > max)
			{
				return max;
			}
			if (value >= min)
			{
				return value;
			}
			return min;
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x00060D20 File Offset: 0x0005EF20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetFinite(this float value)
		{
			if (!float.IsFinite(value))
			{
				return 0f;
			}
			return value;
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x00060D31 File Offset: 0x0005EF31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double GetFinite(this double value)
		{
			if (!double.IsFinite(value))
			{
				return 0.0;
			}
			return value;
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x00060D46 File Offset: 0x0005EF46
		public static Matrix4x4 Matrix4X4LerpHandleNegativeScale(Matrix4x4 a, Matrix4x4 b, float t)
		{
			return Matrix4x4.TRS(Vector3.Lerp(a.Position(), b.Position(), t), Quaternion.Slerp(a.Rotation(), b.Rotation(), t), b.lossyScale);
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00060D7A File Offset: 0x0005EF7A
		public static Matrix4x4 LerpTo_HandleNegativeScale(this Matrix4x4 a, Matrix4x4 b, float t)
		{
			return GTExt.Matrix4X4LerpHandleNegativeScale(a, b, t);
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x0019FE74 File Offset: 0x0019E074
		public static Vector3 LerpToUnclamped(this Vector3 a, in Vector3 b, float t)
		{
			return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x00060D84 File Offset: 0x0005EF84
		public static string ToLongString(this Vector3 self)
		{
			return string.Format("[{0}, {1}, {2}]", self.x, self.y, self.z);
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x00060DB1 File Offset: 0x0005EFB1
		public static int GetRandomIndex<T>(this IReadOnlyList<T> self)
		{
			return UnityEngine.Random.Range(0, self.Count);
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x00060DBF File Offset: 0x0005EFBF
		public static T GetRandomItem<T>(this IReadOnlyList<T> self)
		{
			return self[self.GetRandomIndex<T>()];
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x00060DCD File Offset: 0x0005EFCD
		public static Vector2 xx(this float v)
		{
			return new Vector2(v, v);
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x00060DD6 File Offset: 0x0005EFD6
		public static Vector2 xx(this Vector2 v)
		{
			return new Vector2(v.x, v.x);
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x00060DE9 File Offset: 0x0005EFE9
		public static Vector2 xy(this Vector2 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x00060DFC File Offset: 0x0005EFFC
		public static Vector2 yy(this Vector2 v)
		{
			return new Vector2(v.y, v.y);
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x00060E0F File Offset: 0x0005F00F
		public static Vector2 xx(this Vector3 v)
		{
			return new Vector2(v.x, v.x);
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x00060E22 File Offset: 0x0005F022
		public static Vector2 xy(this Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00060E35 File Offset: 0x0005F035
		public static Vector2 xz(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00060E48 File Offset: 0x0005F048
		public static Vector2 yy(this Vector3 v)
		{
			return new Vector2(v.y, v.y);
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x00060E5B File Offset: 0x0005F05B
		public static Vector2 yz(this Vector3 v)
		{
			return new Vector2(v.y, v.z);
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x00060E6E File Offset: 0x0005F06E
		public static Vector2 zz(this Vector3 v)
		{
			return new Vector2(v.z, v.z);
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x00060E81 File Offset: 0x0005F081
		public static Vector2 xx(this Vector4 v)
		{
			return new Vector2(v.x, v.x);
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x00060E94 File Offset: 0x0005F094
		public static Vector2 xy(this Vector4 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x00060EA7 File Offset: 0x0005F0A7
		public static Vector2 xz(this Vector4 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x00060EBA File Offset: 0x0005F0BA
		public static Vector2 xw(this Vector4 v)
		{
			return new Vector2(v.x, v.w);
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00060ECD File Offset: 0x0005F0CD
		public static Vector2 yy(this Vector4 v)
		{
			return new Vector2(v.y, v.y);
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x00060EE0 File Offset: 0x0005F0E0
		public static Vector2 yz(this Vector4 v)
		{
			return new Vector2(v.y, v.z);
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x00060EF3 File Offset: 0x0005F0F3
		public static Vector2 yw(this Vector4 v)
		{
			return new Vector2(v.y, v.w);
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x00060F06 File Offset: 0x0005F106
		public static Vector2 zz(this Vector4 v)
		{
			return new Vector2(v.z, v.z);
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x00060F19 File Offset: 0x0005F119
		public static Vector2 zw(this Vector4 v)
		{
			return new Vector2(v.z, v.w);
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x00060F2C File Offset: 0x0005F12C
		public static Vector2 ww(this Vector4 v)
		{
			return new Vector2(v.w, v.w);
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x00060F3F File Offset: 0x0005F13F
		public static Vector3 xxx(this float v)
		{
			return new Vector3(v, v, v);
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x00060F49 File Offset: 0x0005F149
		public static Vector3 xxx(this Vector2 v)
		{
			return new Vector3(v.x, v.x, v.x);
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x00060F62 File Offset: 0x0005F162
		public static Vector3 xxy(this Vector2 v)
		{
			return new Vector3(v.x, v.x, v.y);
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x00060F7B File Offset: 0x0005F17B
		public static Vector3 xyy(this Vector2 v)
		{
			return new Vector3(v.x, v.y, v.y);
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x00060F94 File Offset: 0x0005F194
		public static Vector3 yyy(this Vector2 v)
		{
			return new Vector3(v.y, v.y, v.y);
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x00060FAD File Offset: 0x0005F1AD
		public static Vector3 xxx(this Vector3 v)
		{
			return new Vector3(v.x, v.x, v.x);
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x00060FC6 File Offset: 0x0005F1C6
		public static Vector3 xxy(this Vector3 v)
		{
			return new Vector3(v.x, v.x, v.y);
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x00060FDF File Offset: 0x0005F1DF
		public static Vector3 xxz(this Vector3 v)
		{
			return new Vector3(v.x, v.x, v.z);
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x00060FF8 File Offset: 0x0005F1F8
		public static Vector3 xyy(this Vector3 v)
		{
			return new Vector3(v.x, v.y, v.y);
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x00061011 File Offset: 0x0005F211
		public static Vector3 xyz(this Vector3 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0006102A File Offset: 0x0005F22A
		public static Vector3 xzz(this Vector3 v)
		{
			return new Vector3(v.x, v.z, v.z);
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x00061043 File Offset: 0x0005F243
		public static Vector3 yyy(this Vector3 v)
		{
			return new Vector3(v.y, v.y, v.y);
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0006105C File Offset: 0x0005F25C
		public static Vector3 yyz(this Vector3 v)
		{
			return new Vector3(v.y, v.y, v.z);
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x00061075 File Offset: 0x0005F275
		public static Vector3 yzz(this Vector3 v)
		{
			return new Vector3(v.y, v.z, v.z);
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0006108E File Offset: 0x0005F28E
		public static Vector3 zzz(this Vector3 v)
		{
			return new Vector3(v.z, v.z, v.z);
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x000610A7 File Offset: 0x0005F2A7
		public static Vector3 xxx(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.x);
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x000610C0 File Offset: 0x0005F2C0
		public static Vector3 xxy(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.y);
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x000610D9 File Offset: 0x0005F2D9
		public static Vector3 xxz(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.z);
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x000610F2 File Offset: 0x0005F2F2
		public static Vector3 xxw(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.w);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0006110B File Offset: 0x0005F30B
		public static Vector3 xyy(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.y);
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x00061124 File Offset: 0x0005F324
		public static Vector3 xyz(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x0006113D File Offset: 0x0005F33D
		public static Vector3 xyw(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.w);
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x00061156 File Offset: 0x0005F356
		public static Vector3 xzz(this Vector4 v)
		{
			return new Vector3(v.x, v.z, v.z);
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x0006116F File Offset: 0x0005F36F
		public static Vector3 xzw(this Vector4 v)
		{
			return new Vector3(v.x, v.z, v.w);
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x00061188 File Offset: 0x0005F388
		public static Vector3 xww(this Vector4 v)
		{
			return new Vector3(v.x, v.w, v.w);
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x000611A1 File Offset: 0x0005F3A1
		public static Vector3 yyy(this Vector4 v)
		{
			return new Vector3(v.y, v.y, v.y);
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x000611BA File Offset: 0x0005F3BA
		public static Vector3 yyz(this Vector4 v)
		{
			return new Vector3(v.y, v.y, v.z);
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x000611D3 File Offset: 0x0005F3D3
		public static Vector3 yyw(this Vector4 v)
		{
			return new Vector3(v.y, v.y, v.w);
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x000611EC File Offset: 0x0005F3EC
		public static Vector3 yzz(this Vector4 v)
		{
			return new Vector3(v.y, v.z, v.z);
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x00061205 File Offset: 0x0005F405
		public static Vector3 yzw(this Vector4 v)
		{
			return new Vector3(v.y, v.z, v.w);
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x0006121E File Offset: 0x0005F41E
		public static Vector3 yww(this Vector4 v)
		{
			return new Vector3(v.y, v.w, v.w);
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x00061237 File Offset: 0x0005F437
		public static Vector3 zzz(this Vector4 v)
		{
			return new Vector3(v.z, v.z, v.z);
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x00061250 File Offset: 0x0005F450
		public static Vector3 zzw(this Vector4 v)
		{
			return new Vector3(v.z, v.z, v.w);
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x00061269 File Offset: 0x0005F469
		public static Vector3 zww(this Vector4 v)
		{
			return new Vector3(v.z, v.w, v.w);
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x00061282 File Offset: 0x0005F482
		public static Vector3 www(this Vector4 v)
		{
			return new Vector3(v.w, v.w, v.w);
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x0006129B File Offset: 0x0005F49B
		public static Vector4 xxxx(this float v)
		{
			return new Vector4(v, v, v, v);
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x000612A6 File Offset: 0x0005F4A6
		public static Vector4 xxxx(this Vector2 v)
		{
			return new Vector4(v.x, v.x, v.x, v.x);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x000612C5 File Offset: 0x0005F4C5
		public static Vector4 xxxy(this Vector2 v)
		{
			return new Vector4(v.x, v.x, v.x, v.y);
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x000612E4 File Offset: 0x0005F4E4
		public static Vector4 xxyy(this Vector2 v)
		{
			return new Vector4(v.x, v.x, v.y, v.y);
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x00061303 File Offset: 0x0005F503
		public static Vector4 xyyy(this Vector2 v)
		{
			return new Vector4(v.x, v.y, v.y, v.y);
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x00061322 File Offset: 0x0005F522
		public static Vector4 yyyy(this Vector2 v)
		{
			return new Vector4(v.y, v.y, v.y, v.y);
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x00061341 File Offset: 0x0005F541
		public static Vector4 xxxx(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.x, v.x);
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x00061360 File Offset: 0x0005F560
		public static Vector4 xxxy(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.x, v.y);
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x0006137F File Offset: 0x0005F57F
		public static Vector4 xxxz(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.x, v.z);
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x0006139E File Offset: 0x0005F59E
		public static Vector4 xxyy(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.y, v.y);
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x000613BD File Offset: 0x0005F5BD
		public static Vector4 xxyz(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.y, v.z);
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x000613DC File Offset: 0x0005F5DC
		public static Vector4 xxzz(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.z, v.z);
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x000613FB File Offset: 0x0005F5FB
		public static Vector4 xyyy(this Vector3 v)
		{
			return new Vector4(v.x, v.y, v.y, v.y);
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x0006141A File Offset: 0x0005F61A
		public static Vector4 xyyz(this Vector3 v)
		{
			return new Vector4(v.x, v.y, v.y, v.z);
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x00061439 File Offset: 0x0005F639
		public static Vector4 xyzz(this Vector3 v)
		{
			return new Vector4(v.x, v.y, v.z, v.z);
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x00061458 File Offset: 0x0005F658
		public static Vector4 xzzz(this Vector3 v)
		{
			return new Vector4(v.x, v.z, v.z, v.z);
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x00061477 File Offset: 0x0005F677
		public static Vector4 yyyy(this Vector3 v)
		{
			return new Vector4(v.y, v.y, v.y, v.y);
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x00061496 File Offset: 0x0005F696
		public static Vector4 yyyz(this Vector3 v)
		{
			return new Vector4(v.y, v.y, v.y, v.z);
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x000614B5 File Offset: 0x0005F6B5
		public static Vector4 yyzz(this Vector3 v)
		{
			return new Vector4(v.y, v.y, v.z, v.z);
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x000614D4 File Offset: 0x0005F6D4
		public static Vector4 yzzz(this Vector3 v)
		{
			return new Vector4(v.y, v.z, v.z, v.z);
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x000614F3 File Offset: 0x0005F6F3
		public static Vector4 zzzz(this Vector3 v)
		{
			return new Vector4(v.z, v.z, v.z, v.z);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x00061512 File Offset: 0x0005F712
		public static Vector4 xxxx(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.x);
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x00061531 File Offset: 0x0005F731
		public static Vector4 xxxy(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.y);
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x00061550 File Offset: 0x0005F750
		public static Vector4 xxxz(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.z);
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x0006156F File Offset: 0x0005F76F
		public static Vector4 xxxw(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.w);
		}

		// Token: 0x06004B25 RID: 19237 RVA: 0x0006158E File Offset: 0x0005F78E
		public static Vector4 xxyy(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.y, v.y);
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x000615AD File Offset: 0x0005F7AD
		public static Vector4 xxyz(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.y, v.z);
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x000615CC File Offset: 0x0005F7CC
		public static Vector4 xxyw(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.y, v.w);
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x000615EB File Offset: 0x0005F7EB
		public static Vector4 xxzz(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.z, v.z);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x0006160A File Offset: 0x0005F80A
		public static Vector4 xxzw(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.z, v.w);
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x00061629 File Offset: 0x0005F829
		public static Vector4 xxww(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.w, v.w);
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x00061648 File Offset: 0x0005F848
		public static Vector4 xyyy(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.y, v.y);
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x00061667 File Offset: 0x0005F867
		public static Vector4 xyyz(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.y, v.z);
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x00061686 File Offset: 0x0005F886
		public static Vector4 xyyw(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.y, v.w);
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x000616A5 File Offset: 0x0005F8A5
		public static Vector4 xyzz(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.z, v.z);
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x000616C4 File Offset: 0x0005F8C4
		public static Vector4 xyzw(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.z, v.w);
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x000616E3 File Offset: 0x0005F8E3
		public static Vector4 xyww(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.w, v.w);
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x00061702 File Offset: 0x0005F902
		public static Vector4 xzzz(this Vector4 v)
		{
			return new Vector4(v.x, v.z, v.z, v.z);
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x00061721 File Offset: 0x0005F921
		public static Vector4 xzzw(this Vector4 v)
		{
			return new Vector4(v.x, v.z, v.z, v.w);
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x00061740 File Offset: 0x0005F940
		public static Vector4 xzww(this Vector4 v)
		{
			return new Vector4(v.x, v.z, v.w, v.w);
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0006175F File Offset: 0x0005F95F
		public static Vector4 xwww(this Vector4 v)
		{
			return new Vector4(v.x, v.w, v.w, v.w);
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x0006177E File Offset: 0x0005F97E
		public static Vector4 yyyy(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.y, v.y);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x0006179D File Offset: 0x0005F99D
		public static Vector4 yyyz(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.y, v.z);
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x000617BC File Offset: 0x0005F9BC
		public static Vector4 yyyw(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.y, v.w);
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x000617DB File Offset: 0x0005F9DB
		public static Vector4 yyzz(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.z, v.z);
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x000617FA File Offset: 0x0005F9FA
		public static Vector4 yyzw(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.z, v.w);
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x00061819 File Offset: 0x0005FA19
		public static Vector4 yyww(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.w, v.w);
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x00061838 File Offset: 0x0005FA38
		public static Vector4 yzzz(this Vector4 v)
		{
			return new Vector4(v.y, v.z, v.z, v.z);
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x00061857 File Offset: 0x0005FA57
		public static Vector4 yzzw(this Vector4 v)
		{
			return new Vector4(v.y, v.z, v.z, v.w);
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x00061876 File Offset: 0x0005FA76
		public static Vector4 yzww(this Vector4 v)
		{
			return new Vector4(v.y, v.z, v.w, v.w);
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x00061895 File Offset: 0x0005FA95
		public static Vector4 ywww(this Vector4 v)
		{
			return new Vector4(v.y, v.w, v.w, v.w);
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x000618B4 File Offset: 0x0005FAB4
		public static Vector4 zzzz(this Vector4 v)
		{
			return new Vector4(v.z, v.z, v.z, v.z);
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x000618D3 File Offset: 0x0005FAD3
		public static Vector4 zzzw(this Vector4 v)
		{
			return new Vector4(v.z, v.z, v.z, v.w);
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x000618F2 File Offset: 0x0005FAF2
		public static Vector4 zzww(this Vector4 v)
		{
			return new Vector4(v.z, v.z, v.w, v.w);
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x00061911 File Offset: 0x0005FB11
		public static Vector4 zwww(this Vector4 v)
		{
			return new Vector4(v.z, v.w, v.w, v.w);
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x00061930 File Offset: 0x0005FB30
		public static Vector4 wwww(this Vector4 v)
		{
			return new Vector4(v.w, v.w, v.w, v.w);
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x0006194F File Offset: 0x0005FB4F
		public static Vector4 WithX(this Vector4 v, float x)
		{
			return new Vector4(x, v.y, v.z, v.w);
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x00061969 File Offset: 0x0005FB69
		public static Vector4 WithY(this Vector4 v, float y)
		{
			return new Vector4(v.x, y, v.z, v.w);
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x00061983 File Offset: 0x0005FB83
		public static Vector4 WithZ(this Vector4 v, float z)
		{
			return new Vector4(v.x, v.y, z, v.w);
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x0006199D File Offset: 0x0005FB9D
		public static Vector4 WithW(this Vector4 v, float w)
		{
			return new Vector4(v.x, v.y, v.z, w);
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x000619B7 File Offset: 0x0005FBB7
		public static Vector3 WithX(this Vector3 v, float x)
		{
			return new Vector3(x, v.y, v.z);
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x000619CB File Offset: 0x0005FBCB
		public static Vector3 WithY(this Vector3 v, float y)
		{
			return new Vector3(v.x, y, v.z);
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x000619DF File Offset: 0x0005FBDF
		public static Vector3 WithZ(this Vector3 v, float z)
		{
			return new Vector3(v.x, v.y, z);
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x000619F3 File Offset: 0x0005FBF3
		public static Vector4 WithW(this Vector3 v, float w)
		{
			return new Vector4(v.x, v.y, v.z, w);
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x00061A0D File Offset: 0x0005FC0D
		public static Vector2 WithX(this Vector2 v, float x)
		{
			return new Vector2(x, v.y);
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x00061A1B File Offset: 0x0005FC1B
		public static Vector2 WithY(this Vector2 v, float y)
		{
			return new Vector2(v.x, y);
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x00061A29 File Offset: 0x0005FC29
		public static Vector3 WithZ(this Vector2 v, float z)
		{
			return new Vector3(v.x, v.y, z);
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x00061A3D File Offset: 0x0005FC3D
		public static bool IsShorterThan(this Vector2 v, float len)
		{
			return v.sqrMagnitude < len * len;
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x00061A4B File Offset: 0x0005FC4B
		public static bool IsShorterThan(this Vector2 v, Vector2 v2)
		{
			return v.sqrMagnitude < v2.sqrMagnitude;
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x00061A5D File Offset: 0x0005FC5D
		public static bool IsShorterThan(this Vector3 v, float len)
		{
			return v.sqrMagnitude < len * len;
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x00061A6B File Offset: 0x0005FC6B
		public static bool IsShorterThan(this Vector3 v, Vector3 v2)
		{
			return v.sqrMagnitude < v2.sqrMagnitude;
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x00061A7D File Offset: 0x0005FC7D
		public static bool IsLongerThan(this Vector2 v, float len)
		{
			return v.sqrMagnitude > len * len;
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00061A8B File Offset: 0x0005FC8B
		public static bool IsLongerThan(this Vector2 v, Vector2 v2)
		{
			return v.sqrMagnitude > v2.sqrMagnitude;
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x00061A9D File Offset: 0x0005FC9D
		public static bool IsLongerThan(this Vector3 v, float len)
		{
			return v.sqrMagnitude > len * len;
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x00061AAB File Offset: 0x0005FCAB
		public static bool IsLongerThan(this Vector3 v, Vector3 v2)
		{
			return v.sqrMagnitude > v2.sqrMagnitude;
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x0019FEC8 File Offset: 0x0019E0C8
		public static Vector3 GetClosestPoint(this Ray ray, Vector3 target)
		{
			float d = Vector3.Dot(target - ray.origin, ray.direction);
			return ray.origin + ray.direction * d;
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x0019FF08 File Offset: 0x0019E108
		public static float GetClosestDistSqr(this Ray ray, Vector3 target)
		{
			return (ray.GetClosestPoint(target) - target).sqrMagnitude;
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x0019FF2C File Offset: 0x0019E12C
		public static float GetClosestDistance(this Ray ray, Vector3 target)
		{
			return (ray.GetClosestPoint(target) - target).magnitude;
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0019FF50 File Offset: 0x0019E150
		public static Vector3 ProjectToPlane(this Ray ray, Vector3 planeOrigin, Vector3 planeNormalMustBeLength1)
		{
			Vector3 rhs = planeOrigin - ray.origin;
			float d = Vector3.Dot(planeNormalMustBeLength1, rhs);
			float d2 = Vector3.Dot(planeNormalMustBeLength1, ray.direction);
			return ray.origin + ray.direction * d / d2;
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x0019FFA0 File Offset: 0x0019E1A0
		public static Vector3 ProjectToLine(this Ray ray, Vector3 lineStart, Vector3 lineEnd)
		{
			Vector3 normalized = (lineEnd - lineStart).normalized;
			Vector3 normalized2 = Vector3.Cross(Vector3.Cross(ray.direction, normalized), normalized).normalized;
			return ray.ProjectToPlane(lineStart, normalized2);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x00061ABD File Offset: 0x0005FCBD
		public static bool IsNull(this UnityEngine.Object mono)
		{
			return mono == null || !mono;
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x00061ACD File Offset: 0x0005FCCD
		public static bool IsNotNull(this UnityEngine.Object mono)
		{
			return !mono.IsNull();
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x0019FFE4 File Offset: 0x0019E1E4
		public static string GetPath(this Transform transform)
		{
			string text = transform.name;
			while (transform.parent)
			{
				transform = transform.parent;
				text = transform.name + "/" + text;
			}
			return "/" + text;
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x001A002C File Offset: 0x0019E22C
		public static string GetPathQ(this Transform transform)
		{
			Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder();
			string result;
			try
			{
				transform.GetPathQ(ref utf16ValueStringBuilder);
			}
			finally
			{
				result = utf16ValueStringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x001A006C File Offset: 0x0019E26C
		public static void GetPathQ(this Transform transform, ref Utf16ValueStringBuilder sb)
		{
			sb.Append("\"");
			int length = sb.Length;
			do
			{
				if (sb.Length > length)
				{
					sb.Insert(length, "/");
				}
				sb.Insert(length, transform.name);
				transform = transform.parent;
			}
			while (transform != null);
			sb.Append("\"");
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x001A00CC File Offset: 0x0019E2CC
		public static string GetPath(this Transform transform, int maxDepth)
		{
			string text = transform.name;
			int num = 0;
			while (transform.parent && num < maxDepth)
			{
				transform = transform.parent;
				text = transform.name + "/" + text;
				num++;
			}
			return "/" + text;
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x001A0120 File Offset: 0x0019E320
		public static string GetPath(this Transform transform, Transform stopper)
		{
			string text = transform.name;
			while (transform.parent && transform.parent != stopper)
			{
				transform = transform.parent;
				text = transform.name + "/" + text;
			}
			return "/" + text;
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x00061AD8 File Offset: 0x0005FCD8
		public static string GetPath(this GameObject gameObject)
		{
			return gameObject.transform.GetPath();
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x00061AE5 File Offset: 0x0005FCE5
		public static void GetPath(this GameObject gameObject, ref Utf16ValueStringBuilder sb)
		{
			gameObject.transform.GetPathQ(ref sb);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00061AF3 File Offset: 0x0005FCF3
		public static string GetPath(this GameObject gameObject, int limit)
		{
			return gameObject.transform.GetPath(limit);
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x001A0178 File Offset: 0x0019E378
		public static string[] GetPaths(this GameObject[] gobj)
		{
			string[] array = new string[gobj.Length];
			for (int i = 0; i < gobj.Length; i++)
			{
				array[i] = gobj[i].GetPath();
			}
			return array;
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x001A01A8 File Offset: 0x0019E3A8
		public static string[] GetPaths(this Transform[] xform)
		{
			string[] array = new string[xform.Length];
			for (int i = 0; i < xform.Length; i++)
			{
				array[i] = xform[i].GetPath();
			}
			return array;
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x001A01D8 File Offset: 0x0019E3D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetRelativePath(string fromPath, string toPath, ref Utf16ValueStringBuilder ZStringBuilder)
		{
			if (string.IsNullOrEmpty(fromPath) || string.IsNullOrEmpty(toPath))
			{
				return;
			}
			int num = 0;
			while (num < fromPath.Length && fromPath[num] == '/')
			{
				num++;
			}
			int num2 = 0;
			while (num2 < toPath.Length && toPath[num2] == '/')
			{
				num2++;
			}
			int num3 = -1;
			int num4 = Mathf.Min(fromPath.Length - num, toPath.Length - num2);
			bool flag = true;
			for (int i = 0; i < num4; i++)
			{
				if (fromPath[num + i] != toPath[num2 + i])
				{
					flag = false;
					break;
				}
				if (fromPath[num + i] == '/')
				{
					num3 = i;
				}
			}
			num3 = (flag ? num4 : num3);
			int num5 = (num3 < fromPath.Length - num) ? (num3 + 1) : (fromPath.Length - num);
			int num6 = (num3 < toPath.Length - num2) ? (num3 + 1) : (toPath.Length - num2);
			if (num5 < fromPath.Length - num)
			{
				ZStringBuilder.Append("../");
				for (int j = num5; j < fromPath.Length - num; j++)
				{
					if (fromPath[num + j] == '/')
					{
						ZStringBuilder.Append("../");
					}
				}
			}
			else
			{
				ZStringBuilder.Append((toPath.Length - num2 - num6 > 0) ? "./" : ".");
			}
			ZStringBuilder.Append(toPath, num2 + num6, toPath.Length - (num2 + num6));
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x001A0344 File Offset: 0x0019E544
		public static string GetRelativePath(string fromPath, string toPath)
		{
			Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder();
			string result;
			try
			{
				GTExt.GetRelativePath(fromPath, toPath, ref utf16ValueStringBuilder);
			}
			finally
			{
				result = utf16ValueStringBuilder.ToString();
				utf16ValueStringBuilder.Dispose();
			}
			return result;
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x00061B01 File Offset: 0x0005FD01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetRelativePath(this Transform fromXform, Transform toXform, ref Utf16ValueStringBuilder ZStringBuilder)
		{
			GTExt.GetRelativePath(fromXform.GetPath(), toXform.GetPath(), ref ZStringBuilder);
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x001A038C File Offset: 0x0019E58C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetRelativePath(this Transform fromXform, Transform toXform)
		{
			Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder();
			string result;
			try
			{
				fromXform.GetRelativePath(toXform, ref utf16ValueStringBuilder);
			}
			finally
			{
				result = utf16ValueStringBuilder.ToString();
				utf16ValueStringBuilder.Dispose();
			}
			return result;
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x001A03D4 File Offset: 0x0019E5D4
		public static void GetPathWithSiblingIndexes(this Transform transform, ref Utf16ValueStringBuilder strBuilder)
		{
			int length = strBuilder.Length;
			while (transform != null)
			{
				strBuilder.Insert(length, transform.name);
				strBuilder.Insert(length, "|");
				strBuilder.Insert(length, transform.GetSiblingIndex().ToString("0000"));
				strBuilder.Insert(length, "/");
				transform = transform.parent;
			}
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x001A043C File Offset: 0x0019E63C
		public static string GetComponentPath(this Component component, int maxDepth = 2147483647)
		{
			Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder();
			string result;
			try
			{
				component.GetComponentPath(ref utf16ValueStringBuilder, maxDepth);
			}
			finally
			{
				result = utf16ValueStringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x001A047C File Offset: 0x0019E67C
		public static string GetComponentPath<T>(this T component, int maxDepth = 2147483647) where T : Component
		{
			Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder();
			string result;
			try
			{
				component.GetComponentPath(ref utf16ValueStringBuilder, maxDepth);
			}
			finally
			{
				result = utf16ValueStringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x001A04BC File Offset: 0x0019E6BC
		public static void GetComponentPath<T>(this T component, ref Utf16ValueStringBuilder strBuilder, int maxDepth = 2147483647) where T : Component
		{
			Transform transform = component.transform;
			int length = strBuilder.Length;
			if (maxDepth > 0)
			{
				strBuilder.Append("/");
			}
			strBuilder.Append("->/");
			Type typeFromHandle = typeof(T);
			strBuilder.Append(typeFromHandle.Name);
			if (maxDepth <= 0)
			{
				return;
			}
			int num = 0;
			while (transform != null)
			{
				strBuilder.Insert(length, transform.name);
				num++;
				if (maxDepth <= num)
				{
					break;
				}
				strBuilder.Insert(length, "/");
				transform = transform.parent;
			}
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x001A0548 File Offset: 0x0019E748
		public static void GetComponentPathWithSiblingIndexes<T>(this T component, ref Utf16ValueStringBuilder strBuilder) where T : Component
		{
			Transform transform = component.transform;
			int length = strBuilder.Length;
			strBuilder.Append("/->/");
			Type typeFromHandle = typeof(T);
			strBuilder.Append(typeFromHandle.Name);
			while (transform != null)
			{
				strBuilder.Insert(length, transform.name);
				strBuilder.Insert(length, "|");
				strBuilder.Insert(length, transform.GetSiblingIndex().ToString("0000"));
				strBuilder.Insert(length, "/");
				transform = transform.parent;
			}
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x001A05DC File Offset: 0x0019E7DC
		public static string GetComponentPathWithSiblingIndexes<T>(this T component) where T : Component
		{
			Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder();
			string result;
			try
			{
				component.GetComponentPathWithSiblingIndexes(ref utf16ValueStringBuilder);
			}
			finally
			{
				result = utf16ValueStringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x001A061C File Offset: 0x0019E81C
		public static T GetComponentByPath<T>(this GameObject root, string path) where T : Component
		{
			string[] array = path.Split(new string[]
			{
				"/->/"
			}, StringSplitOptions.None);
			if (array.Length < 2)
			{
				return default(T);
			}
			string[] array2 = array[0].Split(new string[]
			{
				"/"
			}, StringSplitOptions.RemoveEmptyEntries);
			Transform transform = root.transform;
			for (int i = 1; i < array2.Length; i++)
			{
				string n = array2[i];
				transform = transform.Find(n);
				if (transform == null)
				{
					return default(T);
				}
			}
			Type type = Type.GetType(array[1].Split('#', StringSplitOptions.None)[0]);
			if (type == null)
			{
				return default(T);
			}
			Component component = transform.GetComponent(type);
			if (component == null)
			{
				return default(T);
			}
			return component as T;
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x001A06F8 File Offset: 0x0019E8F8
		public static int GetDepth(this Transform xform)
		{
			int num = 0;
			Transform parent = xform.parent;
			while (parent != null)
			{
				num++;
				parent = parent.parent;
			}
			return num;
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x001A0728 File Offset: 0x0019E928
		public static string GetPathWithSiblingIndexes(this Transform transform)
		{
			Utf16ValueStringBuilder utf16ValueStringBuilder = ZString.CreateStringBuilder();
			string result;
			try
			{
				transform.GetPathWithSiblingIndexes(ref utf16ValueStringBuilder);
			}
			finally
			{
				result = utf16ValueStringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00061B15 File Offset: 0x0005FD15
		public static void GetPathWithSiblingIndexes(this GameObject gameObject, ref Utf16ValueStringBuilder stringBuilder)
		{
			gameObject.transform.GetPathWithSiblingIndexes(ref stringBuilder);
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x00061B23 File Offset: 0x0005FD23
		public static string GetPathWithSiblingIndexes(this GameObject gameObject)
		{
			return gameObject.transform.GetPathWithSiblingIndexes();
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x00061B30 File Offset: 0x0005FD30
		public static void AddDictValue(Transform xForm, Dictionary<string, Transform> dict)
		{
			GTExt.caseSenseInner.Add(xForm, dict);
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x00061B3E File Offset: 0x0005FD3E
		public static void ClearDicts()
		{
			GTExt.caseSenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();
			GTExt.caseInsenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x001A0768 File Offset: 0x0019E968
		public static bool TryFindByExactPath([NotNull] string path, out Transform result, FindObjectsInactive findObjectsInactive = FindObjectsInactive.Include)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("TryFindByExactPath: Provided path cannot be null or empty.");
			}
			if (findObjectsInactive != FindObjectsInactive.Exclude)
			{
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					if (sceneAt.isLoaded && sceneAt.TryFindByExactPath(path, out result))
					{
						return true;
					}
				}
				result = null;
				return false;
			}
			if (path[0] != '/')
			{
				path = "/" + path;
			}
			GameObject gameObject = GameObject.Find(path);
			if (gameObject)
			{
				result = gameObject.transform;
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x001A07F4 File Offset: 0x0019E9F4
		public static bool TryFindByExactPath(this Scene scene, string path, out Transform result)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("TryFindByExactPath: Provided path cannot be null or empty.");
			}
			string[] splitPath = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			return scene.TryFindByExactPath(splitPath, out result);
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x001A0828 File Offset: 0x0019EA28
		private static bool TryFindByExactPath(this Scene scene, IReadOnlyList<string> splitPath, out Transform result)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				if (GTExt.TryFindByExactPath_Internal(rootGameObjects[i].transform, splitPath, 0, out result))
				{
					return true;
				}
			}
			result = null;
			return false;
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x001A0864 File Offset: 0x0019EA64
		public static bool TryFindByExactPath(this Transform rootXform, string path, out Transform result)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("TryFindByExactPath: Provided path cannot be null or empty.");
			}
			string[] splitPath = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			using (IEnumerator enumerator = rootXform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (GTExt.TryFindByExactPath_Internal((Transform)enumerator.Current, splitPath, 0, out result))
					{
						return true;
					}
				}
			}
			result = null;
			return false;
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x001A08E4 File Offset: 0x0019EAE4
		public static bool TryFindByExactPath(this Transform rootXform, IReadOnlyList<string> splitPath, out Transform result)
		{
			using (IEnumerator enumerator = rootXform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (GTExt.TryFindByExactPath_Internal((Transform)enumerator.Current, splitPath, 0, out result))
					{
						return true;
					}
				}
			}
			result = null;
			return false;
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x001A0944 File Offset: 0x0019EB44
		private static bool TryFindByExactPath_Internal(Transform current, IReadOnlyList<string> splitPath, int index, out Transform result)
		{
			if (current.name != splitPath[index])
			{
				result = null;
				return false;
			}
			if (index == splitPath.Count - 1)
			{
				result = current;
				return true;
			}
			using (IEnumerator enumerator = current.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (GTExt.TryFindByExactPath_Internal((Transform)enumerator.Current, splitPath, index + 1, out result))
					{
						return true;
					}
				}
			}
			result = null;
			return false;
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x001A09D0 File Offset: 0x0019EBD0
		public static bool TryFindByPath(string globPath, out Transform result, bool caseSensitive = false)
		{
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return GTExt._TryFindByPath(null, pathPartsRegex, -1, out result, caseSensitive, true, globPath);
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x001A09F0 File Offset: 0x0019EBF0
		public static bool TryFindByPath(this Scene scene, string globPath, out Transform result, bool caseSensitive = false)
		{
			if (string.IsNullOrEmpty(globPath))
			{
				throw new Exception("TryFindByPath: Provided path cannot be null or empty.");
			}
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return scene.TryFindByPath(pathPartsRegex, out result, globPath, caseSensitive);
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x001A0A24 File Offset: 0x0019EC24
		private static bool TryFindByPath(this Scene scene, IReadOnlyList<string> pathPartsRegex, out Transform result, string globPath, bool caseSensitive = false)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				if (GTExt._TryFindByPath(rootGameObjects[i].transform, pathPartsRegex, 0, out result, caseSensitive, false, globPath))
				{
					return true;
				}
			}
			result = null;
			return false;
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x001A0A64 File Offset: 0x0019EC64
		public static bool TryFindByPath(this Transform rootXform, string globPath, out Transform result, bool caseSensitive = false)
		{
			if (string.IsNullOrEmpty(globPath))
			{
				throw new Exception("TryFindByPath: Provided path cannot be null or empty.");
			}
			char c = globPath[0];
			if (c != ' ' && c != '\n' && c != '\t')
			{
				c = globPath[globPath.Length - 1];
				if (c != ' ' && c != '\n' && c != '\t')
				{
					string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
					return GTExt._TryFindByPath(rootXform, pathPartsRegex, -1, out result, caseSensitive, false, globPath);
				}
			}
			throw new Exception("TryFindByPath: Provided globPath cannot end or start with whitespace.\nProvided globPath=\"" + globPath + "\"");
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x00061B54 File Offset: 0x0005FD54
		public static List<string> ShowAllStringsUsed()
		{
			return GTExt.allStringsUsed.Keys.ToList<string>();
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x001A0AE8 File Offset: 0x0019ECE8
		private static bool _TryFindByPath(Transform current, IReadOnlyList<string> pathPartsRegex, int index, out Transform result, bool caseSensitive, bool isAtSceneLevel, string joinedPath)
		{
			if (joinedPath != null && !GTExt.allStringsUsed.ContainsKey(joinedPath))
			{
				GTExt.allStringsUsed[joinedPath] = joinedPath;
			}
			if (caseSensitive)
			{
				if (GTExt.caseSenseInner.ContainsKey(current))
				{
					if (GTExt.caseSenseInner[current].ContainsKey(joinedPath))
					{
						result = GTExt.caseSenseInner[current][joinedPath];
						return true;
					}
				}
				else
				{
					GTExt.caseSenseInner[current] = new Dictionary<string, Transform>();
				}
			}
			else if (GTExt.caseInsenseInner.ContainsKey(current))
			{
				if (GTExt.caseInsenseInner[current].ContainsKey(joinedPath))
				{
					result = GTExt.caseInsenseInner[current][joinedPath];
					return true;
				}
			}
			else
			{
				GTExt.caseInsenseInner[current] = new Dictionary<string, Transform>();
			}
			string a;
			if (isAtSceneLevel)
			{
				index = ((index == -1) ? 0 : index);
				a = pathPartsRegex[index];
				if (a == ".." || a == "..**" || a == "**..")
				{
					result = null;
					return false;
				}
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					if (sceneAt.isLoaded)
					{
						GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
						for (int j = 0; j < rootGameObjects.Length; j++)
						{
							if (GTExt._TryFindByPath(rootGameObjects[j].transform, pathPartsRegex, index, out result, caseSensitive, false, joinedPath))
							{
								if (caseSensitive)
								{
									GTExt.caseSenseInner[current][joinedPath] = result;
								}
								else
								{
									GTExt.caseInsenseInner[current][joinedPath] = result;
								}
								return true;
							}
						}
					}
				}
			}
			if (index != -1)
			{
				a = pathPartsRegex[index];
				if (!(a == "."))
				{
					if (!(a == ".."))
					{
						if (a == "**")
						{
							goto IL_50A;
						}
						if (!(a == "..**") && !(a == "**.."))
						{
							if (!Regex.IsMatch(current.name, pathPartsRegex[index], caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase))
							{
								goto IL_8CB;
							}
							if (index == pathPartsRegex.Count - 1)
							{
								result = current;
								if (caseSensitive)
								{
									GTExt.caseSenseInner[current][joinedPath] = result;
								}
								else
								{
									GTExt.caseInsenseInner[current][joinedPath] = result;
								}
								return true;
							}
							using (IEnumerator enumerator = current.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (GTExt._TryFindByPath((Transform)enumerator.Current, pathPartsRegex, index + 1, out result, caseSensitive, false, joinedPath))
									{
										if (caseSensitive)
										{
											GTExt.caseSenseInner[current][joinedPath] = result;
										}
										else
										{
											GTExt.caseInsenseInner[current][joinedPath] = result;
										}
										return true;
									}
								}
							}
							goto IL_8CB;
						}
						else
						{
							string a2;
							do
							{
								index++;
								if (index >= pathPartsRegex.Count)
								{
									break;
								}
								a2 = pathPartsRegex[index];
							}
							while (a2 == "..**" || a2 == "**..");
							if (index == pathPartsRegex.Count)
							{
								result = current.root;
								if (caseSensitive)
								{
									GTExt.caseSenseInner[current][joinedPath] = result;
								}
								else
								{
									GTExt.caseInsenseInner[current][joinedPath] = result;
								}
								return true;
							}
							Transform parent = current.parent;
							while (parent)
							{
								if (GTExt._TryFindByPath(parent, pathPartsRegex, index, out result, caseSensitive, false, joinedPath))
								{
									if (caseSensitive)
									{
										GTExt.caseSenseInner[current][joinedPath] = result;
									}
									else
									{
										GTExt.caseInsenseInner[current][joinedPath] = result;
									}
									return true;
								}
								using (IEnumerator enumerator = parent.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										if (GTExt._TryFindByPath((Transform)enumerator.Current, pathPartsRegex, index, out result, caseSensitive, false, joinedPath))
										{
											if (caseSensitive)
											{
												GTExt.caseSenseInner[current][joinedPath] = result;
											}
											else
											{
												GTExt.caseInsenseInner[current][joinedPath] = result;
											}
											return true;
										}
									}
								}
								parent = parent.parent;
							}
							if (parent != null)
							{
								goto IL_8CB;
							}
							bool result2 = GTExt._TryFindByPath(current.root, pathPartsRegex, index, out result, caseSensitive, true, joinedPath);
							if (caseSensitive)
							{
								GTExt.caseSenseInner[current][joinedPath] = result;
								return result2;
							}
							GTExt.caseInsenseInner[current][joinedPath] = result;
							return result2;
						}
					}
				}
				else
				{
					while (pathPartsRegex[index] == ".")
					{
						if (index == pathPartsRegex.Count - 1)
						{
							result = current;
							return true;
						}
						index++;
					}
					if (GTExt._TryFindByPath(current, pathPartsRegex, index, out result, caseSensitive, false, joinedPath))
					{
						if (caseSensitive)
						{
							GTExt.caseSenseInner[current][joinedPath] = result;
						}
						else
						{
							GTExt.caseInsenseInner[current][joinedPath] = result;
						}
						return true;
					}
					using (IEnumerator enumerator = current.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (GTExt._TryFindByPath((Transform)enumerator.Current, pathPartsRegex, index, out result, caseSensitive, false, joinedPath))
							{
								if (caseSensitive)
								{
									GTExt.caseSenseInner[current][joinedPath] = result;
								}
								else
								{
									GTExt.caseInsenseInner[current][joinedPath] = result;
								}
								return true;
							}
						}
						goto IL_8CB;
					}
				}
				Transform transform = current;
				int num = index;
				while (pathPartsRegex[num] == "..")
				{
					if (num + 1 >= pathPartsRegex.Count)
					{
						result = transform.parent;
						return result != null;
					}
					if (transform.parent == null)
					{
						bool result3 = GTExt._TryFindByPath(transform, pathPartsRegex, num + 1, out result, caseSensitive, true, joinedPath);
						if (caseSensitive)
						{
							GTExt.caseSenseInner[current][joinedPath] = result;
							return result3;
						}
						GTExt.caseInsenseInner[current][joinedPath] = result;
						return result3;
					}
					else
					{
						transform = transform.parent;
						num++;
					}
				}
				using (IEnumerator enumerator = transform.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (GTExt._TryFindByPath((Transform)enumerator.Current, pathPartsRegex, num, out result, caseSensitive, false, joinedPath))
						{
							if (caseSensitive)
							{
								GTExt.caseSenseInner[current][joinedPath] = result;
							}
							else
							{
								GTExt.caseInsenseInner[current][joinedPath] = result;
							}
							return true;
						}
					}
					goto IL_8CB;
				}
				IL_50A:
				if (index == pathPartsRegex.Count - 1)
				{
					result = ((current.childCount > 0) ? current.GetChild(0) : null);
					return current.childCount > 0;
				}
				if (index <= pathPartsRegex.Count - 1 && Regex.IsMatch(current.name, pathPartsRegex[index + 1], caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase))
				{
					if (index + 2 == pathPartsRegex.Count)
					{
						result = current;
						return true;
					}
					using (IEnumerator enumerator = current.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (GTExt._TryFindByPath((Transform)enumerator.Current, pathPartsRegex, index + 2, out result, caseSensitive, false, joinedPath))
							{
								return true;
							}
						}
					}
				}
				Transform transform2;
				if (GTExt._TryBreadthFirstSearchNames(current, pathPartsRegex[index + 1], out transform2, caseSensitive))
				{
					if (index + 2 == pathPartsRegex.Count)
					{
						result = transform2;
						if (caseSensitive)
						{
							GTExt.caseSenseInner[current][joinedPath] = result;
						}
						else
						{
							GTExt.caseInsenseInner[current][joinedPath] = result;
						}
						return true;
					}
					if (GTExt._TryFindByPath(transform2, pathPartsRegex, index + 2, out result, caseSensitive, false, joinedPath))
					{
						if (caseSensitive)
						{
							GTExt.caseSenseInner[current][joinedPath] = result;
						}
						else
						{
							GTExt.caseInsenseInner[current][joinedPath] = result;
						}
						return true;
					}
				}
				IL_8CB:
				result = null;
				if (caseSensitive)
				{
					GTExt.caseSenseInner[current][joinedPath] = result;
				}
				else
				{
					GTExt.caseInsenseInner[current][joinedPath] = result;
				}
				return false;
			}
			if (pathPartsRegex.Count == 0)
			{
				result = null;
				return false;
			}
			a = pathPartsRegex[0];
			if (!(a == ".") && !(a == "..") && !(a == "..**") && !(a == "**.."))
			{
				using (IEnumerator enumerator = current.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (GTExt._TryFindByPath((Transform)enumerator.Current, pathPartsRegex, 0, out result, caseSensitive, false, joinedPath))
						{
							if (caseSensitive)
							{
								GTExt.caseSenseInner[current][joinedPath] = result;
							}
							else
							{
								GTExt.caseInsenseInner[current][joinedPath] = result;
							}
							return true;
						}
					}
				}
				result = null;
				if (caseSensitive)
				{
					GTExt.caseSenseInner[current][joinedPath] = result;
				}
				else
				{
					GTExt.caseInsenseInner[current][joinedPath] = result;
				}
				return false;
			}
			bool result4 = GTExt._TryFindByPath(current, pathPartsRegex, 0, out result, caseSensitive, false, joinedPath);
			if (caseSensitive)
			{
				GTExt.caseSenseInner[current][joinedPath] = result;
				return result4;
			}
			GTExt.caseInsenseInner[current][joinedPath] = result;
			return result4;
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x001A1444 File Offset: 0x0019F644
		private static bool _TryBreadthFirstSearchNames(Transform root, string regexPattern, out Transform result, bool caseSensitive)
		{
			Queue<Transform> queue = new Queue<Transform>();
			using (IEnumerator enumerator = root.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform item = (Transform)obj;
					queue.Enqueue(item);
				}
				goto IL_9B;
			}
			IL_3D:
			Transform transform = queue.Dequeue();
			if (Regex.IsMatch(transform.name, regexPattern, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase))
			{
				result = transform;
				return true;
			}
			foreach (object obj2 in transform)
			{
				Transform item2 = (Transform)obj2;
				queue.Enqueue(item2);
			}
			IL_9B:
			if (queue.Count <= 0)
			{
				result = null;
				return false;
			}
			goto IL_3D;
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x001A1518 File Offset: 0x0019F718
		public static T[] FindComponentsByExactPath<T>(string path) where T : Component
		{
			List<T> list;
			T[] result;
			using (UnityEngine.Pool.CollectionPool<List<T>, T>.Get(out list))
			{
				list.EnsureCapacity(64);
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					if (sceneAt.isLoaded)
					{
						list.AddRange(sceneAt.FindComponentsByExactPath(path));
					}
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x001A158C File Offset: 0x0019F78C
		public static T[] FindComponentsByExactPath<T>(this Scene scene, string path) where T : Component
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("FindComponentsByExactPath: Provided path cannot be null or empty.");
			}
			string[] splitPath = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			return scene.FindComponentsByExactPath(splitPath);
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x001A15C0 File Offset: 0x0019F7C0
		private static T[] FindComponentsByExactPath<T>(this Scene scene, string[] splitPath) where T : Component
		{
			List<T> list;
			T[] result;
			using (UnityEngine.Pool.CollectionPool<List<T>, T>.Get(out list))
			{
				list.EnsureCapacity(64);
				GameObject[] rootGameObjects = scene.GetRootGameObjects();
				for (int i = 0; i < rootGameObjects.Length; i++)
				{
					GTExt._FindComponentsByExactPath<T>(rootGameObjects[i].transform, splitPath, 0, list);
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x001A1630 File Offset: 0x0019F830
		public static T[] FindComponentsByExactPath<T>(this Transform rootXform, string path) where T : Component
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("FindComponentsByExactPath: Provided path cannot be null or empty.");
			}
			string[] splitPath = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			List<T> list;
			T[] result;
			using (UnityEngine.Pool.CollectionPool<List<T>, T>.Get(out list))
			{
				list.EnsureCapacity(64);
				foreach (object obj in rootXform)
				{
					GTExt._FindComponentsByExactPath<T>((Transform)obj, splitPath, 0, list);
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x001A16DC File Offset: 0x0019F8DC
		public static T[] FindComponentsByExactPath<T>(this Transform rootXform, string[] splitPath) where T : Component
		{
			List<T> list;
			T[] result;
			using (UnityEngine.Pool.CollectionPool<List<T>, T>.Get(out list))
			{
				list.EnsureCapacity(64);
				foreach (object obj in rootXform)
				{
					GTExt._FindComponentsByExactPath<T>((Transform)obj, splitPath, 0, list);
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x001A1768 File Offset: 0x0019F968
		private static void _FindComponentsByExactPath<T>(Transform current, string[] splitPath, int index, List<T> components) where T : Component
		{
			if (current.name != splitPath[index])
			{
				return;
			}
			if (index == splitPath.Length - 1)
			{
				T component = current.GetComponent<T>();
				if (component)
				{
					components.Add(component);
				}
				return;
			}
			foreach (object obj in current)
			{
				GTExt._FindComponentsByExactPath<T>((Transform)obj, splitPath, index + 1, components);
			}
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x001A17F4 File Offset: 0x0019F9F4
		public static T[] FindComponentsByPathInLoadedScenes<T>(string wildcardPath, bool caseSensitive = false) where T : Component
		{
			List<T> list;
			T[] result;
			using (UnityEngine.Pool.CollectionPool<List<T>, T>.Get(out list))
			{
				list.EnsureCapacity(64);
				string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(wildcardPath);
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					if (sceneAt.isLoaded)
					{
						GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
						for (int j = 0; j < rootGameObjects.Length; j++)
						{
							GTExt._FindComponentsByPath<T>(rootGameObjects[j].transform, pathPartsRegex, list, caseSensitive);
						}
					}
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x001A1894 File Offset: 0x0019FA94
		public static T[] FindComponentsByPath<T>(this Scene scene, string globPath, bool caseSensitive = false) where T : Component
		{
			if (string.IsNullOrEmpty(globPath))
			{
				throw new Exception("FindComponentsByPath: Provided path cannot be null or empty.");
			}
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return scene.FindComponentsByPath(pathPartsRegex, caseSensitive);
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x001A18C4 File Offset: 0x0019FAC4
		private static T[] FindComponentsByPath<T>(this Scene scene, string[] pathPartsRegex, bool caseSensitive = false) where T : Component
		{
			List<T> list;
			T[] result;
			using (UnityEngine.Pool.CollectionPool<List<T>, T>.Get(out list))
			{
				list.EnsureCapacity(64);
				GameObject[] rootGameObjects = scene.GetRootGameObjects();
				for (int i = 0; i < rootGameObjects.Length; i++)
				{
					GTExt._FindComponentsByPath<T>(rootGameObjects[i].transform, pathPartsRegex, list, caseSensitive);
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x001A1934 File Offset: 0x0019FB34
		public static T[] FindComponentsByPath<T>(this Transform rootXform, string globPath, bool caseSensitive = false) where T : Component
		{
			if (string.IsNullOrEmpty(globPath))
			{
				throw new Exception("FindComponentsByPath: Provided path cannot be null or empty.");
			}
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return rootXform.FindComponentsByPath(pathPartsRegex, caseSensitive);
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x001A1964 File Offset: 0x0019FB64
		public static T[] FindComponentsByPath<T>(this Transform rootXform, string[] pathPartsRegex, bool caseSensitive = false) where T : Component
		{
			List<T> list;
			T[] result;
			using (UnityEngine.Pool.CollectionPool<List<T>, T>.Get(out list))
			{
				list.EnsureCapacity(64);
				GTExt._FindComponentsByPath<T>(rootXform, pathPartsRegex, list, caseSensitive);
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x001A19B4 File Offset: 0x0019FBB4
		public static void _FindComponentsByPath<T>(Transform current, string[] pathPartsRegex, List<T> components, bool caseSensitive) where T : Component
		{
			List<Transform> list;
			using (UnityEngine.Pool.CollectionPool<List<Transform>, Transform>.Get(out list))
			{
				list.EnsureCapacity(64);
				if (GTExt._TryFindAllByPath(current, pathPartsRegex, 0, list, caseSensitive, false))
				{
					for (int i = 0; i < list.Count; i++)
					{
						T[] components2 = list[i].GetComponents<T>();
						components.AddRange(components2);
					}
				}
			}
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x001A1A28 File Offset: 0x0019FC28
		private static bool _TryFindAllByPath(Transform current, IReadOnlyList<string> pathPartsRegex, int index, List<Transform> results, bool caseSensitive, bool isAtSceneLevel = false)
		{
			bool flag = false;
			string a;
			if (isAtSceneLevel)
			{
				a = pathPartsRegex[index];
				if (a == ".." || a == "..**" || a == "**..")
				{
					return false;
				}
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					if (sceneAt.isLoaded)
					{
						foreach (GameObject gameObject in sceneAt.GetRootGameObjects())
						{
							flag |= GTExt._TryFindAllByPath(gameObject.transform, pathPartsRegex, index, results, caseSensitive, false);
						}
					}
				}
			}
			a = pathPartsRegex[index];
			if (!(a == "."))
			{
				if (!(a == ".."))
				{
					Transform transform2;
					if (!(a == "**"))
					{
						if (!(a == "..**") && !(a == "**.."))
						{
							if (Regex.IsMatch(current.name, pathPartsRegex[index], caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase))
							{
								if (index == pathPartsRegex.Count - 1)
								{
									results.Add(current);
									return true;
								}
								foreach (object obj in current)
								{
									Transform current2 = (Transform)obj;
									flag |= GTExt._TryFindAllByPath(current2, pathPartsRegex, index + 1, results, caseSensitive, false);
								}
							}
						}
						else
						{
							int k;
							for (k = index + 1; k < pathPartsRegex.Count; k++)
							{
								string a2 = pathPartsRegex[k];
								if (!(a2 == "..**") && !(a2 == "**.."))
								{
									break;
								}
							}
							if (k == pathPartsRegex.Count)
							{
								results.Add(current.root);
								return true;
							}
							Transform transform = current;
							while (transform)
							{
								flag |= GTExt._TryFindAllByPath(transform, pathPartsRegex, index + 1, results, caseSensitive, false);
								transform = transform.parent;
							}
						}
					}
					else if (index == pathPartsRegex.Count - 1)
					{
						for (int l = 0; l < current.childCount; l++)
						{
							results.Add(current.GetChild(l));
							flag = true;
						}
					}
					else if (GTExt._TryBreadthFirstSearchNames(current, pathPartsRegex[index + 1], out transform2, caseSensitive))
					{
						if (index + 2 == pathPartsRegex.Count)
						{
							results.Add(transform2);
							return true;
						}
						flag |= GTExt._TryFindAllByPath(transform2, pathPartsRegex, index + 2, results, caseSensitive, false);
					}
				}
				else if (current.parent)
				{
					if (index == pathPartsRegex.Count - 1)
					{
						results.Add(current.parent);
						return true;
					}
					flag |= GTExt._TryFindAllByPath(current.parent, pathPartsRegex, index + 1, results, caseSensitive, false);
				}
			}
			else
			{
				if (index == pathPartsRegex.Count - 1)
				{
					results.Add(current);
					return true;
				}
				flag |= GTExt._TryFindAllByPath(current, pathPartsRegex, index + 1, results, caseSensitive, false);
			}
			return flag;
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x001A1D10 File Offset: 0x0019FF10
		public static string[] _GlobPathToPathPartsRegex(string path)
		{
			string[] array = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					string a = array[i];
					if (a == "**" || a == "..**" || a == "**..")
					{
						a = array[i - 1];
						if (a == "**" || a == "..**" || a == "**..")
						{
							num++;
						}
					}
				}
				array[i - num] = array[i];
			}
			if (num > 0)
			{
				Array.Resize<string>(ref array, array.Length - num);
			}
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = GTExt._GlobPathPartToRegex(array[j]);
			}
			return array;
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x001A1DD0 File Offset: 0x0019FFD0
		private static string _GlobPathPartToRegex(string pattern)
		{
			if (pattern == "." || pattern == ".." || pattern == "**" || pattern == "..**" || pattern == "**.." || pattern.StartsWith("^"))
			{
				return pattern;
			}
			return "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x001A1E54 File Offset: 0x001A0054
		[CompilerGenerated]
		internal static void <GetComponentsInChildrenUntil>g__GetRecursive|7_0<T, TStop1>(Transform currentTransform, ref List<T> components, ref GTExt.<>c__DisplayClass7_0<T, TStop1> A_2) where T : Component where TStop1 : Component
		{
			foreach (object obj in currentTransform)
			{
				Transform transform = (Transform)obj;
				if ((A_2.includeInactive || transform.gameObject.activeSelf) && !(transform.GetComponent<TStop1>() != null))
				{
					T component = transform.GetComponent<T>();
					if (component != null)
					{
						components.Add(component);
					}
					GTExt.<GetComponentsInChildrenUntil>g__GetRecursive|7_0<T, TStop1>(transform, ref components, ref A_2);
				}
			}
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x001A1EF0 File Offset: 0x001A00F0
		[CompilerGenerated]
		internal static void <GetComponentsInChildrenUntil>g__GetRecursive|8_0<T, TStop1, TStop2>(Transform currentTransform, ref List<T> components, ref GTExt.<>c__DisplayClass8_0<T, TStop1, TStop2> A_2) where T : Component where TStop1 : Component where TStop2 : Component
		{
			foreach (object obj in currentTransform)
			{
				Transform transform = (Transform)obj;
				if ((A_2.includeInactive || transform.gameObject.activeSelf) && !(transform.GetComponent<TStop1>() != null) && !(transform.GetComponent<TStop2>() != null))
				{
					T component = transform.GetComponent<T>();
					if (component != null)
					{
						components.Add(component);
					}
					GTExt.<GetComponentsInChildrenUntil>g__GetRecursive|8_0<T, TStop1, TStop2>(transform, ref components, ref A_2);
				}
			}
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x001A1FA0 File Offset: 0x001A01A0
		[CompilerGenerated]
		internal static void <GetComponentsInChildrenUntil>g__GetRecursive|9_0<T, TStop1, TStop2, TStop3>(Transform currentTransform, ref List<T> components, ref GTExt.<>c__DisplayClass9_0<T, TStop1, TStop2, TStop3> A_2) where T : Component where TStop1 : Component where TStop2 : Component where TStop3 : Component
		{
			foreach (object obj in currentTransform)
			{
				Transform transform = (Transform)obj;
				if ((A_2.includeInactive || transform.gameObject.activeSelf) && !(transform.GetComponent<TStop1>() != null) && !(transform.GetComponent<TStop2>() != null) && !(transform.GetComponent<TStop3>() != null))
				{
					T component = transform.GetComponent<T>();
					if (component != null)
					{
						components.Add(component);
					}
					GTExt.<GetComponentsInChildrenUntil>g__GetRecursive|9_0<T, TStop1, TStop2, TStop3>(transform, ref components, ref A_2);
				}
			}
		}

		// Token: 0x04004CC5 RID: 19653
		private static Dictionary<Transform, Dictionary<string, Transform>> caseSenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();

		// Token: 0x04004CC6 RID: 19654
		private static Dictionary<Transform, Dictionary<string, Transform>> caseInsenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();

		// Token: 0x04004CC7 RID: 19655
		public static Dictionary<string, string> allStringsUsed = new Dictionary<string, string>();

		// Token: 0x02000B9D RID: 2973
		public enum ParityOptions
		{
			// Token: 0x04004CC9 RID: 19657
			XFlip,
			// Token: 0x04004CCA RID: 19658
			YFlip,
			// Token: 0x04004CCB RID: 19659
			ZFlip,
			// Token: 0x04004CCC RID: 19660
			AllFlip
		}
	}
}
