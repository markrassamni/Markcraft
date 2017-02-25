using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyTerrain : Singleton<ModifyTerrain> {

	[SerializeField] private float distanceToLoad = 50f;
	[SerializeField] private float distanceToDestroy = 60f;
	private World world;
	private GameObject character;
	void Start () {
		world = GetComponent<World>();
		character = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		LoadChunks(character.transform.position);
	}

	public void DestroyBlock(float range, byte block){
		Ray ray = new Ray(character.transform.position, character.transform.forward);
		RaycastHit hit;
		if(Physics.Raycast (ray, out hit)){
			if(hit.distance < range){
				DestroyBlockAt(hit, block);
			}
		}
	}

	public void DestroyBlockAt(RaycastHit hit, byte block){
		Vector3 position = hit.point;
		position += (hit.normal * -.5f);
		SetBlockAt(position, block);
	}

	public void AddBlock(float range, byte block){
		Ray ray = new Ray(character.transform.position, character.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)){
			if (hit.distance < range){
				AddBlockAt(hit, block);
			}
		}
	}

	public void AddBlockAt(RaycastHit hit, byte block){
		Vector3 position = hit.point;
		position += (hit.normal * .5f);
		SetBlockAt(position, block);
	}

	public void SetBlockAt(Vector3 position, byte block){
		int x = Mathf.RoundToInt(position.x);
		int y = Mathf.RoundToInt(position.y);
		int z = Mathf.RoundToInt(position.z);

		world.WorldData[x,y,z] = block;
		UpdateChunkAt(x,y,z);
	}

	public void UpdateChunkAt(int x, int y, int z){
		x/= world.ChunkSize;
		y/= world.ChunkSize;
		z/= world.ChunkSize;
		// int updateX = x/world.ChunkSize;
		// int updateY = y/world.ChunkSize;
		// int updateZ = z/world.ChunkSize;
		world.Chunks[x,y,z].IsUpdate = true;

		if(x-(world.ChunkSize * x) == 0 && x!= 0){
			world.Chunks[x-1,y,z].IsUpdate = true;
		}

		if(x-(world.ChunkSize * x) == 15 && x!= world.Chunks.GetLength(0)-1){
			world.Chunks[x+1,y,z].IsUpdate = true;
		}

		if(y-(world.ChunkSize * y) == 0 && y!= 0){
			world.Chunks[x,y-1,z].IsUpdate = true;
		}

		if(y-(world.ChunkSize * y) == 15 && y!= world.Chunks.GetLength(1)-1){
			world.Chunks[x,y+1,z].IsUpdate = true;
		}

		if(z-(world.ChunkSize * z) == 0 && z!= 0){
			world.Chunks[x,y,z-1].IsUpdate = true;
		}

		if(z-(world.ChunkSize * z) == 15 && z!= world.Chunks.GetLength(2)-1){
			world.Chunks[x,y,z+1].IsUpdate = true;
		}
	}

	public void LoadChunks(Vector3 playerPosition){
		for(int x=0; x<world.Chunks.GetLength(0);x++){
			for(int z=0; z<world.Chunks.GetLength(2);z++){
				float distance = Vector2.Distance(new Vector2(x*world.ChunkSize, z*world.ChunkSize), new Vector2(playerPosition.x, playerPosition.z));
				if (distance < distanceToLoad){
					if(world.Chunks[x,0,z] == null){
						world.GenerateChunk(x,z);
					}
				} else if(distance > distanceToDestroy){
					if(world.Chunks[x,0,z] != null){
						world.DestroyChunk(x,z);
					}
				}
			}
		}
	}
}
