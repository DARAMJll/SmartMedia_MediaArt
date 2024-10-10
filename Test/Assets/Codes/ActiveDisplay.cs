using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Display.displays.Length > 1)
        {
            foreach (var item in Display.displays)
            {
                item.Activate();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
