using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoSingleton<JoystickManager>
{
    [SerializeField] PlayerController _char;
    [SerializeField] JoystickUi _joystick;
    [SerializeField] float speed;
    public Vector2 dir;
    JoystickState state;

    private void Start()
    {        
        _joystick.JoystickControlledDelegate.AddListener(MoveOrder);
        StartCoroutine(MoveSequence());
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
                    _char.SetDirection(dir, state);
                    break;
                case JoystickState.OnDrag:
                    _char.SetDirection(dir, state);
                    _char.transform.Translate(new Vector3(dir.x, dir.y, 0) * Time.deltaTime * speed);
                    cam.transform.position = _char.transform.position.WithZ(-10f);
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