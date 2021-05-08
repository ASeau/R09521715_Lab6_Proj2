using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGen : MonoBehaviour
{
    [SerializeField] public GameObject m_CubePrefab;
    [SerializeField] public GameObject m_SpherePrefab;
    [SerializeField] Vector2 m_Dimension = new Vector2 (5,5);
    // Start is called before the first frame update
    void Start()
    {
        GenPre(m_CubePrefab, Random.Range(5, 10));
        GenPre(m_SpherePrefab, Random.Range(5, 10));
    }
    void GenPre(GameObject primitive, int count) 
    {
        for (int i = 0; i < count; i++) 
        {
            var primitiveIns = GameObject.Instantiate(primitive);
            primitiveIns.transform.localPosition =
                new Vector3(Random.Range(-m_Dimension.x, m_Dimension.x),
                3f,
                Random.Range(-m_Dimension.y, m_Dimension.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
