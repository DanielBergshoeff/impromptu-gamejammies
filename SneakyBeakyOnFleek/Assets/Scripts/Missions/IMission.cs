using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMission
{
	public bool Evaluate(MissionEvaluateInfo missionInformation);

	public void Create(MissionCreateInfo missionInformation);

	public MissionType MyType { get; set; }

	public string Title { get; set; }
}

public enum MissionType
{
	None,
	GoTo,
	Fetch
}
