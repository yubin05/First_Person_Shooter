using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// CSV 데이터를 보관하고 있는 클래스
public class CSVDataContainer
{
    /// <summary>
    /// 여러 CSV 데이터를 저장하고 있는 사전 구조의 변수
    /// </summary>
    private Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();

    /// <summary>
    /// 리소스 폴더에 저장되어 있는 CSV 파일 데이터를 읽어오는 함수
    /// </summary>
    /// <typeparam name="T">CSV 데이터를 읽어올 클래스 타입(단, T는 Data 클래스를 상속받아야 함)</typeparam>
    /// <param name="dataTableName">CSV 데이터를 읽어올 클래스 이름(typeof(T).Name하여 사용)</param>
    /// <param name="path">CSV 데이터를 읽어올 리소스 경로(Resource.Load<T> 사용)</param>
    public void LoadData<T>(string dataTableName, string path) where T : Data
    {
        var instances = GetData<T>(path);
        foreach (var instance in instances)
        {
            AddData(dataTableName, instance.Id, instance);
        }
    }
    /// <summary>
    /// 리소스 폴더에 저장되어 있는 CSV 파일 데이터를 읽어오는 함수(CSV 파일에 Id 변수가 없어도 자동으로 증가)
    /// </summary>
    /// <typeparam name="T">CSV 데이터를 읽어올 클래스 타입(단, T는 Data 클래스를 상속받아야 함)</typeparam>
    /// <param name="dataTableName">CSV 데이터를 읽어올 클래스 이름(typeof(T).Name하여 사용)</param>
    /// <param name="path">CSV 데이터를 읽어올 리소스 경로(Resource.Load<T> 사용)</param>
    public void LoadDataAutoIndexing<T>(string dataTableName, string path) where T : Data
    {
        var instances = GetData<T>(path);
        foreach (var instance in instances)
        {
            AddData(dataTableName, instance);
        }
    }
    /// <summary>
    /// 원하는 CSV의 매칭된 Id값의 데이터를 읽어오는 함수
    /// </summary>
    /// <typeparam name="T">CSV 데이터를 읽어올 클래스 타입(단, T는 Data 클래스를 상속받아야 함)</typeparam>
    /// <param name="dataTableName">CSV 데이터를 읽어올 클래스 이름(typeof(T).Name하여 사용)</param>
    /// <param name="id">해당 테이블의 데이터를 불러올 Id값</param>
    /// <returns></returns>
    public T ReturnData<T>(string dataTableName, int id) where T : Data
    {
        if (!dataTables.ContainsKey(dataTableName))
        {
            Debug.LogWarning($"{dataTableName} 테이블이 존재하지 않습니다.");
            return null;
        }

        var dataTable = dataTables[dataTableName];
        return (T)dataTable.GetData(id);
    }
    /// <summary>
    /// 원하는 CSV의 모든 데이터를 읽어오는 함수
    /// </summary>
    /// <typeparam name="T">CSV 데이터를 읽어올 클래스 타입(단, T는 Data 클래스를 상속받아야 함)</typeparam>
    /// <param name="dataTableName">CSV 데이터를 읽어올 클래스 이름(typeof(T).Name하여 사용)</param>
    /// <returns></returns>
    public T[] ReturnDatas<T>(string dataTableName) where T : Data
    {
        if (!dataTables.ContainsKey(dataTableName))
        {
            return new T[0];
        }
        
        var dataTable = dataTables[dataTableName];
        return dataTable.GetDatas().Select(x => (T)x).ToArray();
    }
    /// <summary>
    /// 원하는 데이터 테이블(사전 구조)의 id값과 데이터를 추가하는 함수
    /// </summary>
    /// <param name="dataTableName">데이터를 추가할 데이터 테이블의 이름(typeof(T).Name하여 사용)</param>
    /// <param name="id">데이터 테이블에 추가할 id</param>
    /// <param name="data">데이터 테이블에 추가할 데이터</param>
    public void AddData(string dataTableName, int id, Data data)
    {
        if (!dataTables.ContainsKey(dataTableName))
            dataTables.Add(dataTableName, new DataTable(dataTableName, new Dictionary<int, Data>()));

        data.Id = id;
        data.TableModel = dataTables[dataTableName];
        dataTables[dataTableName].AddData(id, data);
    }
    /// <summary>
    /// 원하는 데이터 테이블(사전 구조)의 데이터를 추가하는 함수
    /// </summary>
    /// <param name="dataTableName">데이터를 추가할 데이터 테이블의 이름(typeof(T).Name하여 사용)</param>
    /// <param name="data">데이터 테이블에 추가할 데이터</param>
    public void AddData(string dataTableName, Data data)
    {
        if (!dataTables.ContainsKey(dataTableName))
            dataTables.Add(dataTableName, new DataTable(dataTableName, new Dictionary<int, Data>()));

        var dataTable = dataTables[dataTableName];
        data.TableModel = dataTable;
        dataTable.AddData(data);
    }
    /// <summary>
    /// 원하는 데이터 테이블(사전 구조)의 데이터를 제거하는 함수
    /// </summary>
    /// <param name="dataTableName">데이터를 제거할 데이터 테이블의 이름(typeof(T).Name하여 사용)</param>
    /// <param name="data">데이터 테이블에 제거할 데이터</param>
    public void RemoveData(string dataTableName, Data data)
    {
        if (!dataTables.ContainsKey(dataTableName)) return;

        dataTables[dataTableName].RemoveData(data);
    }
    /// <summary>
    /// CSV 파일을 데이터 테이블(사전 구조)의 형태로 파싱하는 함수
    /// </summary>
    /// <typeparam name="T">파싱할 클래스(단, T는 Data 클래스를 상속받아야 함)</typeparam>
    /// <param name="_file">파싱할 CSV 파일의 리소스 경로(Resource.Load<T> 사용)</param>
    /// <returns>파싱 결과를 리스트로 반환</returns>
    public List<T> GetData<T>(string _file) where T : Data
    {
        var list = new List<T>();
        var lines = Regex.Split(_file, @"\r\n|\n\r|\n|\r").ToList();
        var headerLine = lines.First();
        var colNames = headerLine.Split(',');
        var rows = lines.Skip(1);

        var properties = typeof(T).GetProperties();

        rows.ToList().ForEach(r =>
        {
            var cells = r.Split(',');

            var obj = (T)Activator.CreateInstance(typeof(T));

            var index = 0;

            foreach (var colName in colNames)
            {
                var prop = properties.SingleOrDefault(p => p.Name == colName);
                // Debug.Log(colName); Debug.Log(prop);
                Type propertyType = prop.PropertyType;
                var value = cells[index++];

                if (!propertyType.IsEnum)
                    prop.SetValue(obj, Convert.ChangeType(value, propertyType));
                else
                    prop.SetValue(obj, Enum.Parse(propertyType, value));
            }

            list.Add(obj);
        });

        return list;
    }
    /// <summary>
    /// 데이터 테이블을 초기화하는 함수
    /// </summary>
    public void Clear()
    {
        dataTables.Clear();
    }
}
