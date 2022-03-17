using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class SavedTouch : Object
{
    public int fingerId;
    public Vector2 position;
}

public class WorldCameraCtrl : MonoBehaviour
{
    public Camera worldCamera;

    public Vector3 originalPos;
    public float originalDistance;

    public float eulerMin;
    public float eulerMax;
    public float distanceMin;
    public float distanceMax;

    public float offsetX, offsetY;

    //鼠标
    float sensitivityMouse = 5;
    float sensitivetyMouseWheel = 20;

    //触摸
    float sensitivityDrag = 0.1f;
    float sensitivetyPinch = 0.2f;
    enum TouchState
    {
        None,
        Dragging,
        Pinching,
    }
    TouchState state = TouchState.None;
    List<SavedTouch> originalTouchs = new List<SavedTouch>();
    bool canDrag = false;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        originalDistance = -worldCamera.transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Client_UI.Instance)
        //{
        //    if (Client_UI.Instance.x != 0 || Client_UI.Instance.y != 0)
        //    {
        //        GetMouseButton(Client_UI.Instance.y, Client_UI.Instance.x);
        //    }

        //    if (Client_UI.Instance.valueSize != 0)
        //    {
        //        MouseScrollWheel(Client_UI.Instance.valueSize);
        //    }
        //}


        //如果首次点击是在UI上则不触发
        //if (Input.GetMouseButtonDown(0))
        //{
        //    canDrag = !EventSystem.current.IsPointerOverGameObject();
        //}
        //else if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    canDrag = !EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        //}

        //if (canDrag == false)
        //{
        //    return;
        //}

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)//安卓下的单点触摸可能也会被识别成鼠标事件
        //触摸
        if (Input.touchCount > 0)
        {
            if (state == TouchState.None)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.touches[i];
                    if (t.phase == TouchPhase.Began && originalTouchs.Count < 2)
                    {
                        bool have = false;
                        foreach (SavedTouch t0 in originalTouchs)
                        {
                            if (t0.fingerId == t.fingerId)
                            {
                                have = true;
                                break;
                            }
                        }
                        if (have == false)
                        {
                            SavedTouch st = new SavedTouch();
                            st.fingerId = t.fingerId;
                            st.position = t.position;
                            originalTouchs.Add(st);
                        }
                    }
                    else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        foreach (SavedTouch t0 in originalTouchs)
                        {
                            if (t0.fingerId == t.fingerId)
                            {
                                originalTouchs.Remove(t0);
                                break;
                            }
                        }
                    }
                    else if (t.phase == TouchPhase.Moved)
                    {
                        foreach (SavedTouch t0 in originalTouchs)
                        {
                            if (t0.fingerId == t.fingerId)
                            {
                                if (originalTouchs.Count == 1)
                                {
                                    state = TouchState.Dragging;
                                    //t0.position = t.position;
                                    this.dragging(t);
                                }
                                else if (originalTouchs.Count == 2)
                                {
                                    state = TouchState.Pinching;
                                    //t0.position = t.position;
                                    this.pinching(t);
                                }
                                break;
                            }
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.touches[i];
                    foreach (SavedTouch t0 in originalTouchs)
                    {
                        if (t0.fingerId == t.fingerId)
                        {
                            if (t.phase == TouchPhase.Moved)
                            {
                                if (state == TouchState.Dragging)
                                {
                                    this.dragging(t);
                                }
                                else if (state == TouchState.Pinching)
                                {
                                    this.pinching(t);
                                }
                            }
                            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                            {
                                //if (state == TouchState.Dragging)
                                //{
                                //    state = TouchState.None;
                                //    originalTouchs.Clear();
                                //}
                                //else if (state == TouchState.Pinching)
                                //{
                                //    state = TouchState.None;

                                //    originalTouchs.Remove(t0);
                                //}

                                //全部清空
                                state = TouchState.None;
                                originalTouchs.Clear();
                            }
                            break;
                        }
                    }
                }
            }
        }
#else
        //按着鼠标实现视角转动
        if (Input.GetMouseButton(0))
        {       
            GetMouseButton(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        }

        //滚轮实现镜头缩进和拉远
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            MouseScrollWheel(Input.GetAxis("Mouse ScrollWheel"));
        }
#endif
    }

    void GetMouseButton(float MouseY,float MouseX)
    {
        float eulerX = transform.eulerAngles.x;
        eulerX -= MouseY * sensitivityMouse;
        if (eulerX < eulerMin)
        {
            eulerX = eulerMin;
        }
        else if (eulerX > eulerMax)
        {
            eulerX = eulerMax;
        }
        transform.eulerAngles = new Vector3(eulerX, transform.eulerAngles.y, 0);

        transform.Rotate(0, MouseX * sensitivityMouse, 0, Space.World);
    }

    void MouseScrollWheel(float mouseWheel)
    {
        float distance = -worldCamera.transform.localPosition.z;
        distance -= mouseWheel * sensitivetyMouseWheel;

        if (distance < distanceMin)
        {
            distance = distanceMin;
        }
        else if (distance > distanceMax)
        {
            distance = distanceMax;
        }
        worldCamera.transform.localPosition = new Vector3(0, 0, -distance);
    }

    void LateUpdate()
    {
        //偏移
        float z = worldCamera.transform.localPosition.z;
        worldCamera.transform.localPosition = new Vector3(offsetX * -z, offsetY * -z, z);
    }

    void dragging(Touch newTouch)
    {
        foreach (SavedTouch t0 in originalTouchs)
        {
            if (t0.fingerId == newTouch.fingerId)
            {
                float x = newTouch.position.x - t0.position.x;
                float y = newTouch.position.y - t0.position.y;

                //transform.Rotate(-Input.GetAxis("Mouse Y") * sensitivityMouse, 0, 0, Space.Self);
                float eulerX = transform.eulerAngles.x;
                eulerX -= y * sensitivityDrag;
                if (eulerX < eulerMin)
                {
                    eulerX = eulerMin;
                }
                else if (eulerX > eulerMax)
                {
                    eulerX = eulerMax;
                }
                transform.eulerAngles = new Vector3(eulerX, transform.eulerAngles.y, 0);

                transform.Rotate(0, x * sensitivityDrag, 0, Space.World);

                //记录新的
                t0.position = newTouch.position;
                break;
            }
        }
        
    }
    void pinching(Touch newTouch)
    {
        if (originalTouchs.Count < 2)
        {
            return;
        }

        float touchDistanse = Vector2.Distance(originalTouchs[0].position, originalTouchs[1].position);
        foreach (SavedTouch t0 in originalTouchs)
        {
            if (t0.fingerId == newTouch.fingerId)
            {
                //记录新的
                t0.position = newTouch.position;
                float newTouchDistanse = Vector2.Distance(originalTouchs[0].position, originalTouchs[1].position);
                float change = newTouchDistanse - touchDistanse;

                float cameraDistance = -worldCamera.transform.localPosition.z;
                cameraDistance -= change * sensitivetyPinch;
                if (cameraDistance < distanceMin)
                {
                    cameraDistance = distanceMin;
                }
                else if (cameraDistance > distanceMax)
                {
                    cameraDistance = distanceMax;
                }
                worldCamera.transform.localPosition = new Vector3(0, 0, -cameraDistance);
                
                break;
            }
        }
    }
}
