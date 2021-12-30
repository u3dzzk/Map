using UnityEditor;
using UnityEngine;

public class TerrainLoadEditor : Editor
{

    [MenuItem("Terrain/Load")]
    private static void Load()
    {
        int XMax = 4, YMax = 4;
        Vector3 offset = Vector3.zero;
        // 1 加载，也需要场景中有Terrain对象
        Terrain terr = GameObject.FindObjectOfType<Terrain>();
        if (terr != null)
        {
            offset = terr.transform.position;
            terr.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("not found terrain");
            return;
        }

        string str_root_terrain = "terrain_root";
        string str_root_collider = "collider_root";
        string str_root_parts = "parts";
        //1 terrain_root collider_root场景中必须有这两个对象，没有则创建
        GameObject root_terrain = GameObject.Find(str_root_terrain);
        GameObject root_collider = GameObject.Find(str_root_collider);
        //
        GameObject root_part = GameObject.Find(str_root_parts);
        if (root_terrain == null) root_terrain = AttachGameobject(str_root_terrain);
        if (root_collider == null) root_collider = AttachGameobject(str_root_collider);
        // 2 加载Parts数据，加载总地形配置数据；初始化地形加载管者器；此时，所有的数据都被反序列化了
        TerrainLoadMgr.sington.SetRoot(root_terrain.transform, root_collider.transform, terr.name);
        
        for (int xx = 0; xx < XMax; ++xx)
        {
            for (int yy = 0; yy < YMax; ++yy)
            {
                // 3 这才是加载分割后的地形数据 的地方；包括加载该分块儿下的parts对象
                TerrainLoadMgr.sington.LoadItem(xx, yy);
                TerrainLoadMgr.sington.LoadCollider(xx, yy);
            }
        }
        Debug.Log(root_part);
        //如果场景中有之前的parts，则disable
        if (root_part != null) root_part.SetActive(false);
        TerrainLoadMgr.sington.ResetRootPos();
    }



    private static GameObject AttachGameobject(string name)
    {
        GameObject go = new GameObject(name);
        var root = GameObject.Find("raceTrackLakeLevel");
        go.transform.SetParent(root.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        return go;
    }
}