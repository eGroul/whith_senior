using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Map")]
    [SerializeField]
    private Vector2 mapLimit;

    [Header("EnemySpawn")]
    [SerializeField]
    private Transform allEnemy;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private short startSpawnCount;
    [SerializeField]
    private float spawnDelay;
    [SerializeField]
    private float spawnDelayMinus;
    [SerializeField]
    private float spawnMinDelay;
    private float spawnTimer;

    [Header("Player")]
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float playerMoveSpeed;

    [Header("Game")]
    [SerializeField]
    private Image Warring;
    [SerializeField]
    private GameObject startUI;
    [SerializeField]
    private GameObject dieUI;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int score = 0;
    [SerializeField]
    private float maxStopTime;
    private float stopTime;

    private void Start()
    {
        // startSpawnCount 개수 만큼 재사용할 오브젝트르 소환하고 비활성화시커 줍니다.
        for(int i = 0; i < startSpawnCount; ++i)
        {
            GameObject enemy = Instantiate(enemyPrefab, allEnemy.position, Quaternion.identity);
            enemy.transform.parent = allEnemy.transform;
            enemy.SetActive(false);
            Time.timeScale = 0;
        }
    }
    private void Update()
    {
        // dieUI가 활성화 상태이면 플레이어가 죽은 상태이기 떄문에 게임의 진행을 하지 않습니다.
        if (dieUI.activeSelf)
            return;

        // startUI가 켜져 있으면 게임 시작 전이기 때문에 게임 시작이 될 때 (w 키를 누를 때) 까지 게임의 진행을 하지 않습니다.
        if (startUI.activeSelf)
        {
            if (Input.GetKey(KeyCode.W))
            {
                startUI.SetActive(false);
                Time.timeScale = 1;
            }
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            stopTime = 0;

            // 적 이동
            Mover();
            // 적 소환
            Spawner();
        }
        else
        {
            // 멈출 시 숨참기
            Stoping();
        }
        // UI 적용
        UIRender();
        // 플레이어 좌, 우 이동
        playerLeftRight();
    }
    // 멈출 시 숨참기
    private void Stoping()
    {
        stopTime += Time.deltaTime;
        if (stopTime >= maxStopTime)
        {
            Die();
        }
    }
    // UI 적용
    private void UIRender()
    {
        scoreText.text = score.ToString();
        Warring.color = new Color32(255, 0, 0, (byte)((stopTime / maxStopTime) * 255));
    }
    // 플레이어 좌, 우 이동
    private void playerLeftRight()
    {
        if (Input.GetKey(KeyCode.A) && player.transform.position.x > -mapLimit.x)
            player.Translate(Vector2.left * playerMoveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D) && player.transform.position.x < mapLimit.x)
            player.Translate(Vector2.right * playerMoveSpeed * Time.deltaTime);
    }
    // 적 소환
    private void Spawner()
    {
        spawnTimer += Time.deltaTime;
        for (int i = 0; i < allEnemy.childCount; ++i)
        {  
            if (allEnemy.GetChild(i).gameObject.activeSelf)
                continue;

            // 소환할 때가 되면 소환합니다.
            if (spawnTimer >= spawnDelay)
            {
                spawnTimer = 0;

                // 난이도 향상을 위해 시간이 지날 수록 적 소환 딜레이가 줄어들도록 설정했습니다.
                if (spawnDelay > spawnMinDelay)
                    spawnDelay -= spawnDelayMinus;

                // map에 있는 적 하나를 활성화시켜 플레이어가 보았을 때, 적이 소환되는 것처럼 연출합니다.
                allEnemy.GetChild(i).gameObject.SetActive(true);
                allEnemy.GetChild(i).position = new Vector2(Random.Range(-mapLimit.x, mapLimit.x), mapLimit.y);
            }
            else
                break;
        }
    }
    // 적 이동
    private void Mover()
    {
        for (int i = 0; i < allEnemy.childCount; ++i)
        {
            // 모든 활성화된 오브젝트가 밑으로 이동합니다.
            if (allEnemy.GetChild(i).gameObject.activeSelf)
                allEnemy.GetChild(i).Translate(Vector2.down * playerMoveSpeed * Time.deltaTime);

            // 맵 밖으로 나가면 비활성화됩니다.
            if (allEnemy.GetChild(i).position.y < -mapLimit.y)
                resetEnemy(allEnemy.GetChild(i));

            // 적이 플레이어 위치면 점수를 얻습니다.
            if (allEnemy.GetChild(i).position.y <= player.position.y)
            {
                SpriteRenderer enemySprite = allEnemy.GetChild(i).GetComponent<SpriteRenderer>();
                Color32 originColor = enemyPrefab.GetComponent<SpriteRenderer>().color;
                if (enemySprite.color == originColor)
                {
                    score++;
                    enemySprite.color = new Color32(originColor.r, originColor.g, originColor.b, 100);
                }    
            }
    }
}
    private void resetEnemy(Transform enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.position = allEnemy.position;
        enemy.transform.GetComponent<SpriteRenderer>().color = enemyPrefab.GetComponent<SpriteRenderer>().color;
    }
    public void Die()
    {
        dieUI.SetActive(true);
    }
    public void ResetButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
