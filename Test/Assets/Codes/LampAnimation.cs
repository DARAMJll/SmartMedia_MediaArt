using UnityEngine;

public class LampAnimation : MonoBehaviour
{

    public float minAmplitude = 0.1f;
    public float maxAmplitude = 0.5f;
    public float minFrequency = 0.5f;
    public float maxFrequency = 2f;




    // 랜덤설정할거니?
    public bool randomPosition = true;
    // 움직임의 강도
    public float amplitude = 0.5f; 
    // 움직임의 속도
    public float frequency = 1f; 

    // 시작 위치 저장
    private Vector3 startPosition;

    void Start()
    {
        if(randomPosition == true){
            amplitude = Random.Range(minAmplitude, maxAmplitude);
            frequency = Random.Range(minFrequency, maxFrequency);
        }
        // 초기 위치 저장
        startPosition = transform.position;
    }

    void Update()
    {
        // 시간에 따라 진동 효과 계산
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        float offsetX = Mathf.Cos(Time.time * frequency * 0.5f) * amplitude;

        // 오브젝트 위치 조정 (최종적으로 startPosition 기준)
        transform.position = startPosition + new Vector3(offsetX, offsetY, 0f);
    }
}
