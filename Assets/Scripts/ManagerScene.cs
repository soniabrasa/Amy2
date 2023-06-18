using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    [SerializeField]
    GameObject elevator;

    [SerializeField]
    Light iluminacion;

    bool fullLevel;

    public static ManagerScene instance;

    public bool FullLevel { get { return fullLevel; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;

        fullLevel = false;
    }


    void Start()
    {
        InitializeLevel();
    }


    void Update()
    {
    }

    void InitializeLevel()
    {
        elevator = GameObject.Find("Elevator");
        iluminacion = GameObject.Find("Directional Light").GetComponent<Light>();
        elevator.SetActive(false);
    }


    void LoadNextLevel()
    {
        SceneManager.LoadScene("AmyPlataformas");
    }

    public void ActiveElevator()
    {
        elevator.SetActive(true);
    }

    public void GoNewLevel(Vector3 elevatorPosition)
    {
        Debug.Log($"{gameObject.name}.GoNewLevel({elevatorPosition})");

        fullLevel = true;

        StartCoroutine(FadeLight());

        GameObject player = GameObject.Find("Amy");

        player.GetComponent<PlayerCs>().ActivateTeleport(elevatorPosition);

        // Invoke("LoadNexLevel", 3f);
    }

    public IEnumerator FadeLight()
    {
        while (iluminacion.intensity > 0.02f)
        {
            iluminacion.intensity -= 0.004f;

            yield return new WaitForSeconds(0.01f);
        }

        Invoke("LoadNextLevel", 1f);
    }
}
