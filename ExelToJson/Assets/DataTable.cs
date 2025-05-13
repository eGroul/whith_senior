using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DataTable : ScriptableObject
{
	//public List<EntityType> Difintion; // Replace 'EntityType' to an actual type that is serializable.
	public List<EntityType> Data; // Replace 'EntityType' to an actual type that is serializable.
}
