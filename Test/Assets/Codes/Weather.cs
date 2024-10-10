using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Xml;

//�ܱ⿹����ȸ

public class Weather : MonoBehaviour
{
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

        Debug.Log("Time:" + Time + " base_Date:" + base_date + " base_Time:" + base_time);
        Debug.Log("getTime:" + get_Time);
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

    //url ���ϴ� �Լ� 

    //public void Geturl()
    //{
    //    url = "https://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst";
    //    url += "?ServiceKey=" + "XtDPA1fEnePF0FZU5BhnMkykFdyFDEJtZh%2FznJJ9AVBbXVUte9F2dylUfh6Dg86e9jQzf%2BbDCAooHg4GEOkKCA%3D%3D"; //api ���� Ű
    //    //�� �ð��뿡 �ִ� �����۰��� �� 12���� 1100�̸� �� 12���� ���� base_time ���� �� switch���� �ִ� 3���� �������ϱ� 36���� ����
    //    url += "&numOfRows=36";             // �������� ��� ��(Default : 12)  //�� �� �κ� 36�� ����  
    //    url += "&pageNo=1";                 // ������ ��ȣ(Default : 1)
    //    url += "&dataType=XML";             // ���� �ڷ�����(XML, JSON)
    //                                        //url += "&ftype=ODAM";
    //    url += "&base_date=" + base_date;   // ��û ��¥(yyMMdd)
    //    url += "&base_time=" + base_time;   // ��û �ð�(HHmm)
    //    url += "&nx=55";                    // ��û ���� x��ǥ
    //    url += "&ny=127";                   // ��û ���� y�·�

    //    Debug.Log(url);
    //}

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

    //url �ȿ� ������ �ҷ����� ���� 
    //IEnumerator LoadData()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(url);

    //    yield return www.SendWebRequest();
    //    if (www.error == null)   //�ҷ����� ����
    //    {
    //        //�������� ��� 
    //        Debug.Log(www.downloadHandler.text);
    //    }

    //    XmlDocument xml = new XmlDocument();
    //    xml.Load(url);
    //    XmlNodeList xmResponse = xml.GetElementsByTagName("response");  //response �������� ����
    //    XmlNodeList xmlList = xml.GetElementsByTagName("item");

    //    //XML ���� �� ������ �ҷ��� 
    //    foreach (XmlNode node in xmResponse)
    //    {
    //        if (node["header"]["resultMsg"].InnerText.Equals("NORMAL_SERVICE")) // ���� ������ ���
    //        {
    //            foreach (XmlNode node1 in xmlList)  // <item> �� �о� ���̱�
    //            {
    //                //��get_Time �ִ°� ���⼭ ���� �ð� �̾Ƽ� ���ϴ� ���� ���� 
    //                if (node1["fcstTime"].InnerText.Equals(get_Time))
    //                {
    //                    if (node1["category"].InnerText.Equals("SKY"))  // �ϴû����� ���
    //                    {
    //                        switch (node1["fcstValue"].InnerText)
    //                        {
    //                            case "1":
    //                                Debug.Log("����");
    //                                break;
    //                            case "3":
    //                                Debug.Log("��������");
    //                                break;
    //                            case "4":
    //                                Debug.Log("�帲");
    //                                break;
    //                            default:
    //                                Debug.Log("�ش��ϴ� �ڷᰡ ����");
    //                                break;
    //                        }
    //                    }

    //                    if (node1["category"].InnerText.Equals("PTY"))  // ���������� ���
    //                    {
    //                        switch (node1["fcstValue"].InnerText)
    //                        {
    //                            case "0":
    //                                Debug.Log("����");
    //                                break;
    //                            case "1":
    //                                Debug.Log("��");
    //                                break;
    //                            case "2":
    //                                //��/��/��������
    //                                Debug.Log("��/��/��������");
    //                                break;
    //                            case "3":
    //                                Debug.Log("��");
    //                                break;
    //                            case "4":
    //                                Debug.Log("�ҳ���");
    //                                break;
    //                            default:
    //                                Debug.Log("�ش��ϴ� �ڷᰡ �����ϴ�.");
    //                                break;
    //                        }
    //                    }

    //                    if (node1["category"].InnerText.Equals("TMP"))  // ���� ��� �ҷ��� 
    //                    {
    //                        //Debug.Log("TMP ��"+ node1["fcstDate"].InnerText);
    //                        Debug.Log("���� TMP:" + node1["fcstValue"].InnerText);
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