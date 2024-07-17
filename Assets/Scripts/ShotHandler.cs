using System.Collections;
using UnityEngine;

public class ShotHandler : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Delete());
    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
