using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChanged
{
    public readonly Mesh NewMesh;

    public MeshChanged(Mesh mesh)
    {
        NewMesh = mesh;
    }
}
