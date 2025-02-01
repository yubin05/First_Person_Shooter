using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Json 데이터를 보관하고 있는 클래스
public class JsonDataContainer
{
    private Dictionary<string, List<Data>> dataTables = new Dictionary<string, List<Data>>();

    public string Extension => ".json";

    public void LoadData<T>(string tableName, string path) where T : Data
    {
        var jsonAsset = File.ReadAllText(path);

        var jArray = JArray.Parse(jsonAsset.ToString());
        var instances = jArray.Select(x => JsonConvert.DeserializeObject<T>(x.ToString())).ToArray();

        foreach (var instance in instances)
        {
            AddData(tableName, instance.Id, instance);
        }
    }

    public T ReturnData<T>(string dataTableName, int id) where T : Data
    {
        var dataTable = dataTables[dataTableName];
        return dataTable.Where(x => x.Id == id).Select(x => (T)x).FirstOrDefault();
    }
    public T[] ReturnDatas<T>(string dataTableName) where T : Data
    {
        var dataTable = dataTables[dataTableName];
        return dataTable.Select(x => (T)x).ToArray();
    }

    public void AddData(string dataTableName, int id, Data data)
    {
        var datas = new List<Data>();

        if (!dataTables.ContainsKey(dataTableName))
        {
            datas.Add(data);
            dataTables.Add(dataTableName, datas);
        }
        else
        {
            dataTables[dataTableName].Add(data);
        }
    }
}
