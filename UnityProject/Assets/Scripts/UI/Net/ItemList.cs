﻿using System;
using UnityEngine;
using Util;
/// prefab-based for now
/// all server only
public class ItemList : NetUIDynamicList {

	public bool AddItem( string prefabName ) {
		foreach ( DynamicEntry item in Entries.Values ) {
			if ( String.Equals( ( (ItemEntry) item )?.Prefab.ExpensiveName(), prefabName,
				StringComparison.CurrentCultureIgnoreCase ) ) 
			{
				Debug.Log( $"Item {prefabName} already exists in ItemList" );
				return false;
			}
		}
		//load prefab
		GameObject prefab = Resources.Load( prefabName ) as GameObject;
		if ( !prefab || !prefab.GetComponent<ItemAttributes>() ) {
			Debug.LogWarning( $"No valid prefab found: {prefabName}" );
			return false;
		}
		
		//add new entry
		ItemEntry newEntry = Add() as ItemEntry;
		if ( !newEntry ) {
			Debug.LogWarning( $"Added {newEntry} is not an ItemEntry!" );
			return false;
		}
		//set its elements
		newEntry.Prefab = prefab;
		
		newEntry.Init();
		Debug.Log( $"ItemList: Item add success! newEntry={newEntry}" );

		//reinit and notify
		NetworkTabManager.Instance.ReInit( MasterTab.NetworkTab );
		
		UpdatePeepers(); //todo: should probably move this to parent
		
		return true;
	}


	public bool RemoveItem( string prefabName ) { //todo
		foreach ( var pair in Entries ) {
			if ( String.Equals( ( (ItemEntry) pair.Value )?.Prefab.name, prefabName,
				StringComparison.CurrentCultureIgnoreCase ) ) 
			{
				Remove( pair.Key );
				return true;
			}
		}
		Debug.LogWarning( $"Didn't find any prefabs called '{prefabName}' in the list" );
		return false;
	}
}