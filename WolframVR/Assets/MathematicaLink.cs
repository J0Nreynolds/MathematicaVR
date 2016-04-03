using UnityEngine;
using Wolfram.NETLink;

public class MathematicaLink : MonoBehaviour
{
    IKernelLink ml = null;
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

        // Now compute 2+2 in several different ways.

        //// The easiest way. Send the computation as a string and get the result in a single call:
        //string result = ml.EvaluateToOutputForm("2+2", 0);
        //Debug.Log("2 + 2 = " + result);



        //instantiateObject(queries[0], ml);

        plot3DFunction(functionExample);
    }

    void plot3DFunction(string function)
    {
        string query = "gr1 = Plot3D["+ function+", {x, -5, 5}, {y, -5, 5}, ColorFunction->\"RustTones\"]; Export[\"3DPlot.obj\", gr1]";
        // Trying to get 3D objects from Mathematica to Unity
        string fileName = ml.EvaluateToOutputForm(query, 0);
        Debug.Log("Filename is " + fileName);

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
        loadOBJFile(fileName);
    }

    void generateGeometricShape(string shapeName)
    {
            string query = "shape = Graphics3D[" + shapeName + "[]]; Export[\"Shape.obj\", shape]";
            // Trying to get 3D objects from Mathematica to Unity
            string fileName = ml.EvaluateToOutputForm(query, 0);
            Debug.Log("Filename is " + fileName);

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
            loadOBJFile(fileName);
        }

    void loadOBJFile(string fileName)
    {
        GameObject unityObj = OBJLoader.LoadOBJFile(fileName);
        unityObj.AddComponent<MeshRenderer>();
        Transform child = unityObj.transform.GetChild(0);
        child.renderer.sharedMaterials = OBJLoader.LoadMTLFile(fileName + ".mtl");
        child.rotation = Quaternion.Euler(270, 180, 180);
        child.position = new Vector3(0, 0, 0);
    }

    void instantiateObject(string query, IKernelLink ml)
    {
        // Trying to get 3D objects from Mathematica to Unity
        string fileName = ml.EvaluateToOutputForm(query, 0);
        Debug.Log("Filename is " + fileName);
        // Always Close link when done:

        ml.Close();
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
        GameObject unityObj = OBJLoader.LoadOBJFile(fileName);
        unityObj.AddComponent<MeshRenderer>();
        Transform child = unityObj.transform.GetChild(0);
        child.renderer.sharedMaterials = OBJLoader.LoadMTLFile(fileName + ".mtl");
        child.rotation = Quaternion.Euler(270, 180, 180);
        child.position = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void onApplicationQuit()
    {
        // Always Close link when done:
        ml.Close();
    }
   
}
