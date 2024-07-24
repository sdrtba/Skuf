using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FabricHandler : MonoBehaviour
{
    [SerializeField] private RectTransform press;
    [SerializeField] private float pressSpeed;
    private float defY;
    private bool _canPress = false;

    [SerializeField] private GameObject[] objects;
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private RectTransform prefabsParentTransform;
    [SerializeField] private RectTransform waitingTransform;
    [SerializeField] private float prefabsSpeed;
    private RectTransform itemTransform;
    private List<int> id = new List<int>();
    private int index = 0;

    private void Start()
    {
        SkufHandler.instance.SetHUDVisibility(false);
        itemTransform = prefabs[index].GetComponent<RectTransform>();

        defY = press.position.y;

        int rand;
        for (int i = 0; i < prefabs.Length; i++)
        {
            rand = Random.Range(0, items.Length);
            id.Add(rand);
            Instantiate(items[rand], prefabs[i].transform);
        }

        StartCoroutine(MoveLine());

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canPress)
        {
            StartCoroutine(Press());
        }
    }

    private IEnumerator MoveLine()
    {
        while (itemTransform.position.x > waitingTransform.position.x)
        {
            prefabsParentTransform.position -= new Vector3(prefabsSpeed * Time.deltaTime, 0f, 0f);
            yield return null;
        }
        _canPress = true;
    }

    private IEnumerator Press()
    {
        _canPress = false;
        while (press.position.y > waitingTransform.position.y)
        {
            press.position -= new Vector3(0, pressSpeed * Time.deltaTime, 0);
            yield return null;
        }

        Destroy(itemTransform.GetChild(0).gameObject);
        Instantiate(objects[id[0]], itemTransform);
        id.RemoveAt(0);

        index += 1;
        index %= 10;
        itemTransform = prefabs[index].GetComponent<RectTransform>();

        while (press.position.y < defY)
        {
            press.position += new Vector3(0, pressSpeed * Time.deltaTime, 0);
            yield return null;
        }

        StartCoroutine(MoveLine());
    }
}
