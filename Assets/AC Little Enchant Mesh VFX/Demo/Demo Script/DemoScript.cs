using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AlphaimeCorporationDemo
{
    public class DemoScript : MonoBehaviour
    {
        private int ind;
        private void Start()
        {
            transform.GetChild(ind).gameObject.SetActive(true);
        }


        private void Update()
        {
            if (Input.GetKeyDown("q"))
                Previous();
            else if (Input.GetKeyDown("w"))
                Next();
        }

        void Next()
        {
            transform.GetChild(ind).gameObject.SetActive(false);
            ind++;
            ind = Mathf.Min(ind, transform.childCount - 1);
            transform.GetChild(ind).gameObject.SetActive(true);
        }

        void Previous()
        {
            transform.GetChild(ind).gameObject.SetActive(false);
            ind--;
            ind = Mathf.Max(ind, 0);
            transform.GetChild(ind).gameObject.SetActive(true);
        }
    }
}
