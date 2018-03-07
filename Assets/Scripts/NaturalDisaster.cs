using System;
using System.Collections.Generic;

public class NaturalDisaster
{
	public String name { get; }
	public Property property { get;}


	public NaturalDisaster (string name, Property property) {
		this.name = name;
		this.property = property;
	}
}
	
public enum Property { 
	//TODO: INSERT all properties
	WIND, WATER, EARTHQUAKE
}


