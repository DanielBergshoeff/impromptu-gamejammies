using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchMission : IMission
{
	private FetchMissionInfo createInfo;

	public MissionType MyType { get { return MissionType.Fetch; } set { } }

	public string Title { get; set; }

	public void Create(MissionCreateInfo info)
	{
		createInfo = (FetchMissionInfo)info;
	}

	public bool Evaluate(MissionEvaluateInfo info)
	{
		FetchMissionEvaluateInfo evaluateInfo = (FetchMissionEvaluateInfo)info;

		Vector3 dir = createInfo.TargetPosition - createInfo.FetchObject.position;
		if (dir.sqrMagnitude < createInfo.MaxDistanceSquared)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

public class FetchMissionInfo : MissionCreateInfo
{
	public Vector3 TargetPosition;
	public float MaxDistanceSquared;
	public Transform FetchObject;

	public FetchMissionInfo(Vector3 target, float maxDistSquared, Transform fetchObject)
	{
		TargetPosition = target;
		MaxDistanceSquared = maxDistSquared;
		FetchObject = fetchObject;
	}
}

public class FetchMissionEvaluateInfo : MissionEvaluateInfo
{
	
}
