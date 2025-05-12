using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

public class ExelToJson : MonoBehaviour
{
    [SerializeField]
    private DataTable dataTable;       // 데이터 테이블

    [SerializeField]
    private Text printText;            // 출력할 텍스트

    [SerializeField]
    private List<EntityType> data;     // 저장할 데이터

    [ContextMenu("데이터 테이블에서 데이터를 추출해서 Json데이터로 파싱합니다.")]
    public void ToJson()
    {
        Debug.Log("Refresh 해주세요.");

        data = dataTable.Data;
        string jsonData_P = JsonConvert.SerializeObject(data, Formatting.Indented);
        string path_P = Path.Combine(Application.dataPath, "json_data.json");
        File.WriteAllText(path_P, jsonData_P);

        printText.text = "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" +
            "인스펙터 창에 Manager의 ContextMenu를 사용하면 됩니다.";
    }

    [ContextMenu("Json데이터를 불러와 출력합니다.")]
    public void PrintJson()
    {
        string path_P = Path.Combine(Application.dataPath, "json_data.json");
        string JsonData_P = File.ReadAllText(path_P);

        printText.text = JsonData_P;
    }
}
