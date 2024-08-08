using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipeline : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        float random_y = Random.Range(-2, 3);
        this.transform.localPosition += new Vector3(0, random_y, 0);
        Destroy(this.gameObject, 5f);
    }
    void Update()
    {
        this.transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
    }
}
