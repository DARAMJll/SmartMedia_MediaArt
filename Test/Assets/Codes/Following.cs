using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Following : MonoBehaviour
{
	public GameObject[] followingPointsPrefabs; 
	public float moveSpeed = 0.25f;
	public float finalPathSpeed = 0.1f;
	private Transform[] followingPoints;
	private int currentIndex = 0;
	private float t = 0; // 곡선 보간 값

	// 마지막으로 사용된 경로의 인덱스를 저장
	private static int lastUsedPathIndex = -1;

	private void Start()
	{
		if (followingPointsPrefabs != null && followingPointsPrefabs.Length > 0)
		{
			// 이전 경로를 제외한 랜덤 선택
			int randomIndex;
			if (followingPointsPrefabs.Length > 1)
			{
				do
				{
					randomIndex = Random.Range(0, followingPointsPrefabs.Length);
				} while (randomIndex == lastUsedPathIndex);
			}
			else
			{
				randomIndex = 0;
			}

			// 선택된 인덱스 저장
			lastUsedPathIndex = randomIndex;

			// 선택된 경로로 초기화
			GameObject selectedPath = followingPointsPrefabs[randomIndex];
			followingPoints = new Transform[selectedPath.transform.childCount];
			for (int i = 0; i < followingPoints.Length; i++)
			{
				followingPoints[i] = selectedPath.transform.GetChild(i);
			}

			StartCoroutine(FollowPath());
		}
	}

	private IEnumerator FollowPath()
	{
		while (currentIndex < followingPoints.Length - 1)
		{
			Vector3 p0 = GetPoint(currentIndex - 1);
			Vector3 p1 = GetPoint(currentIndex);
			Vector3 p2 = GetPoint(currentIndex + 1);
			Vector3 p3 = GetPoint(currentIndex + 2);

			while (t < 1f)
			{
				// 포인트 9부터 12까지는 느린 속도 적용
				float currentSpeed = (currentIndex >= 8 && currentIndex <= 12) ? finalPathSpeed : moveSpeed;
				t += currentSpeed * Time.deltaTime;

				Vector3 newPosition = CatmullRom(p0, p1, p2, p3, t);
				transform.position = newPosition;

				Vector3 direction = (newPosition - transform.position).normalized;
				if (direction != Vector3.zero)
				{
					Quaternion targetRotation = Quaternion.LookRotation(direction);
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentSpeed * Time.deltaTime);
				}
				yield return null;
			}

			t = 0;
			currentIndex++;
		}

		// 마지막 포인트에 도달했을 때 객체 삭제
		Debug.Log($"{gameObject.name} reached final point and was destroyed");
		Destroy(gameObject);
	}


	// Catmull-Rom 스플라인 공식
	private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;
		return 0.5f * (
			(2 * p1) +
			(-p0 + p2) * t +
			(2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
			(-p0 + 3 * p1 - 3 * p2 + p3) * t3
		);
	}
	// 경로가 끝나면 끝 포인트를 계속 참조하여 오류 방지
	private Vector3 GetPoint(int index)
	{
		if (index < 0)
			return followingPoints[0].position;
		if (index >= followingPoints.Length)
			return followingPoints[followingPoints.Length - 1].position;
		return followingPoints[index].position;
	}
}