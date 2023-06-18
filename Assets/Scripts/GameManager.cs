using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject fruitPrefab;

    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    AudioClip fruitSpawned, fruitCaptured;

    List<GameObject> fruits;


    int totalFruits, totalPoints, targetPoints;

    float fruitMaxPositionToPlane, fruitMinPositionToPlayer, fruitDistanceCaptured;

    AudioSource audioSource;

    public static GameManager instance;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }


    void Start()
    {
        playerTransform = GameObject.Find("Amy").transform;
        audioSource = GetComponent<AudioSource>();

        ClearFruits();

        StartCoroutine(CoSpawnFruit());

        // No destruyas el objeto de destino al cargar una nueva escena
        DontDestroyOnLoad(this);
    }


    void Update()
    {
        if (fruits.Count > 0)
        {
            foreach (GameObject fruit in fruits)
            {
                if (Vector3.Distance(fruit.transform.position, playerTransform.position) < fruitDistanceCaptured)
                {
                    int points = fruit.GetComponent<Fruit>().FruitPoints;

                    totalPoints += points;

                    Debug.Log($"{gameObject.name} Capture fruit {fruit.name} by {points} Points. Current Points {totalPoints}");

                    audioSource.PlayOneShot(fruitCaptured);
                    Destroy(fruit);
                }
            }

            if (totalPoints >= targetPoints)
            {
                ManagerScene.instance.ActiveElevator();
            }
        }
    }

    void ClearFruits()
    {
        totalPoints = 0;
        totalFruits = 0;
        targetPoints = 200;
        fruitMaxPositionToPlane = 8f;
        fruitMinPositionToPlayer = 4f;
        fruitDistanceCaptured = 1.5f;

        fruits = new List<GameObject>();
    }


    void SpawnFruit(Vector3 position)
    {
        // Debug.Log($"{gameObject.name}.SpawnFruit Position {position}");
        audioSource.PlayOneShot(fruitSpawned);

        // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
        GameObject fruit = Instantiate(fruitPrefab, position, Quaternion.identity);
        totalFruits++;

        fruit.name = "Fruit_" + totalFruits;
        fruits.Add(fruit);
    }


    IEnumerator CoSpawnFruit()
    {
        while (!ManagerScene.instance.FullLevel)
        {
            // Random.Range (min, max) > probability
            if (Random.Range(0f, 1f) < 0.05f)
            {
                float x = Random.Range(-fruitMaxPositionToPlane, fruitMaxPositionToPlane + 0.01f);
                float z = Random.Range(-fruitMaxPositionToPlane, fruitMaxPositionToPlane + 0.01f);

                Vector3 position = new Vector3(x, 1f, z);

                // Vector3.Distance(a,b) es lo mismo que (a-b).magnitude.
                while (Vector3.Distance(position, playerTransform.position) < fruitMinPositionToPlayer)
                {
                    position.x = Random.Range(-fruitMaxPositionToPlane, fruitMaxPositionToPlane + 0.01f);
                    position.z = Random.Range(-fruitMaxPositionToPlane, fruitMaxPositionToPlane + 0.01f);
                }

                SpawnFruit(position);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }


    public void RemoveFruit(GameObject fruit)
    {
        // List<T>.Remove(T) Method
        fruits.Remove(fruit);
    }


    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), $"Puntos: {totalPoints}");
    }
}
