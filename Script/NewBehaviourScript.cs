using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Material material;
    List<int> triangles�@�@�@= new List<int>();
    List<Vector3> vertices   = new List<Vector3>();
    List<Vector2> uv         = new List<Vector2>();

    // Start�Ńu���b�N�����
    //�u���b�N��3D��voxel(�{�N�Z��)�ƌĂ΂��B
    void Start()
    {
        Append(new Voxel(0, 0, 0, 0));
        Init(new GameObject());
    }
    //�u���b�N��3D�f�[�^����荞��
    void Append(params Voxel[] voxels)
    {
        foreach (Voxel v in voxels)
        {
            foreach (int t in v.triangles) triangles.Add(vertices.Count + t);
            vertices.AddRange(v.vertices);
            uv.AddRange(v.uv);
        }

    }
    // 3D�\�����s��
    void Init(GameObject g)
    {
        // ���b�V��
        // ���_�ƃe�N�X�`���ƃ|���S�����g��
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

// �u���b�N�N���X�ɂ��ėʎY�ł���悤�ɂ���
public class Voxel 
{
    static List<int> data = new List<int>();
    static int[] order = { 6,7,2,3, 2,3,0,1, 0,1,4,5, 6,2,4,0, 7,6,5,4, 3,7,1,5};
    static int slice = 8;
    // �ÓI�R���X�g���N�^�Ń|���S���̐��`�����܂�
    static Voxel()
    {
        for(int i = 0; i < order.Length; i += 4)
        {
            //�����`���΂߂ɐ؂��ĂQ�̃|���S���ɂ���
            data.AddRange(new[] { i, i + 1, i + 2, i + 3, i + 2, i + 1 });
        }
    }

    public List<int> triangles      = data;
    public List<Vector3> vertices   = new List<Vector3>();
    public List<Vector2> uv         = new List<Vector2>();
    public Voxel(int x, int y, int z, int type)
    {
        var cube = new Vector3[8];              // �u���b�N�̒��_��8����
        var face = new[] { 0, 1, 2, 1, 1, 1 };  // �u���b�N�̃e�N�X�`����6�p�ӂ���
     
        for(int i = 0; i < 8; i++)
        {
            // �u���b�N�̒��_�ɂ��Ă���8��10�i����2�i���ɕϊ�����ƁA���̂܂ܒ��_�̍��W�ɂȂ�
            cube[i] = new Vector3((i & 1) + x, (i >> 1 & 1) + y, (i >> 2 & 1) + z);
        }
        for(int i = 0; i < order.Length; i++)
        {
            vertices.Add(cube[order[i]]);
            // �e�N�X�`�����z��̓Y����2�i���ɂ��ă}�b�s���O�̒��_�ɂ���
            uv.Add(new Vector2(type + (i & 1), slice - (i >> 1 & 1) - face[i >> 2] / slice));
        }
    }

}
