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
        // startSpawnCount ���� ��ŭ ������ ������Ʈ�� ��ȯ�ϰ� ��Ȱ��ȭ��Ŀ �ݴϴ�.
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
        // dieUI�� Ȱ��ȭ �����̸� �÷��̾ ���� �����̱� ������ ������ ������ ���� �ʽ��ϴ�.
        if (dieUI.activeSelf)
            return;

        // startUI�� ���� ������ ���� ���� ���̱� ������ ���� ������ �� �� (w Ű�� ���� ��) ���� ������ ������ ���� �ʽ��ϴ�.
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

            // �� �̵�
            Mover();
            // �� ��ȯ
            Spawner();
        }
        else
        {
            // ���� �� ������
            Stoping();
        }
        // UI ����
        UIRender();
        // �÷��̾� ��, �� �̵�
        playerLeftRight();
    }
    // ���� �� ������
    private void Stoping()
    {
        stopTime += Time.deltaTime;
        if (stopTime >= maxStopTime)
        {
            Die();
        }
    }
    // UI ����
    private void UIRender()
    {
        scoreText.text = score.ToString();
        Warring.color = new Color32(255, 0, 0, (byte)((stopTime / maxStopTime) * 255));
    }
    // �÷��̾� ��, �� �̵�
    private void playerLeftRight()
    {
        if (Input.GetKey(KeyCode.A) && player.transform.position.x > -mapLimit.x)
            player.Translate(Vector2.left * playerMoveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D) && player.transform.position.x < mapLimit.x)
            player.Translate(Vector2.right * playerMoveSpeed * Time.deltaTime);
    }
    // �� ��ȯ
    private void Spawner()
    {
        spawnTimer += Time.deltaTime;
        for (int i = 0; i < allEnemy.childCount; ++i)
        {  
            if (allEnemy.GetChild(i).gameObject.activeSelf)
                continue;

            // ��ȯ�� ���� �Ǹ� ��ȯ�մϴ�.
            if (spawnTimer >= spawnDelay)
            {
                spawnTimer = 0;

                // ���̵� ����� ���� �ð��� ���� ���� �� ��ȯ �����̰� �پ�鵵�� �����߽��ϴ�.
                if (spawnDelay > spawnMinDelay)
                    spawnDelay -= spawnDelayMinus;

                // map�� �ִ� �� �ϳ��� Ȱ��ȭ���� �÷��̾ ������ ��, ���� ��ȯ�Ǵ� ��ó�� �����մϴ�.
                allEnemy.GetChild(i).gameObject.SetActive(true);
                allEnemy.GetChild(i).position = new Vector2(Random.Range(-mapLimit.x, mapLimit.x), mapLimit.y);
            }
            else
                break;
        }
    }
    // �� �̵�
    private void Mover()
    {
        for (int i = 0; i < allEnemy.childCount; ++i)
        {
            // ��� Ȱ��ȭ�� ������Ʈ�� ������ �̵��մϴ�.
            if (allEnemy.GetChild(i).gameObject.activeSelf)
                allEnemy.GetChild(i).Translate(Vector2.down * playerMoveSpeed * Time.deltaTime);

            // �� ������ ������ ��Ȱ��ȭ�˴ϴ�.
            if (allEnemy.GetChild(i).position.y < -mapLimit.y)
                resetEnemy(allEnemy.GetChild(i));

            // ���� �÷��̾� ��ġ�� ������ ����ϴ�.
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
