using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

public class ExelToJson : MonoBehaviour
{
    [SerializeField]
    private DataTable dataTable;       // ������ ���̺�

    [SerializeField]
    private Text printText;            // ����� �ؽ�Ʈ

    [SerializeField]
    private List<EntityType> data;     // ������ ������

    [ContextMenu("������ ���̺��� �����͸� �����ؼ� Json�����ͷ� �Ľ��մϴ�.")]
    public void ToJson()
    {
        Debug.Log("Refresh ���ּ���.");

        data = dataTable.Data;
        string jsonData_P = JsonConvert.SerializeObject(data, Formatting.Indented);
        string path_P = Path.Combine(Application.dataPath, "json_data.json");
        File.WriteAllText(path_P, jsonData_P);

        printText.text = "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" +
            "�ν����� â�� Manager�� ContextMenu�� ����ϸ� �˴ϴ�.";
    }

    [ContextMenu("Json�����͸� �ҷ��� ����մϴ�.")]
    public void PrintJson()
    {
        string path_P = Path.Combine(Application.dataPath, "json_data.json");
        string JsonData_P = File.ReadAllText(path_P);

        printText.text = JsonData_P;
    }
}
