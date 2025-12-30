using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum PlayerState
{
    HoldFood,
    EmptyHand,
    Slicing,
    Cooking
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    public float interactDistance = 2f;
    private CounterBase currentCounter;
    private FoodCounter currentFoodCounter;
    private KitchenObjectSO kitchenData;
    private PlayerState currentState = PlayerState.EmptyHand;
    private bool canInteractCounter = false;
    private bool canTakeFoodFromFoodCouner = false;
    private bool canTakeFoodFromClearCounter = false;


    void Update()
    {
        switch (currentState)
        {
            case PlayerState.EmptyHand:
                canTakeFoodFromClearCounter = true;
                canTakeFoodFromFoodCouner = true;
                break;
            case PlayerState.HoldFood:
                canTakeFoodFromClearCounter = false;
                canTakeFoodFromFoodCouner = false;
                break;
            case PlayerState.Slicing:
                //HandleSlicing();
                break;
            case PlayerState.Cooking:
                //HandleCooking();
                break;
        }
        HandleApproach();
        if (Input.GetKeyDown(KeyCode.E))
            if (canInteractCounter)
            {
                if (canTakeFoodFromFoodCouner && currentCounter is FoodCounter)
                {
                    currentState = PlayerState.HoldFood;
                    canTakeFoodFromFoodCouner = false;
                    EventDispatcher.Dispatch(new EventDefine.OnPlayerInteractToFoodCounter { Counter = currentFoodCounter, HoldPoint = holdPoint });
                    kitchenData = KitchenManager.Instance.GetKitchenObjectData(currentCounter.CounterType);
                }

                if (canTakeFoodFromClearCounter)
                {
                    if (currentCounter is ClearCounter clearCounter && clearCounter.KitchenData != null)
                    {
                        Debug.Log("Take Food From Clear Counter");
                        kitchenData = clearCounter.KitchenData;
                        currentState = PlayerState.HoldFood;
                        EventDispatcher.Dispatch(new EventDefine.OnPlayerTakeFoodFromClearCounter { KitchenData = kitchenData, HoldPoint = holdPoint });

                    }
                }
                else
                {
                    if (currentCounter is ClearCounter clearCounter && clearCounter.KitchenData == null)
                    {
                        EventDispatcher.Dispatch(new EventDefine.OnPlayerGiveFoodToClearCounter { KitchenData = kitchenData, HoldPoint = holdPoint });
                        currentState = PlayerState.EmptyHand;
                        kitchenData = null;
                    }
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
                            canTakeFoodFromFoodCouner = true;
                        }
                        else
                        {
                            canTakeFoodFromFoodCouner = false;
                        }
                    }
                    else
                    {
                        currentFoodCounter = null;
                        canTakeFoodFromFoodCouner = false;
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
