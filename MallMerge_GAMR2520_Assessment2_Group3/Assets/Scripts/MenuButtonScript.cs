using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableMenu()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().ResetTrigger("Disable");
        GetComponent<Animator>().SetTrigger("Enable");
    }

    public void DisableMenu()
    {
        GetComponent<Animator>().ResetTrigger("Enable");
        GetComponent<Animator>().SetTrigger("Disable");
    }
}
