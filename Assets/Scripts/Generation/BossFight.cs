using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    [SerializeField] private float doorCloseTime;
    public List<GameObject> Bosses = new List<GameObject>();
    [SerializeField] private Altar altar;

    [SerializeField] private AudioSource doorSound;

    private void Start()
    {
        altar.OnInteract.AddListener(InitBoss);
    }

    private void BossDied()
    {
        DungeonGeneration.Instance.IncreaseLevel();
        DungeonGeneration.Instance.RegenerateDungeon();
        var pos = transform.position;
        pos.y = 0.5f;
        DungeonGeneration.Instance.OnDungeonCreate.AddListener(() => StartCoroutine(MoveDoor(pos, pos - Vector3.up * 1.2f)));
    }

    private void InitBoss()
    {
        var boss = Bosses[DungeonGeneration.Level];
        var bosshittable = Instantiate(boss, Altar.Instance.transform.position, Quaternion.identity).GetComponent<Hittable>();
        Altar.Instance.gameObject.SetActive(false);
        bosshittable.OnDie.AddListener(BossDied);
        bosshittable.OnDie.AddListener(() => { Altar.Instance.gameObject.SetActive(true); Altar.Instance.transform.position = bosshittable.transform.position; });
        var pos = transform.position;
        pos.y = - 0.7f;
        StartCoroutine(MoveDoor(pos, pos + Vector3.up * 1.2f));
    }

    private IEnumerator MoveDoor(Vector3 startPop, Vector3 endPos)
    {
        doorSound.Play();
        var startPos = startPop;
        var dist = (endPos - startPop).magnitude;
        var pathElapsed = 0.0f;
        var speed = (dist / doorCloseTime);
        while (true)
        {
            pathElapsed += speed * Time.deltaTime;
            var t = Mathf.Clamp01(pathElapsed / dist);
            transform.position = Vector3.Lerp(startPos, endPos, pathElapsed / dist);

            if (t >= 1.0f)
            {
                break;
            }

            yield return null;
        }
        yield break;
    }
}
