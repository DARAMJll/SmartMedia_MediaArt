using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Xml;

using UnityEngine.Rendering;
using UnityEditor;

//단기예보조회

public class Weather : MonoBehaviour
{

	[Header("Skybox Materials")]
	public Material sunsetClearSkybox;      // Cartoon Base NightSky
	public Material sunsetCloudySkybox;     // Deep Dusk
	public Material nightClearSkybox;       // Cold Night
	public Material nightCloudySkybox;      // AllSky_Night_MoonBurst
	public Material dayClearSkybox;         // Day_BlueSky_Nothing
	public Material dayPartlyCloudySkybox;  // Epic_BlueSunset
	public Material dayCloudySkybox;        // Epic_GloriousPink

	private string currentSkyCondition = ""; // 현재 하늘 상태 저장

	[Header("Test Mode")]
	public bool isTestMode = false;  // 테스트 모드 활성화 여부
	[Range(0, 23)]
	public int testHour = 0;        // 테스트할 시간
	public enum WeatherCondition { 맑음, 구름많음, 흐림 }
	public WeatherCondition testWeather = WeatherCondition.맑음;  // 테스트할 날씨
	public bool applyTest = false;   // 테스트 설정 적용 버튼 (Inspector에서 체크박스로 표시됨)



	int Time;   //현재 시간 (HH)    base_Time 구하는 함수에 사용됨
    string base_date;    //현재 날짜 yyyy.MM.dd     나중에 url에 사용됨 

    //base_time : 0200, 0500, 0800, 1100, 1400, 1700, 2000, 2300 (1일 8회)
    string base_time;    //HH00 ex) 1300 1700

    string url;     //공공데이터포털 url 저장 -> Debug 확인해보면 get_Time 넣은 시간대의 item만 받아옴 (신기함 ㄹㅇ)

    string get_Time;    //base_time 구하는 switch안에 넣어서 정확히 어떤 시간대 정보값을 받아올지 정함

    //예외 만약 24시 1시 2시의 경우엔 다음 날 업데이트 되는 날짜로 받아올 수 없기 때문에 전날 2300으로 받아서 확인해야한다.
    //ex) 2022.06.06 01시 -> 2시에 업데이트 따라서 2022.06.05 23시 업데이트 정보로 01시의 기상정보를 얻어야함. 
    DateTime exception_base_time = DateTime.Now.AddDays(-1);

    private void Awake()
    {
        Time = int.Parse(DateTime.Now.ToString("HH"));
        base_date = DateTime.Now.ToString("yyyyMMdd");
    }

    private void Start()
    {
        Get_Base_Time(Time);
        Get_Get_Time(Time);
        Geturl();

        StartCoroutine(LoadData());
		//UpdateSkybox(); // 시작할 때 하늘 배경 업데이트

		Debug.Log("Time:" + Time + " base_Date:" + base_date + " base_Time:" + base_time);
        Debug.Log("getTime:" + get_Time);
    }

	void Update()
	{
		if (isTestMode && applyTest)
		{
			ApplyTestSettings();
			applyTest = false;  // 자동으로 체크 해제
		}
	}

	private void ApplyTestSettings()
	{
		if (isTestMode)
		{
			get_Time = $"{testHour:D2}00";
			UpdateSkybox(testWeather.ToString());
			Debug.Log($"Test settings applied - Time: {testHour}:00, Weather: {testWeather}");
		}
	}

	// Unity Inspector에서 스카이박스 머티리얼 할당을 위한 OnValidate
	private void OnValidate()
	{
		sunsetClearSkybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/AllSkyFree/Cartoon Base NightSky/Cartoon Base NightSky.mat");
		sunsetCloudySkybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/AllSkyFree/Deep Dusk/Deep Dusk.mat");
		nightClearSkybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/AllSkyFree/Cold Night/Cold Night.mat");
		nightCloudySkybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/AllSkyFree/Night MoonBurst/AllSky_Night_MoonBurst Equirect.mat");
		dayClearSkybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/AllSkyFree/Cartoon Base BlueSky/Day_BlueSky_Nothing.mat");
		dayPartlyCloudySkybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/AllSkyFree/Epic_BlueSunset/Epic_BlueSunset.mat");
		dayCloudySkybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/AllSkyFree/Epic_GloriousPink/Epic_GloriousPink.mat");
	}

	

	//base_Time 구해주는 함수 추후 url 구할때 쓰임
	//base_time : 0200, 0500, 0800, 1100, 1400, 1700, 2000, 2300 (1일 8회) 따라서 스위치도 해당 방법으로 저장 
	// 02 -> 03, 05 -> 06 이런식으로 base_time보다 1시간 앞에것 나옴 
	public void Get_Base_Time(int time)
    {
        switch (time)
        {
            case 3:
            case 4:
            case 5:
                base_time = "0200";
                break;
            case 6:
            case 7:
            case 8:
                base_time = "0500";
                break;
            case 9:
            case 10:
            case 11:
                base_time = "0800";
                break;
            case 12:
            case 13:
            case 14:
                base_time = "1100";
                break;
            case 15:
            case 16:
            case 17:
                base_time = "1400";
                break;
            case 18:
            case 19:
            case 20:
                base_time = "1700";
                break;
            case 21:
            case 22:
            case 23:

                base_time = "2000";
                break;
            case 24:
            case 1:
            case 2:
                base_time = "2300";
                base_date = exception_base_time.ToString("yyyyMMdd");
                break;
            default:
                base_time = "2300";
                break;
        }
    }

    public void Get_Get_Time(int time)
    {
        switch (time)
        {
            case 1:
                get_Time = "0100";
                break;
            case 2:
                get_Time = "0200";
                break;
            case 3:
                get_Time = "0300";
                break;
            case 4:
                get_Time = "0400";
                break;
            case 5:
                get_Time = "0500";
                break;
            case 6:
                get_Time = "0600";
                break;
            case 7:
                get_Time = "0700";
                break;
            case 8:
                get_Time = "0800";
                break;
            case 9:
                get_Time = "0900";
                break;
            case 10:
                get_Time = "1000";
                break;
            case 11:
                get_Time = "1100";
                break;
            case 12:
                get_Time = "1200";
                break;
            case 13:
                get_Time = "1300";
                break;
            case 14:
                get_Time = "1400";
                break;
            case 15:
                get_Time = "1500";
                break;
            case 16:
                get_Time = "1600";
                break;
            case 17:
                get_Time = "1700";
                break;
            case 18:
                get_Time = "1800";
                break;
            case 19:
                get_Time = "1900";
                break;
            case 20:
                get_Time = "2000";
                break;
            case 21:
                get_Time = "2100";
                break;
            case 22:
                get_Time = "2200";
                break;
            case 23:
                get_Time = "2300";
                break;
            case 24:
                get_Time = "2400";
                break;
            default:
                get_Time = "2300";
                break;
        }

    }

    

    public void Geturl()
    {
        url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // https를 http로 변경
        url += "?ServiceKey=" + "XtDPA1fEnePF0FZU5BhnMkykFdyFDEJtZh%2FznJJ9AVBbXVUte9F2dylUfh6Dg86e9jQzf%2BbDCAooHg4GEOkKCA%3D%3D"; // API 키 입력
        url += "&numOfRows=36";
        url += "&pageNo=1";
        url += "&dataType=XML";
        url += "&base_date=" + base_date;
        url += "&base_time=" + base_time;
        url += "&nx=55"; //나중에 학교 좌표로 수정하기
        url += "&ny=127";

        //Debug.Log(url);
    }

    

    IEnumerator LoadData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
                yield break;
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(www.downloadHandler.text);

            XmlNodeList itemList = xml.GetElementsByTagName("item");

            foreach (XmlNode item in itemList)
            {
                string category = item["category"].InnerText;
                string fcstTime = item["fcstTime"].InnerText;
                string fcstValue = item["fcstValue"].InnerText;

                if (fcstTime == get_Time)
                {
                    switch (category)
                    {
                        case "TMP":
                            Debug.Log($"현재 기온: {fcstValue}°C");
                            break;
                        case "SKY":
                            string skyCondition = fcstValue switch
                            {
                                "1" => "맑음",
                                "3" => "구름많음",
                                "4" => "흐림",
                                _ => "알 수 없음"
                            };
							UpdateSkybox(skyCondition); // 하늘 상태에 따라 스카이박스 업데이트
							Debug.Log($"하늘 상태: {skyCondition}");
                            break;
                        case "PTY":
                            string precipitationType = fcstValue switch
                            {
                                "0" => "없음",
                                "1" => "비",
                                "2" => "비/눈",
                                "3" => "눈",
                                "4" => "소나기",
                                _ => "알 수 없음"
                            };
                            Debug.Log($"강수 형태: {precipitationType}");
                            break;
                    }
                }
            }
        }
    }

	private void UpdateSkybox(string skyCondition)
	{
		currentSkyCondition = skyCondition;
		int currentHour = int.Parse(get_Time.Substring(0, 2));
		Material selectedSkybox = nightCloudySkybox; // 기본값

		// 시간대와 날씨 조건에 따라 스카이박스 선택
		if ((currentHour >= 17 && currentHour <= 20) || (currentHour >= 6 && currentHour <= 8))
		{
			// 일몰/일출 시간대
			if (skyCondition == "맑음")
			{
				selectedSkybox = sunsetClearSkybox;
			}
			else // "구름많음" 또는 "흐림"
			{
				selectedSkybox = sunsetCloudySkybox;
			}
		}
		else if (currentHour >= 21 || currentHour <= 5)
		{
			// 밤 시간대
			if (skyCondition == "맑음")
			{
				selectedSkybox = nightClearSkybox;
			}
			else // "구름많음" 또는 "흐림"
			{
				selectedSkybox = nightCloudySkybox;
			}
		}
		else if (currentHour >= 7 && currentHour <= 16)
		{
			// 낮 시간대
			if (skyCondition == "맑음")
			{
				selectedSkybox = dayClearSkybox;
			}
			else if (skyCondition == "구름많음")
			{
				selectedSkybox = dayPartlyCloudySkybox;
			}
			else // "흐림"
			{
				selectedSkybox = dayCloudySkybox;
			}
		}

		// 스카이박스 적용
		RenderSettings.skybox = selectedSkybox;
		DynamicGI.UpdateEnvironment();
		Debug.Log($"Changed skybox based on time: {currentHour}:00 and condition: {skyCondition}");
	}
	//            else
	//            {
	//                string apiErrorMsg = String.Empty;

	//                // API 응답 에러 메세지 조사
	//                apiErrorMsg = node["header"]["resultMsg"].InnerText switch
	//                {
	//                    "APPLICATION_ERROR" => "어플리케이션 에러",
	//                    "DB_ERROR" => "데이터베이스 에러",
	//                    "NODATA_ERROR" => "데이터 없음",
	//                    "HTTP_ERROR" => "HTTP 에러",
	//                    "SERVICETIME_OUT" => "서비스 연결실패",
	//                    "INVALID_REQUEST_PARAMETER_ERROR" => "잘못된 요청 파라메터",
	//                    "NO_MANDATORY_REQUEST_PARAMETERS_ERROR" => "필수요청 파라메터가 없음",
	//                    "NO_OPENAPI_SERVICE_ERROR" => "해당 오픈 API서비스가 없거나 폐기됨",
	//                    "SERVICE_ACCESS_DENIED_ERROR" => "서비스 접근 거부",
	//                    "TEMPORARILY_DISABLE_THE_SERVICEKEY_ERROR" => "일시적으로 사용할 수 없는 서비스 키",
	//                    "LIMITED_NUMBER_OF_SERVICE_REQUESTS_EXCEEDS_ERROR" => "서비스 요청제한횟수 초과",
	//                    "SERVICE_KEY_IS_NOT_REGISTERED_ERROR" => "등록되지 않은 서비스 키",
	//                    "DEADLINE_HAS_EXPIRED_ERROR" => "기한 만료된 서비스 키",
	//                    "UNREGISTERED_IP_ERROR" => "등록되지 않은 IP",
	//                    "UNSIGNED_CALL_ERROR" => "서명되지 않은 호출",
	//                    "UNKNOWN_ERROR" => "기타에러",
	//                    _ => "해당하는 에러가 존재하지 않음",
	//                };

	//                // API 응답 에러 메세지 출력
	//                Debug.Log(apiErrorMsg);
	//            }
	//        }
	//    }
}