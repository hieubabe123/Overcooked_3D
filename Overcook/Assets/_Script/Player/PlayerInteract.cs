using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    public float interactDistance = 2f;
    private CounterBase currentCounter;
    private FoodCounter currentFoodCounter;
    private KitchenObjectSO kitchenData;
    private bool canInteractCounter = false;
    private bool canTakeFood = false;
    private bool canPlaceFood = false;

    void Update()
    {
        HandleApproach();
        if (canTakeFood && canInteractCounter)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canTakeFood = false;
                Debug.Log("Player interacted with counter.");
                EventDispatcher.Dispatch(new EventDefine.OnPlayerInteractToFoodCounter { Counter = currentFoodCounter, HoldPoint = holdPoint });
                kitchenData = KitchenManager.Instance.GetKitchenObjectData(currentCounter.CounterType);
            }
        }
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
                    canInteractCounter = true;
                    EventDispatcher.Dispatch(new EventDefine.OnPlayerApproachCounter { Counter = counter });
                    if (counter is FoodCounter foodCounter)
                    {
                        currentFoodCounter = foodCounter;
                        if (kitchenData == null)
                        {
                            canTakeFood = true;
                        }
                        else
                        {
                            canTakeFood = false;
                        }
                    }
                    else
                    {
                        currentFoodCounter = null;
                        canTakeFood = false;
                    }
                }
            }
            else
            {
                if (currentCounter != null)
                {
                    EventDispatcher.Dispatch(new EventDefine.OnPlayerGoAwayFromCounter { Counter = currentCounter });
                    canInteractCounter = false;
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
                canInteractCounter = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactDistance);
    }
}
