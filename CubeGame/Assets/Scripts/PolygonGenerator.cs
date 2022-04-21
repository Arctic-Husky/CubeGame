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

    // Mesh = vertices, triangulos e UVs. Salvar tudo neste mesh
    private Mesh mesh;

    private float tUnit = 0.25f;
    public Vector2 tStone = new Vector2(0, 0); // Canto inferior esquerdo, coordenadas da textura da "Pedra"
    private Vector2 tGrass = new Vector2(0, 1);

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        newVertices.Add(new Vector3(x, y, z)); // Canto superior esquerdo
        newVertices.Add(new Vector3(x + 1, y, z)); // Canto superior direito
        newVertices.Add(new Vector3(x + 1, y - 1, z)); // Canto inferior direito
        newVertices.Add(new Vector3(x, y - 1, z)); // Canto inferior esquerdo

        newTriangles.Add(0);
        newTriangles.Add(1);
        newTriangles.Add(3);
        newTriangles.Add(1);
        newTriangles.Add(2);
        newTriangles.Add(3);

        // Indicando os 4 cantos da textura (coordenadas) que 
        newUV.Add(new Vector2(tUnit * tStone.x, tUnit * tStone.y + tUnit));
        newUV.Add(new Vector2(tUnit * tStone.x + tUnit, tUnit * tStone.y + tUnit));
        newUV.Add(new Vector2(tUnit * tStone.x + tUnit, tUnit * tStone.y));
        newUV.Add(new Vector2(tUnit * tStone.x, tUnit * tStone.y));

        
    }

    private void Update()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
