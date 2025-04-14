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
	// Token: 0x02000B72 RID: 2930
	public static class GTExt
	{
		// Token: 0x06004936 RID: 18742 RVA: 0x00164210 File Offset: 0x00162410
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

		// Token: 0x06004937 RID: 18743 RVA: 0x001642A8 File Offset: 0x001624A8
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

		// Token: 0x06004938 RID: 18744 RVA: 0x001642F0 File Offset: 0x001624F0
		public static List<Object> GetComponentsInHierarchy(this Scene scene, Type type, bool includeInactive = true, int capacity = 64)
		{
			List<Object> list = new List<Object>(capacity);
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				Component[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren(type, includeInactive);
				list.AddRange(componentsInChildren);
			}
			return list;
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x0016432D File Offset: 0x0016252D
		public static List<GameObject> GetGameObjectsInHierarchy(this Scene scene, bool includeInactive = true, int capacity = 64)
		{
			return scene.GetComponentsInHierarchy(includeInactive, capacity);
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00164338 File Offset: 0x00162538
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

		// Token: 0x0600493B RID: 18747 RVA: 0x0016437C File Offset: 0x0016257C
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

		// Token: 0x0600493C RID: 18748 RVA: 0x001643C0 File Offset: 0x001625C0
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

		// Token: 0x0600493D RID: 18749 RVA: 0x00164404 File Offset: 0x00162604
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

		// Token: 0x0600493E RID: 18750 RVA: 0x00164464 File Offset: 0x00162664
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

		// Token: 0x0600493F RID: 18751 RVA: 0x001644D8 File Offset: 0x001626D8
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

		// Token: 0x06004940 RID: 18752 RVA: 0x0016455E File Offset: 0x0016275E
		public static void GetComponentsInChildrenUntil<T, TStop1, TStop2, TStop3>(this Component root, out List<T> out_included, out HashSet<T> out_excluded, bool includeInactive = false, bool stopAtRoot = true, int capacity = 64) where T : Component where TStop1 : Component where TStop2 : Component where TStop3 : Component
		{
			out_included = root.GetComponentsInChildrenUntil(includeInactive, stopAtRoot, capacity);
			out_excluded = new HashSet<T>(root.GetComponentsInChildren<T>(includeInactive));
			out_excluded.ExceptWith(new HashSet<T>(out_included));
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x0016458C File Offset: 0x0016278C
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

		// Token: 0x06004942 RID: 18754 RVA: 0x00164624 File Offset: 0x00162824
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

		// Token: 0x06004943 RID: 18755 RVA: 0x00164680 File Offset: 0x00162880
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

		// Token: 0x06004944 RID: 18756 RVA: 0x001646D0 File Offset: 0x001628D0
		private static List<T> GetComponentsWithRegex_Internal<T>(IEnumerable<T> allComponents, string regexString, bool includeInactive, int capacity = 64) where T : Component
		{
			List<T> result = new List<T>(capacity);
			Regex regex = new Regex(regexString);
			GTExt.GetComponentsWithRegex_Internal<T>(allComponents, regex, ref result);
			return result;
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x001646F8 File Offset: 0x001628F8
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

		// Token: 0x06004946 RID: 18758 RVA: 0x00164758 File Offset: 0x00162958
		public static List<T> GetComponentsWithRegex<T>(this Scene scene, string regexString, bool includeInactive, int capacity) where T : Component
		{
			return GTExt.GetComponentsWithRegex_Internal<T>(scene.GetComponentsInHierarchy(includeInactive, capacity), regexString, includeInactive, capacity);
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x0016476A File Offset: 0x0016296A
		public static List<T> GetComponentsWithRegex<T>(this Component root, string regexString, bool includeInactive, int capacity) where T : Component
		{
			return GTExt.GetComponentsWithRegex_Internal<T>(root.GetComponentsInChildren<T>(includeInactive), regexString, includeInactive, capacity);
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x0016477C File Offset: 0x0016297C
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

		// Token: 0x06004949 RID: 18761 RVA: 0x001647E4 File Offset: 0x001629E4
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

		// Token: 0x0600494A RID: 18762 RVA: 0x00164874 File Offset: 0x00162A74
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

		// Token: 0x0600494B RID: 18763 RVA: 0x001648C4 File Offset: 0x00162AC4
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

		// Token: 0x0600494C RID: 18764 RVA: 0x001649C8 File Offset: 0x00162BC8
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

		// Token: 0x0600494D RID: 18765 RVA: 0x00164A34 File Offset: 0x00162C34
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

		// Token: 0x0600494E RID: 18766 RVA: 0x00164AA0 File Offset: 0x00162CA0
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

		// Token: 0x0600494F RID: 18767 RVA: 0x00164AF0 File Offset: 0x00162CF0
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

		// Token: 0x06004950 RID: 18768 RVA: 0x00164B3C File Offset: 0x00162D3C
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

		// Token: 0x06004951 RID: 18769 RVA: 0x00164BBE File Offset: 0x00162DBE
		public static T GetOrAddComponent<T>(this GameObject gameObject, ref T component) where T : Component
		{
			if (component == null)
			{
				component = gameObject.GetOrAddComponent<T>();
			}
			return component;
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x00164BE8 File Offset: 0x00162DE8
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T result;
			if (!gameObject.TryGetComponent<T>(out result))
			{
				result = gameObject.AddComponent<T>();
			}
			return result;
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x00164C08 File Offset: 0x00162E08
		public static void SetLossyScale(this Transform transform, Vector3 scale)
		{
			scale = transform.InverseTransformVector(scale);
			Vector3 lossyScale = transform.lossyScale;
			transform.localScale = new Vector3(scale.x / lossyScale.x, scale.y / lossyScale.y, scale.z / lossyScale.z);
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00164C57 File Offset: 0x00162E57
		public static Quaternion TransformRotation(this Transform transform, Quaternion localRotation)
		{
			return transform.rotation * localRotation;
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x00164C65 File Offset: 0x00162E65
		public static Quaternion InverseTransformRotation(this Transform transform, Quaternion localRotation)
		{
			return Quaternion.Inverse(transform.rotation) * localRotation;
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x00164C78 File Offset: 0x00162E78
		public static Vector3 ProjectOnPlane(this Vector3 point, Vector3 planeAnchorPosition, Vector3 planeNormal)
		{
			return planeAnchorPosition + Vector3.ProjectOnPlane(point - planeAnchorPosition, planeNormal);
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x00164C90 File Offset: 0x00162E90
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

		// Token: 0x06004958 RID: 18776 RVA: 0x00164CD8 File Offset: 0x00162ED8
		public static void AddSortedUnique<T>(this List<T> list, T item)
		{
			int num = list.BinarySearch(item);
			if (num < 0)
			{
				list.Insert(~num, item);
			}
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x00164CFC File Offset: 0x00162EFC
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

		// Token: 0x0600495A RID: 18778 RVA: 0x00164D44 File Offset: 0x00162F44
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

		// Token: 0x0600495B RID: 18779 RVA: 0x00164DD8 File Offset: 0x00162FD8
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

		// Token: 0x0600495C RID: 18780 RVA: 0x00164E7C File Offset: 0x0016307C
		public static Vector3 Position(this Matrix4x4 matrix)
		{
			float m = matrix.m03;
			float m2 = matrix.m13;
			float m3 = matrix.m23;
			return new Vector3(m, m2, m3);
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x00164EA4 File Offset: 0x001630A4
		public static Vector3 Scale(this Matrix4x4 m)
		{
			Vector3 result = new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
			if (Vector3.Cross(m.GetColumn(0), m.GetColumn(1)).normalized != m.GetColumn(2).normalized)
			{
				result.x *= -1f;
			}
			return result;
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void SetLocalRelativeToParentMatrixWithParityAxis(this Matrix4x4 matrix, GTExt.ParityOptions parity = GTExt.ParityOptions.XFlip)
		{
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x00164F3C File Offset: 0x0016313C
		public static void MultiplyInPlaceWith(this Vector3 a, in Vector3 b)
		{
			a.x *= b.x;
			a.y *= b.y;
			a.z *= b.z;
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x00164F70 File Offset: 0x00163170
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

		// Token: 0x06004961 RID: 18785 RVA: 0x00164FEC File Offset: 0x001631EC
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

		// Token: 0x06004962 RID: 18786 RVA: 0x0016501C File Offset: 0x0016321C
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

		// Token: 0x06004963 RID: 18787 RVA: 0x001650F0 File Offset: 0x001632F0
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

		// Token: 0x06004964 RID: 18788 RVA: 0x0016519C File Offset: 0x0016339C
		public static Quaternion RotationWithScaleContext(this Matrix4x4 m, in Vector3 scale)
		{
			Matrix4x4 matrix4x = m * GTExt.Matrix4x4Scale(scale);
			int num = 2;
			Vector3 forward = matrix4x.GetColumnNoCopy(num);
			int num2 = 1;
			return Quaternion.LookRotation(forward, matrix4x.GetColumnNoCopy(num2));
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x001651E0 File Offset: 0x001633E0
		public static Quaternion Rotation(this Matrix4x4 m)
		{
			int num = 2;
			Vector3 forward = m.GetColumnNoCopy(num);
			int num2 = 1;
			return Quaternion.LookRotation(forward, m.GetColumnNoCopy(num2));
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00165210 File Offset: 0x00163410
		public static Vector3 x0y(this Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x00165228 File Offset: 0x00163428
		public static Vector3 x0y(this Vector3 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x00165240 File Offset: 0x00163440
		public static Vector3 xy0(this Vector2 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x00165258 File Offset: 0x00163458
		public static Vector3 xy0(this Vector3 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x00165270 File Offset: 0x00163470
		public static Vector3 xz0(this Vector3 v)
		{
			return new Vector3(v.x, v.z, 0f);
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x00037822 File Offset: 0x00035A22
		public static Vector3 x0z(this Vector3 v)
		{
			return new Vector3(v.x, 0f, v.z);
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00165288 File Offset: 0x00163488
		public static Matrix4x4 LocalMatrixRelativeToParentNoScale(this Transform transform)
		{
			return Matrix4x4.TRS(transform.localPosition, transform.localRotation, Vector3.one);
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x001652A0 File Offset: 0x001634A0
		public static Matrix4x4 LocalMatrixRelativeToParentWithScale(this Transform transform)
		{
			if (transform.parent == null)
			{
				return transform.localToWorldMatrix;
			}
			return transform.parent.worldToLocalMatrix * transform.localToWorldMatrix;
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x001652CD File Offset: 0x001634CD
		public static void SetLocalMatrixRelativeToParent(this Transform transform, Matrix4x4 matrix)
		{
			transform.localPosition = matrix.Position();
			transform.localRotation = matrix.Rotation();
			transform.localScale = matrix.Scale();
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x001652F4 File Offset: 0x001634F4
		public static void SetLocalMatrixRelativeToParentNoScale(this Transform transform, Matrix4x4 matrix)
		{
			transform.localPosition = matrix.Position();
			transform.localRotation = matrix.Rotation();
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x0016530F File Offset: 0x0016350F
		public static void SetLocalToWorldMatrixNoScale(this Transform transform, Matrix4x4 matrix)
		{
			transform.position = matrix.Position();
			transform.rotation = matrix.Rotation();
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x0016532A File Offset: 0x0016352A
		public static Matrix4x4 localToWorldNoScale(this Transform transform)
		{
			return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x00165342 File Offset: 0x00163542
		public static void SetLocalToWorldMatrixWithScale(this Transform transform, Matrix4x4 matrix)
		{
			transform.position = matrix.Position();
			transform.rotation = matrix.rotation;
			transform.SetLossyScale(matrix.lossyScale);
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x0016536A File Offset: 0x0016356A
		public static Matrix4x4 Matrix4X4LerpNoScale(Matrix4x4 a, Matrix4x4 b, float t)
		{
			return Matrix4x4.TRS(Vector3.Lerp(a.Position(), b.Position(), t), Quaternion.Slerp(a.rotation, b.rotation, t), b.lossyScale);
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x0016539E File Offset: 0x0016359E
		public static Matrix4x4 LerpTo(this Matrix4x4 a, Matrix4x4 b, float t)
		{
			return GTExt.Matrix4X4LerpNoScale(a, b, t);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x001653A8 File Offset: 0x001635A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(this Vector3 v)
		{
			return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x001653D1 File Offset: 0x001635D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNan(this Quaternion q)
		{
			return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00165407 File Offset: 0x00163607
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInfinity(this Vector3 v)
		{
			return float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z);
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x00165430 File Offset: 0x00163630
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInfinity(this Quaternion q)
		{
			return float.IsInfinity(q.x) || float.IsInfinity(q.y) || float.IsInfinity(q.z) || float.IsInfinity(q.w);
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00165466 File Offset: 0x00163666
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ValuesInRange(this Vector3 v, in float maxVal)
		{
			return Mathf.Abs(v.x) < maxVal && Mathf.Abs(v.y) < maxVal && Mathf.Abs(v.z) < maxVal;
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00165497 File Offset: 0x00163697
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsValid(this Vector3 v, in float maxVal = 10000f)
		{
			return !v.IsNaN() && !v.IsInfinity() && v.ValuesInRange(maxVal);
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x001654B4 File Offset: 0x001636B4
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

		// Token: 0x0600497C RID: 18812 RVA: 0x001654E0 File Offset: 0x001636E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetValueSafe(this Vector3 v, in Vector3 newVal)
		{
			float num = 10000f;
			if (newVal.IsValid(num))
			{
				v = newVal;
			}
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00165509 File Offset: 0x00163709
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsValid(this Quaternion q)
		{
			return !q.IsNan() && !q.IsInfinity();
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x0016551E File Offset: 0x0016371E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Quaternion GetValidWithFallback(this Quaternion q, in Quaternion safeVal)
		{
			if (!q.IsValid())
			{
				return safeVal;
			}
			return q;
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00165535 File Offset: 0x00163735
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetValueSafe(this Quaternion q, in Quaternion newVal)
		{
			if (newVal.IsValid())
			{
				q = newVal;
			}
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x0016554C File Offset: 0x0016374C
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

		// Token: 0x06004981 RID: 18817 RVA: 0x001655A4 File Offset: 0x001637A4
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

		// Token: 0x06004982 RID: 18818 RVA: 0x00165604 File Offset: 0x00163804
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

		// Token: 0x06004983 RID: 18819 RVA: 0x00165674 File Offset: 0x00163874
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

		// Token: 0x06004984 RID: 18820 RVA: 0x001656EA File Offset: 0x001638EA
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

		// Token: 0x06004985 RID: 18821 RVA: 0x00165711 File Offset: 0x00163911
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

		// Token: 0x06004986 RID: 18822 RVA: 0x0016573E File Offset: 0x0016393E
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

		// Token: 0x06004987 RID: 18823 RVA: 0x0016576C File Offset: 0x0016396C
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

		// Token: 0x06004988 RID: 18824 RVA: 0x001657A0 File Offset: 0x001639A0
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

		// Token: 0x06004989 RID: 18825 RVA: 0x001657C7 File Offset: 0x001639C7
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

		// Token: 0x0600498A RID: 18826 RVA: 0x001657F4 File Offset: 0x001639F4
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

		// Token: 0x0600498B RID: 18827 RVA: 0x00165822 File Offset: 0x00163A22
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

		// Token: 0x0600498C RID: 18828 RVA: 0x00165856 File Offset: 0x00163A56
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

		// Token: 0x0600498D RID: 18829 RVA: 0x00165894 File Offset: 0x00163A94
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

		// Token: 0x0600498E RID: 18830 RVA: 0x001658E7 File Offset: 0x00163AE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetFinite(this float value)
		{
			if (!float.IsFinite(value))
			{
				return 0f;
			}
			return value;
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x001658F8 File Offset: 0x00163AF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double GetFinite(this double value)
		{
			if (!double.IsFinite(value))
			{
				return 0.0;
			}
			return value;
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0016590D File Offset: 0x00163B0D
		public static Matrix4x4 Matrix4X4LerpHandleNegativeScale(Matrix4x4 a, Matrix4x4 b, float t)
		{
			return Matrix4x4.TRS(Vector3.Lerp(a.Position(), b.Position(), t), Quaternion.Slerp(a.Rotation(), b.Rotation(), t), b.lossyScale);
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00165941 File Offset: 0x00163B41
		public static Matrix4x4 LerpTo_HandleNegativeScale(this Matrix4x4 a, Matrix4x4 b, float t)
		{
			return GTExt.Matrix4X4LerpHandleNegativeScale(a, b, t);
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x0016594C File Offset: 0x00163B4C
		public static Vector3 LerpToUnclamped(this Vector3 a, in Vector3 b, float t)
		{
			return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x001659A0 File Offset: 0x00163BA0
		public static string ToLongString(this Vector3 self)
		{
			return string.Format("[{0}, {1}, {2}]", self.x, self.y, self.z);
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x001659CD File Offset: 0x00163BCD
		public static int GetRandomIndex<T>(this IReadOnlyList<T> self)
		{
			return Random.Range(0, self.Count);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x001659DB File Offset: 0x00163BDB
		public static T GetRandomItem<T>(this IReadOnlyList<T> self)
		{
			return self[self.GetRandomIndex<T>()];
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x001659E9 File Offset: 0x00163BE9
		public static Vector2 xx(this float v)
		{
			return new Vector2(v, v);
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x001659F2 File Offset: 0x00163BF2
		public static Vector2 xx(this Vector2 v)
		{
			return new Vector2(v.x, v.x);
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00165A05 File Offset: 0x00163C05
		public static Vector2 xy(this Vector2 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00165A18 File Offset: 0x00163C18
		public static Vector2 yy(this Vector2 v)
		{
			return new Vector2(v.y, v.y);
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00165A2B File Offset: 0x00163C2B
		public static Vector2 xx(this Vector3 v)
		{
			return new Vector2(v.x, v.x);
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x00165A3E File Offset: 0x00163C3E
		public static Vector2 xy(this Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00165A51 File Offset: 0x00163C51
		public static Vector2 xz(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00165A64 File Offset: 0x00163C64
		public static Vector2 yy(this Vector3 v)
		{
			return new Vector2(v.y, v.y);
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00165A77 File Offset: 0x00163C77
		public static Vector2 yz(this Vector3 v)
		{
			return new Vector2(v.y, v.z);
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x00165A8A File Offset: 0x00163C8A
		public static Vector2 zz(this Vector3 v)
		{
			return new Vector2(v.z, v.z);
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00165A9D File Offset: 0x00163C9D
		public static Vector2 xx(this Vector4 v)
		{
			return new Vector2(v.x, v.x);
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00165AB0 File Offset: 0x00163CB0
		public static Vector2 xy(this Vector4 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00165AC3 File Offset: 0x00163CC3
		public static Vector2 xz(this Vector4 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x00165AD6 File Offset: 0x00163CD6
		public static Vector2 xw(this Vector4 v)
		{
			return new Vector2(v.x, v.w);
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x00165AE9 File Offset: 0x00163CE9
		public static Vector2 yy(this Vector4 v)
		{
			return new Vector2(v.y, v.y);
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x00165AFC File Offset: 0x00163CFC
		public static Vector2 yz(this Vector4 v)
		{
			return new Vector2(v.y, v.z);
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x00165B0F File Offset: 0x00163D0F
		public static Vector2 yw(this Vector4 v)
		{
			return new Vector2(v.y, v.w);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x00165B22 File Offset: 0x00163D22
		public static Vector2 zz(this Vector4 v)
		{
			return new Vector2(v.z, v.z);
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x00165B35 File Offset: 0x00163D35
		public static Vector2 zw(this Vector4 v)
		{
			return new Vector2(v.z, v.w);
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x00165B48 File Offset: 0x00163D48
		public static Vector2 ww(this Vector4 v)
		{
			return new Vector2(v.w, v.w);
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00165B5B File Offset: 0x00163D5B
		public static Vector3 xxx(this float v)
		{
			return new Vector3(v, v, v);
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x00165B65 File Offset: 0x00163D65
		public static Vector3 xxx(this Vector2 v)
		{
			return new Vector3(v.x, v.x, v.x);
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00165B7E File Offset: 0x00163D7E
		public static Vector3 xxy(this Vector2 v)
		{
			return new Vector3(v.x, v.x, v.y);
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x00165B97 File Offset: 0x00163D97
		public static Vector3 xyy(this Vector2 v)
		{
			return new Vector3(v.x, v.y, v.y);
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x00165BB0 File Offset: 0x00163DB0
		public static Vector3 yyy(this Vector2 v)
		{
			return new Vector3(v.y, v.y, v.y);
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x00165BC9 File Offset: 0x00163DC9
		public static Vector3 xxx(this Vector3 v)
		{
			return new Vector3(v.x, v.x, v.x);
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x00165BE2 File Offset: 0x00163DE2
		public static Vector3 xxy(this Vector3 v)
		{
			return new Vector3(v.x, v.x, v.y);
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x00165BFB File Offset: 0x00163DFB
		public static Vector3 xxz(this Vector3 v)
		{
			return new Vector3(v.x, v.x, v.z);
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x00165C14 File Offset: 0x00163E14
		public static Vector3 xyy(this Vector3 v)
		{
			return new Vector3(v.x, v.y, v.y);
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x00165C2D File Offset: 0x00163E2D
		public static Vector3 xyz(this Vector3 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x00165C46 File Offset: 0x00163E46
		public static Vector3 xzz(this Vector3 v)
		{
			return new Vector3(v.x, v.z, v.z);
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x00165C5F File Offset: 0x00163E5F
		public static Vector3 yyy(this Vector3 v)
		{
			return new Vector3(v.y, v.y, v.y);
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x00165C78 File Offset: 0x00163E78
		public static Vector3 yyz(this Vector3 v)
		{
			return new Vector3(v.y, v.y, v.z);
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x00165C91 File Offset: 0x00163E91
		public static Vector3 yzz(this Vector3 v)
		{
			return new Vector3(v.y, v.z, v.z);
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x00165CAA File Offset: 0x00163EAA
		public static Vector3 zzz(this Vector3 v)
		{
			return new Vector3(v.z, v.z, v.z);
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x00165CC3 File Offset: 0x00163EC3
		public static Vector3 xxx(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.x);
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x00165CDC File Offset: 0x00163EDC
		public static Vector3 xxy(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.y);
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x00165CF5 File Offset: 0x00163EF5
		public static Vector3 xxz(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.z);
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x00165D0E File Offset: 0x00163F0E
		public static Vector3 xxw(this Vector4 v)
		{
			return new Vector3(v.x, v.x, v.w);
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x00165D27 File Offset: 0x00163F27
		public static Vector3 xyy(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.y);
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x00165D40 File Offset: 0x00163F40
		public static Vector3 xyz(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00165D59 File Offset: 0x00163F59
		public static Vector3 xyw(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.w);
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x00165D72 File Offset: 0x00163F72
		public static Vector3 xzz(this Vector4 v)
		{
			return new Vector3(v.x, v.z, v.z);
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x00165D8B File Offset: 0x00163F8B
		public static Vector3 xzw(this Vector4 v)
		{
			return new Vector3(v.x, v.z, v.w);
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x00165DA4 File Offset: 0x00163FA4
		public static Vector3 xww(this Vector4 v)
		{
			return new Vector3(v.x, v.w, v.w);
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x00165DBD File Offset: 0x00163FBD
		public static Vector3 yyy(this Vector4 v)
		{
			return new Vector3(v.y, v.y, v.y);
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x00165DD6 File Offset: 0x00163FD6
		public static Vector3 yyz(this Vector4 v)
		{
			return new Vector3(v.y, v.y, v.z);
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x00165DEF File Offset: 0x00163FEF
		public static Vector3 yyw(this Vector4 v)
		{
			return new Vector3(v.y, v.y, v.w);
		}

		// Token: 0x060049C6 RID: 18886 RVA: 0x00165E08 File Offset: 0x00164008
		public static Vector3 yzz(this Vector4 v)
		{
			return new Vector3(v.y, v.z, v.z);
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x00165E21 File Offset: 0x00164021
		public static Vector3 yzw(this Vector4 v)
		{
			return new Vector3(v.y, v.z, v.w);
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x00165E3A File Offset: 0x0016403A
		public static Vector3 yww(this Vector4 v)
		{
			return new Vector3(v.y, v.w, v.w);
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x00165E53 File Offset: 0x00164053
		public static Vector3 zzz(this Vector4 v)
		{
			return new Vector3(v.z, v.z, v.z);
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x00165E6C File Offset: 0x0016406C
		public static Vector3 zzw(this Vector4 v)
		{
			return new Vector3(v.z, v.z, v.w);
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x00165E85 File Offset: 0x00164085
		public static Vector3 zww(this Vector4 v)
		{
			return new Vector3(v.z, v.w, v.w);
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x00165E9E File Offset: 0x0016409E
		public static Vector3 www(this Vector4 v)
		{
			return new Vector3(v.w, v.w, v.w);
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x00165EB7 File Offset: 0x001640B7
		public static Vector4 xxxx(this float v)
		{
			return new Vector4(v, v, v, v);
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x00165EC2 File Offset: 0x001640C2
		public static Vector4 xxxx(this Vector2 v)
		{
			return new Vector4(v.x, v.x, v.x, v.x);
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x00165EE1 File Offset: 0x001640E1
		public static Vector4 xxxy(this Vector2 v)
		{
			return new Vector4(v.x, v.x, v.x, v.y);
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x00165F00 File Offset: 0x00164100
		public static Vector4 xxyy(this Vector2 v)
		{
			return new Vector4(v.x, v.x, v.y, v.y);
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x00165F1F File Offset: 0x0016411F
		public static Vector4 xyyy(this Vector2 v)
		{
			return new Vector4(v.x, v.y, v.y, v.y);
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00165F3E File Offset: 0x0016413E
		public static Vector4 yyyy(this Vector2 v)
		{
			return new Vector4(v.y, v.y, v.y, v.y);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x00165F5D File Offset: 0x0016415D
		public static Vector4 xxxx(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.x, v.x);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x00165F7C File Offset: 0x0016417C
		public static Vector4 xxxy(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.x, v.y);
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x00165F9B File Offset: 0x0016419B
		public static Vector4 xxxz(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.x, v.z);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x00165FBA File Offset: 0x001641BA
		public static Vector4 xxyy(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.y, v.y);
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x00165FD9 File Offset: 0x001641D9
		public static Vector4 xxyz(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.y, v.z);
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x00165FF8 File Offset: 0x001641F8
		public static Vector4 xxzz(this Vector3 v)
		{
			return new Vector4(v.x, v.x, v.z, v.z);
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00166017 File Offset: 0x00164217
		public static Vector4 xyyy(this Vector3 v)
		{
			return new Vector4(v.x, v.y, v.y, v.y);
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x00166036 File Offset: 0x00164236
		public static Vector4 xyyz(this Vector3 v)
		{
			return new Vector4(v.x, v.y, v.y, v.z);
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x00166055 File Offset: 0x00164255
		public static Vector4 xyzz(this Vector3 v)
		{
			return new Vector4(v.x, v.y, v.z, v.z);
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x00166074 File Offset: 0x00164274
		public static Vector4 xzzz(this Vector3 v)
		{
			return new Vector4(v.x, v.z, v.z, v.z);
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x00166093 File Offset: 0x00164293
		public static Vector4 yyyy(this Vector3 v)
		{
			return new Vector4(v.y, v.y, v.y, v.y);
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x001660B2 File Offset: 0x001642B2
		public static Vector4 yyyz(this Vector3 v)
		{
			return new Vector4(v.y, v.y, v.y, v.z);
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x001660D1 File Offset: 0x001642D1
		public static Vector4 yyzz(this Vector3 v)
		{
			return new Vector4(v.y, v.y, v.z, v.z);
		}

		// Token: 0x060049E0 RID: 18912 RVA: 0x001660F0 File Offset: 0x001642F0
		public static Vector4 yzzz(this Vector3 v)
		{
			return new Vector4(v.y, v.z, v.z, v.z);
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x0016610F File Offset: 0x0016430F
		public static Vector4 zzzz(this Vector3 v)
		{
			return new Vector4(v.z, v.z, v.z, v.z);
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x0016612E File Offset: 0x0016432E
		public static Vector4 xxxx(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.x);
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x0016614D File Offset: 0x0016434D
		public static Vector4 xxxy(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.y);
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x0016616C File Offset: 0x0016436C
		public static Vector4 xxxz(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.z);
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x0016618B File Offset: 0x0016438B
		public static Vector4 xxxw(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.x, v.w);
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x001661AA File Offset: 0x001643AA
		public static Vector4 xxyy(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.y, v.y);
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x001661C9 File Offset: 0x001643C9
		public static Vector4 xxyz(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.y, v.z);
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x001661E8 File Offset: 0x001643E8
		public static Vector4 xxyw(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.y, v.w);
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x00166207 File Offset: 0x00164407
		public static Vector4 xxzz(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.z, v.z);
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x00166226 File Offset: 0x00164426
		public static Vector4 xxzw(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.z, v.w);
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x00166245 File Offset: 0x00164445
		public static Vector4 xxww(this Vector4 v)
		{
			return new Vector4(v.x, v.x, v.w, v.w);
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x00166264 File Offset: 0x00164464
		public static Vector4 xyyy(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.y, v.y);
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00166283 File Offset: 0x00164483
		public static Vector4 xyyz(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.y, v.z);
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x001662A2 File Offset: 0x001644A2
		public static Vector4 xyyw(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.y, v.w);
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x001662C1 File Offset: 0x001644C1
		public static Vector4 xyzz(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.z, v.z);
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x001662E0 File Offset: 0x001644E0
		public static Vector4 xyzw(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.z, v.w);
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x001662FF File Offset: 0x001644FF
		public static Vector4 xyww(this Vector4 v)
		{
			return new Vector4(v.x, v.y, v.w, v.w);
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x0016631E File Offset: 0x0016451E
		public static Vector4 xzzz(this Vector4 v)
		{
			return new Vector4(v.x, v.z, v.z, v.z);
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x0016633D File Offset: 0x0016453D
		public static Vector4 xzzw(this Vector4 v)
		{
			return new Vector4(v.x, v.z, v.z, v.w);
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x0016635C File Offset: 0x0016455C
		public static Vector4 xzww(this Vector4 v)
		{
			return new Vector4(v.x, v.z, v.w, v.w);
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x0016637B File Offset: 0x0016457B
		public static Vector4 xwww(this Vector4 v)
		{
			return new Vector4(v.x, v.w, v.w, v.w);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0016639A File Offset: 0x0016459A
		public static Vector4 yyyy(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.y, v.y);
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x001663B9 File Offset: 0x001645B9
		public static Vector4 yyyz(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.y, v.z);
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x001663D8 File Offset: 0x001645D8
		public static Vector4 yyyw(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.y, v.w);
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x001663F7 File Offset: 0x001645F7
		public static Vector4 yyzz(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.z, v.z);
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00166416 File Offset: 0x00164616
		public static Vector4 yyzw(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.z, v.w);
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x00166435 File Offset: 0x00164635
		public static Vector4 yyww(this Vector4 v)
		{
			return new Vector4(v.y, v.y, v.w, v.w);
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x00166454 File Offset: 0x00164654
		public static Vector4 yzzz(this Vector4 v)
		{
			return new Vector4(v.y, v.z, v.z, v.z);
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00166473 File Offset: 0x00164673
		public static Vector4 yzzw(this Vector4 v)
		{
			return new Vector4(v.y, v.z, v.z, v.w);
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x00166492 File Offset: 0x00164692
		public static Vector4 yzww(this Vector4 v)
		{
			return new Vector4(v.y, v.z, v.w, v.w);
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x001664B1 File Offset: 0x001646B1
		public static Vector4 ywww(this Vector4 v)
		{
			return new Vector4(v.y, v.w, v.w, v.w);
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x001664D0 File Offset: 0x001646D0
		public static Vector4 zzzz(this Vector4 v)
		{
			return new Vector4(v.z, v.z, v.z, v.z);
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x001664EF File Offset: 0x001646EF
		public static Vector4 zzzw(this Vector4 v)
		{
			return new Vector4(v.z, v.z, v.z, v.w);
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x0016650E File Offset: 0x0016470E
		public static Vector4 zzww(this Vector4 v)
		{
			return new Vector4(v.z, v.z, v.w, v.w);
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0016652D File Offset: 0x0016472D
		public static Vector4 zwww(this Vector4 v)
		{
			return new Vector4(v.z, v.w, v.w, v.w);
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x0016654C File Offset: 0x0016474C
		public static Vector4 wwww(this Vector4 v)
		{
			return new Vector4(v.w, v.w, v.w, v.w);
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x0016656B File Offset: 0x0016476B
		public static Vector4 WithX(this Vector4 v, float x)
		{
			return new Vector4(x, v.y, v.z, v.w);
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x00166585 File Offset: 0x00164785
		public static Vector4 WithY(this Vector4 v, float y)
		{
			return new Vector4(v.x, y, v.z, v.w);
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0016659F File Offset: 0x0016479F
		public static Vector4 WithZ(this Vector4 v, float z)
		{
			return new Vector4(v.x, v.y, z, v.w);
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x001665B9 File Offset: 0x001647B9
		public static Vector4 WithW(this Vector4 v, float w)
		{
			return new Vector4(v.x, v.y, v.z, w);
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x001665D3 File Offset: 0x001647D3
		public static Vector3 WithX(this Vector3 v, float x)
		{
			return new Vector3(x, v.y, v.z);
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x001665E7 File Offset: 0x001647E7
		public static Vector3 WithY(this Vector3 v, float y)
		{
			return new Vector3(v.x, y, v.z);
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x001665FB File Offset: 0x001647FB
		public static Vector3 WithZ(this Vector3 v, float z)
		{
			return new Vector3(v.x, v.y, z);
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x0016660F File Offset: 0x0016480F
		public static Vector4 WithW(this Vector3 v, float w)
		{
			return new Vector4(v.x, v.y, v.z, w);
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x00166629 File Offset: 0x00164829
		public static Vector2 WithX(this Vector2 v, float x)
		{
			return new Vector2(x, v.y);
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x00166637 File Offset: 0x00164837
		public static Vector2 WithY(this Vector2 v, float y)
		{
			return new Vector2(v.x, y);
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x00166645 File Offset: 0x00164845
		public static Vector3 WithZ(this Vector2 v, float z)
		{
			return new Vector3(v.x, v.y, z);
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x00166659 File Offset: 0x00164859
		public static bool IsShorterThan(this Vector2 v, float len)
		{
			return v.sqrMagnitude < len * len;
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x00166667 File Offset: 0x00164867
		public static bool IsShorterThan(this Vector2 v, Vector2 v2)
		{
			return v.sqrMagnitude < v2.sqrMagnitude;
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x00166679 File Offset: 0x00164879
		public static bool IsShorterThan(this Vector3 v, float len)
		{
			return v.sqrMagnitude < len * len;
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x00166687 File Offset: 0x00164887
		public static bool IsShorterThan(this Vector3 v, Vector3 v2)
		{
			return v.sqrMagnitude < v2.sqrMagnitude;
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x00166699 File Offset: 0x00164899
		public static bool IsLongerThan(this Vector2 v, float len)
		{
			return v.sqrMagnitude > len * len;
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x001666A7 File Offset: 0x001648A7
		public static bool IsLongerThan(this Vector2 v, Vector2 v2)
		{
			return v.sqrMagnitude > v2.sqrMagnitude;
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x001666B9 File Offset: 0x001648B9
		public static bool IsLongerThan(this Vector3 v, float len)
		{
			return v.sqrMagnitude > len * len;
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x001666C7 File Offset: 0x001648C7
		public static bool IsLongerThan(this Vector3 v, Vector3 v2)
		{
			return v.sqrMagnitude > v2.sqrMagnitude;
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x001666DC File Offset: 0x001648DC
		public static Vector3 GetClosestPoint(this Ray ray, Vector3 target)
		{
			float d = Vector3.Dot(target - ray.origin, ray.direction);
			return ray.origin + ray.direction * d;
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x0016671C File Offset: 0x0016491C
		public static float GetClosestDistSqr(this Ray ray, Vector3 target)
		{
			return (ray.GetClosestPoint(target) - target).sqrMagnitude;
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x00166740 File Offset: 0x00164940
		public static float GetClosestDistance(this Ray ray, Vector3 target)
		{
			return (ray.GetClosestPoint(target) - target).magnitude;
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x00166764 File Offset: 0x00164964
		public static Vector3 ProjectToPlane(this Ray ray, Vector3 planeOrigin, Vector3 planeNormalMustBeLength1)
		{
			Vector3 rhs = planeOrigin - ray.origin;
			float d = Vector3.Dot(planeNormalMustBeLength1, rhs);
			float d2 = Vector3.Dot(planeNormalMustBeLength1, ray.direction);
			return ray.origin + ray.direction * d / d2;
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x001667B4 File Offset: 0x001649B4
		public static Vector3 ProjectToLine(this Ray ray, Vector3 lineStart, Vector3 lineEnd)
		{
			Vector3 normalized = (lineEnd - lineStart).normalized;
			Vector3 normalized2 = Vector3.Cross(Vector3.Cross(ray.direction, normalized), normalized).normalized;
			return ray.ProjectToPlane(lineStart, normalized2);
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x001667F5 File Offset: 0x001649F5
		public static bool IsNull(this Object mono)
		{
			return mono == null || !mono;
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x00166805 File Offset: 0x00164A05
		public static bool IsNotNull(this Object mono)
		{
			return !mono.IsNull();
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x00166810 File Offset: 0x00164A10
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

		// Token: 0x06004A20 RID: 18976 RVA: 0x00166858 File Offset: 0x00164A58
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

		// Token: 0x06004A21 RID: 18977 RVA: 0x00166898 File Offset: 0x00164A98
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

		// Token: 0x06004A22 RID: 18978 RVA: 0x001668F8 File Offset: 0x00164AF8
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

		// Token: 0x06004A23 RID: 18979 RVA: 0x0016694C File Offset: 0x00164B4C
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

		// Token: 0x06004A24 RID: 18980 RVA: 0x001669A2 File Offset: 0x00164BA2
		public static string GetPath(this GameObject gameObject)
		{
			return gameObject.transform.GetPath();
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x001669AF File Offset: 0x00164BAF
		public static void GetPath(this GameObject gameObject, ref Utf16ValueStringBuilder sb)
		{
			gameObject.transform.GetPathQ(ref sb);
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x001669BD File Offset: 0x00164BBD
		public static string GetPath(this GameObject gameObject, int limit)
		{
			return gameObject.transform.GetPath(limit);
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x001669CC File Offset: 0x00164BCC
		public static string[] GetPaths(this GameObject[] gobj)
		{
			string[] array = new string[gobj.Length];
			for (int i = 0; i < gobj.Length; i++)
			{
				array[i] = gobj[i].GetPath();
			}
			return array;
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x001669FC File Offset: 0x00164BFC
		public static string[] GetPaths(this Transform[] xform)
		{
			string[] array = new string[xform.Length];
			for (int i = 0; i < xform.Length; i++)
			{
				array[i] = xform[i].GetPath();
			}
			return array;
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x00166A2C File Offset: 0x00164C2C
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

		// Token: 0x06004A2A RID: 18986 RVA: 0x00166B98 File Offset: 0x00164D98
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

		// Token: 0x06004A2B RID: 18987 RVA: 0x00166BE0 File Offset: 0x00164DE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetRelativePath(this Transform fromXform, Transform toXform, ref Utf16ValueStringBuilder ZStringBuilder)
		{
			GTExt.GetRelativePath(fromXform.GetPath(), toXform.GetPath(), ref ZStringBuilder);
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x00166BF4 File Offset: 0x00164DF4
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

		// Token: 0x06004A2D RID: 18989 RVA: 0x00166C3C File Offset: 0x00164E3C
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

		// Token: 0x06004A2E RID: 18990 RVA: 0x00166CA4 File Offset: 0x00164EA4
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

		// Token: 0x06004A2F RID: 18991 RVA: 0x00166CE4 File Offset: 0x00164EE4
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

		// Token: 0x06004A30 RID: 18992 RVA: 0x00166D24 File Offset: 0x00164F24
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

		// Token: 0x06004A31 RID: 18993 RVA: 0x00166DB0 File Offset: 0x00164FB0
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

		// Token: 0x06004A32 RID: 18994 RVA: 0x00166E44 File Offset: 0x00165044
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

		// Token: 0x06004A33 RID: 18995 RVA: 0x00166E84 File Offset: 0x00165084
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

		// Token: 0x06004A34 RID: 18996 RVA: 0x00166F60 File Offset: 0x00165160
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

		// Token: 0x06004A35 RID: 18997 RVA: 0x00166F90 File Offset: 0x00165190
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

		// Token: 0x06004A36 RID: 18998 RVA: 0x00166FD0 File Offset: 0x001651D0
		public static void GetPathWithSiblingIndexes(this GameObject gameObject, ref Utf16ValueStringBuilder stringBuilder)
		{
			gameObject.transform.GetPathWithSiblingIndexes(ref stringBuilder);
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x00166FDE File Offset: 0x001651DE
		public static string GetPathWithSiblingIndexes(this GameObject gameObject)
		{
			return gameObject.transform.GetPathWithSiblingIndexes();
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x00166FEB File Offset: 0x001651EB
		public static void AddDictValue(Transform xForm, Dictionary<string, Transform> dict)
		{
			GTExt.caseSenseInner.Add(xForm, dict);
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x00166FF9 File Offset: 0x001651F9
		public static void ClearDicts()
		{
			GTExt.caseSenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();
			GTExt.caseInsenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x00167010 File Offset: 0x00165210
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

		// Token: 0x06004A3B RID: 19003 RVA: 0x0016709C File Offset: 0x0016529C
		public static bool TryFindByExactPath(this Scene scene, string path, out Transform result)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("TryFindByExactPath: Provided path cannot be null or empty.");
			}
			string[] splitPath = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			return scene.TryFindByExactPath(splitPath, out result);
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x001670D0 File Offset: 0x001652D0
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

		// Token: 0x06004A3D RID: 19005 RVA: 0x0016710C File Offset: 0x0016530C
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

		// Token: 0x06004A3E RID: 19006 RVA: 0x0016718C File Offset: 0x0016538C
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

		// Token: 0x06004A3F RID: 19007 RVA: 0x001671EC File Offset: 0x001653EC
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

		// Token: 0x06004A40 RID: 19008 RVA: 0x00167278 File Offset: 0x00165478
		public static bool TryFindByPath(string globPath, out Transform result, bool caseSensitive = false)
		{
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return GTExt._TryFindByPath(null, pathPartsRegex, -1, out result, caseSensitive, true, globPath);
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x00167298 File Offset: 0x00165498
		public static bool TryFindByPath(this Scene scene, string globPath, out Transform result, bool caseSensitive = false)
		{
			if (string.IsNullOrEmpty(globPath))
			{
				throw new Exception("TryFindByPath: Provided path cannot be null or empty.");
			}
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return scene.TryFindByPath(pathPartsRegex, out result, globPath, caseSensitive);
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x001672CC File Offset: 0x001654CC
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

		// Token: 0x06004A43 RID: 19011 RVA: 0x0016730C File Offset: 0x0016550C
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

		// Token: 0x06004A44 RID: 19012 RVA: 0x0016738F File Offset: 0x0016558F
		public static List<string> ShowAllStringsUsed()
		{
			return GTExt.allStringsUsed.Keys.ToList<string>();
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x001673A0 File Offset: 0x001655A0
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

		// Token: 0x06004A46 RID: 19014 RVA: 0x00167CFC File Offset: 0x00165EFC
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

		// Token: 0x06004A47 RID: 19015 RVA: 0x00167DD0 File Offset: 0x00165FD0
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

		// Token: 0x06004A48 RID: 19016 RVA: 0x00167E44 File Offset: 0x00166044
		public static T[] FindComponentsByExactPath<T>(this Scene scene, string path) where T : Component
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("FindComponentsByExactPath: Provided path cannot be null or empty.");
			}
			string[] splitPath = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			return scene.FindComponentsByExactPath(splitPath);
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x00167E78 File Offset: 0x00166078
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

		// Token: 0x06004A4A RID: 19018 RVA: 0x00167EE8 File Offset: 0x001660E8
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

		// Token: 0x06004A4B RID: 19019 RVA: 0x00167F94 File Offset: 0x00166194
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

		// Token: 0x06004A4C RID: 19020 RVA: 0x00168020 File Offset: 0x00166220
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

		// Token: 0x06004A4D RID: 19021 RVA: 0x001680AC File Offset: 0x001662AC
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

		// Token: 0x06004A4E RID: 19022 RVA: 0x0016814C File Offset: 0x0016634C
		public static T[] FindComponentsByPath<T>(this Scene scene, string globPath, bool caseSensitive = false) where T : Component
		{
			if (string.IsNullOrEmpty(globPath))
			{
				throw new Exception("FindComponentsByPath: Provided path cannot be null or empty.");
			}
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return scene.FindComponentsByPath(pathPartsRegex, caseSensitive);
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x0016817C File Offset: 0x0016637C
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

		// Token: 0x06004A50 RID: 19024 RVA: 0x001681EC File Offset: 0x001663EC
		public static T[] FindComponentsByPath<T>(this Transform rootXform, string globPath, bool caseSensitive = false) where T : Component
		{
			if (string.IsNullOrEmpty(globPath))
			{
				throw new Exception("FindComponentsByPath: Provided path cannot be null or empty.");
			}
			string[] pathPartsRegex = GTExt._GlobPathToPathPartsRegex(globPath);
			return rootXform.FindComponentsByPath(pathPartsRegex, caseSensitive);
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x0016821C File Offset: 0x0016641C
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

		// Token: 0x06004A52 RID: 19026 RVA: 0x0016826C File Offset: 0x0016646C
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

		// Token: 0x06004A53 RID: 19027 RVA: 0x001682E0 File Offset: 0x001664E0
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

		// Token: 0x06004A54 RID: 19028 RVA: 0x001685C8 File Offset: 0x001667C8
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

		// Token: 0x06004A55 RID: 19029 RVA: 0x00168688 File Offset: 0x00166888
		private static string _GlobPathPartToRegex(string pattern)
		{
			if (pattern == "." || pattern == ".." || pattern == "**" || pattern == "..**" || pattern == "**.." || pattern.StartsWith("^"))
			{
				return pattern;
			}
			return "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0016872C File Offset: 0x0016692C
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

		// Token: 0x06004A58 RID: 19032 RVA: 0x001687C8 File Offset: 0x001669C8
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

		// Token: 0x06004A59 RID: 19033 RVA: 0x00168878 File Offset: 0x00166A78
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

		// Token: 0x04004BE1 RID: 19425
		private static Dictionary<Transform, Dictionary<string, Transform>> caseSenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();

		// Token: 0x04004BE2 RID: 19426
		private static Dictionary<Transform, Dictionary<string, Transform>> caseInsenseInner = new Dictionary<Transform, Dictionary<string, Transform>>();

		// Token: 0x04004BE3 RID: 19427
		public static Dictionary<string, string> allStringsUsed = new Dictionary<string, string>();

		// Token: 0x02000B73 RID: 2931
		public enum ParityOptions
		{
			// Token: 0x04004BE5 RID: 19429
			XFlip,
			// Token: 0x04004BE6 RID: 19430
			YFlip,
			// Token: 0x04004BE7 RID: 19431
			ZFlip,
			// Token: 0x04004BE8 RID: 19432
			AllFlip
		}
	}
}
