using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Arcy.Utils
{
	public class BenchmarkBtn : MonoBehaviour
	{
		public string BenchmarkName { get; protected set; }
		public int MaxRecommendedIterations { get; protected set; } = int.MaxValue;
		protected readonly List<BenchmarkData> Benchmarks = new List<BenchmarkData>();

		public IEnumerable<BenchmarkData> RunBenchmark(int iterations)
		{
			foreach (var benchmark in Benchmarks)
			{
				yield return Run(benchmark);
			}

			BenchmarkData Run(BenchmarkData benchmarkData)
			{
				Stopwatch watch = Stopwatch.StartNew();

				for (var i = 0; i < iterations; i++)
				{
					benchmarkData.Action();
				}

				watch.Stop();
				benchmarkData.Result = watch.ElapsedMilliseconds;
				Debug.Log(benchmarkData.Result);
				return benchmarkData;
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
			GUILayout.Space(20);

			BenchmarkBtn myThingue = (BenchmarkBtn)target;

			iterations = EditorGUILayout.IntSlider("Iterations", iterations, 1, 1000);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("In Editor Btn", GUILayout.MinWidth(240), GUILayout.MinHeight(80)))
			{
				// myThingue.RunBenchmark(iterations);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
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
