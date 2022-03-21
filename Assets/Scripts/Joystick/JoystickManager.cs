using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoSingleton<JoystickManager>
{    
    [SerializeField] JoystickUi _joystick;
    [SerializeField] float speed;
    public Vector2 dir;
    JoystickState state;

    private void Start()
    {        
        _joystick.JoystickControlledDelegate.AddListener(MoveOrder);        
    }

    public void MoveOrder(Vector2 dir, JoystickState state)
    {
        this.dir = dir;
        this.state = state;
    }

    IEnumerator MoveSequence()
    {
        Camera cam = Camera.main;

        while (true)
        {   
            switch (state)
            {
                case JoystickState.EndDrag:
                    
                    break;
                case JoystickState.OnDrag:
                    
                    break;
                default:
                    
                    break;
            }
            
            yield return null;
        }
        
    }



}

//while (true)
//{
//          float axisX = Input.GetAxis("Mouse X");
//          float axisY = Input.GetAxis("Mouse Y");

//          if (Input.GetMouseButton(0))
//          {
//              if (axisX > 0 && transform.position.x < ScreenHorizontalSize)
//              {
//                  transform.position = new Vector3(transform.position.x + axisX * 25f * HorizontalVelocity, transform.position.y, transform.position.z);
//              }
//              else if (axisX < 0 && transform.position.x > -ScreenHorizontalSize)
//              {
//                  transform.position = new Vector3(transform.position.x + axisX * 25f * HorizontalVelocity, transform.position.y, transform.position.z);
//              }
//              else if (transform.position.x > ScreenHorizontalSize)
//              {
//                  transform.position = new Vector3(ScreenHorizontalSize, transform.position.y, transform.position.z);
//              }
//              else if (transform.position.x < -ScreenHorizontalSize)
//              {
//                  transform.position = new Vector3(-ScreenHorizontalSize, transform.position.y, transform.position.z);
//              }
//          }

//          yield return null;
//      }