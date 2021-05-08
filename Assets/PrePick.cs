using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePick : MonoBehaviour
{
    GameObject m_clickobj;
    // Start is called before the first frame update
    void FireRaycast() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) 
        {
            MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();
            
            if (renderer != null) 
            {
                renderer.material.color = Color.red;

                m_clickobj = hit.collider.gameObject;
            }
        }

    }
    void RecoverClickObj() 
    {
        if (m_clickobj != null) 
        {
            m_clickobj.GetComponent<MeshRenderer>().material.color = Color.white;
            m_clickobj = null; 
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireRaycast();
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            RecoverClickObj();
        }
    }
}
