using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Material material;
    List<int> triangles　　　= new List<int>();
    List<Vector3> vertices   = new List<Vector3>();
    List<Vector2> uv         = new List<Vector2>();

    // Startでブロックを作る
    //ブロックは3Dでvoxel(ボクセル)と呼ばれる。
    void Start()
    {
        Append(new Voxel(0, 0, 0, 0));
        Init(new GameObject());
    }
    //ブロックの3Dデータを取り込む
    void Append(params Voxel[] voxels)
    {
        foreach (Voxel v in voxels)
        {
            foreach (int t in v.triangles) triangles.Add(vertices.Count + t);
            vertices.AddRange(v.vertices);
            uv.AddRange(v.uv);
        }

    }
    // 3D表示を行う
    void Init(GameObject g)
    {
        // メッシュ
        // 頂点とテクスチャとポリゴンを使う
        var f = g.AddComponent<MeshFilter>();
        var r = g.AddComponent<MeshRenderer>();
        var m = new Mesh()
        {
            vertices = vertices.ToArray(),
            uv = uv.ToArray(),
            triangles = triangles.ToArray(),
        };
        f.mesh = m;
        r.material = material;
        m.RecalculateNormals();
    }
}

// ブロッククラスにして量産できるようにする
public class Voxel 
{
    static List<int> data = new List<int>();
    static int[] order = { 6,7,2,3, 2,3,0,1, 0,1,4,5, 6,2,4,0, 7,6,5,4, 3,7,1,5};
    static int slice = 8;
    // 静的コンストラクタでポリゴンの雛形を作ります
    static Voxel()
    {
        for(int i = 0; i < order.Length; i += 4)
        {
            //正方形を斜めに切って２つのポリゴンにする
            data.AddRange(new[] { i, i + 1, i + 2, i + 3, i + 2, i + 1 });
        }
    }

    public List<int> triangles      = data;
    public List<Vector3> vertices   = new List<Vector3>();
    public List<Vector2> uv         = new List<Vector2>();
    public Voxel(int x, int y, int z, int type)
    {
        var cube = new Vector3[8];              // ブロックの頂点を8個つくる
        var face = new[] { 0, 1, 2, 1, 1, 1 };  // ブロックのテクスチャを6個用意する
     
        for(int i = 0; i < 8; i++)
        {
            // ブロックの頂点についていた8個の10進数を2進数に変換すると、そのまま頂点の座標になる
            cube[i] = new Vector3((i & 1) + x, (i >> 1 & 1) + y, (i >> 2 & 1) + z);
        }
        for(int i = 0; i < order.Length; i++)
        {
            vertices.Add(cube[order[i]]);
            // テクスチャも配列の添字を2進数にしてマッピングの頂点にする
            uv.Add(new Vector2(type + (i & 1), slice - (i >> 1 & 1) - face[i >> 2] / slice));
        }
    }

}
