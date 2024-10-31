using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour
{
    public GameObject followingPointsParent; // 부모 오브젝트
    public float moveSpeed = 5f;

    private Transform[] followingPoints;
    private int currentIndex = 0;
    private float t = 0; // 곡선 보간 값

    private void Start()
    {
        // 부모 오브젝트의 모든 자식을 배열에 저장
        followingPoints = new Transform[followingPointsParent.transform.childCount];
        for (int i = 0; i < followingPoints.Length; i++)
        {
            followingPoints[i] = followingPointsParent.transform.GetChild(i);
        }

        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        while (currentIndex < followingPoints.Length - 1)
        {
            // Catmull-Rom 스플라인을 위한 4개의 포인트 가져오기
            Vector3 p0 = GetPoint(currentIndex - 1);
            Vector3 p1 = GetPoint(currentIndex);
            Vector3 p2 = GetPoint(currentIndex + 1);
            Vector3 p3 = GetPoint(currentIndex + 2);

            // t 값이 1에 도달할 때까지 곡선을 따라 이동
            while (t < 1f)
            {
                t += moveSpeed * Time.deltaTime;

                // Catmull-Rom 곡선을 따라 이동
                Vector3 newPosition = CatmullRom(p0, p1, p2, p3, t);
                transform.position = newPosition;

                // 이동 방향에 따라 회전
                Vector3 direction = (newPosition - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
                }

                yield return null;
            }

            // 다음 구간으로 이동
            t = 0;
            currentIndex++;
        }
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