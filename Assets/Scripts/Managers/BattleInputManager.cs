using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BattleInputManager : MonoBehaviour
{
	public static BattleInputManager Ins = null;

	public bool isPause = false;
	private Camera __mainCamera;
	private HeroBase _selectedCharacter = null;
	private Vector3 _mousePos = Vector3.zero;
	private bool _isDragging = false;
	Vector2 min;
	Vector2 max;

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

	public (GameObject, Vector2) FindClickPoint(LayerMask layer)
	{
		RaycastHit2D hit = Physics2D.Raycast(MainCamera.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15f, layer);

		if (hit)
		{
			return (hit.transform.gameObject, hit.point);
		}

		return (null, Vector3.zero);
	}

	public (GameObject, Vector2) FindNearestFromClick(LayerMask layer, params GameObject[] ignores)
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
		if (Ins == null)
		{
			Ins = this;
		}
	}

	public const string HeroTag = "Hero";
	public const string EnemyTag = "Enemy";
	public const string FieldTag = "Field";
	public const string SuppliesTag = "Supplies";    
	public static readonly string[] CharacterTags = { HeroTag, EnemyTag };

	private void Start()
	{
		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
	}

	private void Update()
	{
		if (!isPause)
		{
			if (Input.GetMouseButtonDown(0))
			{				
				var (target, pos) = FindClickPoint(LayerManager.Ins.FieldAndSupplies);
				
				if (target != null)
				{
					if (target.tag == FieldTag)
					{
						//���Ӽ��� �߻�ü�� �߻�
						//������ ���� ������?
						EffectManager.Ins.ShowFx("TestHitFx", pos);

						EnemyBase _target = null;

						if (EnemyBase.Enemies.Count > 0)
						{
							List<EnemyBase> enemies = EnemyBase.Enemies.OrderBy(a => Vector2.Distance(a.transform.position, pos)).ToList();

							for (int i = 0; i < enemies.Count; i++)
							{
								if (enemies[i].transform.position.x < max.x && enemies[i].transform.position.x > min.x &&
									enemies[i].transform.position.y < max.y && enemies[i].transform.position.y > min.y)
								{
									if (enemies[i].Stat.CurHp > 0f)
									{
										_target = enemies[i];										
										break;
									}
								}
							}
						}

						Vector2 dir = Vector2.up;

						if (_target != null)
							dir = ((Vector2)_target.transform.position - pos).normalized;

						List<ProjectileChart> projectiles = CsvData.Ins.ProjectileChart["touch_attack_" + StageManager.Ins.PlayerData.TouchAttackLv];
						double _atk = 0f;

						for(int i = 0; i < HeroBase.Heroes.Count; i++)
						{
							_atk += HeroBase.Heroes[i].Stat.Atk;
						}

						_atk = _atk / 5f;

						for (int i = 0; i < projectiles.Count; i++)
						{
							ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/" + projectiles[i].Model) as GameObject);
							projectile.transform.position = pos;
							
							projectile.Setup(projectiles[i], _atk, dir, _target);
						}
						
					}
					else if(target.tag == SuppliesTag)
					{
						var supplies = target.GetComponent<SuppliesBase>();
						if (supplies == null)
						{
							return;
						}

						supplies.GetReward();
					}
				}
			}

			if (Input.GetMouseButton(0))
			{
				
			}

			if (Input.GetMouseButtonUp(0))
			{
				
			}

		}
	}

}