using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EnemyLevel03Buildings : MonoBehaviour
{
    
    //Because level three is a time level and I still wanted the opponent to look alive and different in each attempt.
    //He always rolls the dice to decide which building to build next
    
    public List<GameObject> haveToBuildThisBuildings;
    public List<GameObject> haveBuildedThisBuildings;

    public int startBuildings;

    [SerializeField] private float timer;
    [SerializeField] private float buildingTimer;
    [Tooltip("Setze hier den Timer wie lange das Level gehen soll")]
    [SerializeField] private float levelTimer;
    [SerializeField] private GameObject temple;
    public float lastBuildingTimer = 120f;
    public bool isBuildingTempleNow;
    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<IngameMenuController>().GetComponent<IngameMenuController>().isBuildingTemple = this;
        startBuildings = haveToBuildThisBuildings.Count;
        buildingTimer = levelTimer / startBuildings;
    }

    // Update is called once per frame
    void Update()
    {
        if(haveToBuildThisBuildings.Count > 0)
            CountDown();
        else if(haveToBuildThisBuildings.Count == 0 && SceneManager.GetSceneByName("Level03").isLoaded)
        {
            StartCoroutine(LastBuildingCountdown());
            if (!isBuildingTempleNow)
                isBuildingTempleNow = true;
        }
    }

    void CountDown()
    {
        if (timer >= 0)
        {
            timer -= 1 * Time.deltaTime;
        }
        else if (timer <= 0)
        {
            var randomBuilding = haveToBuildThisBuildings[Random.Range(0, haveToBuildThisBuildings.Count)];
            randomBuilding.layer = 12;
            randomBuilding.SetActive(true);
            haveBuildedThisBuildings.Add(randomBuilding);
            haveToBuildThisBuildings.Remove(randomBuilding);
            timer = buildingTimer;
        }
    }

    IEnumerator LastBuildingCountdown()
    {
        if (lastBuildingTimer >= 0)
            lastBuildingTimer -= 1 * Time.deltaTime;
        else if (lastBuildingTimer <= 0)
        {
            temple.SetActive(true);
            yield return new WaitForSecondsRealtime(10f);
            PauseMenuController.instance.panelGameOverMenu.SetActive(true);
        }

    }
}
