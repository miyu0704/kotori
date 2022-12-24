using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBulletManager : MonoBehaviour
    {
        // 生成た弾の親となるTransform
        private Transform bulletsParentTransform;

        // 生成する弾のPrefabを格納するリスト
        // TODO. SeiralizeFieldを解除し、Resources.Loadで設定するようにする。
        [SerializeField]
        private GameObject[] bulletPrefabList;

        // 生成した弾のクラスを管理するリスト。Prefabを追加した際に、下に同じ形で追加する。
        private List<EnemyBulletController_Red> bulletList_Red = new List<EnemyBulletController_Red>();
        private List<EnemyBulletController_Blue> bulletList_Blue = new List<EnemyBulletController_Blue>();

        // bulletList_*** たちへの参照を格納するリスト。CreateBullet<T>で使用する弾のリストを特定するために必要。
        private List<System.Object> bulletListRefs = new List<System.Object>();

        // 未使用の弾が待機する座標
        [System.NonSerialized]
        public Vector2 bulletWaitingPosition = new Vector2(500, 500);


        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            CreateParentObject();
            CreateBullets_FirstTime();
        }


        /// <summary>
        /// 最初に必要な量の弾を一括で生成する。
        /// </summary>
        private void CreateBullets_FirstTime()
        {
            // bulletList_***を作り次第、この下に同じ形で追加していく。
            bulletListRefs.Add(bulletList_Red);
            bulletListRefs.Add(bulletList_Blue);
            var tempBulletListToDisable_Red = new List<EnemyBulletController_Red>();
            var tempBulletListToDisable_Blue = new List<EnemyBulletController_Blue>();

            // 生成する弾の数を決める。
            int createAmount = 20;

            // CreateBullet_***を作り次第、この下に同じ形で追加していく。
            for (int i = 0; i < createAmount; i++)
            {
                tempBulletListToDisable_Red.Add(CreateBullet_Red());
                tempBulletListToDisable_Blue.Add(CreateBullet_Blue());
            }

            // ここにも適宜同じ形で追加する。
            for (int i = 0; i < createAmount; i++)
            {
                tempBulletListToDisable_Red[i].Disable();
                tempBulletListToDisable_Blue[i].Disable();
            }
        }

        /// <summary>
        /// このオブジェクトの子に空オブジェクトを生成し、今後生成する弾の親オブジェクトとして設定する。
        /// </summary>
        private void CreateParentObject()
        {
            bulletsParentTransform = new GameObject().transform;
            bulletsParentTransform.parent = this.transform;
            bulletsParentTransform.name = "BulletsParent";
        }

        /// <summary>
        /// 敵弾赤を生成/再利用する。
        /// </summary>
        /// <returns> 生成/再利用した弾のEnemyBulletController_Red< /returns>
        public EnemyBulletController_Red CreateBullet_Red()
        {
            EnemyBulletController_Red createdBullet = CreateBullet<EnemyBulletController_Red>();

            return createdBullet;
        }


        /// <summary>
        /// 敵弾赤を生成/再利用する。
        /// </summary>
        /// <returns> 生成/再利用した弾のEnemyBulletController_Red< /returns>
        public EnemyBulletController_Blue CreateBullet_Blue()
        {
            EnemyBulletController_Blue createdBullet = CreateBullet<EnemyBulletController_Blue>();

            return createdBullet;
        }

        /// <summary>
        /// Tで指定したクラスを持つ弾を生成/再利用する。
        /// </summary>
        /// <typeparam name="T"> EnemyBulletBaseから派生したクラス </typeparam>
        /// <returns> 生成/再利用した弾の、Tで指定したクラス </returns>
        private T CreateBullet<T>() where T : EnemyBulletBase
        {
            // bulletListRefsからList<T>を見つける。
            List<T> targetList = bulletListRefs.OfType<List<T>>().ToList()[0];

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
            foreach (GameObject pref in bulletPrefabList)
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
                Debug.LogError($"Object that has {typeof(T).Name} is not found. at [{this.GetType().FullName}]");
                return null;
            }

            // 弾を生成し、そのクラスを管理リストに格納する。
            T toReturnClass = Instantiate(toInstancePrefab, bulletWaitingPosition, Quaternion.identity, bulletsParentTransform).GetComponent<T>();
            targetList.Add(toReturnClass);

            // 通し番号があると見分けがついて便利なので付ける。
            toReturnClass.name = $"{toReturnClass.name} {targetList.Count}";

            return toReturnClass;
        }
    }
}