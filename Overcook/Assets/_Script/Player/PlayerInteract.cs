using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 2f;
    private CounterBase currentCounter;


    void Update()
    {
        HandleApproach();
    }

    private void HandleApproach()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            if (hit.transform.TryGetComponent<CounterBase>(out CounterBase counter))
            {
                if (currentCounter != counter)
                {
                    EventDispatcher.Dispatch(new EventDefine.OnPlayerGoAwayFromCounter { Counter = currentCounter });
                    currentCounter = counter;
                    EventDispatcher.Dispatch(new EventDefine.OnPlayerApproachCounter { Counter = counter });
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        EventDispatcher.Dispatch(new EventDefine.OnPlayerInteractCounter { Counter = counter });
                    }
                }
            }
            else
            {
                if (currentCounter != null)
                {
                    EventDispatcher.Dispatch(new EventDefine.OnPlayerGoAwayFromCounter { Counter = currentCounter });
                    currentCounter = null;
                }
            }
        }
        else
        {
            if (currentCounter != null)
            {
                EventDispatcher.Dispatch(new EventDefine.OnPlayerGoAwayFromCounter { Counter = currentCounter });
                currentCounter = null;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactDistance);
    }
}
