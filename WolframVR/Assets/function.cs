//public void plot(string input){
//    query1 = "graph = Plot3D[" + input + ", {x, -10, 10}, {y, -10, 10}]";
//    query2 = "Export[\"output.obj\", graph]";
//}
//
//public void plot(string[] funcArray){
//    String res = "";
//    for(func in funcArray){
//        res = func + ", ";
//    }
//    res = res.Remove(res.Length - 2);
//    query1 = "graph = Plot3D[{" + res + "}, {x, -50, 50}, {y, -50, 50}]";
//    query2 = "Export[\"output.obj\", graph]";
//}
//
//
//public void contour(string input){
//    query1 = "graph = ContourPlot3D[" + input + ", {x, -10, 10}, {y, -10, 10}, {z, -10, 10}]";
//    query2 = "Export[\"output.obj\", graph]";
//}
//
////public void vector(){
////    query1 = "graph = VectorPlot3D[" + input + ", {x, -10, 10}, {y, -10, 10}, {z, -10, 10}]";
////}
//
//public void chemicals(string input){
//    query1 = "chemical = ChemicalData[\"" + input + "\", \"MoleculePlot\"]";
//    query2 = "Export[\"output.obj\", chemical]";
//}