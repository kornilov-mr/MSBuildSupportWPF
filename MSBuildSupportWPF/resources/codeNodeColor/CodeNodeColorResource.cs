using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace MSBuildSupportWPF.resources.codeNodeColor
{
    public class CodeNodeColorResource
    {
        private static readonly string jsonPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\resources\codeNodeColor\CodeNodeColorJson.json"));
        private static Dictionary<string, Color> resourses;
        CodeNodeColorResource()
        {

        }
        public static Color GetColor(string name)
        {
            if (resourses is null)
            {
                loadResourses();
            }
            if (!resourses.ContainsKey(name))
            {
                return Color.Gray;
            
            }
            return resourses[name];
        }
        private static void loadResourses()
        {
            resourses = new Dictionary<string, Color>();
            List<ColorKeyValueToSerialize> o = JsonSerializer.Deserialize<List<ColorKeyValueToSerialize>>(File.ReadAllText(jsonPath));
            foreach (ColorKeyValueToSerialize keyValue in o)
            {
                resourses.Add(keyValue.name, keyValue.ColorRGB.ToColor());
            }
        }
    }
}
