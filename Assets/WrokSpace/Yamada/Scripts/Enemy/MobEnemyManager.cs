using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyManager : MonoBehaviour
    {
        // 生成した敵の親となるTransform
        private Transform enemiesParentTransform;

        // 生成する敵のPrefabを格納するリスト
        // TODO. SerializeFieldを解除し、Resources.Loadで設定するようにする。
        [SerializeField]
        private GameObject[] enemyPrefabList;

        // 生成した敵のクラスを格納するリスト
        private List<MobEnemyController_Red> enemyList_Red = new List<MobEnemyController_Red>();
        private List<MobEnemyController_Blue> enemyList_Blue = new List<MobEnemyController_Blue>();
        private List<MobEnemyController_Yellow> enemyList_Yellow = new List<MobEnemyController_Yellow>();
        private List<MobEnemyController_Purple> enemyList_Purple = new List<MobEnemyController_Purple>();

        // enemyList_*** たちへの参照を格納するリスト。CreateEnemy<T>で使用する敵のリストを特定するために必要
        private List<System.Object> enemyListRefs = new List<System.Object>();

        // 未使用の敵が待機する座標
        [System.NonSerialized]
        public Vector2 enemyWaitingPosition = new Vector2(500, 500);


        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            CreateParentObject();
            CreateEnemies_FirstTime();
        }


        /// <summary>
        /// 最初に必要な量の敵を一括で生成する。
        /// </summary>
        private void CreateEnemies_FirstTime()
        {
            // enemyList_***を作り次第、この下に同じ形で追加していく。
            enemyListRefs.Add(enemyList_Red);
            enemyListRefs.Add(enemyList_Blue);
            enemyListRefs.Add(enemyList_Yellow);
            enemyListRefs.Add(enemyList_Purple);
            var tempEnemyListToDisable_Red = new List<MobEnemyController_Red>();
            var tempEnemyListToDisable_Blue = new List<MobEnemyController_Blue>();
            var tempEnemyListToDisable_Yellow = new List<MobEnemyController_Yellow>();
            var tempEnemyListToDisable_Purple = new List<MobEnemyController_Purple>();

            // 生成する敵の数を決める。
            int createAmount = 1;

            // CreateEnemy_***を作り次第、この下に同じ形で追加していく。
            for (int i = 0; i < createAmount; i++)
            {
                tempEnemyListToDisable_Red.Add(CreateEnemy_Red());
                tempEnemyListToDisable_Blue.Add(CreateEnemy_Blue());
                tempEnemyListToDisable_Yellow.Add(CreateEnemy_Yellow());
                tempEnemyListToDisable_Purple.Add(CreateEnemy_Purple());
            }

            // ここにも適宜同じ形で追加する。
            for (int i = 0; i < createAmount; i++)
            {
                tempEnemyListToDisable_Red[i].Disable();
                tempEnemyListToDisable_Blue[i].Disable();
                tempEnemyListToDisable_Yellow[i].Disable();
                tempEnemyListToDisable_Purple[i].Disable();
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
            MobEnemyController_Red createdEnemy = CreateEnemy<MobEnemyController_Red>();
            createdEnemy.Init();

            return createdEnemy;
        }


        /// <summary>
        /// ザコ妖精青を生成/再利用する。
        /// </summary>
        /// <returns> 生成/再利用した敵のMobEnemyController_Blue </returns>
        public MobEnemyController_Blue CreateEnemy_Blue()
        {
            MobEnemyController_Blue createdEnemy = CreateEnemy<MobEnemyController_Blue>();
            createdEnemy.Init();

            return createdEnemy;
        }


        /// <summary>
        /// ザコ妖精黄を生成/再利用する。
        /// </summary>
        /// <returns> 生成/再利用した敵のMobEnemyController_Yellow </returns>
        public MobEnemyController_Yellow CreateEnemy_Yellow()
        {
            MobEnemyController_Yellow createdEnemy = CreateEnemy<MobEnemyController_Yellow>();
            createdEnemy.Init();

            return createdEnemy;
        }


        /// <summary>
        /// ザコ妖精紫を生成/再利用する。
        /// </summary>
        /// <returns> 生成/再利用した敵のMobEnemyController_Purple </returns>
        public MobEnemyController_Purple CreateEnemy_Purple()
        {
            MobEnemyController_Purple createdEnemy = CreateEnemy<MobEnemyController_Purple>();
            createdEnemy.Init();

            return createdEnemy;
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
            T toReturnClass = Instantiate(toInstancePrefab, enemyWaitingPosition, Quaternion.identity, enemiesParentTransform).GetComponent<T>();
            targetList.Add(toReturnClass);

            // 通し番号があると見分けがついて便利なので付ける。
            toReturnClass.name = $"{toReturnClass.name} {targetList.Count}";

            return toReturnClass;
        }
    }
}