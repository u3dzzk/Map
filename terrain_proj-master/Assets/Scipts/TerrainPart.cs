using UnityEngine;


/// <summary>
/// 场景物件管理物体，数据 也是逻辑，负责实例化该场景对象，包括光照贴图设置
/// </summary>
public class TerrainPart
{

    public Vector3 pos;

    public Quaternion rot;

    public Vector3 scale;

    public string path;

    public int lightmapIndex;

    public Vector4 lightmapOffsetScale;

    public Transform root;

    private GameObject go;

    private ResourceRequest req;//这就是要改的地方，因为这是这个模块自带的资源管理框架中的东西

    public void Load()
    {
        AsynLoadMgr.sington.Load(path, LoadFinish);
    }
        // 加载该
    private void LoadFinish(Object obj)
    {
        go = GameObject.Instantiate(obj) as GameObject;
        go.transform.SetParent(TerrainLoadMgr.sington.PartRoot);
        go.transform.position = pos;
        go.transform.localScale = scale;
        go.transform.rotation = rot;

        MeshRenderer rend = go.GetComponent<MeshRenderer>();
        if (rend != null && lightmapIndex != -1)
        {
            rend.lightmapIndex = lightmapIndex;
            rend.lightmapScaleOffset = lightmapOffsetScale;
        }
    }

    public void Unload()
    {
        if (go != null)
        {
            GameObject.Destroy(go, 0.5f);
        }
    }

    public bool InRange(Bounds bound)
    {
        return bound.Contains(pos);
    }

}