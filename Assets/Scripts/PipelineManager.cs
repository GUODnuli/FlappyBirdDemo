using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineManager : MonoBehaviour
{
    public GameObject Pipeline_template;
    Coroutine coroutine = null;

    public void StartGenerate()
    {
        coroutine = StartCoroutine(GeneratePipelines());
    }

    public void StopGenerate()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator GeneratePipelines()
    {
        while(true)
        {
            GeneratePipeline();

            yield return new WaitForSeconds(2f);
        }
    }

    void GeneratePipeline()
    {
        Instantiate(Pipeline_template, this.transform);
    }
}
