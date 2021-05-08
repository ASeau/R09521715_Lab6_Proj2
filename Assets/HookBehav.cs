using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehav : MonoBehaviour
{
    const float Move_Speed = 1f;
    const float rotation_speed = 10f;
    const float AttachDist = 5f;
    GameObject m_DetectedObject;
    [SerializeField] GameObject m_jib;
    [SerializeField] GameObject m_trolley;
    [SerializeField] GameObject m_hook;
    ConfigurableJoint m_JointForObject;
    [SerializeField] LineRenderer m_Cable;
    [SerializeField] LineRenderer m_Cable2;

    // Start is called before the first frame update
    void DetectObjects() 
    {
        Ray ray = new Ray(m_hook.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, AttachDist))
        {
            if (m_DetectedObject == hit.collider.gameObject)
            {
                return;
            }

            RecoverDetechedObject();
            MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow;
                m_DetectedObject = hit.collider.gameObject;
            }
        }
        else 
        {
            RecoverDetechedObject();
        }
    }

    void RecoverDetechedObject() 
    {
        if (m_DetectedObject != null) 
        {
            m_DetectedObject.GetComponent<MeshRenderer>().material.color = Color.white;
            m_DetectedObject = null; 
        }
    }

    void AttachOrDetachObject() 
    {
        if (m_JointForObject == null)
        {
            if (m_DetectedObject != null)
            {
                var joint = m_hook.gameObject.AddComponent<ConfigurableJoint>();
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;
                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Free;
                joint.angularZMotion = ConfigurableJointMotion.Free;

                var limit = joint.linearLimit;
                limit.limit = 4;

                joint.linearLimit = limit;

                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = new Vector3(0f, 0.5f, 0f);
                joint.anchor = new Vector3(0f, 0f, 0f);

                joint.connectedBody = m_DetectedObject.GetComponent<Rigidbody>();

                m_JointForObject = joint;

                m_DetectedObject.GetComponent<MeshRenderer>().material.color = Color.red;
                m_DetectedObject = null;
            }
        }
        else 
        {
            GameObject.Destroy(m_JointForObject);
            m_JointForObject = null;
        } 
    }
    void UpdateCable() 
    {
        m_Cable.enabled = m_JointForObject != null && m_JointForObject.connectedBody != null;

        if (m_Cable.enabled) 
        {
            m_Cable.SetPosition(0, m_JointForObject.transform.position);
            var connectedBodyTransform = m_JointForObject.connectedBody.transform;
            m_Cable.SetPosition(1, connectedBodyTransform.TransformPoint(m_JointForObject.connectedAnchor));
        }
    }
    void UpdateCable2()
    {
        m_Cable2.SetPosition(0, m_trolley.transform.position);
        m_Cable2.SetPosition(1, m_hook.transform.position);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCable2();

        if (Input.GetKey(KeyCode.W)) 
        {
            Vector3 current_Pos = m_trolley.transform.position;
            float current_Posz = m_trolley.transform.position.z;
            
            if (current_Posz < 17)
            {
                float new_Posz = current_Posz + Move_Speed * Time.deltaTime;
                Vector3 new_Pos = current_Pos + new Vector3(0, 0, (new_Posz - current_Posz));
                m_trolley.transform.position = new_Pos;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 current_Pos = m_trolley.transform.position;
            float current_Posz = m_trolley.transform.position.z;
            
            if (current_Posz > 1)
            {
                float new_Posz = current_Posz - Move_Speed * Time.deltaTime;
                Vector3 new_Pos = current_Pos + new Vector3(0, 0, (new_Posz - current_Posz));
                m_trolley.transform.position = new_Pos;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_jib.transform.Rotate(0, 0, rotation_speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_jib.transform.Rotate(0, 0, -rotation_speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            //m_hook.transform.Translate(0,0,Move_Speed * Time.deltaTime);
            Vector3 current_Pos = m_hook.transform.position;
            float current_Posy = m_hook.transform.position.y;
            print(current_Posy);
            if (current_Posy < 14.5)
            {
                float new_Posy = current_Posy + Move_Speed * Time.deltaTime;
                Vector3 new_Pos = current_Pos + new Vector3(0,(new_Posy - current_Posy),0);
                m_hook.transform.position = new_Pos;
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Vector3 current_Pos = m_hook.transform.position;
            float current_Posy = m_hook.transform.position.y;
            print(current_Posy);
            if (current_Posy > 1)
            {
                float new_Posy = current_Posy + Move_Speed * Time.deltaTime;
                Vector3 new_Pos = current_Pos + new Vector3(0, -(new_Posy - current_Posy), 0);
                m_hook.transform.position = new_Pos;
            }
        }
        if (m_JointForObject == null) 
        {
            DetectObjects();
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            AttachOrDetachObject();
        }
        UpdateCable();

    }
}
