using UnityEngine;

namespace ArowMain.Public.Scripts.Runtime
{
/// <summary>
/// Prefab の高さ情報を保持する。
/// 地面の標高 + Height の位置に Prefab を設置するとちょうど接地する。
/// </summary>
public class HavePrefabSize : MonoBehaviour
{
    [SerializeField] public float Height = 0f;
}
}
