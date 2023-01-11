using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSwitch : MonoBehaviour
{
    
    //public CameraShake cameraShake;
    public int selectedExplosion = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectExplosion ();
    }

    // Update is called once per frame
    void Update()
    {
       
            //if (Input.GetKeyDown(KeyCode.E))
            
            //StartCoroutine(cameraShake.Shake(.1f, .2f));
            
        {
            //if (Input.GetKeyDown(KeyCode.Q))
        
        
            //StartCoroutine(cameraShake.Shake(.1f, .2f));
        }

        int previousSelectedExplosion = selectedExplosion;

        if (Input.GetKeyDown(KeyCode.E))

        {
            if (selectedExplosion >= transform.childCount - 1)
                selectedExplosion = 0;
            else
             selectedExplosion++;

           

        }

        if (Input.GetKeyDown(KeyCode.Q))

            if (selectedExplosion <= 0)
            selectedExplosion = transform.childCount - 1;
        else

            selectedExplosion--;



        if (previousSelectedExplosion != selectedExplosion)
        {
            SelectExplosion();
        }
    }
    void SelectExplosion ()
    {
        int i = 0;
        foreach (Transform explosion in transform)
        {
            if (i == selectedExplosion)
                explosion.gameObject.SetActive(true);
            else
               explosion.gameObject.SetActive(false);
            i++;
        }
    }
}
