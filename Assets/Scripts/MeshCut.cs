using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLINDED_AM_ME
{
    public class MeshCut
    {
        public class MeshCutSide
        {
            public List<Vector3>  vertices  = new List<Vector3>();
            public List<Vector3>  normals   = new List<Vector3>();
            public List<Vector2>  uvs       = new List<Vector2>();
            public List<int>      triangles = new List<int>();
            public List<List<int>> subIndices = new List<List<int>>();


            //各オブジェクト構成要素を初期化するメソッド
            public void ClearAll()
            {
                vertices.Clear();
                normals.Clear();
                uvs.Clear();
                triangles.Clear();
                subIndices.Clear();
            }

            //各オブジェクト構成要素を追加するメソッド
            public void AddTriangle(int p1, int p2, int p3, int submesh)
            {
                int base_index = vertices.Count;

                subIndices[submesh].Add(base_index + 0);
                subIndices[submesh].Add(base_index + 1);
                subIndices[submesh].Add(base_index + 2);

                triangles.Add(base_index + 0);
                triangles.Add(base_index + 1);
                triangles.Add(base_index + 2);

                vertices.Add(victim_mesh.vertices[p1]);
                vertices.Add(victim_mesh.vertices[p2]);
                vertices.Add(victim_mesh.vertices[p3]);

                normals.Add(victim_mesh.normals[p1]);
                normals.Add(victim_mesh.normals[p2]);
                normals.Add(victim_mesh.normals[p3]);

                uvs.Add(victim_mesh.uv[p1]);
                uvs.Add(victim_mesh.uv[p2]);
                uvs.Add(victim_mesh.uv[p3]);
            }

            //AddTriangleメソッドをオーバーロード、FillCapメソッドで利用する
            public void AddTriangle(Vector3[] points3, Vector3[] normals3, Vector2[] uvs3, Vector3 faceNormal, int submesh)
            {
                Vector3 calculated_normal = Vector3.Cross((points3[1] - points3[0]).normalized, (points3[2] - points3[0]).normalized);

                int p1 = 0;
                int p2 = 1;
                int p3 = 2;

                if (Vector3.Dot(calculated_normal, faceNormal) < 0)
                {
                    p1 = 2;
                    p2 = 1;
                    p3 = 0;
                }

                int base_index = vertices.Count;

                subIndices[submesh].Add(base_index + 0);
                subIndices[submesh].Add(base_index + 1);
                subIndices[submesh].Add(base_index + 2);

                triangles.Add(base_index + 0);
                triangles.Add(base_index + 1);
                triangles.Add(base_index + 2);

                vertices.Add(points3[p1]);
                vertices.Add(points3[p2]);
                vertices.Add(points3[p3]);

                normals.Add(normals3[p1]);
                normals.Add(normals3[p2]);
                normals.Add(normals3[p3]);

                uvs.Add(uvs3[p1]);
                uvs.Add(uvs3[p2]);
                uvs.Add(uvs3[p3]);
            }

        }

        //オブジェクトを構成するメッシュの入れ物を左右それぞれ作る
        private static MeshCutSide left_side = new MeshCutSide();
        private static MeshCutSide right_side = new MeshCutSide();
        //カットする平面
        private static Plane blade;
        //カット対象のメッシュ
        private static Mesh victim_mesh;
        //切断により新たに生成される頂点を格納するための配列
        private static List<Vector3> new_vertices = new List<Vector3>();

        //カット処理全体を行うメソッド
        public static GameObject[] Cut(GameObject victim, Vector3 anchorPoint, Vector3 normalDirection, Material capMaterial)
        {
            //法線とグローバルでのアンカーポイントを元にカットする平面を定義
            blade = new Plane(
                victim.transform.InverseTransformDirection(-normalDirection),
                victim.transform.InverseTransformPoint(anchorPoint)
            );
            //カット対象オブジェクトのメッシュ取得
            victim_mesh = victim.GetComponent<MeshFilter>().mesh;
            //カット後の左右のオブジェクト、新頂点リストを初期化
            new_vertices.Clear();
            left_side.ClearAll();
            right_side.ClearAll();

            bool[] sides = new bool[3];//三角形を構成する各頂点がカット平面の左右どちらにあるかを表す配列
            int[] indices;//サブメッシュのインデックスを格納する配列
            int p1,p2,p3;

            //メッシュに含まれるサブメッシュの数だけループ処理
            for (int sub = 0; sub < victim_mesh.subMeshCount; sub++)
            {
                indices = victim_mesh.GetIndices(sub);
                left_side.subIndices.Add(new List<int>());  // 左側に振り分けられるサブメッシュ
                right_side.subIndices.Add(new List<int>()); // 右側に振り分けられるサブメッシュ

                //各サブメッシュに対して構成頂点がカット平面の左右のどちらに存在するかによってleft_side or right_sideに振り分ける or カット処理
                for (int i = 0; i < indices.Length; i += 3)
                {
                    p1 = indices[i + 0];
                    p2 = indices[i + 1];
                    p3 = indices[i + 2];

                    sides[0] = blade.GetSide(victim_mesh.vertices[p1]);
                    sides[1] = blade.GetSide(victim_mesh.vertices[p2]);
                    sides[2] = blade.GetSide(victim_mesh.vertices[p3]);

                    if (sides[0] == sides[1] && sides[0] == sides[2])
                    {
                        if (sides[0])
                        { 
                            left_side.AddTriangle(p1, p2, p3, sub);
                        }
                        else
                        {
                            right_side.AddTriangle(p1, p2, p3, sub);
                        }
                    }
                    else
                    {
                        //三角形を構成する頂点がカット面の左右にばらけていた場合にカット処理
                        Cut_this_Face(sub, sides, p1, p2, p3);
                    }
                }
            }

            //対象オブジェクトを構成するマテリアルの配列を取得
            Material[] mats = victim.GetComponent<MeshRenderer>().sharedMaterials;
            //カット面を構成するためのマテリアルがマテリアル配列の最後の要素と一致しない場合
            if (mats[mats.Length - 1].name != capMaterial.name)
            {
                //カット面用のサブメッシュ追加
                left_side.subIndices.Add(new List<int>());
                right_side.subIndices.Add(new List<int>());
                //サブメッシュと配列要素数を一致させるため
                Material[] newMats = new Material[mats.Length + 1];
                mats.CopyTo(newMats, 0);
                newMats[mats.Length] = capMaterial;
                mats = newMats;
            }

            //カットにより新しく生成された頂点からカット面を埋める
            Capping();

            //ここまででカットにより新しくできた左側のメッシュを配列化して再セット
            Mesh left_HalfMesh = new Mesh();
            left_HalfMesh.name = "Split Mesh Left";
            left_HalfMesh.vertices  = left_side.vertices.ToArray();
            left_HalfMesh.triangles = left_side.triangles.ToArray();
            left_HalfMesh.normals   = left_side.normals.ToArray();
            left_HalfMesh.uv        = left_side.uvs.ToArray();
            left_HalfMesh.subMeshCount = left_side.subIndices.Count;
            for (int i = 0; i < left_side.subIndices.Count; i++)
            {
                left_HalfMesh.SetIndices(left_side.subIndices[i].ToArray(), MeshTopology.Triangles, i); 
            }

            //ここまででカットにより新しくできた右側のメッシュを配列化して再セット
            Mesh right_HalfMesh = new Mesh();
            right_HalfMesh.name = "Split Mesh Right";
            right_HalfMesh.vertices  = right_side.vertices.ToArray();
            right_HalfMesh.triangles = right_side.triangles.ToArray();
            right_HalfMesh.normals   = right_side.normals.ToArray();
            right_HalfMesh.uv        = right_side.uvs.ToArray();
            right_HalfMesh.subMeshCount = right_side.subIndices.Count;
            for (int i = 0; i < right_side.subIndices.Count; i++)
            {
                right_HalfMesh.SetIndices(right_side.subIndices[i].ToArray(), MeshTopology.Triangles, i);
            }
            var clone = Object.Instantiate(victim) as GameObject;
            //左側のメッシュを元のオブジェクトとして扱う
            victim.name = "left side";
            victim.GetComponent<MeshFilter>().mesh = left_HalfMesh;
            GameObject leftSideObj = victim;
            leftSideObj.GetComponent<CapsuleCollider>().enabled = true;
            leftSideObj.GetComponent<Collider>().attachedRigidbody.useGravity = true;

            GameObject rightSideObj = clone;
            rightSideObj.transform.parent = victim.transform.parent;
            rightSideObj.transform.localPosition = victim.transform.localPosition;
            rightSideObj.transform.localScale = victim.transform.localScale;
            rightSideObj.name = "right side";
            rightSideObj.GetComponent<MeshFilter>().mesh = right_HalfMesh;
            //右側の分は新しくGameObjectを作る
            /*GameObject rightSideObj = new GameObject("right side", typeof(MeshFilter), typeof(MeshRenderer));
            rightSideObj.transform.position = victim.transform.position;
            rightSideObj.transform.rotation = victim.transform.rotation;
            rightSideObj.GetComponent<MeshFilter>().mesh = right_HalfMesh;
            */
            //それぞれマテリアルを設定
            leftSideObj.GetComponent<MeshRenderer>().materials = mats;
            rightSideObj.GetComponent<MeshRenderer>().materials = mats;
            //最後にGameObjectの配列として戻す
            return new GameObject[]{leftSideObj, rightSideObj };
        }

        static void Cut_this_Face(int submesh, bool[] sides, int index1, int index2, int index3)
        {
            Vector3[] leftPoints = new Vector3[2];
            Vector3[] leftNormals = new Vector3[2];
            Vector2[] leftUvs = new Vector2[2];
            Vector3[] rightPoints = new Vector3[2];
            Vector3[] rightNormals = new Vector3[2];
            Vector2[] rightUvs = new Vector2[2];

            bool didset_left = false;
            bool didset_right = false;

            int p = index1;
            for (int side = 0; side < 3; side++)
            {
                switch(side)
                {
                    case 0:
                        p = index1;
                        break;
                    case 1:
                        p = index2;
                        break;
                    case 2:
                        p = index3;
                        break;
                }

                if (sides[side])
                {
                    if (!didset_left)
                    {
                        didset_left = true;

                        leftPoints[0]  = victim_mesh.vertices[p];
                        leftPoints[1]  = leftPoints[0];

                        leftUvs[0]     = victim_mesh.uv[p];
                        leftUvs[1]     = leftUvs[0];

                        leftNormals[0] = victim_mesh.normals[p];
                        leftNormals[1] = leftNormals[0];
                    }
                    else
                    {
                        leftPoints[1]  = victim_mesh.vertices[p];
                        leftUvs[1]     = victim_mesh.uv[p];
                        leftNormals[1] = victim_mesh.normals[p];
                    }
                }
                else
                {
                    if (!didset_right)
                    {
                        didset_right = true;

                        rightPoints[0]  = victim_mesh.vertices[p];
                        rightPoints[1]  = rightPoints[0];
                        rightUvs[0]     = victim_mesh.uv[p];
                        rightUvs[1]     = rightUvs[0];
                        rightNormals[0] = victim_mesh.normals[p];
                        rightNormals[1] = rightNormals[0];
                    }
                    else
                    {
                        rightPoints[1]  = victim_mesh.vertices[p];
                        rightUvs[1]     = victim_mesh.uv[p];
                        rightNormals[1] = victim_mesh.normals[p];
                    }
                }
            }

            float normalizedDistance = 0f;

            float distance = 0f;

            blade.Raycast(new Ray(leftPoints[0], (rightPoints[0] - leftPoints[0]).normalized), out distance);

            normalizedDistance = distance / (rightPoints[0] - leftPoints[0]).magnitude;

            Vector3 newVertex1 = Vector3.Lerp(leftPoints[0], rightPoints[0], normalizedDistance);
            Vector2 newUv1     = Vector2.Lerp(leftUvs[0], rightUvs[0], normalizedDistance);
            Vector3 newNormal1 = Vector3.Lerp(leftNormals[0] , rightNormals[0], normalizedDistance);

            new_vertices.Add(newVertex1);

            blade.Raycast(new Ray(leftPoints[1], (rightPoints[1] - leftPoints[1]).normalized), out distance);

            normalizedDistance = distance / (rightPoints[1] - leftPoints[1]).magnitude;
            Vector3 newVertex2 = Vector3.Lerp(leftPoints[1], rightPoints[1], normalizedDistance);
            Vector2 newUv2     = Vector2.Lerp(leftUvs[1], rightUvs[1], normalizedDistance);
            Vector3 newNormal2 = Vector3.Lerp(leftNormals[1] , rightNormals[1], normalizedDistance);

            new_vertices.Add(newVertex2);

            left_side.AddTriangle(
                new Vector3[]{leftPoints[0], newVertex1, newVertex2},
                new Vector3[]{leftNormals[0], newNormal1, newNormal2 },
                new Vector2[]{leftUvs[0], newUv1, newUv2},
                newNormal1,
                submesh
            );

            left_side.AddTriangle(
                new Vector3[]{leftPoints[0], leftPoints[1], newVertex2},
                new Vector3[]{leftNormals[0], leftNormals[1], newNormal2},
                new Vector2[]{leftUvs[0], leftUvs[1], newUv2},
                newNormal2,
                submesh
            );

            right_side.AddTriangle(
                new Vector3[]{rightPoints[0], newVertex1, newVertex2},
                new Vector3[]{rightNormals[0], newNormal1, newNormal2},
                new Vector2[]{rightUvs[0], newUv1, newUv2},
                newNormal1,
                submesh
            );

            right_side.AddTriangle(
                new Vector3[]{rightPoints[0], rightPoints[1], newVertex2},
                new Vector3[]{rightNormals[0], rightNormals[1], newNormal2},
                new Vector2[]{rightUvs[0], rightUvs[1], newUv2},
                newNormal2,
                submesh
            );
        }

        private static List<Vector3> capVertTracker = new List<Vector3>();
        private static List<Vector3> capVertpolygon = new List<Vector3>();

        static void Capping()
        {
            capVertTracker.Clear();

            for (int i = 0; i < new_vertices.Count; i++)
            {
                if (capVertTracker.Contains(new_vertices[i]))
                {
                    continue;
                }

                capVertpolygon.Clear();

                capVertpolygon.Add(new_vertices[i + 0]);
                capVertpolygon.Add(new_vertices[i + 1]);

                capVertTracker.Add(new_vertices[i + 0]);
                capVertTracker.Add(new_vertices[i + 1]);

                bool isDone = false;
                while (!isDone)
                {
                    isDone = true;

                    for (int k = 0; k < new_vertices.Count; k += 2)
                    { 
                        if (new_vertices[k] == capVertpolygon[capVertpolygon.Count - 1] && !capVertTracker.Contains(new_vertices[k + 1]))
                        {   
                            isDone = false;
                            capVertpolygon.Add(new_vertices[k + 1]);
                            capVertTracker.Add(new_vertices[k + 1]);
                        }
                        else if (new_vertices[k + 1] == capVertpolygon[capVertpolygon.Count - 1] && !capVertTracker.Contains(new_vertices[k]))
                        {   
                            isDone = false;
                            capVertpolygon.Add(new_vertices[k]);
                            capVertTracker.Add(new_vertices[k]);
                        }
                    }
                }

                FillCap(capVertpolygon);
            }
        }

        static void FillCap(List<Vector3> vertices)
        {
            Vector3 center = Vector3.zero;

            foreach(Vector3 point in vertices)
            {
                center += point;
            }

            center = center / vertices.Count;

            Vector3 upward = Vector3.zero;

            upward.x =  blade.normal.y;
            upward.y = -blade.normal.x;
            upward.z =  blade.normal.z;

            Vector3 left = Vector3.Cross(blade.normal, upward);

            Vector3 displacement = Vector3.zero;
            Vector3 newUV1 = Vector3.zero;
            Vector3 newUV2 = Vector3.zero;

            for (int i = 0; i < vertices.Count; i++)
            {
                displacement = vertices[i] - center;

                newUV1 = Vector3.zero;
                newUV1.x = 0.5f + Vector3.Dot(displacement, left);
                newUV1.y = 0.5f + Vector3.Dot(displacement, upward);
                newUV1.z = 0.5f + Vector3.Dot(displacement, blade.normal);

                displacement = vertices[(i + 1) % vertices.Count] - center;

                newUV2 = Vector3.zero;
                newUV2.x = 0.5f + Vector3.Dot(displacement, left);
                newUV2.y = 0.5f + Vector3.Dot(displacement, upward);
                newUV2.z = 0.5f + Vector3.Dot(displacement, blade.normal);

                left_side.AddTriangle(
                    new Vector3[]{
                        vertices[i],
                        vertices[(i + 1) % vertices.Count],
                        center
                    },
                    new Vector3[]{
                        -blade.normal,
                        -blade.normal,
                        -blade.normal
                    },
                    new Vector2[]{
                        newUV1,
                        newUV2,
                        new Vector2(0.5f, 0.5f)
                    },
                    -blade.normal,
                    left_side.subIndices.Count - 1 
                );

                right_side.AddTriangle(
                    new Vector3[]{
                        vertices[i],
                        vertices[(i + 1) % vertices.Count],
                        center
                    },
                    new Vector3[]{
                        blade.normal,
                        blade.normal,
                        blade.normal
                    },
                    new Vector2[]{
                        newUV1,
                        newUV2,
                        new Vector2(0.5f, 0.5f)
                    },
                    blade.normal,
                    right_side.subIndices.Count - 1 
                );
            }
        }
    }
}