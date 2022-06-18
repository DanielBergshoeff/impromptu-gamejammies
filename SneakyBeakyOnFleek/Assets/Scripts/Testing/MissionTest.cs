using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTest : MonoBehaviour
{
	MissionManager missionManager;

	public GameObject TestTarget1;
	public GameObject TestTarget2;
	public GameObject TestTarget3;
	public GameObject TestTarget4;

	public InteractableData TestCombineFuel;

	private void Awake()
	{
		missionManager = GetComponent<MissionManager>();
	}

	// Start is called before the first frame update
	void Start()
	{
		//missionManager.CreateGoToMission(TestTarget2.transform.position, 1f, "GO TO OBJECT 2");
		//missionManager.CreateGoToMission(TestTarget3.transform.position, 1f, "GO TO OBJECT 3");
		//missionManager.CreateFetchMission(TestTarget4.transform, Vector3.zero, 1f, "FETCH OBJECT 4");
		missionManager.CreateCombineMission(TestCombineFuel, "CREATE FUEL");
		missionManager.CreateGoToMission(TestTarget1.transform.position, 1f, "GO TO OBJECT 1");
	}
}
