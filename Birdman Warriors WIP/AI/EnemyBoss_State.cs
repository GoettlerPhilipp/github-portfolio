using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class EnemyBoss_State
{
    public enum STATE
    {
        IDLE_Boss,
        CHOOSE_Boss,
        ATTACK_1_Boss,
        ATTACK_2_Boss,
        ATTACK_3_Boss,
        Attack_4_Boss,
        ATTACK_5_Boss,
        LOST_Boss
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }
    
    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected EnemyBoss_State nextState;
    
    
    protected GameObject player;

    public EnemyBoss_State(GameObject _npc)
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

    public EnemyBoss_State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}

public class IDLE_Boss : EnemyBoss_State
{
    
    //Start Phase
    
    private float leaveStateTimer = 2f;
    
    public IDLE_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.IDLE_Boss;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Enter()
    {
        if (npc.GetComponent<EnemyBoss>().startBoss)
        {
            npc.GetComponent<EnemyBoss>().getKnockback = true;
            base.Enter();
        }
        
    }

    public override void Update()
    {
        if (leaveStateTimer > 0)
            leaveStateTimer -= Time.deltaTime;
        else
        {
            nextState = new CHOOSE_Boss(npc);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class CHOOSE_Boss : EnemyBoss_State
{
    //Sucht sich die verschiedenen Angriffe aus
    public bool rolledTheDice;
    private float resetTimer = 3f;
    public CHOOSE_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.CHOOSE_Boss;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npc.GetComponent<EnemyBoss>().bossHealth <= 0)
        {
            nextState = new LOST_Boss(npc);
            stage = EVENT.EXIT;
            return;
        }
        Debug.Log("Boss Leben: " + npc.GetComponent<EnemyBoss>().bossHealth);
        if (!rolledTheDice)
        {
            rolledTheDice = true;
            int randomNum = Random.Range(0, 6);
            switch (randomNum)
            {
                case 0:
                    //Basic Attack
                    nextState = new ATTACK_1_Boss(npc);
                    stage = EVENT.EXIT;
                    break;
                case 1:
                    //Cloud Attack
                    nextState = new ATTACK_2_Boss(npc);
                    stage = EVENT.EXIT;
                    break;
                case 2:
                    //Cone Attack
                    nextState = new ATTACK_3_Boss(npc);
                    stage = EVENT.EXIT;
                    break;
                case 3:
                    //Laser Attack
                    resetTimer = 1.5f;
                    //nextState = new ATTACK_4_Boss(npc);
                    //stage = EVENT.EXIT;
                    break;
                case 4:
                    //Explotion Attack
                    nextState = new ATTACK_5_Boss(npc);
                    stage = EVENT.EXIT;
                    break;
                case 5:
                    resetTimer = 1.5f;
                    break;
            }
        }
        
        if (resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
        }
        else
            rolledTheDice = false;
    }

    public override void Exit()
    {
        new WaitForSeconds(5f);
        base.Exit();
    }
}

public class ATTACK_1_Boss : EnemyBoss_State
{
    //Schuss angriff
    private float bulletCooldown = 0.2f;
    private float switchStateCooldown = 3f;
    private int shotsInMagazin;
    
    
    public ATTACK_1_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_1_Boss;
    }

    public override void Enter()
    {
        switch (npc.GetComponent<EnemyBoss>().bossHealth)
        {
            case 2:
                shotsInMagazin = Random.Range(7, 13);
                break;
            case 1:
                shotsInMagazin = Random.Range(11, 16);
                break;
        }
        base.Enter();
        
    }

    public override void Update()
    {
        if (npc.GetComponent<EnemyBoss>().bossHealth <= 0)
        {
            nextState = new LOST_Boss(npc);
            stage = EVENT.EXIT;
            return;
        }
        //Schießt immer nur eine Kugel im Intervall + leert das Magazin
        if (bulletCooldown <= 0 && shotsInMagazin > 0)
        {
            switch (npc.GetComponent<EnemyBoss>().bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnBullet(75);
                    bulletCooldown = Random.Range(0.3f, 0.45f);
                    shotsInMagazin -= 1;
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>().SpawnBullet(100);
                    bulletCooldown = Random.Range(0.25f, 0.35f);
                    shotsInMagazin -= 1;
                    break;
            }
        }
        else if (bulletCooldown > 0)
            bulletCooldown -= Time.deltaTime;

        if (shotsInMagazin <= 0)
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss(npc);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_2_Boss : EnemyBoss_State
{
    //Angriff für die Wolke
    
    private bool calledOnce;
    private float leaveStateTimer;
    public ATTACK_2_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_2_Boss;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!calledOnce)
        {
            calledOnce = true;
            switch (npc.GetComponent<EnemyBoss>().bossHealth)
            {
                case 2:
                    leaveStateTimer = 2.5f;
                    break;
                case 1:
                    leaveStateTimer = 1f;
                    break;
            }
            npc.GetComponent<CreateAttack>().SpawnCloud();
        }
        else
        {
            if (leaveStateTimer > 0)
                leaveStateTimer -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss(npc);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class LOST_Boss : EnemyBoss_State
{
    // Player Won against the Enemy
    public LOST_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.LOST_Boss;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("Du hast gewonnen");
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_3_Boss : EnemyBoss_State
{
    //Cone angriff
    private float leaveStateTimer;

    public ATTACK_3_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_3_Boss;
    }

    public override void Enter()
    {
        switch (npc.GetComponent<EnemyBoss>().bossHealth)
        {
            case 2:
                leaveStateTimer = 3.5f;
                break;
            case 1:
                leaveStateTimer = 3f;
                break;
        }

        bool randomBool = (Random.value > 0.5f);
        npc.GetComponent<CreateAttack>().SpawnBulletsInCone(50, randomBool);
        base.Enter();
    }

    public override void Update()
    {
        if (leaveStateTimer > 0)
            leaveStateTimer -= Time.deltaTime;
        else
        {
            nextState = new CHOOSE_Boss(npc);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_4_Boss : EnemyBoss_State
{
    //Laser Attack
    
    private float leaveStateTimer;
    
    public ATTACK_4_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.Attack_4_Boss;
    }

    public override void Enter()
    {
        switch (npc.GetComponent<EnemyBoss>().bossHealth)
        {
            case 2:
                leaveStateTimer = 2.5f;
                break;
            case 1:
                leaveStateTimer = 2.0f;
                break;
        }
        npc.GetComponent<CreateAttack>().SpawnLaser();
        base.Enter();
    }

    public override void Update()
    {
        if (!npc.GetComponent<CreateAttack>().leftLaser.GetComponent<Laser>().checkIfLaserStarted)
        {
            if (leaveStateTimer > 0)
                leaveStateTimer -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss(npc);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_5_Boss : EnemyBoss_State
{
    //Explotion Attack
    private float leaveStateTimer;
    private float firstBulletSpeed;
    private int bulletsAfterExplotion;
    public ATTACK_5_Boss(GameObject _npc)
        : base(_npc)
    {
        name = STATE.ATTACK_5_Boss;
    }

    public override void Enter()
    {
        switch (npc.GetComponent<EnemyBoss>().bossHealth)
        {
            case 2:
                leaveStateTimer = 1.75f;
                firstBulletSpeed = 80;
                bulletsAfterExplotion = Random.Range(7, 10);
                break;
            case 1:
                leaveStateTimer = 1.5f;
                firstBulletSpeed = 115;
                bulletsAfterExplotion = Random.Range(9, 12);
                break;
        }
        npc.GetComponent<CreateAttack>().SpawnSplittingExplodingBullet(firstBulletSpeed, bulletsAfterExplotion, 100);
        base.Enter();
    }

    public override void Update()
    {
        if (leaveStateTimer > 0)
            leaveStateTimer -= Time.deltaTime;
        else
        {
            nextState = new CHOOSE_Boss(npc);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
