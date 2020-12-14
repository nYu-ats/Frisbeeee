using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLINDED_AM_ME
{
    public class MeshCut : MonoBehaviour
    {
        public class MeshCutSide
        {
            public List<Vector3>  vertices  = new List<Vector3>();
            public List<Vector3>  normals   = new List<Vector3>();
            public List<Vector2>  uvs       = new List<Vector2>();
            public List<int>      triangles = new List<int>();
            public List<List<int>> subIndices = new List<List<int>>();

            //初期化
            public void ClearAll()
            {
                vertices.Clear();
                normals.Clear();
                uvs.Clear();
                triangles.Clear();
                subIndices.Clear();
            }

            //カット後のmeshを左と右それぞれに追加するメソッド
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

            //AddTriangleメソッドをオーバーロード、FillCapで利用
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

        //カット後のメッシュの入れ物を左右それぞれ作る
        private static MeshCutSide left_side = new MeshCutSide();
        private static MeshCutSide right_side = new MeshCutSide();
        //カットする平面
        private static Plane blade;
        //カット対象のメッシュ
        private static Mesh victim_mesh;
        //切断により新たに生成される頂点を格納するための配列
        private static List<Vector3> new_vertices = new List<Vector3>();

        //カット処理
        public static GameObject[] Cut(GameObject victim, Vector3 anchorPoint, Vector3 normalDirection, Material capMaterial, Material cuttedMaterial, float globalPosisionZ)
        {
            //法線とグローバル座標を元にカットする平面を決定
            blade = new Plane(
                victim.transform.InverseTransformDirection(-normalDirection),
                victim.transform.InverseTransformPoint(anchorPoint)
            );
            //カット対象のメッシュを格納
            victim_mesh = victim.GetComponent<MeshFilter>().mesh;
            //カット後の左右のオブジェクト、新頂点リストを初期化
            new_vertices.Clear();
            left_side.ClearAll();
            right_side.ClearAll();

            bool[] sides = new bool[3];//三角形を構成する各頂点がカット平面の左右どちらにあるかを表す配列
            int[] indices;//サブメッシュのインデックスを格納する配列
            int p1,p2,p3;

            /*
            複数マテリアルが付けられている場合、サブメッシュがマテリアルと同じ数だけある
            なのでサブメッシュの数だけ振り分け処理をループさせる
            */
            for (int sub = 0; sub < victim_mesh.subMeshCount; sub++)
            {
                indices = victim_mesh.GetIndices(sub);
                left_side.subIndices.Add(new List<int>());  // 左側に振り分けられるサブメッシュ
                right_side.subIndices.Add(new List<int>()); // 右側に振り分けられるサブメッシュ

                /*
                頂点がカット平面の左右のどちらに存在するかによって左側 or 右側に振り分ける
                三角形の頂点が平面をまたぐ場合、カット処理を行う
                */
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
                        Cut_this_Face(sub, sides, p1, p2, p3); //三角形を構成する頂点がカット面の左右それぞれに存在していた場合にカット処理
                    }
                }
            }

            Material[] mats = victim.GetComponent<MeshRenderer>().sharedMaterials; //対象オブジェクトを構成するマテリアルの配列を取得
            
            //カット面を埋めるためのマテリアルを追加する
            if (mats[mats.Length - 1].name != capMaterial.name)
            {
                //カット面用のサブメッシュ追加
                left_side.subIndices.Add(new List<int>());
                right_side.subIndices.Add(new List<int>());
                Material[] newMats = new Material[mats.Length + 1]; //サブメッシュと配列要素数を一致させるため
                mats.CopyTo(newMats, 0);
                newMats[mats.Length] = capMaterial;
                mats = newMats;
            }

            Capping(); 

            //ここまでの処理でカットにより新しくできた左右それぞれのメッシュを再セット
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

            //カット後の左右2つのオブジェクトを新規で生成する
            //左側
            var cloneleft = Object.Instantiate(victim) as GameObject;
            cloneleft.name = "left side";
            cloneleft.transform.position = new Vector3(cloneleft.transform.position.x, cloneleft.transform.position.y, cloneleft.transform.position.z + globalPosisionZ);  //親オブジェクトが変わるので、その分Z方向の位置を調整
            cloneleft.GetComponent<MeshFilter>().mesh = left_HalfMesh;
            AfterCutSetting(cloneleft, cuttedMaterial, true);

            //右側
            var cloneright = Object.Instantiate(victim) as GameObject;
            cloneright.name = "right side";
            cloneright.transform.position = new Vector3(cloneright.transform.position.x, cloneright.transform.position.y, cloneright.transform.position.z + globalPosisionZ);
            cloneright.GetComponent<MeshFilter>().mesh = right_HalfMesh;
            AfterCutSetting(cloneright, cuttedMaterial, true);

            return new GameObject[]{cloneleft, cloneright };
        }

        //カット処理後のポールの性質をセット
        static void AfterCutSetting(GameObject cuttedObj, Material cuttedMaterial, bool rotateFlag)
        {
            Destroy(cuttedObj.GetComponent<CapsuleCollider>()); //colliderのサイズを新規のオブジェクトに合わせるためにDestroy->再セット
            cuttedObj.AddComponent<CapsuleCollider>().isTrigger = false;  //カット後のオブジェクトは再度カットできないようにするためにトリガーはfalse
            Destroy(cuttedObj.transform.GetChild(0).gameObject); //カット前に子オブジェクトとして含んでいるポイントライトを消去する
            cuttedObj.GetComponent<MeshRenderer>().material = cuttedMaterial;
            cuttedObj.GetComponent<Rigidbody>().useGravity = true; //カット後は重力落下させたいのでtrueに
            cuttedObj.tag = "Disactive";
        }
        
        //頂点を左右に振り分け、各辺とカット面の接点を新頂点としてセットする
        static void Cut_this_Face(int submesh, bool[] sides, int index1, int index2, int index3)
        {
            //左右それぞれの情報を保持するための配列
            Vector3[] leftPoints = new Vector3[2];
            Vector3[] leftNormals = new Vector3[2];
            Vector2[] leftUvs = new Vector2[2];
            Vector3[] rightPoints = new Vector3[2];
            Vector3[] rightNormals = new Vector3[2];
            Vector2[] rightUvs = new Vector2[2];

            bool didset_left = false;
            bool didset_right = false;

            //三角形の3頂点を左右に振り分ける
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
                    //左側
                    //3頂点が左右に振り分けられるため、左右いずれかは2つの頂点を持つはず
                    if (!didset_left)
                    {
                        didset_left = true;
                        //1頂点しかなかった場合にleftPoints[0, 1]の値を使って分割点を求めるため、ここで同じ値をセットしている
                        leftPoints[0]  = victim_mesh.vertices[p];
                        leftPoints[1]  = leftPoints[0];

                        leftUvs[0]     = victim_mesh.uv[p];
                        leftUvs[1]     = leftUvs[0];

                        leftNormals[0] = victim_mesh.normals[p];
                        leftNormals[1] = leftNormals[0];
                    }
                    else
                    {
                        //2頂点目の場合はleftPoints[1]に直接値を入れる
                        leftPoints[1]  = victim_mesh.vertices[p];
                        leftUvs[1]     = victim_mesh.uv[p];
                        leftNormals[1] = victim_mesh.normals[p];
                    }
                }
                else
                {
                    //右側
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

            blade.Raycast(new Ray(leftPoints[0], (rightPoints[0] - leftPoints[0]).normalized), out distance); //左->右へレイを飛ばして交差点を見つける
            normalizedDistance = distance / (rightPoints[0] - leftPoints[0]).magnitude; //見つかった交差点を頂点間の距離で割って左右の分割割合を計算する
            //カット後の新頂点を求めて追加する
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

            //カットにより追加された新頂点から、新しいトライアングルを追加する
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

        //実際のカットにあたる処理
        static void Capping()
        {
            //カット面の重複頂点を排除するために新頂点を全て調査する
            //調査済みの頂点を格納するためのリスト
            capVertTracker.Clear();
            //新頂点分だけループ
            for (int i = 0; i < new_vertices.Count; i++)
            {
                //調査済みの場合はスキップ
                if (capVertTracker.Contains(new_vertices[i]))
                {
                    continue;
                }

                //重複頂点を排除した頂点リストを初期化
                capVertpolygon.Clear();

                //調査頂点と次の頂点をポリゴン配列に保持
                capVertpolygon.Add(new_vertices[i + 0]);
                capVertpolygon.Add(new_vertices[i + 1]);

                //調査済み頂点として追加
                capVertTracker.Add(new_vertices[i + 0]);
                capVertTracker.Add(new_vertices[i + 1]);

                bool isDone = false;
                while (!isDone)
                {
                    isDone = true;

                    //2頂点ごとに調査をする
                    //1トライアングルからは必ず2頂点が生成されており、1つの頂点に対してほぼ同じ位置に存在する新頂点が存在するため
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

                FillCap(capVertpolygon); //精査済みの頂点群を元にポリゴンを形成する
            }
        }

        //カット面を埋める
        static void FillCap(List<Vector3> vertices)
        {
            Vector3 center = Vector3.zero; //カット面の中心点をいれる変数
            //新頂点の位置を全て合計
            foreach(Vector3 point in vertices)
            {
                center += point;
            }
            //合計値を頂点数で割って中心点を計算する
            center = center / vertices.Count;
            //カット面の左側を上方向にする
            Vector3 upward = Vector3.zero;
            upward.x =  blade.normal.y;
            upward.y = -blade.normal.x;
            upward.z =  blade.normal.z;

            Vector3 left = Vector3.Cross(blade.normal, upward); //法線と上方向から横軸を算出

            Vector3 displacement = Vector3.zero;
            Vector3 newUV1 = Vector3.zero;
            Vector3 newUV2 = Vector3.zero;

            for (int i = 0; i < vertices.Count; i++)
            {
                displacement = vertices[i] - center; //中心点から各頂点へのベクトル
                //UVの位置を決める
                newUV1 = Vector3.zero;
                newUV1.x = 0.5f + Vector3.Dot(displacement, left);
                newUV1.y = 0.5f + Vector3.Dot(displacement, upward);
                newUV1.z = 0.5f + Vector3.Dot(displacement, blade.normal);

                displacement = vertices[(i + 1) % vertices.Count] - center;
                newUV2 = Vector3.zero;
                newUV2.x = 0.5f + Vector3.Dot(displacement, left);
                newUV2.y = 0.5f + Vector3.Dot(displacement, upward);
                newUV2.z = 0.5f + Vector3.Dot(displacement, blade.normal);

                //左側のポリゴンとして、トライアングルを追加
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
                //右側
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