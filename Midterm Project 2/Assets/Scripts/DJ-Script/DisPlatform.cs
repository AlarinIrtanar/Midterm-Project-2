using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisPlatform : MonoBehaviour
{
    [SerializeField] float appearTime;
    [SerializeField] float disappearTime;

    [SerializeField] Renderer PlatRenderer;
    [SerializeField] new Collider collider;


    // Start is called before the first frame update
    void Start()
    {
        PlatRenderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();

        StartCoroutine(DisappearandAppear());
        
    }

    // Update is called once per frame
    IEnumerator DisappearandAppear()
    {
        while (true)
        {
            yield return new WaitForSeconds(disappearTime);

            PlatRenderer.enabled = false;
            collider.enabled = false ;

            yield return new WaitForSeconds(appearTime);

            PlatRenderer.enabled =true;
            collider.enabled =true; 



        }
    }
}
