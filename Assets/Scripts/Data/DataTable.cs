using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTablePath
{
    public static string CSVFilePath { get; } = "Data/TableList/";
    public static string JsonFilePath { get; } = "Data/Json/";
}
/// <summary>
/// CSV 파일에서 파싱한 데이터를 저장하고 있는 데이터 테이블
/// </summary>
public class DataTable
{
    public string TableName { get; set; }   // 데이터 테이블 이름(=typeof(T).Name)
    private Dictionary<int, Data> DataContainer;    // id와 데이터를 저장하고 있는 데이터 컨테이너(사전 구조)
    private int instanceId; // 자동 인덱싱을 위해 추가한 인스턴스 id(=추가할 데이터의 id)
    /// <summary>
    /// 데이터 테이블을 생성할 때 호출하는 생성자
    /// </summary>
    /// <param name="TableName">생성할 데이터 테이블의 이름(=typeof(T).Name)</param>
    /// <param name="DataContainer">생성할 데이터 컨테이너(사전 구조)</param>
    public DataTable(string TableName, Dictionary<int, Data> DataContainer)
    {
        this.TableName = TableName;
        this.DataContainer = DataContainer;
        instanceId = 0;
    }
    /// <summary>
    /// 데이터 컨테이너에 id와 데이터를 추가
    /// </summary>
    /// <param name="id">추가할 데이터의 id</param>
    /// <param name="data">추가할 데이터</param>
    public void AddData(int id, Data data)
    {
        DataContainer.Add(id, data);
    }
    /// <summary>
    /// 데이터 컨테이너에 데이터를 추가(id는 오토 인덱싱 방식)
    /// </summary>
    /// <param name="data">추가할 데이터</param>
    public void AddData(Data data)
    {
        data.Id = ++instanceId;
        DataContainer.Add(data.Id, data);
    }
    /// <summary>
    /// 데이터 컨테이너에 데이터를 제거
    /// </summary>
    /// <param name="data">제거할 데이터</param>
    public void RemoveData(Data data)
    {
        var dataContainer = DataContainer.SingleOrDefault(x => x.Value == data);
        if (dataContainer.Equals(default(KeyValuePair<int, Data>))) return; // null 체크

        DataContainer.Remove(dataContainer.Key);
    }
    /// <summary>
    /// 데이터 컨테이너에서 원하는 id의 데이터를 반환
    /// </summary>
    /// <param name="id">반환할 데이터의 id</param>
    /// <returns>반환되는 데이터</returns>
    public Data GetData(int id)
    {
        return DataContainer[id];
    }
    /// <summary>
    /// 데이터 컨테이너에서 모든 데이터를 반환
    /// </summary>
    /// <returns>모든 데이터를 배열 형태로 반환</returns>
    public Data[] GetDatas()
    {
        return DataContainer.Values.Select(x => x).ToArray();
    }
}
