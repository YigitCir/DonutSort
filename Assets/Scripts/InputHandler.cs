using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PoleClickedEvent : UnityEvent<Vector3> { }

public class InputHandler : MonoBehaviour
{
    public LayerMask inputLayerMask; 
    public PoleClickedEvent OnPoleClicked;
    public float cooldownTime = 0.5f; 
    private bool isCooldown = false; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCooldown) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f); 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, inputLayerMask))
            {
                Debug.Log("Hit detected: " + hit.collider.name);
                OnPoleClicked.Invoke(hit.point);
                StartCoroutine(Cooldown());
            }
            else
            {
                Debug.Log("No hit detected");
            }
        }
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}