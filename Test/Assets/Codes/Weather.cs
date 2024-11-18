using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Xml;

using UnityEngine.Rendering;
using UnityEditor;

//�ܱ⿹����ȸ

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

	private string currentSkyCondition = ""; // ���� �ϴ� ���� ����

	[Header("Test Mode")]
	public bool isTestMode = false;  // �׽�Ʈ ��� Ȱ��ȭ ����
	[Range(0, 23)]
	public int testHour = 0;        // �׽�Ʈ�� �ð�
	public enum WeatherCondition { ����, ��������, �帲 }
	public WeatherCondition testWeather = WeatherCondition.����;  // �׽�Ʈ�� ����
	public bool applyTest = false;   // �׽�Ʈ ���� ���� ��ư (Inspector���� üũ�ڽ��� ǥ�õ�)



	int Time;   //���� �ð� (HH)    base_Time ���ϴ� �Լ��� ����
    string base_date;    //���� ��¥ yyyy.MM.dd     ���߿� url�� ���� 

    //base_time : 0200, 0500, 0800, 1100, 1400, 1700, 2000, 2300 (1�� 8ȸ)
    string base_time;    //HH00 ex) 1300 1700

    string url;     //�������������� url ���� -> Debug Ȯ���غ��� get_Time ���� �ð����� item�� �޾ƿ� (�ű��� ����)

    string get_Time;    //base_time ���ϴ� switch�ȿ� �־ ��Ȯ�� � �ð��� �������� �޾ƿ��� ����

    //���� ���� 24�� 1�� 2���� ��쿣 ���� �� ������Ʈ �Ǵ� ��¥�� �޾ƿ� �� ���� ������ ���� 2300���� �޾Ƽ� Ȯ���ؾ��Ѵ�.
    //ex) 2022.06.06 01�� -> 2�ÿ� ������Ʈ ���� 2022.06.05 23�� ������Ʈ ������ 01���� ��������� ������. 
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
		//UpdateSkybox(); // ������ �� �ϴ� ��� ������Ʈ

		Debug.Log("Time:" + Time + " base_Date:" + base_date + " base_Time:" + base_time);
        Debug.Log("getTime:" + get_Time);
    }

	void Update()
	{
		if (isTestMode && applyTest)
		{
			ApplyTestSettings();
			applyTest = false;  // �ڵ����� üũ ����
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

	// Unity Inspector���� ��ī�̹ڽ� ��Ƽ���� �Ҵ��� ���� OnValidate
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

	

	//base_Time �����ִ� �Լ� ���� url ���Ҷ� ����
	//base_time : 0200, 0500, 0800, 1100, 1400, 1700, 2000, 2300 (1�� 8ȸ) ���� ����ġ�� �ش� ������� ���� 
	// 02 -> 03, 05 -> 06 �̷������� base_time���� 1�ð� �տ��� ���� 
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
        url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // https�� http�� ����
        url += "?ServiceKey=" + "XtDPA1fEnePF0FZU5BhnMkykFdyFDEJtZh%2FznJJ9AVBbXVUte9F2dylUfh6Dg86e9jQzf%2BbDCAooHg4GEOkKCA%3D%3D"; // API Ű �Է�
        url += "&numOfRows=36";
        url += "&pageNo=1";
        url += "&dataType=XML";
        url += "&base_date=" + base_date;
        url += "&base_time=" + base_time;
        url += "&nx=55"; //���߿� �б� ��ǥ�� �����ϱ�
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
                            Debug.Log($"���� ���: {fcstValue}��C");
                            break;
                        case "SKY":
                            string skyCondition = fcstValue switch
                            {
                                "1" => "����",
                                "3" => "��������",
                                "4" => "�帲",
                                _ => "�� �� ����"
                            };
							UpdateSkybox(skyCondition); // �ϴ� ���¿� ���� ��ī�̹ڽ� ������Ʈ
							Debug.Log($"�ϴ� ����: {skyCondition}");
                            break;
                        case "PTY":
                            string precipitationType = fcstValue switch
                            {
                                "0" => "����",
                                "1" => "��",
                                "2" => "��/��",
                                "3" => "��",
                                "4" => "�ҳ���",
                                _ => "�� �� ����"
                            };
                            Debug.Log($"���� ����: {precipitationType}");
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
		Material selectedSkybox = nightCloudySkybox; // �⺻��

		// �ð���� ���� ���ǿ� ���� ��ī�̹ڽ� ����
		if ((currentHour >= 17 && currentHour <= 20) || (currentHour >= 6 && currentHour <= 8))
		{
			// �ϸ�/���� �ð���
			if (skyCondition == "����")
			{
				selectedSkybox = sunsetClearSkybox;
			}
			else // "��������" �Ǵ� "�帲"
			{
				selectedSkybox = sunsetCloudySkybox;
			}
		}
		else if (currentHour >= 21 || currentHour <= 5)
		{
			// �� �ð���
			if (skyCondition == "����")
			{
				selectedSkybox = nightClearSkybox;
			}
			else // "��������" �Ǵ� "�帲"
			{
				selectedSkybox = nightCloudySkybox;
			}
		}
		else if (currentHour >= 7 && currentHour <= 16)
		{
			// �� �ð���
			if (skyCondition == "����")
			{
				selectedSkybox = dayClearSkybox;
			}
			else if (skyCondition == "��������")
			{
				selectedSkybox = dayPartlyCloudySkybox;
			}
			else // "�帲"
			{
				selectedSkybox = dayCloudySkybox;
			}
		}

		// ��ī�̹ڽ� ����
		RenderSettings.skybox = selectedSkybox;
		DynamicGI.UpdateEnvironment();
		Debug.Log($"Changed skybox based on time: {currentHour}:00 and condition: {skyCondition}");
	}
	//            else
	//            {
	//                string apiErrorMsg = String.Empty;

	//                // API ���� ���� �޼��� ����
	//                apiErrorMsg = node["header"]["resultMsg"].InnerText switch
	//                {
	//                    "APPLICATION_ERROR" => "���ø����̼� ����",
	//                    "DB_ERROR" => "�����ͺ��̽� ����",
	//                    "NODATA_ERROR" => "������ ����",
	//                    "HTTP_ERROR" => "HTTP ����",
	//                    "SERVICETIME_OUT" => "���� �������",
	//                    "INVALID_REQUEST_PARAMETER_ERROR" => "�߸��� ��û �Ķ����",
	//                    "NO_MANDATORY_REQUEST_PARAMETERS_ERROR" => "�ʼ���û �Ķ���Ͱ� ����",
	//                    "NO_OPENAPI_SERVICE_ERROR" => "�ش� ���� API���񽺰� ���ų� ����",
	//                    "SERVICE_ACCESS_DENIED_ERROR" => "���� ���� �ź�",
	//                    "TEMPORARILY_DISABLE_THE_SERVICEKEY_ERROR" => "�Ͻ������� ����� �� ���� ���� Ű",
	//                    "LIMITED_NUMBER_OF_SERVICE_REQUESTS_EXCEEDS_ERROR" => "���� ��û����Ƚ�� �ʰ�",
	//                    "SERVICE_KEY_IS_NOT_REGISTERED_ERROR" => "��ϵ��� ���� ���� Ű",
	//                    "DEADLINE_HAS_EXPIRED_ERROR" => "���� ����� ���� Ű",
	//                    "UNREGISTERED_IP_ERROR" => "��ϵ��� ���� IP",
	//                    "UNSIGNED_CALL_ERROR" => "������� ���� ȣ��",
	//                    "UNKNOWN_ERROR" => "��Ÿ����",
	//                    _ => "�ش��ϴ� ������ �������� ����",
	//                };

	//                // API ���� ���� �޼��� ���
	//                Debug.Log(apiErrorMsg);
	//            }
	//        }
	//    }
}