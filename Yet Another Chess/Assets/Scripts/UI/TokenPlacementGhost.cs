using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(Collider))]
public class TokenPlacementGhost : MonoBehaviour
{
    public float dampSpeed = 0.075f;

    public Token controller { get; private set; }
    protected MeshRenderer[] m_MeshRenderers;
    public Collider ghostCollider { get; private set; }
    protected Vector3 m_MoveVel;
    protected bool m_ValidPos;
    protected Vector3 m_TargetPosition;



    protected virtual void Update()
    {
        Vector3 currentPos = transform.position;

        if (Vector3.SqrMagnitude(currentPos - m_TargetPosition) > 0.01f)
        {
            currentPos = Vector3.SmoothDamp(currentPos, m_TargetPosition, ref m_MoveVel, dampSpeed);

            transform.position = currentPos;
        }
        else
        {
            m_MoveVel = Vector3.zero;
        }
    }
    public virtual void Initialize(Token token)
    {
        m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        controller = token;
        if (GameUI.instanceExists)
        {
            GameUI.instance.SetupRadiusVisualizer(controller, transform);
        }
        ghostCollider = GetComponent<Collider>();
        m_MoveVel = Vector3.zero;
        m_ValidPos = false;
    }


}
