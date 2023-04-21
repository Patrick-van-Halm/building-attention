using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEmitter : MonoBehaviour
{
    [Header("Materials")]
    [Tooltip("Array of all possible colors for notes")] [SerializeField] private Material[] _materials;

    [Header("Prefab")]
    [Tooltip("The prefab that will be instantiated")] [SerializeField] private GameObject _note;


    private List<GameObject> _gameObjects;
    private int _minNotes = 10;
    private int _maxNotes = 15;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateNotes();
    }

    private void InstantiateNotes()
    {
        _gameObjects = CreateList();
        StartCoroutine(CoroEmitNotes(_gameObjects));
    }

    private List<GameObject> CreateList()
    {
        int randomNumber = Random.Range(_minNotes, _maxNotes);
        List<GameObject> notes = new List<GameObject>();

        for (int i = 0; i < randomNumber; i++)
        {
            notes.Add(_note);
        }

        return notes;
    }

    private void AddColor(GameObject note)
    {
        int randomNumber = Random.Range(0, _materials.Length);
        note.GetComponentInChildren<MeshRenderer>().sharedMaterial = _materials[randomNumber];
    }

    IEnumerator CoroEmitNotes(List<GameObject> gameObjects)
    {
        while (gameObjects.Count != 0)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                Instantiate(gameObjects[i]);
                AddColor(gameObjects[i]);
                gameObjects.Remove(gameObjects[i]);
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
}
