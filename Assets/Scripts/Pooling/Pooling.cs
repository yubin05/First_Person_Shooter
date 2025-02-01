using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// 오브젝트 풀링 관리하는 클래스
public class Pooling : GlobalSingleton<Pooling>, IChangeScene
{
    private Dictionary<int, PoolContainer> poolContainers;

    public void Init()
    {
        poolContainers = new Dictionary<int, PoolContainer>();

        SceneManager.activeSceneChanged -= OnChangeScene;
        SceneManager.activeSceneChanged += OnChangeScene;
    }
    public void OnChangeScene(Scene beforeScene, Scene afterScene)
    {
        poolContainers.Clear();
    }

    // 풀링 오브젝트 꺼내오기
    public K CreatePoolObject<K>(int id) where K : PoolObject
    {
        if (!poolContainers.ContainsKey(id))
            poolContainers.Add(id, new PoolContainer());

        return poolContainers[id].CreatePoolObject<K>(id);
    }

    // 풀링 오브젝트 반납
    public void ReturnPoolObject<K>(int id, K poolObject) where K : PoolObject
    {
        poolContainers[id].ReturnPoolObject(poolObject);
    }
}

// 각 오브젝트 클래스에 따른 PoolObject 보관하는 클래스
// Ex) 사운드 오브젝트 보관 컨테이너, 캐릭터 오브젝트 보관 컨테이너 등 각각 다른 클래스로 분리해서 관리
public class PoolContainer
{
    private Dictionary<int, PoolObject> poolObjects;
    private int instanceId;

    public PoolContainer()
    {
        poolObjects = new Dictionary<int, PoolObject>();
        instanceId = 0;
    }

    // 여분의 오브젝트가 없거나 오브젝트를 모두 활성화 상태라면 새로 생성
    // 오브젝트를 활성화하여 생성 또는 꺼내온 오브젝트를 사용할 수 있도록 함
    public K CreatePoolObject<K>(int id) where K : PoolObject
    {
        var poolObject = poolObjects.FirstOrDefault(x => !x.Value.gameObject.activeSelf);

        K instance = null;
        if (poolObject.Equals(default(KeyValuePair<int, PoolObject>)))  // null 체크
        {
            var path = GameApplication.Instance.GameModel.PresetData.ReturnData<PrefabInfo>(nameof(PrefabInfo), id).Path;
            instance = GameObject.Instantiate(Resources.Load<K>(path));
            poolObjects.Add(++instanceId, instance);
        }
        else
        {
            instance = poolObject.Value as K;
        }

        instance.gameObject.SetActive(true);
        return instance;
    }

    // 풀링 오브젝트 비활성화
    public void ReturnPoolObject<K>(K poolObject) where K : PoolObject
    {
        poolObject.gameObject.SetActive(false);
    }
}