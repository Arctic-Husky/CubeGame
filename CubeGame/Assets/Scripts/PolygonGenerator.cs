using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    // Contem todos os vertices do mesh
    public List<Vector3> newVertices = new List<Vector3>();

    // Triangulos dao instrucoes para o Unity de como constrir as secoes do mesh conectando nos vertices
    public List<int> newTriangles = new List<int>();

    // Como a textura e direcionada com cada poligono
    public List<Vector2> newUV = new List<Vector2>();

    // array 2d, cabe 0-255 tipos diferentes de blocos, usando isto no lugar de enumerators
    public byte[,] blocks;

    public List<Vector3> colVertices = new List<Vector3>();

    public List<int> colTriangles = new List<int>();

    private int colCount;

    private MeshCollider col;

    // Mesh = vertices, triangulos e UVs. Salvar tudo neste mesh
    private Mesh mesh;

    private float tUnit = 0.25f;
    public Vector2 tStone = new Vector2(1, 0); // Canto inferior esquerdo, coordenadas da textura da "Pedra"
    public Vector2 tGrass = new Vector2(0, 1);

    private int squareCount;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        col = GetComponent<MeshCollider>();

        /*float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;*/

        //GenSquare(x, y, tStone);

        GenTerrain();
        BuildMesh();

        UpdateMesh();
    }

    private void BuildMesh()
    {
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                //se o bloco nao for ar
                if (blocks[px, py] != 0)
                {

                    //gerar collider aqui, para nao gerar collider em ar
                    GenCollider(px, py);

                    if (blocks[px, py] == 1)
                    {
                        GenSquare(px, py, tStone);
                    }
                    else if (blocks[px, py] == 2)
                    {
                        GenSquare(px, py, tGrass);
                    }
                }
            }
        }
    }

    private byte Block(int x, int y)
    {
        if( x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
        {
            return (byte)1;
        }

        return blocks[x, y];
    }

    private void GenCollider(int x, int y)
    {
        //Topo
        if(Block(x,y+1) == 0) // Se for ar
        {
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 0));
            colVertices.Add(new Vector3(x, y, 0));

            ColliderTriangles();

            colCount++;
        }

        //Baixo
        if (Block(x, y - 1) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x, y - 1, 1));

            ColliderTriangles();

            colCount++;
        }

        //Esquerda
        if (Block(x-1, y) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 1));
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x, y, 0));
            colVertices.Add(new Vector3(x, y - 1, 0));

            ColliderTriangles();

            colCount++;
        }

        //Direita
        if (Block(x+1, y) == 0)
        {
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y, 0));

            ColliderTriangles();

            colCount++;
        }
    }

    private void ColliderTriangles()
    {
        colTriangles.Add((colCount * 4) + 0);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 3);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 2);
        colTriangles.Add((colCount * 4) + 3);
    }

    private void GenTerrain()
    {
        blocks = new byte[10, 10];

        // cria um grid maior parte um tipo e linha 0 outro tipo
        for(int px=0; px < blocks.GetLength(0); px++)
        {
            for(int py = 0; py < blocks.GetLength(1); py++)
            {
                if(py == 5)
                {
                    blocks[px, py] = 2;
                }
                else if(py < 5)
                {
                    blocks[px, py] = 1;
                }

                if(px == 5)
                {
                    blocks[px, py] = 0;
                }

            }
        }
    }

    private void GenSquare(float x, float y, Vector2 texture)
    {
        if(Block((int)x, (int)y + 1) == 0 || Block((int)x, (int)y - 1) == 0 || Block((int)x+1, (int)y) == 0 || Block((int)x-1, (int)y) == 0) // se tiver contato com o ar
        {
            newVertices.Add(new Vector3(x, y, 0)); // Canto superior esquerdo
            newVertices.Add(new Vector3(x + 1, y, 0)); // Canto superior direito
            newVertices.Add(new Vector3(x + 1, y - 1, 0)); // Canto inferior direito
            newVertices.Add(new Vector3(x, y - 1, 0)); // Canto inferior esquerdo

            newTriangles.Add((squareCount * 4) + 0);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);

            // Indicando os 4 cantos da textura (coordenadas) que 
            newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));
            newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
            newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y));
            newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y));

            squareCount++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        Mesh newMesh = new Mesh();
        newMesh.vertices = colVertices.ToArray();
        newMesh.triangles = colTriangles.ToArray();
        col.sharedMesh = newMesh;

        squareCount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;
    }
}
