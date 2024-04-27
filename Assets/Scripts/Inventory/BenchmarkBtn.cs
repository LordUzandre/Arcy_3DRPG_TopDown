using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Arcy.Utils
{
	public class BenchmarkBtn : MonoBehaviour
	{
		public string BenchmarkName { get; protected set; }
		public int MaxRecommendedIterations { get; protected set; } = int.MaxValue;
		protected readonly List<BenchmarkData> Benchmarks = new List<BenchmarkData>();

		public IEnumerable<BenchmarkData> RunBenchmark(int iterations)
		{
			if (Benchmarks == null)
			{
				Debug.LogError("No benchmarks found");
			}

			foreach (var benchmark in Benchmarks)
			{
				yield return Run(benchmark);
			}

			BenchmarkData Run(BenchmarkData data)
			{
				Stopwatch watch = Stopwatch.StartNew();

				for (var i = 0; i < iterations; i++)
				{
					data.Action();
				}

				watch.Stop();
				data.Result = watch.ElapsedMilliseconds;
				return data;
			}
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(BenchmarkBtn))]
	public class BenchmarkEditor : Editor
	{
		int iterations;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			BenchmarkBtn myThingue = (BenchmarkBtn)target;

			iterations = EditorGUILayout.IntSlider(iterations, 1, 100);

			if (GUILayout.Button("In Editor Btn"))
			{
				myThingue.RunBenchmark(iterations);
			}
		}
	}

	public struct BenchmarkData
	{
		public readonly string Name;
		public readonly Action Action;
		public long Result;

		public BenchmarkData(string name, Action action)
		{
			Name = name;
			Action = action;
			Result = 0;
		}
#endif
	}
}
