using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Condition
{
	[SerializeField]
	Disjunction[] and;

	public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
	{
		foreach (Disjunction dis in and)
		{
			if (!dis.Check(evaluators))
			{
				return false;
			}
		}
		return true;
	}

	class Disjunction
	{
		[SerializeField]
		Predicate[] or;

		public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
		{
			foreach (Predicate pred in or)
			{
				if (pred.Check(evaluators))
				{
					return true;
				}
			}
			return false;
		}
	}

	class Predicate
	{
		[SerializeField] string predicate;
		[SerializeField] string[] parameters;
		[SerializeField] bool negate = false;

		public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
		{
			foreach (var evaluator in evaluators)
			{
				bool? result = evaluator.Evaluate(predicate, parameters);

				if (result == null) continue;

				if (result == negate) return false;
			}

			return true;
		}
	}

	public interface IPredicateEvaluator
	{
		bool? Evaluate(string predicate, string[] parameters);
	}
}