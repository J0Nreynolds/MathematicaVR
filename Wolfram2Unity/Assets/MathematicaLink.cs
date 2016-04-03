using UnityEngine;
using Wolfram.NETLink;

public class MathematicaLink : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        // This launches the Mathematica kernel:
        IKernelLink ml = MathLinkFactory.CreateKernelLink();

        // Discard the initial InputNamePacket the kernel will send when launched.
        ml.WaitAndDiscardAnswer();

        // Now compute 2+2 in several different ways.

        // The easiest way. Send the computation as a string and get the result in a single call:
        string result = ml.EvaluateToOutputForm("2+2", 0);
        Debug.Log("2 + 2 = " + result);

        // Trying to get 3D objects from Mathematica to Unity
        string fileName = ml.EvaluateToOutputForm("gr1 = Plot3D[Sin[x + y], {x, -5, 5}, {y, -5, 5}, ColorFunction-> (ColorData[\"DarkRainbow\"][#3] &)]; Export[\"3DPlot.obj\", gr1]", 0);
        Debug.Log("Filename is " + fileName);
        // Always Close link when done:

        ml.Close();
        //Correct the format
        string[] lines = System.IO.File.ReadAllLines(fileName);
        for (int i = 0; i < lines.Length; i++) 
        {
            if(lines[i].Length > 0) { 
                if (lines[i][0] == 'f')
                {
                    lines[i] = lines[i].Replace("//", "");
                }
            }
        }
        System.IO.File.WriteAllLines(fileName, lines);

        GameObject unityObj = OBJLoader.LoadOBJFile(fileName);

    }

    // Update is called once per frame
    void Update()
    {

    }
   
}
