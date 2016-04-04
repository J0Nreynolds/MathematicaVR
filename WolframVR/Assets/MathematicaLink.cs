using UnityEngine;
using Wolfram.NETLink;

public class MathematicaLink : MonoBehaviour
{
    IKernelLink ml = null;
	GameObject center = null;
    string[] queries = { "gr1 = Plot3D[Sin[x + y], {x, -5, 5}, {y, -5, 5}, ColorFunction->\"RustTones\"]; Export[\"3DPlot.obj\", gr1]", "molecule = ChemicalData[\"Dopamine\", \"MoleculePlot\"]; Export[\"Molecule.obj\", molecule]" };
    string functionExample = "Sin[x + y]";
    string chemistryExample = "CH4";

    // Use this for initialization
    void Start()
    {

        // This launches the Mathematica kernel:
        ml = MathLinkFactory.CreateKernelLink();

        // Discard the initial InputNamePacket the kernel will send when launched.
        ml.WaitAndDiscardAnswer();

        // Now compute 2+2 in several different ways.C:\Users\Jonathan Reynolds\Documents\GitHub\WolframVR\WolframVR\Assets\MathematicaLink.cs

        //// The easiest way. Send the computation as a string and get the result in a single call:
        //string result = ml.EvaluateToOutputForm("2+2", 0);
        //Debug.Log("2 + 2 = " + result);


        //instantiateObject(queries[0], ml);

        plot3DFunction(functionExample);
        //generateGeometricShape("RhombicHexecontahedron");
        //generateMolecularStructure("FormicAcid");
    }

    public void plot3DFunction(string function)
    {

        // This launches the Mathematica kernel:
        ml = MathLinkFactory.CreateKernelLink();

        // Discard the initial InputNamePacket the kernel will send when launched.
        ml.WaitAndDiscardAnswer();
        string query = "gr1 = Plot3D["+ function+", {x, -5, 5}, {y, -5, 5}, ColorFunction->\"RustTones\"]; Export[\"3DPlot.obj\", gr1]";
        // Trying to get 3D objects from Mathematica to Unity
        string fileName = ml.EvaluateToOutputForm(query, 0);
        Debug.Log("Filename is " + fileName);
        ml.Close();

        //Correct the format
        string[] lines = System.IO.File.ReadAllLines(fileName);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > 0)
            {
                if (lines[i][0] == 'f')
                {
                    lines[i] = lines[i].Replace("//", "");
                }
            }
        }
        System.IO.File.WriteAllLines(fileName, lines);
        loadOBJFileNoMTL(fileName);
    }

    public void generateGeometricShape(string shapeName)
    {

        // This launches the Mathematica kernel:
        ml = MathLinkFactory.CreateKernelLink();

        // Discard the initial InputNamePacket the kernel will send when launched.
        ml.WaitAndDiscardAnswer();
        string query = "";
        switch (shapeName)
        {
            case "Dodecahedron": query ="shape = PolyhedronData[\"Dodecahedron\"]; Export[\"Shape.obj\", shape]"; break;
            case "RhombicHexecontahedron":
                query = "shape = PolyhedronData[\"RhombicHexecontahedron\"]; Export[\"Shape.obj\", shape]"; break;
            default:
                query = "shape = Graphics3D[" + shapeName + "[]]; Export[\"Shape.obj\", shape]"; break;

        }
        // Trying to get 3D objects from Mathematica to Unity
        string fileName = ml.EvaluateToOutputForm(query, 0);
        Debug.Log("Filename is " + fileName);
        ml.Close();

        //Correct the format
        string[] lines = System.IO.File.ReadAllLines(fileName);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > 0)
            {
                if (lines[i][0] == 'f')
                {
                    lines[i] = lines[i].Replace("//", "");
                }
            }
        }
        System.IO.File.WriteAllLines(fileName, lines);
        loadOBJFileNoMTL(fileName);
    }

    public void generateMolecularStructure(string molecule)
    {
        // This launches the Mathematica kernel:
        ml = MathLinkFactory.CreateKernelLink();

        // Discard the initial InputNamePacket the kernel will send when launched.
        ml.WaitAndDiscardAnswer();
        string query = "molecule = ChemicalData[\""+molecule+"\", \"MoleculePlot\"]; Export[\"Molecule.obj\", molecule]";
        // Trying to get 3D objects from Mathematica to Unity
        string fileName = ml.EvaluateToOutputForm(query, 0);
        Debug.Log("Filename is " + fileName);
        ml.Close();
        loadOBJFile(fileName);

    }

    private void loadOBJFile(string fileName)
    {

        if (center != null)
        {
            Destroy(center);
        }
        center = OBJLoader.LoadOBJFile(fileName);
        center.AddComponent<MeshRenderer>();
        Transform child = center.transform.GetChild(0);
        child.renderer.sharedMaterials = OBJLoader.LoadMTLFile(fileName + ".mtl");
        child.rotation = Quaternion.Euler(270, 180, 180);
        child.position = new Vector3(0, 0, 0);
    }

    private void loadOBJFileNoMTL(string fileName)
    {

        if (center != null)
        {
            Destroy(center);
        }
        center = OBJLoader.LoadOBJFile(fileName);
        center.AddComponent<MeshRenderer>();
        Transform child = center.transform.GetChild(0);
        child.renderer.sharedMaterial = this.renderer.material;
        child.rotation = Quaternion.Euler(270, 180, 180);
        child.position = new Vector3(0, 0, 0);

    }

    void instantiateObject(string query, IKernelLink ml)
    {
        // Trying to get 3D objects from Mathematica to Unity
        string fileName = ml.EvaluateToOutputForm(query, 0);
        Debug.Log("Filename is " + fileName);
        // Always Close link when done:
        
        //Correct the .OBJ format
        string[] lines = System.IO.File.ReadAllLines(fileName);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > 0)
            {
                if (lines[i][0] == 'f')
                {
                    lines[i] = lines[i].Replace("//", "");
                }
            }
        }
        System.IO.File.WriteAllLines(fileName, lines);
     	center = OBJLoader.LoadOBJFile(fileName);
       	center.AddComponent<MeshRenderer>();
        Transform child = center.transform.GetChild(0);
        child.renderer.sharedMaterials = OBJLoader.LoadMTLFile(fileName + ".mtl");
        child.rotation = Quaternion.Euler(270, 180, 180);
        child.position = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            plot3DFunction(functionExample);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            generateGeometricShape("RhombicHexecontahedron");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            plot3DFunction("Sin[2*x]*Cos[4*y]");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            generateGeometricShape("Dodecahedron");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            plot3DFunction("z=x^2-y^2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            plot3DFunction("8*Sin[2*y]*Sin[16*y]");
        }


        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            plot3DFunction("Sin[10*(x^2+y^2)]/10");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            generateGeometricShape("Torus");
        }

    }

    void Destroy()
    {
        // Always Close link when done:
        ml.Close();
    }
   
}
