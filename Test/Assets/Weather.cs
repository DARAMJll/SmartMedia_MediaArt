using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Xml;

//단기예보조회

public class Weather : MonoBehaviour
{
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

        Debug.Log("Time:" + Time + " base_Date:" + base_date + " base_Time:" + base_time);
        Debug.Log("getTime:" + get_Time);
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

    //url 구하는 함수 

    //public void Geturl()
    //{
    //    url = "https://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst";
    //    url += "?ServiceKey=" + "XtDPA1fEnePF0FZU5BhnMkykFdyFDEJtZh%2FznJJ9AVBbXVUte9F2dylUfh6Dg86e9jQzf%2BbDCAooHg4GEOkKCA%3D%3D"; //api 인증 키
    //    //한 시간대에 있는 아이템값은 총 12개임 1100이면 총 12개가 있음 base_time 구할 때 switch보면 최대 3개씩 가져오니까 36개로 변경
    //    url += "&numOfRows=36";             // 한페이지 결과 수(Default : 12)  //★ 이 부분 36로 변경  
    //    url += "&pageNo=1";                 // 페이지 번호(Default : 1)
    //    url += "&dataType=XML";             // 받을 자료형식(XML, JSON)
    //                                        //url += "&ftype=ODAM";
    //    url += "&base_date=" + base_date;   // 요청 날짜(yyMMdd)
    //    url += "&base_time=" + base_time;   // 요청 시간(HHmm)
    //    url += "&nx=55";                    // 요청 지역 x좌표
    //    url += "&ny=127";                   // 요청 지역 y좌료

    //    Debug.Log(url);
    //}

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

    //url 안에 데이터 불러오기 성공 
    //IEnumerator LoadData()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(url);

    //    yield return www.SendWebRequest();
    //    if (www.error == null)   //불러오기 성공
    //    {
    //        //정보값들 출력 
    //        Debug.Log(www.downloadHandler.text);
    //    }

    //    XmlDocument xml = new XmlDocument();
    //    xml.Load(url);
    //    XmlNodeList xmResponse = xml.GetElementsByTagName("response");  //response 기준으로 생성
    //    XmlNodeList xmlList = xml.GetElementsByTagName("item");

    //    //XML 파일 뜯어서 정보를 불러옴 
    //    foreach (XmlNode node in xmResponse)
    //    {
    //        if (node["header"]["resultMsg"].InnerText.Equals("NORMAL_SERVICE")) // 정상 응답일 경우
    //        {
    //            foreach (XmlNode node1 in xmlList)  // <item> 값 읽어 들이기
    //            {
    //                //★get_Time 넣는곳 여기서 현재 시간 뽑아서 원하는 정보 뽑음 
    //                if (node1["fcstTime"].InnerText.Equals(get_Time))
    //                {
    //                    if (node1["category"].InnerText.Equals("SKY"))  // 하늘상태일 경우
    //                    {
    //                        switch (node1["fcstValue"].InnerText)
    //                        {
    //                            case "1":
    //                                Debug.Log("맑음");
    //                                break;
    //                            case "3":
    //                                Debug.Log("구름많음");
    //                                break;
    //                            case "4":
    //                                Debug.Log("흐림");
    //                                break;
    //                            default:
    //                                Debug.Log("해당하는 자료가 없음");
    //                                break;
    //                        }
    //                    }

    //                    if (node1["category"].InnerText.Equals("PTY"))  // 강수형태일 경우
    //                    {
    //                        switch (node1["fcstValue"].InnerText)
    //                        {
    //                            case "0":
    //                                Debug.Log("없음");
    //                                break;
    //                            case "1":
    //                                Debug.Log("비");
    //                                break;
    //                            case "2":
    //                                //비/눈/진눈개비
    //                                Debug.Log("비/눈/진눈개비");
    //                                break;
    //                            case "3":
    //                                Debug.Log("눈");
    //                                break;
    //                            case "4":
    //                                Debug.Log("소나기");
    //                                break;
    //                            default:
    //                                Debug.Log("해당하는 자료가 없습니다.");
    //                                break;
    //                        }
    //                    }

    //                    if (node1["category"].InnerText.Equals("TMP"))  // 현재 기온 불러옴 
    //                    {
    //                        //Debug.Log("TMP 들어감"+ node1["fcstDate"].InnerText);
    //                        Debug.Log("현재 TMP:" + node1["fcstValue"].InnerText);
    //                    }
    //                }

    //            }
    //        }

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