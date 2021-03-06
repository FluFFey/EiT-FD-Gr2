﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job  {
	public int id { get; set;}
	public int numWorkers { get; set;}
	public JobType jobbType;

	public Job (JobType jobbType, int id, int numWorkers)
	{
		this.jobbType = jobbType;
		this.id = id;
		this.numWorkers = numWorkers;
	}	
}

public enum JobType {
	FISH, HUNT, CLONE, SPLICE
}