using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
	[SerializeField] private GameObject historyRecord;
	public static Dictionary<string, Tuple<int, bool>> History;

	private void Awake()
	{
		StartCoroutine(LoadHistory());
	}

	private IEnumerator LoadHistory()
	{
		StartCoroutine(RealtimeDatabase.ExportHistory());
		yield return new WaitUntil(() => RealtimeDatabase.HistoryLoaded);

		foreach (var pair in History)
		{
			var record = Instantiate(historyRecord, transform).GetComponent<HistoryRecord>();
			record.SetRecord(pair.Key, pair.Value.Item1, pair.Value.Item2);
		}
	}
}