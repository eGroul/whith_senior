using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;


[System.Serializable]
public class ChangeEntityType
{
    public int id;
    public string name;
    public struct Stats
    {
        public float attack;
        public float defense;
        public int maxHP;
    }
    public Stats stats;
}

public class ExelToJson : MonoBehaviour
{
    [SerializeField]
    private DataTable dataTable;       // ������ ���̺�

    [SerializeField]
    private Text printText;            // ����� �ؽ�Ʈ

    [SerializeField]
    private List<ChangeEntityType> data = new List<ChangeEntityType>();     // ������ ������

    [ContextMenu("������ ���̺��� �����͸� �����ؼ� Json�����ͷ� �Ľ��մϴ�.")]
    public void ToJson()
    {
        Debug.Log("Refresh ���ּ���.");

        List<EntityType>  originData = dataTable.Data;

        data = new List<ChangeEntityType>();
        foreach (EntityType type in originData)
        {
            ChangeEntityType inPut = new ChangeEntityType();
            inPut.id = type.ID;
            inPut.name = type.NameString;
            inPut.stats.attack = type.ATK;
            inPut.stats.defense = type.DEF;
            inPut.stats.maxHP = type.MaxHP;

            data.Add(inPut);
        }

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
