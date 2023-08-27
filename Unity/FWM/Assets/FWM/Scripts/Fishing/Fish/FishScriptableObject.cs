using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish Config", menuName = "ScriptableObject/Fish Config")]




public class FishScriptableObject : ScriptableObject
{
	
	
	public string fishName = "fush";
	
	public Biome spawnBiome = Biome.Lake;
	public Vector2 spawnHeight = new Vector2(0f, -50f);
	
	public float wanderRadius = 5f;
	public float arrivedErrorRadius = 0.3f;
	public float heightTruncationFactor = 0.7f;
	
	public float swimSpeed = 0.5f;
	public float accelFactor = 0.5f;
	public float rotFactor = 1.3f;
	
	public float furthestRadius = 2.5f;
	public float closestRadius = 0.5f;
	public float getBoredDistance = 5f;
	
	public int initAttention = 25;
	public int attentionIncrement = 15;
	public int attentionDecrement = 5;
	public int maxAttention = 100;
	
	public Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
	public float sightDist = 20f;
	public float sightRadius = 5f;
}

public enum Biome
{
	Lake,
	River,
	Beach
};
