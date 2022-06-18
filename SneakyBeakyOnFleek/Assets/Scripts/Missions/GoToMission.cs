using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMission : IMission
{
	private GoToMissionInfo createInfo;

	public MissionType MyType { get { return MissionType.GoTo; } set { } }

	public string Title { get; set; }

	public void Create(MissionCreateInfo info)
	{
		createInfo = (GoToMissionInfo)info;
	}

	public bool Evaluate(MissionEvaluateInfo info)
	{
		GoToMissionEvaluateInfo evaluateInfo = (GoToMissionEvaluateInfo)info;

		Vector3 dir = createInfo.TargetPosition - evaluateInfo.CurrentPosition;
		if(dir.sqrMagnitude < createInfo.MaxDistanceSquared)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

public class GoToMissionInfo : MissionCreateInfo
{
	public Vector3 TargetPosition;
	public float MaxDistanceSquared;

	public GoToMissionInfo(Vector3 target, float maxDistanceSquared)
	{
		TargetPosition = target;
		MaxDistanceSquared = maxDistanceSquared;
	}
}

public class GoToMissionEvaluateInfo : MissionEvaluateInfo
{
	public Vector3 CurrentPosition;

	public GoToMissionEvaluateInfo(Vector3 pos)
	{
		CurrentPosition = pos;
	}
}
