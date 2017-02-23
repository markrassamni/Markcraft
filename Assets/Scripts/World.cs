using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noise;

public class World : MonoBehaviour {

	[SerializeField] private GameObject chunk;
	[SerializeField] private int worldX = 16;
	[SerializeField] private int worldY = 16;
	[SerializeField] private int worldZ = 16;
	[SerializeField] private int chunkSize = 16;
	private byte[,,] worldData;
	private Chunk[,,] chunks;


	void Start () {
		worldData = new byte[worldX, worldY, worldZ];
		for(int x=0; x<worldX;x++){
			for(int z=0; z<worldZ; z++){
				int rock = PerlinNoise(x,0,z, 10f, 3f, 1.2f) + PerlinNoise(x,200,z,20f,8f,0f) + 10;
				int grass = PerlinNoise(x,100,z,50f, 30f, 0f) + 1;
				for(int y=0; y<worldY; y++){
					if(y<=rock){
						worldData[x,y,z] = (byte)TextureType.grass.GetHashCode();
					} else if (y<=grass){
						worldData[x,y,z] = (byte)TextureType.rock.GetHashCode();
					}
				}
			}
		}

		chunks = new Chunk[worldX/chunkSize,worldY/chunkSize,worldZ/chunkSize];
		for(int x=0; x<chunks.GetLength(0);x++){
			for(int y=0; y<chunks.GetLength(1);y++){
				for(int z=0; z<chunks.GetLength(2);z++){
					GameObject newChunk = Instantiate(chunk, new Vector3(x*chunkSize, y*chunkSize, z*chunkSize), Quaternion.identity);
					chunks[x,y,z] = newChunk.GetComponent<Chunk>() as Chunk;
					chunks[x,y,z].WorldGameObj = gameObject;
					chunks[x,y,z].ChunkSize = chunkSize;
					chunks[x,y,z].ChunkX = x*chunkSize;
					chunks[x,y,z].ChunkY = y*chunkSize;
					chunks[x,y,z].ChunkZ = z*chunkSize;



				}	
			}
		}

	}
	
	void Update () {
		
	}

	private int PerlinNoise(int x, int y, int z, float scale, float height, float power){
		float perlinVal = Noise.Noise.GetNoise((double) x/scale, (double) y/scale, (double) z/scale) * height;
		if (power != 0) {
			perlinVal = Mathf.Pow(perlinVal, power);
		}
		return (int) perlinVal;
	}

	public byte Block(int x, int y, int z){
		if (x>= worldX || x < 0 || y >= worldY || y<0 || z >= worldZ || z<0){
			return (byte)TextureType.rock.GetHashCode(); //return rock by default if outside array
		}
		return worldData[x,y,z];
	}
}
