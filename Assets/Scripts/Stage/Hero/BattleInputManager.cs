using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BattleInputManager : MonoBehaviour
{
	public static BattleInputManager Instance = null;

	public bool isPause = false;

	private Camera __mainCamera;

	private HeroBase _selectedCharacter = null;

	private Vector3 _mousePos = Vector3.zero;

	private bool _isDragging = false;

	private Camera MainCamera
	{
		get
		{
			if (__mainCamera == null)
			{
				__mainCamera = Camera.main;
			}
			return __mainCamera;
		}
	}

	public (GameObject, Vector3) FindClickPoint(LayerMask layer)
	{
        RaycastHit2D hit = Physics2D.Raycast(MainCamera.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15f, layer);

        if (hit)
        {
            return (hit.transform.gameObject, hit.point);
        }
		
		return (null, Vector3.zero);
	}

	public (GameObject, Vector3) FindNearestFromClick(LayerMask layer, params GameObject[] ignores)
	{
		const float determineRadius = 0.3f;
		var hits = Physics.SphereCastAll(MainCamera.ScreenPointToRay(Input.mousePosition), determineRadius, 1000f, layer);
		if (hits.Length <= 0)
		{
			return default;
		}
		var candidates = ignores.GetSize() > 0 ? hits.Where(elem => !ignores.Contains(elem.collider.gameObject)) : hits;
		var nearest = candidates.MinBy(elem => elem.distance);
		return nearest.collider != null ? (nearest.transform.gameObject, nearest.point) : default;
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public const string HeroTag = "Hero";
	public const string EnemyTag = "Enemy";
	public static readonly string[] CharacterTags = { HeroTag, EnemyTag };

    private void Update()
    {
        if (!isPause)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var (target, _) = FindClickPoint(LayerManager.Ins.Hero);
                                
                if (target != null)
                {
                    if (target.tag == HeroTag)
                    {
                        var characterObject = target.GetComponent<HeroBase>();
                        if (characterObject == null)
                        {
                            return;
                        }

                        _isDragging = true;
                        _selectedCharacter = characterObject;
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (_isDragging)
                {
                    var (target, curPoint) = FindClickPoint(LayerManager.Ins.Field);
                    
                    if(target != null)
                    {
                        _selectedCharacter.transform.position = curPoint;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;

                //var (target, curPoint) = FindNearestFromClick(LayerManager.Instance().PlayerAndEnemy);
                //if (_selectedCharacter != null)
                //{
                //    _selectedCharacter.GetInput = true;

                //    if (target != null && target.tag == "Enemy")
                //    {
                //        _selectedCharacter.ForceAttack(target.GetComponent<CharacterBase>());
                //    }
                //    else
                //    {
                //        _selectedCharacter.StopForceAttack();
                //        (target, curPoint) = FindClickPoint(LayerManager.Instance().WalkableAndUnWalkable);
                //    }

                //    _selectedCharacter.Move(target, curPoint);
                //    _selectedCharacter.characterUI.CloseAttackRange();
                //    Time.timeScale = 1f;
                //}                
            }

        }
    }

}
