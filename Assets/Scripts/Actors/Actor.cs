using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MapObject
{
    protected Vector2Int targetPosition;

    protected int maxHealth;
    public int MaxHealth { get { return maxHealth; } }
    protected int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    public float MoveSpeed = 5.0f;
    public int InitialHealth;

    protected Animator animator;
    //used also to handle animations
    public enum eActorState
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    public eActorState ActorState;

    Actor attackTarget;
    float attackDuration = 0.5f;
    float attackStartTime;

    void GoToState(eActorState _state) {
        if (animator != null) {
            switch (_state) {
                case eActorState.Idle:
                    animator.SetTrigger("Stop");
                    break;
                case eActorState.Moving:
                    animator.SetTrigger("Move");
                    break;
                case eActorState.Attacking:
                    animator.SetTrigger("Dead");
                    break;
                case eActorState.Dead:
                    animator.SetTrigger("Attack");
                    break;
            }
        }
    }
    //===================================================================
    public void SetPosition(Vector2Int _position)
    {
        TilePosition = _position;
        transform.position = new Vector3(TilePosition.x, 0, TilePosition.y);
    }
    //===================================================================
    public virtual bool CanMoveToTile(Vector2Int _position)
    {
        if (ActorState != eActorState.Idle || _position.x < 0 || _position.y < 0
            || _position.x >= DungeonController.Instance.CurrentRoom.Size.x
            || _position.y >= DungeonController.Instance.CurrentRoom.Size.y)
        {
            return false;
        }

        return true;
    }
    //===================================================================
    public virtual void BeginMove(Vector2 _direction)
    {
        //converting Vector2 to Vector2Int
        Vector2Int direction = new Vector2Int((int)_direction.x, (int)_direction.y);
        Vector2Int position = TilePosition + direction;


        if (!CanMoveToTile(position))
        {
            return;
        }


        Tile tile = DungeonController.Instance.GetTile(position);
        if (tile != null)
        {
            if (tile.IsPassable())
            {
                for (int i = 0; i < tile.MapObjects.Count; i++)
                {
                    if (tile.MapObjects[i].GetType() == typeof(Door))
                    {
                        ActorState = eActorState.Moving;
                        targetPosition = position;
                    }
                }

                if (ActorState == eActorState.Idle)
                {
                    ActorState = eActorState.Moving;
                    targetPosition = position;
                }
            }
            else
            {
                //if the tile the actor wants to move to is not passable, it gets attacked
                for (int i = 0; i < tile.MapObjects.Count; i++)
                {
                    if (tile.MapObjects[i].GetType() == typeof(Monster))
                    {
                        attackStartTime = Time.time;
                        ActorState = eActorState.Attacking;
                        attackTarget = tile.MapObjects[i] as Monster;
                    }
                }
            }
        }
    }
    //===================================================================
    public virtual void BeginAttack(Vector2 _direction)
    {
        Vector2Int direction = new Vector2Int((int)_direction.x, (int)_direction.y);
        Vector2Int position = TilePosition + direction;

        if (!CanMoveToTile(position))
        {
            return;
        }

        bool hasPlayer = false;
        for (int i = 0; i < DungeonController.Instance.GetTile(position).MapObjects.Count; i++)
        {
            if (DungeonController.Instance.GetTile(position).MapObjects[i].GetType() == typeof(Player))
            {
                hasPlayer = true;
            }
        }
        if (!hasPlayer)
        {
            return;
        }

        GameController.Instance.Player.TakeDamage(GetAttackDamage());
    }
    //===================================================================
    protected virtual void Update()
    {
        switch (ActorState)
        {
            case eActorState.Moving:

                Vector3 targetPos = new Vector3(targetPosition.x, 0, targetPosition.y);
                //if the distance from the actor and the target is greater than a really small value,
                //continue moving
                if (Vector3.Distance(transform.position, targetPos) > float.Epsilon)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * MoveSpeed);
                }
                else
                //if the actor has pretty much arrived to his destination
                {
                    EnterTile(targetPosition);
                    EndTurn();
                }

                break;


            case eActorState.Attacking:

                Vector3 attackPos = new Vector3(attackTarget.TilePosition.x, 0, attackTarget.TilePosition.y);
                float t = (Time.time - attackStartTime) / attackDuration;
                Vector3 attackDir = attackPos - transform.position;
                transform.position = DungeonController.Instance.CurrentRoom.Tiles[TilePosition.x, TilePosition.y].gameObject.transform.position + attackDir * Mathf.PingPong(t, 0.5f);
                if (t > 1f)
                {
                    int attackDamage = GetAttackDamage();
                    attackTarget.TakeDamage(attackDamage);
                    EndTurn();
                }

                break;
        }
    }
    //===================================================================
    protected virtual void EndTurn()
    {
        ActorState = eActorState.Idle;
    }
    //===================================================================
    public virtual int GetAttackDamage()
    {
        return 5;
    }
    //===================================================================
    public virtual void TakeDamage(int _amount)
    {
        int totalDamage = _amount;

        if (_amount >= currentHealth)
        {
            totalDamage = currentHealth;
        }

        currentHealth -= totalDamage;

        UIController.Instance.ShowDamageTag(transform.position, totalDamage.ToString());

        if (currentHealth <= 0)
        {
            OnKill();
        }
    }
    //===================================================================
    public virtual void OnKill()
    {
        DungeonController.Instance.GetTile(TilePosition).MapObjects.Remove(this);
        ActorState = eActorState.Dead;
        Destroy(gameObject);
    }
    //===================================================================
    public virtual void EnterTile(Vector2Int _tilePosition)
    {
        Debug.Log("enter tile started");
        DungeonController.Instance.GetTile(_tilePosition).MapObjects.Remove(this);
        TilePosition = targetPosition;
        DungeonController.Instance.GetTile(_tilePosition).MapObjects.Add(this);
    }
}
