using System;
using System.Collections.Generic;

public class NaturalDisaster
{
	public String name { get; }
	public DisasterProperty property { get;}


	public NaturalDisaster (string name, DisasterProperty property) {
		this.name = name;
		this.property = property;
	}
}
	
public enum DisasterProperty { 
	//TODO: INSERT all properties
	WIND, WATER, EARTHQUAKE
}


