using UnityEngine;
using Enemy;

public class TestPlayerController : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * 0.1f, Input.GetAxisRaw("Vertical") * 0.1f, 0));

        if (Input.GetKeyDown(KeyCode.Space))
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));

        DebugKeyInput();
    }


    public void Dead()
    {
        Debug.Log("Dead");
    }


    private void DebugKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var refEM = (MobEnemyManager)FindObjectOfType<MobEnemyManager>();
            var mobR = refEM.CreateEnemy_Red();
            mobR.transform.position = new Vector2(-4, 2);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            var refEM = (MobEnemyManager)FindObjectOfType<MobEnemyManager>();
            var mobB = refEM.CreateEnemy_Blue();
            mobB.transform.position = new Vector2(0, 2);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            var refEM = (MobEnemyManager)FindObjectOfType<MobEnemyManager>();
            var mobY = refEM.CreateEnemy_Yellow();
            mobY.transform.position = new Vector2(0, 2);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            var refEM = (MobEnemyManager)FindObjectOfType<MobEnemyManager>();
            var mobG = refEM.CreateEnemy_Green();
            mobG.transform.position = new Vector2(4, 2);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var refEM = (MobEnemyManager)FindObjectOfType<MobEnemyManager>();
            var mobP = refEM.CreateEnemy_Purple();
            mobP.transform.position = new Vector2(-4, -2);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            var refEM = (MobEnemyManager)FindObjectOfType<MobEnemyManager>();
            var mobW = refEM.CreateEnemy_White();
            mobW.transform.position = new Vector2(-4, -2);
        }

    }
}
