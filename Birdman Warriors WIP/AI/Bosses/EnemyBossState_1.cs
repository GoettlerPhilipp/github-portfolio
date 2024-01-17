using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBossState_1 
{
    public enum STATE_Boss_1
    {
        IDLE_Boss,
        CHOOSE_Boss,
        ATTACK_1_Boss, // Basic Attack
        ATTACK_2_Boss, // Follow Player Attack
        ATTACK_3_Boss, // Cone Attack
        Attack_4_Boss, // Multi Attack
        ATTACK_5_Boss, // Explotion Attack
        ATTACK_6_Boss, // Feint Attack
        ATTACK_7_Boss, // V Formation Attack
        ATTACK_8_Boss, // Around Me
        ATTACK_9_Boss, // Around Me Spirale
        ATTACK_10_Boss,// Around Me V Formation
        ATTACK_11_Boss,// Comet
        LOST_Boss
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }
    
    public STATE_Boss_1 name;
    protected EVENT stage;
    protected GameObject npc;
    protected EnemyBossState_1 nextState;
    protected EnemyBoss_1 npcScript;
    
    
    protected GameObject player;

    public EnemyBossState_1(GameObject _npc, EnemyBoss_1 _npcScript)
    {
        npc = _npc;
        npcScript = _npcScript;
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

    public EnemyBossState_1 Process()
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

public class IDLE_Boss_1 : EnemyBossState_1
{
    
    //Start Phase
    
    private float leaveStateTimer = 2f;
    
    public IDLE_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.IDLE_Boss;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Enter()
    {
        if (npcScript.bossController.startBoss)
        {
            npcScript.bossController.getKnockback = true;
            base.Enter();
        }
        
    }

    public override void Update()
    {
        if (leaveStateTimer > 0)
            leaveStateTimer -= Time.deltaTime;
        else
        {
            nextState = new CHOOSE_Boss_1(npc, npcScript);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class CHOOSE_Boss_1 : EnemyBossState_1
{
    //Sucht sich die verschiedenen Angriffe aus
    public bool rolledTheDice;
    private float resetTimer = 3f;
    public CHOOSE_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.CHOOSE_Boss;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npc.GetComponent<EnemyBoss_1>().bossHealth <= 0)
        {
            nextState = new LOST_Boss_1(npc, npcScript);
            stage = EVENT.EXIT;
            return;
        }
        Debug.Log("Boss Leben: " + npc.GetComponent<EnemyBoss_1>().bossHealth);
        if (!rolledTheDice)
        {
            rolledTheDice = true;
            int randomNum = Random.Range(0, 12);
            switch (randomNum)
            {
                case 0:
                    resetTimer = 1.5f;
                    break;
                case 1:
                    //Basic Attack
                    nextState = new ATTACK_1_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 2:
                    // Attack
                    nextState = new ATTACK_2_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 3:
                    //Cone Attack
                    nextState = new ATTACK_3_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 4:
                    //Multi Attack
                    resetTimer = 1.5f;
                    nextState = new ATTACK_4_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 5:
                    //Explotion Attack
                    nextState = new ATTACK_5_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 6:
                    //Feint Attack
                    nextState = new ATTACK_6_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 7:
                    //V Formation Attack
                    nextState = new ATTACK_7_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 8:
                    //Around Me Attack
                    nextState = new ATTACK_8_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 9:
                    //Spirale Attack
                    nextState = new ATTACK_9_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 10:
                    //V Formation Around Me Attack
                    nextState = new ATTACK_10_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
                case 11:
                    //Comet Attack
                    nextState = new ATTACK_11_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
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

public class ATTACK_1_Boss_1 : EnemyBossState_1
{
    //Schuss angriff
    private float bulletCooldown = 0.2f;
    private float switchStateCooldown;
    private int shotsInMagazin;
    
    
    public ATTACK_1_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_1_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        switch (npcScript.bossHealth)
        {
            case 2:
                shotsInMagazin = (int)Random.Range(npcScript.normalBulletExecute.x, npcScript.normalBulletExecute.y);
                break;
            case 1:
                shotsInMagazin = (int)Random.Range(npcScript.normalBulletExecute.x * npcScript.normalBulletMultiplier, (npcScript.normalBulletExecute.y +1) * npcScript.normalBulletMultiplier);
                break;
        }
        base.Enter();
    }

    public override void Update()
    {
        if (npcScript.bossHealth <= 0)
        {
            nextState = new LOST_Boss_1(npc, npcScript);
            stage = EVENT.EXIT;
            return;
        }
        //Schießt immer nur eine Kugel im Intervall + leert das Magazin
        if (bulletCooldown <= 0 && shotsInMagazin > 0)
        {
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnBullet(npcScript.normalBulletSpeed);
                    bulletCooldown = Random.Range(npcScript.normalBulletExecutionCooldown.x, npcScript.normalBulletExecutionCooldown.y);
                    shotsInMagazin -= 1;
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>().SpawnBullet(npcScript.normalBulletSpeed * npcScript.normalBulletMultiplier);
                    bulletCooldown = Random.Range(npcScript.normalBulletExecutionCooldown.x / npcScript.normalBulletMultiplier, npcScript.normalBulletExecutionCooldown.y / npcScript.normalBulletMultiplier);
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
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_2_Boss_1 : EnemyBossState_1
{
    //Angriff für die Follow Player
    
    private float bulletCooldown;
    private int shotsInMagazin;
    
    private float switchStateCooldown;
    public ATTACK_2_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_2_Boss;
        shotsInMagazin = (int)Random.Range(npcScript.followPlayerBulletCount.x, npcScript.followPlayerBulletCount.y +1);
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (npcScript.bossHealth <= 0)
        {
            nextState = new LOST_Boss_1(npc, npcScript);
            stage = EVENT.EXIT;
            return;
        }
        //Schießt immer nur eine Kugel im Intervall + leert das Magazin
        if (bulletCooldown <= 0 && shotsInMagazin > 0)
        {
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnFollowTargetBullet(npcScript.followPlayerBulletSpeed);
                    bulletCooldown = Random.Range(npcScript.followPlayerBulletExecutionCooldown.x * npcScript.followPlayerBulletMultiplier, npcScript.followPlayerBulletExecutionCooldown.y + npcScript.followPlayerBulletMultiplier);
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>().SpawnFollowTargetBullet(npcScript.followPlayerBulletSpeed);
                    bulletCooldown = Random.Range(npcScript.followPlayerBulletExecutionCooldown.x / npcScript.followPlayerBulletMultiplier, npcScript.followPlayerBulletExecutionCooldown.y / npcScript.followPlayerBulletMultiplier);
                    break;
                default:
                    nextState = new CHOOSE_Boss_1(npc, npcScript);
                    stage = EVENT.EXIT;
                    break;
            }
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
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class LOST_Boss_1 : EnemyBossState_1
{
    // Player Won against the Enemy
    public LOST_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.LOST_Boss;
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

public class ATTACK_3_Boss_1 : EnemyBossState_1
{
    //Cone angriff
    private float switchStateCooldown;
    private bool getCalledOnce;

    public ATTACK_3_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_3_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        
        base.Enter();
    }

    public override void Update()
    {
        if (!getCalledOnce)
        {
            bool randomBool = Random.value > 0.5f;
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnBulletsInCone(npcScript.coneBulletSpeed, randomBool);
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>()
                        .SpawnBulletsInCone(npcScript.coneBulletMultiplier * npcScript.coneBulletMultiplier,
                            randomBool);
                    switchStateCooldown /= npcScript.coneBulletMultiplier;
                    break;
            }
            getCalledOnce = true;
        }

        if (switchStateCooldown > 0)
            switchStateCooldown -= Time.deltaTime;
        else
        {
            nextState = new CHOOSE_Boss_1(npc, npcScript);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_4_Boss_1 : EnemyBossState_1
{
    //Multi Attack
    
    private float switchStateCooldown;
    private int execut;
    private float seconds;
    
    public ATTACK_4_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.Attack_4_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
        execut = npcScript.multiAttackExecution;
    }

    public override void Enter()
    {
        
        base.Enter();
    }

    public override void Update()
    {
        if (execut > 0 && seconds <= 0)
        {
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>()
                        .MultiAttack(
                            (int)Random.Range(npcScript.multiAttackBulletCount.x,
                                npcScript.multiAttackBulletCount.y), npcScript.multiAttackBulletSpeed,
                            npcScript.multiAttackDistanceBetweenEachBullet);
                    seconds = npcScript.multiAttackSecondsBetweenEachWave;
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>()
                        .MultiAttack((int)Random.Range(npcScript.multiAttackBulletCount.x * npcScript.multiAttackMultiplier, npcScript.multiAttackBulletCount.y * npcScript.multiAttackMultiplier), npcScript.multiAttackBulletSpeed * npcScript.multiAttackMultiplier,
                            npcScript.multiAttackDistanceBetweenEachBullet);
                    seconds = npcScript.multiAttackSecondsBetweenEachWave / npcScript.multiAttackMultiplier;
                    break;
            }
            execut--;
        }

        if (seconds > 0)
            seconds -= Time.deltaTime;
        
        if(execut <= 0)
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_5_Boss_1 : EnemyBossState_1
{
    //Explotion Attack
    private float switchStateCooldown;
    private int explodingBulletsBulletSpawning;
    private bool executed;
    public ATTACK_5_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_5_Boss;
        explodingBulletsBulletSpawning = (int)Random.Range(npcScript.explodingBulletBulletsToSpawn.x, npcScript.explodingBulletBulletsToSpawn.y +1);
        switchStateCooldown = 1f;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(!executed)
        {
            npc.GetComponent<CreateAttack>().SpawnSplittingExplodingBullet(npcScript.explodingBulletBulletSpeed, explodingBulletsBulletSpawning, npcScript.normalBulletSpeed);
            executed = true;
        }
        else
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_6_Boss_1 : EnemyBossState_1
{
    // Feint Attack
    private float switchStateCooldown;
    
    private bool executed;
    
    public ATTACK_6_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_6_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!executed)
        {
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnFeint(npcScript.feintBulletSpeed, npcScript.feintFocusAfterTime, (int)Random.Range(npcScript.feintBulletCount.x, npcScript.feintBulletCount.y +1));
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>().SpawnFeint(npcScript.feintBulletSpeed * npcScript.feintMultiplier, npcScript.feintFocusAfterTime, (int)Random.Range(npcScript.feintBulletCount.x * npcScript.feintMultiplier, npcScript.feintBulletCount.y * npcScript.feintMultiplier +1));
                    break;
            }
            executed = true;
        }
        
        if (switchStateCooldown > 0)
            switchStateCooldown -= Time.deltaTime;
        else
        {
            nextState = new CHOOSE_Boss_1(npc, npcScript); 
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class ATTACK_7_Boss_1 : EnemyBossState_1
{
    // V Formation Attack
    private float switchStateCooldown;
    private int execute;
    private bool executed;
    
    public ATTACK_7_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_7_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
        execute = npcScript.vFormationExecution;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!executed)
        {
            for (int i = 0; i < execute; i++)
            {
                switch (npcScript.bossHealth)
                {
                    case 2:
                        npc.GetComponent<CreateAttack>().SpawnBulletsInVFormation(npcScript.vFormationBulletSpeed,
                            npcScript.vFormationDistanceBetweenEachBullet, npcScript.vFormationSeconds,
                            (int)Random.Range(npcScript.vFormationBulletCount.x, npcScript.vFormationBulletCount.y));
                        break;
                    case 1:
                        npc.GetComponent<CreateAttack>().SpawnBulletsInVFormation(
                            npcScript.vFormationBulletSpeed * npcScript.vFormationMultiplier,
                            npcScript.vFormationDistanceBetweenEachBullet,
                            npcScript.vFormationSeconds / npcScript.vFormationMultiplier,
                            (int)Random.Range(npcScript.vFormationBulletCount.x * npcScript.vFormationMultiplier,
                                npcScript.vFormationBulletCount.y * npcScript.vFormationMultiplier));
                        break;
                }
            }
            executed = true;
        }
        else
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class ATTACK_8_Boss_1 : EnemyBossState_1
{
    // Around Me Attack
    private float switchStateCooldown;
    private bool executed;
    private int execute;
    
    public ATTACK_8_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_8_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
        execute = npcScript.aroundMeWaveCounter;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!executed)
        {
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnBulletsAroundMe(execute,
                        npcScript.aroundMeSecondsBetweenEachWave,
                        (int)Random.Range(npcScript.aroundMeBulletCount.x, npcScript.aroundMeBulletCount.y),
                        npcScript.aroundMeBulletSpeed);
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>().SpawnBulletsAroundMe(execute,
                        npcScript.aroundMeSecondsBetweenEachWave / npcScript.aroundMeMultiplier,
                        (int)Random.Range(npcScript.aroundMeBulletCount.x * npcScript.aroundMeMultiplier, npcScript.aroundMeBulletCount.y * npcScript.aroundMeMultiplier),
                        npcScript.aroundMeBulletSpeed * npcScript.aroundMeMultiplier);
                    break;
            }
            executed = true;
        }
        else
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class ATTACK_9_Boss_1 : EnemyBossState_1
{
    // Around Me Spirale Attack
    private float switchStateCooldown;
    private bool executed;
    
    public ATTACK_9_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_9_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!executed)
        {
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnBulletsInSpirale((int)Random.Range(npcScript.spiraleBulletCount.x, npcScript.spiraleBulletCount.y), npcScript.spiraleBulletSpeed, npcScript.spiraleDuration);
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>().SpawnBulletsInSpirale((int)Random.Range(npcScript.spiraleBulletCount.x * npcScript.spiraleMultiplier, npcScript.spiraleBulletCount.y * npcScript.spiraleMultiplier), npcScript.spiraleBulletSpeed, npcScript.spiraleDuration);
                    break;
            }
            executed = true;
        }
        else
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class ATTACK_10_Boss_1 : EnemyBossState_1
{
    // Around Me V Formation Attack
    private float switchStateCooldown;
    private bool executed;
    
    public ATTACK_10_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_10_Boss;
        switchStateCooldown = npcScript.switchStateCooldown;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!executed)
        {
            switch (npcScript.bossHealth)
            {
                case 2:
                    npc.GetComponent<CreateAttack>().SpawnVFormationAroundMe(npcScript.vFormationAmBulletSpeed,
                        npcScript.vFormationAmDistanceBetweenEachBullet, npcScript.vFormationAmSeconds,(int)Random.Range(npcScript.vFormationAmBulletCount.x, npcScript.vFormationAmFormationCount.y),
                        (int)Random.Range(npcScript.vFormationAmBulletCount.x,
                            npcScript.vFormationAmBulletCount.y));
                    break;
                case 1:
                    npc.GetComponent<CreateAttack>().SpawnVFormationAroundMe(npcScript.vFormationAmBulletSpeed * npcScript.vFormationAmMultiplier,
                        npcScript.vFormationAmDistanceBetweenEachBullet, npcScript.vFormationAmSeconds / npcScript.vFormationAmMultiplier,
                        (int)Random.Range(npcScript.vFormationAmBulletCount.x * npcScript.vFormationAmMultiplier, npcScript.vFormationAmFormationCount.y * npcScript.vFormationAmMultiplier),
                        (int)Random.Range(npcScript.vFormationAmBulletCount.x * npcScript.vFormationAmMultiplier,
                            npcScript.vFormationAmBulletCount.y * npcScript.vFormationAmMultiplier));
                    break;
            }
            executed = true;
        }
        else
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class ATTACK_11_Boss_1 : EnemyBossState_1
{
    // Comet Attack
    private float switchStateCooldown;
    private bool executed;
    
    public ATTACK_11_Boss_1(GameObject _npc, EnemyBoss_1 _npcScript)
        : base(_npc, _npcScript)
    {
        name = STATE_Boss_1.ATTACK_11_Boss;
        switchStateCooldown = 1;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!executed)
        {
            npc.GetComponent<CreateAttack>().SpawnComet();
            executed = true;
        }
        else
        {
            if (switchStateCooldown > 0)
                switchStateCooldown -= Time.deltaTime;
            else
            {
                nextState = new CHOOSE_Boss_1(npc, npcScript);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}