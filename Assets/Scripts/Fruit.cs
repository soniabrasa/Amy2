using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField]
    List<Material> fruitMaterials;

    [SerializeField]
    List<int> fruitPoints;

    [SerializeField]
    List<float> fruitTimeToLife;

    int fruitIndex;

    public int FruitPoints { get { return fruitPoints[fruitIndex]; } }

    void Start()
    {
        fruitIndex = Random.Range(0, fruitMaterials.Count);
        GetComponent<MeshRenderer>().material = fruitMaterials[fruitIndex];

        Destroy(gameObject, fruitTimeToLife[fruitIndex]);

        // Debug.Log($"{gameObject.name} Time to life {fruitTimeToLife[fruitIndex]}");
    }


    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDestroy.html
    void OnDestroy()
    {
        // Debug.Log($"{gameObject.name}.OnDestroy");

        GameManager.instance.RemoveFruit(gameObject);
    }
}
