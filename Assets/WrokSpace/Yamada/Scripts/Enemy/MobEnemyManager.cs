using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Enemy
{
    public class MobEnemyManager : MonoBehaviour
    {
        // 生成した敵の親となるTransform
        Transform enemiesParentTransform;

        // 生成する敵のPrefabを格納するリスト
        // TODO. SerializeFieldを解除し、Resources.Loadで設定するようにする。
        [SerializeField] GameObject[] enemyPrefabList;

        // 生成した敵のクラスを格納するリスト
        List<MobEnemyController_Red> enemyList_Red = new List<MobEnemyController_Red>();

        // enemyList_*** たちへの参照を格納するリスト。CreateEnemy<T>で使用する敵のリストを特定するために必要
        List<System.Object> enemyListRefs = new List<System.Object>();


        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            CreateEnemies_FirstTime();
        }


        /// <summary>
        /// 最初に必要な量の敵を一括で生成する。
        /// </summary>
        private void CreateEnemies_FirstTime()
        {
            CreateParentObject();

            // enemyList_***を作り次第、この下に同じ形で追加していく。
            enemyListRefs.Add(enemyList_Red);

            // CreateEnemy_***を作り次第、この下に同じ形で追加していく。
            for (int i = 0; i < 20; i++)
            {
                CreateEnemy_Red();
            }
        }


        /// <summary>
        /// このオブジェクトの子に空オブジェクトを生成し、今後生成する敵の親オブジェクトとして設定する。
        /// </summary>
        private void CreateParentObject()
        {
            enemiesParentTransform = new GameObject().transform;
            enemiesParentTransform.parent = this.transform;
            enemiesParentTransform.name = "EnemiesParent";
        }


        /// <summary>
        /// ザコ妖精赤を生成/再利用する。
        /// </summary>
        /// <returns> 生成/再利用した敵のMobEnemyController_Red </returns>
        public MobEnemyController_Red CreateEnemy_Red()
        {
            MobEnemyController_Red CreatedEnemy = CreateEnemy<MobEnemyController_Red>();
            CreatedEnemy.Init();

            return CreatedEnemy;
        }


        /// <summary>
        /// Tで指定したクラスを持つ敵を生成/再利用する。
        /// </summary>
        /// <typeparam name="T"> MobEnemyBaseから派生したクラス </typeparam>
        /// <returns> 生成/再利用した敵の、Tで指定したクラス </returns>
        private T CreateEnemy<T>() where T : MobEnemyBase
        {
            // enemyListRefsからList<T>を見つける。
            List<T> targetList = enemyListRefs.OfType<List<T>>().ToList()[0];

            // 再利用可能なオブジェクトを検索し、見つかればそのクラスを返す。
            foreach (T type in targetList)
            {
                if (type.isActive == false)
                {
                    return type;
                }
            }

            // 見つからなかった場合、新規オブジェクトの作成に入る。
            GameObject toInstancePrefab = null;
            foreach (GameObject pref in enemyPrefabList)
            {
                // Tがアタッチされているかチェックし、されていればそれをインスタンスするPrefabにする。
                if (pref.TryGetComponent<T>(out T notUse))
                {
                    toInstancePrefab = pref;
                    break;
                }
            }

            // 見つからなければエラーを返す。
            if (toInstancePrefab == null)
            {
                Debug.LogError($"Object that has {typeof(T).Name} is not found. at: [{this.GetType().FullName}]");
                return null;
            }

            // 敵を生成し、そのクラスを管理リストに格納する。
            T toReturnClass = Instantiate(toInstancePrefab, parent: enemiesParentTransform).GetComponent<T>();
            targetList.Add(toReturnClass);

            // 通し番号があると見分けがついて便利なので付ける。
            toReturnClass.name = $"{toReturnClass.name} {targetList.Count}";

            return toReturnClass;
        }
    }
}