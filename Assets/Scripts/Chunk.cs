using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextureType{
	air, grass, rock
}

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class Chunk : MonoBehaviour {

	private GameObject worldGameObj;
	private World world;
	private List <Vector3> newVertices = new List<Vector3>();
	private List <int> newTriangles = new List<int>();
	private List <Vector2> newUV = new List<Vector2>();
	private Mesh mesh;
	private MeshCollider chunkCollider;
	private float textureWidth = 0.083333f;
	private int chunkSize = 16;
	private int chunkX;
	private int chunkY;
	private int chunkZ;
	private int faceCount;
	private bool isUpdate = false;

	//Textures
	private Vector2 grassTop = new Vector2(1,11);
	private Vector2 grassSide = new Vector2(0,10);
	private Vector2 stoneTop = new Vector2(7,8);
	private Vector2 stoneSide = new Vector2(6,7);
	public int ChunkSize {
		get{
			return chunkSize;
		} set {
			chunkSize = value;
		}
	}
	public int ChunkX {
		get{
			return chunkX;
		} set {
			chunkX = value;
		}
	}
	public int ChunkY {
		get{
			return chunkY;
		} set {
			chunkY = value;
		}
	}
	public int ChunkZ {
		get{
			return chunkZ;
		} set {
			chunkZ = value;
		}
	}
	public GameObject WorldGameObj{
		get{
			return worldGameObj;
		} set {
			worldGameObj = value;
		}
	}
	public bool IsUpdate{
		get{
			return isUpdate;
		} set {
			isUpdate = value;
		}
	}

	void Start () {
		world = worldGameObj.GetComponent<World>(); //FIX??
		mesh = GetComponent<MeshFilter>().mesh;
		chunkCollider = GetComponent<MeshCollider>();

		GenerateMesh();
	}
	
	void LateUpdate () {
		if(isUpdate){
			GenerateMesh();
			isUpdate = false;
		}
	}

	//called every time surface created
	private void UpdateMesh(){ 
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.RecalculateNormals();


		//chunkCollider.sharedMesh = null;
		chunkCollider.sharedMesh = mesh;

		newVertices.Clear();
		newUV.Clear();
		newTriangles.Clear();
		faceCount = 0;
	}

	public void GenerateMesh(){
		for(int x=0; x<chunkSize; x++){
			for(int y=0; y<chunkSize; y++){
				for(int z=0; z<chunkSize; z++){
					if(Block(x,y,z) != (byte)TextureType.air.GetHashCode()){
						//Block above is air
						if(Block(x,y+1,z) == (byte)TextureType.air.GetHashCode()){
							CubeTop(x,y,z,Block(x,y,z));
						}
						//Block below is air
						if(Block(x,y-1,z) == (byte)TextureType.air.GetHashCode()){
							CubeBottom(x,y,z,Block(x,y,z));
						}
						//Block east is air
						if(Block(x+1,y,z) == (byte)TextureType.air.GetHashCode()){
							CubeEast(x,y,z,Block(x,y,z));
						}
						//Block west is air
						if(Block(x-1,y,z) == (byte)TextureType.air.GetHashCode()){
							CubeWest(x,y,z,Block(x,y,z));
						}
						//Block north is air
						if(Block(x,y,z+1) == (byte)TextureType.air.GetHashCode()){
							CubeNorth(x,y,z,Block(x,y,z));
						}
						//Block south is air
						if(Block(x,y,z-1) == (byte)TextureType.air.GetHashCode()){
							CubeSouth(x,y,z,Block(x,y,z));
						}
					}
				}
			}
		}
		UpdateMesh();
	}

	private void CubeTop(int x, int y, int z, byte block){
		newVertices.Add( new Vector3(x,y,z+1));
		newVertices.Add( new Vector3(x+1,y,z+1));
		newVertices.Add( new Vector3(x+1,y,z));
		newVertices.Add( new Vector3(x,y,z));

		Vector2 texturePosition = Vector2.zero;

		if (block == (byte)TextureType.rock.GetHashCode()){
			texturePosition = stoneTop;
		} else if (block ==  (byte)TextureType.grass.GetHashCode()){
			texturePosition = grassTop;
		}
		Cube(texturePosition);
	}
	private void CubeNorth(int x, int y, int z, byte block){
		newVertices.Add( new Vector3(x+1,y-1,z+1));
		newVertices.Add( new Vector3(x+1,y,z+1));
		newVertices.Add( new Vector3(x,y,z+1));
		newVertices.Add( new Vector3(x,y-1,z+1));
		

		Vector2 texturePosition = SetSideTextures(x,y,z,block);
		Cube(texturePosition);
	}

	private void CubeEast(int x, int y, int z, byte block){
		newVertices.Add( new Vector3(x+1,y-1,z));
		newVertices.Add( new Vector3(x+1,y,z));
		newVertices.Add( new Vector3(x+1,y,z+1));
		newVertices.Add( new Vector3(x+1,y-1,z+1));

		Vector2 texturePosition = SetSideTextures(x,y,z,block);
		Cube(texturePosition);
	}

	private void CubeSouth(int x, int y, int z, byte block){
		newVertices.Add( new Vector3(x,y-1,z));
		newVertices.Add( new Vector3(x,y,z));
		newVertices.Add( new Vector3(x+1,y,z));
		newVertices.Add( new Vector3(x+1,y-1,z));

		Vector2 texturePosition = SetSideTextures(x,y,z,block);
		Cube(texturePosition);
	}

	private void CubeWest(int x, int y, int z, byte block){
		newVertices.Add( new Vector3(x,y-1,z+1));
		newVertices.Add( new Vector3(x,y,z+1));
		newVertices.Add( new Vector3(x,y,z));
		newVertices.Add( new Vector3(x,y-1,z));

		Vector2 texturePosition = SetSideTextures(x,y,z,block);
		Cube(texturePosition);
	}

	private void CubeBottom(int x, int y, int z, byte block){
		newVertices.Add( new Vector3(x,y-1,z));
		newVertices.Add( new Vector3(x+1,y-1,z));
		newVertices.Add( new Vector3(x+1,y-1,z+1));
		newVertices.Add( new Vector3(x,y-1,z+1));

		Vector2 texturePosition = SetSideTextures(x,y,z,block);
		Cube(texturePosition);
	}

	private Vector2 SetSideTextures(int x, int y, int z, byte block){
		Vector2 texturePosition = Vector2.zero;
		if (block == (byte)TextureType.rock.GetHashCode()){
			texturePosition = stoneSide;
		} else if (block == (byte)TextureType.grass.GetHashCode()){
			texturePosition = grassSide;
		}
		return texturePosition;
	}

	private void Cube(Vector2 texturePosition){
		newTriangles.Add(faceCount * 4); 		//1
		newTriangles.Add(faceCount * 4 + 1); 	//2
		newTriangles.Add(faceCount * 4 + 2); 	//3
		newTriangles.Add(faceCount * 4); 		//1 
		newTriangles.Add(faceCount * 4 + 2); 	//3 
		newTriangles.Add(faceCount * 4 + 3); 	//4

		newUV.Add(new Vector2(textureWidth * texturePosition.x + textureWidth, textureWidth * texturePosition.y));
		newUV.Add(new Vector2(textureWidth * texturePosition.x + textureWidth, textureWidth * texturePosition.y + textureWidth));
		newUV.Add(new Vector2(textureWidth * texturePosition.x, textureWidth * texturePosition.y + textureWidth));
		newUV.Add(new Vector2(textureWidth * texturePosition.x, textureWidth * texturePosition.y));

		faceCount++;

	}

	private byte Block(int x, int y, int z){
		return world.Block(x+chunkX, y+chunkY, z+chunkZ);
	}

	


}
