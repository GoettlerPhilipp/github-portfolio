using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemySmallState
{
    public enum STATE
    {
        IDLE_Small,
        CHOOSE_Small,
        MOVE_Small,
        ATTACK_1_Small,
        ATTACK_2_Small,
        ATTACK_3_Small,
        ATTACK_4_Small,
        LOST_Small
    }

    public enum EVENT
    {
        ENTER,
        UPDATE,
        EXIT
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected EnemySmallState nextState;

    public EnemySmallState(GameObject _npc)
    {
        npc = _npc;
        stage = EVENT.ENTER;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }
    
    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }
    
    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public EnemySmallState Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if(stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    public bool PlayerInRange()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, npc.transform.position) <=
            npc.GetComponent<EnemySmall>().distanceToPlayer)
            return true;
        return false;
    }

    public void SwitchState(EnemySmallState _nextState)
    {
        nextState = _nextState;
        stage = EVENT.EXIT;
    }
}

public class IDLE_Small : EnemySmallState
{
    public IDLE_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.IDLE_Small;
    }

    public override void Enter()
    {
        if (PlayerInRange())
        {
            base.Enter();
        }
        
    }

    public override void Update()
    {
        nextState = new CHOOSE_Small(npc);
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class CHOOSE_Small : EnemySmallState
{
    private bool rolledTheDice;
    private float resetTimer = 3f;
    
    public CHOOSE_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.CHOOSE_Small;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npc.GetComponent<EnemySmall>().health <= 0)
        {
            nextState = new LOST_Small(npc);
            stage = EVENT.EXIT;
            return;
        }
        
        if (!rolledTheDice)
        {
            rolledTheDice = true;
            int randomNum = Random.Range(0, npc.GetComponent<EnemySmall>().maxAttacks+2);
            switch (randomNum)
            {
                case 0:
                    resetTimer = 0.5f;
                    break;
                case 1:
                    nextState = new MOVE_Small(npc);
                    stage = EVENT.EXIT;
                    return;
                case 2:
                    nextState = new ATTACK_1_Small(npc);
                    stage = EVENT.EXIT;
                    return;
                case 3:
                    nextState = new ATTACK_2_Small(npc);
                    stage = EVENT.EXIT;
                    return;
                case 4:
                    nextState = new ATTACK_3_Small(npc);
                    stage = EVENT.EXIT;
                    return;
                case 5:
                    nextState = new ATTACK_4_Small(npc);
                    stage = EVENT.EXIT;
                    return;
                default:
                    resetTimer = 0.5f;
                    break;
            }
        }
        if (resetTimer > 0)
            resetTimer -= Time.deltaTime;
        else
            rolledTheDice = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class MOVE_Small : EnemySmallState
{
    private EnemySmall npcScript;
    private List<GameObject> TotemsInRange;
    public MOVE_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.MOVE_Small;
        npcScript = npc.GetComponent<EnemySmall>();
    }
    
    public override void Enter()
    {
        TotemsInRange = new List<GameObject>();
        for (int i = 0; i < npcScript.Totems.Count; i++)
        {
            float distance = Vector3.Distance(npc.transform.position,npcScript.Totems[i].transform.position);
            if (distance <= npcScript.jumpDistance)
            {
                TotemsInRange.Add(npcScript.Totems[i]);
            }
            if (i == npcScript.Totems.Count -1)
            {
                int randomNum = Random.Range(0, TotemsInRange.Count);
                npcScript.nextTotem = TotemsInRange[randomNum];
            }
        }
        base.Enter();
    }

    public override void Update()
    {
        if (npcScript.interp == 0)
        {
            nextState = new CHOOSE_Small(npc);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_1_Small : EnemySmallState
{
    //Schuss angriff
    private float bulletCooldown;
    private float switchStateCooldown;
    private int shotsInMagazin;

    private EnemySmall npcScript;
    
    public ATTACK_1_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_1_Small;
        npcScript = npc.GetComponent<EnemySmall>();
        shotsInMagazin = (int)Random.Range(npcScript.normalBulletExecute.x, npcScript.normalBulletExecute.y +1);
        switchStateCooldown = npcScript.switchStateCooldown;
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npcScript.health <= 0)
        {
            nextState = new LOST_Small(npc);
            stage = EVENT.EXIT;
            return;
        }
        //Schießt immer nur eine Kugel im Intervall + leert das Magazin
        if (bulletCooldown <= 0 && shotsInMagazin > 0)
        {
            npc.GetComponent<CreateAttack>().SpawnBullet(npcScript.normalBulletSpeed);
            bulletCooldown = Random.Range(npcScript.normalBulletExecutionCooldown.x, npcScript.normalBulletExecutionCooldown.y);
            shotsInMagazin -= 1;
        }
        else if (bulletCooldown > 0)
            bulletCooldown -= Time.deltaTime;

        if (shotsInMagazin <= 0)
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                SwitchState(new CHOOSE_Small(npc));
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_2_Small : EnemySmallState
{
    //Follow Player Attack
    private float bulletCooldown;
    private float switchStateCooldown;
    private int shotsInMagazin;
    
    private EnemySmall npcScript;
    public ATTACK_2_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_2_Small;
        npcScript = npc.GetComponent<EnemySmall>();
        shotsInMagazin = (int)Random.Range(npcScript.followPlayerBulletCount.x, npcScript.followPlayerBulletCount.y +1);
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npcScript.health <= 0)
        {
            nextState = new LOST_Small(npc);
            stage = EVENT.EXIT;
            return;
        }
        //Schießt immer nur eine Kugel im Intervall + leert das Magazin
        if (bulletCooldown <= 0 && shotsInMagazin > 0)
        {
            npc.GetComponent<CreateAttack>().SpawnFollowTargetBullet(npcScript.followPlayerBulletSpeed);
            bulletCooldown = Random.Range(npcScript.followPlayerBulletExecutionCooldown.x, npcScript.followPlayerBulletExecutionCooldown.y);
            shotsInMagazin -= 1;
        }
        else if (bulletCooldown > 0)
            bulletCooldown -= Time.deltaTime;

        if (shotsInMagazin <= 0)
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                SwitchState(new CHOOSE_Small(npc));
            }
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_3_Small : EnemySmallState
{
    //Exploding Bullet Attack
    private EnemySmall npcScript;
    private int explodingBulletsBulletSpawning;
    
    private float switchStateCooldown;
    private bool executed;
    public ATTACK_3_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_3_Small;
        npcScript = npc.GetComponent<EnemySmall>();
        explodingBulletsBulletSpawning = (int)Random.Range(npcScript.explodingBulletBulletsToSpawn.x, npcScript.explodingBulletBulletsToSpawn.y +1);
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npcScript.health <= 0)
        {
            nextState = new LOST_Small(npc);
            stage = EVENT.EXIT;
            return;
        }

        if(!executed)
        {
            npc.GetComponent<CreateAttack>().SpawnSplittingExplodingBullet(npcScript.explodingBulletBulletSpeed, explodingBulletsBulletSpawning, npcScript.normalBulletSpeed);
            executed = true;
        }

        if (switchStateCooldown > 0)
            switchStateCooldown -= Time.deltaTime;
        else
        {
            SwitchState(new CHOOSE_Small(npc));
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_4_Small : EnemySmallState
{
    //Feint Attack
    private EnemySmall npcScript;
    private float switchStateCooldown;
    private bool executed;
    
    public ATTACK_4_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_4_Small;
        npcScript = npc.GetComponent<EnemySmall>();
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npcScript.health <= 0)
        {
            nextState = new LOST_Small(npc);
            stage = EVENT.EXIT;
            return;
        }

        if (!executed)
        {
            npc.GetComponent<CreateAttack>().SpawnFeint(npcScript.feintBulletSpeed, npcScript.feintFocusAfterTime, (int)Random.Range(npcScript.feintBulletCount.x, npcScript.feintBulletCount.y +1));
            executed = true;
        }

        if (switchStateCooldown > 0)
            switchStateCooldown -= Time.deltaTime;
        else
        {
            SwitchState(new CHOOSE_Small(npc));
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class LOST_Small : EnemySmallState
{
    private EnemySmall npcScript;
    public LOST_Small(GameObject _npc)
        : base(_npc)
    {
        name = STATE.LOST_Small;
        npcScript = npc.GetComponent<EnemySmall>();
    }

    public override void Enter()
    {
        npcScript.DestroyMe();
    }
}
