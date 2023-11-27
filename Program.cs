using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;
using Patchwork;

namespace LobotomyBaseGUI
{
    public class Copper
    {
        public Copper()
        {
            Form1.__instance.DescWriteLine("패치 파일 복사 중...");
            FileInfo basemod = new FileInfo(Environment.CurrentDirectory + "/Assembly-CSharp.dll");
            File.Copy(basemod.FullName, PatchProgram.Managed.FullName + "/Assembly-CSharp.dll", true);
            FileInfo harmony = new FileInfo(Environment.CurrentDirectory + "/0Harmony.dll");
            File.Copy(harmony.FullName, PatchProgram.Managed.FullName + "/0Harmony.dll", true);
            FileInfo lobo = new FileInfo(Environment.CurrentDirectory + "/LobotomyBaseModLib.dll");
            File.Copy(lobo.FullName, PatchProgram.Managed.FullName + "/LobotomyBaseModLib.dll", true);
            FileInfo naudio = new FileInfo(Environment.CurrentDirectory + "/NAudio.dll");
            File.Copy(naudio.FullName, PatchProgram.Managed.FullName + "/NAudio.dll", true);

            FileInfo Facepunch = new FileInfo(Environment.CurrentDirectory + "/Facepunch.Steamworks.Win64.dll");
            File.Copy(Facepunch.FullName, PatchProgram.Managed.FullName + "/Facepunch.Steamworks.Win64.dll", true);
            FileInfo steam_api64 = new FileInfo(Environment.CurrentDirectory + "/steam_api64.dll");
            File.Copy(steam_api64.FullName, PatchProgram.Managed.FullName + "/steam_api64.dll", true);
            if (!Directory.Exists(PatchProgram.Managed.FullName + "/BaseMod"))
            {
                CopyDirectory(Environment.CurrentDirectory+"/BaseMod", PatchProgram.Managed.FullName + "/BaseMod");
            }


            Form1.__instance.DescWriteLine("패치 파일 복사 완료");
        }
        public static void CopyDirectory(string sourceDir, string destDir)
        {
            // Create the destination directory if it doesn't exist
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destDir, fileName);
                File.Copy(filePath, destFilePath,false);
            }
            foreach (string subDirPath in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDirPath);
                string destSubDirPath = Path.Combine(destDir, subDirName);
                CopyDirectory(subDirPath, destSubDirPath);
            }
        }
    }
    public class Folder
    {
        public Folder()
        {
            Form1.__instance.DescWriteLine("BaseMods 폴더 확인 중...");
            bool flag = !Directory.Exists(PatchProgram.Managed.Parent.FullName + "/BaseMods");
            if (flag)
            {
                Form1.__instance.DescWriteLine("모드 폴더가 확인되지 않았습니다. 해당 폴더를 생성합니다.");
                Directory.CreateDirectory(PatchProgram.Managed.Parent.FullName + "/BaseMods");
            }
            else
            {
                Form1.__instance.DescWriteLine("모드 폴더 확인됨");
            }
        }
    }
    public class Patcher
    {
        public Patcher()
        {
            Form1.__instance.DescWriteLine("스크립트 파일 패치 중...");
            string path = PatchProgram.Managed.FullName + "/Assembly-CSharp.dll";
            AssemblyPatcher patcher = new AssemblyPatcher(path, null);
            patcher.PatchAssembly(Environment.CurrentDirectory + "/Lobotomypatch.dll", null, true);
            patcher.WriteTo(PatchProgram.Managed.FullName + "/Assembly-CSharp.dll");
            Form1.__instance.DescWriteLine("패치 성공!");
        }

        // Token: 0x06000007 RID: 7 RVA: 0x0000234C File Offset: 0x0000054C
        public static void MakeOpenAssembly(AssemblyDefinition assembly, bool modifyEvents)
        {
            IEnumerable<TypeDefinition> enumerable = assembly.MainModule.GetAllTypes();
            enumerable = enumerable.ToList<TypeDefinition>();
            foreach (TypeDefinition typeDefinition in enumerable)
            {
                foreach (FieldDefinition fieldDefinition in typeDefinition.Fields)
                {
                    fieldDefinition.IsPublic = true;
                    fieldDefinition.IsInitOnly = false;
                }
                foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                {
                    methodDefinition.IsPublic = true;
                }
                if (modifyEvents)
                {
                    using (var enumerator4 = typeDefinition.Events.GetEnumerator())
                    {
                        while (enumerator4.MoveNext())
                        {
                            EventDefinition vent = enumerator4.Current;
                            bool flag = typeDefinition.Fields.Any((FieldDefinition x) => x.Name == vent.Name) || typeDefinition.Properties.Any((PropertyDefinition x) => x.Name == vent.Name);
                            bool flag2 = flag;
                            if (flag2)
                            {
                                EventDefinition vent2 = vent;
                                EventDefinition eventDefinition = vent2;
                                eventDefinition.Name += "Event";
                            }
                        }
                    }
                }
                typeDefinition.IsSealed = false;
                bool isNested = typeDefinition.IsNested;
                bool flag3 = isNested;
                if (flag3)
                {
                    typeDefinition.IsNestedPublic = true;
                }
                else
                {
                    typeDefinition.IsPublic = true;
                }
            }
        }
    }
    public class PatchProgram
    {
        public static bool Patch(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
                DirectoryInfo startdir = dir.Parent;
                Managed = null;
                FindPath(new DirectoryInfo(path));
                bool flag = Managed == null || !Managed.Exists;
                if (flag)
                {
                    Form1.__instance.DescWriteLine("패치 경로를 찾지 못했습니다.");
                }
                new Copper();
                new Folder();
                patcher = new Patcher();
            }
            catch (Exception e)
            {
                Form1.__instance.DescWriteLine("에러 : " + e.Message);
                Form1.__instance.DescWriteLine(e.StackTrace);
                return false;
            }
            return true;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002120 File Offset: 0x00000320
        public static void FindPath(DirectoryInfo curdir)
        {
            bool flag = File.Exists(curdir.FullName + "/Assembly-CSharp.dll") && curdir.Name == "Managed" && PatchProgram.Managed == null;
            if (flag)
            {
                Managed = curdir;
                Console.WriteLine("패치 파일 위치 : " + Managed.FullName);
            }
            else
            {
                foreach (DirectoryInfo dir in curdir.GetDirectories())
                {
                    FindPath(dir);
                }
            }
        }

        // Token: 0x04000001 RID: 1
        public static DirectoryInfo Managed;

        // Token: 0x04000002 RID: 2
        public static Patcher patcher;
    }
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*if (args.Length == 0)
            {
                Process.Start(Environment.CurrentDirectory + "/LobotomyModManagerUpdater.exe");
                return;
            }
            if (args[0] != "-Updated")
            {
                Process.Start(Environment.CurrentDirectory + "/LobotomyModManagerUpdater.exe");
                return;
            }*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            theForm1 = new Form1();
            theForm2 = new Form2();
            Application.Run(theForm2);
        }
        public static void CreateLocalizeFile()
        {
            LocalizeDataList list = new LocalizeDataList();

            LocalizeData data = new LocalizeData();
            data.id = "GetGamePath";
            data.value = "찾아보기";
            list.list.Add(data);

            data = new LocalizeData();
            data.id = "AddMod";
            data.value = "모드 추가";
            list.list.Add(data);

            LocalizeDataList.SerializeData(list, Application.StartupPath + "/Localize/kr/Localize.xml");
        }
        public static string AppDataPath
        {
            get
            {
                DirectoryInfo folderpath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lobotomy Mod Manager");
                if (!folderpath.Exists)
                {
                    folderpath.Create();
                }
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lobotomy Mod Manager";
            }
        }
        public static uint appid = 2531580;
        public static uint Lobotomyappid = 568220u;
        public static Form1 theForm1;
       public static Form2 theForm2;
        public static string CurLang = "kr";
    }
    public class ModListXml
    {
        public static void SerializeData(ModListXml data, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                new XmlSerializer(typeof(ModListXml)).Serialize(streamWriter, data);
            }
        }
        public static ModListXml LoadData(string path)
        {
            ModListXml result;
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                result = (ModListXml)new XmlSerializer(typeof(ModListXml)).Deserialize(stringReader);
            }
            return result;
        }
        public List<ModInfoXml> list = new List<ModInfoXml>();
    }
    public class ModInfoXml
    {
        public string modfoldername = string.Empty;
        public bool Useit = true;
        public bool IsWorkShop = false;
    }
    public class ModInfo
    {

        public void Init(DirectoryInfo dir, bool IsWorkshop)
        {
            this.IsWorkshop = IsWorkshop;
            this.foldername = dir.Name;
            this.modpath = dir;
            string str = string.Empty;
            string str2 = string.Empty;
            string text = string.Empty;
            this.modid = string.Empty;
            string lang = Program.CurLang;
            requiremods = new List<string>();
            bool flag = File.Exists(dir.FullName + "/Info/"+ lang + "/info.xml");
            if(!flag)
            {
                lang = "kr";
                flag = File.Exists(dir.FullName + "/Info/" + lang + "/info.xml");
            }
            if (!flag)
            {
                lang = "en";
                flag = File.Exists(dir.FullName + "/Info/" + lang + "/info.xml");
            }
            if (flag)
            {
                string xml = File.ReadAllText(dir.FullName + "/Info/"+lang+"/info.xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("/info/name");
                XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("/info/descs").SelectNodes("desc");
                this.modname = xmlNode.InnerText;
                str = string.Concat(new string[]
                {
                "Folder : ",
                this.foldername,
                Environment.NewLine,
                "Name : ",
                this.modname,
                Environment.NewLine
                });
                xmlNode = xmlDocument.SelectSingleNode("/info/ID");
                bool flag2 = xmlNode != null;
                if (flag2)
                {
                    str2 = "ID : " + xmlNode.InnerText + Environment.NewLine;
                    this.modid = xmlNode.InnerText;
                }
                if (xmlNodeList != null)
                {
                    foreach (object obj in xmlNodeList)
                    {
                        XmlNode xmlNode2 = (XmlNode)obj;
                        text = text + Environment.NewLine + xmlNode2.InnerText;
                    }
                }
                bool flag3 = File.Exists(dir.FullName + "/Info/GlobalInfo.xml");
                if (flag3)
                {
                    xml = File.ReadAllText(dir.FullName + "/Info/GlobalInfo.xml");
                    xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xml);
                    xmlNode = xmlDocument.SelectSingleNode("/info/ID");
                    bool flag4 = xmlNode != null;
                    if (flag4)
                    {
                        str2 = "ID : " + xmlNode.InnerText + Environment.NewLine;
                        this.modid = xmlNode.InnerText;
                    }
                    XmlNodeList requires = xmlDocument.SelectSingleNode("/info").SelectNodes("Require");
                    if (requires != null)
                    {
                        foreach (object obj in requires)
                        {
                            XmlNode xmlNode2 = (XmlNode)obj;
                            requiremods.Add(xmlNode2.InnerText);
                        }
                    }
                }
                this.modinfo = str + str2 + text;
            }
            else
            {
                str = string.Concat(new string[]
               {
                "Folder : ",
                this.foldername,
                Environment.NewLine,
                "No Name",
                Environment.NewLine,
                "No ID"
               });
                this.modinfo = str;
                this.modname = this.foldername;
            }
        }
        public List<string> requiremods;

        public string foldername;

        public string modname;

        public string modinfo;

        public string modid;

        public DirectoryInfo modpath;

        public bool IsWorkshop;
    }
    public class PatchInfo
    {
        public static void SerializeData(PatchInfo data, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                new XmlSerializer(typeof(PatchInfo)).Serialize(streamWriter, data);
            }
        }
        public static PatchInfo LoadData(string path)
        {
            PatchInfo result;
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                result = (PatchInfo)new XmlSerializer(typeof(PatchInfo)).Deserialize(stringReader);
            }
            return result;
        }
        public string CurVersion = string.Empty;
        public List<PatchFileInfo> list = new List<PatchFileInfo>();
    }
    public class PatchFileInfo
    {
        public string filename = string.Empty;
        public string path = string.Empty;
    }
    public class ProgramConfig
    {
        public static void SerializeData(ProgramConfig data, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                new XmlSerializer(typeof(ProgramConfig)).Serialize(streamWriter, data);
            }
        }
        public static ProgramConfig LoadData(string path)
        {
            ProgramConfig result;
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                result = (ProgramConfig)new XmlSerializer(typeof(ProgramConfig)).Deserialize(stringReader);
            }
            return result;
        }
        public string LobotomyPath = string.Empty;
        public string CurVersion = string.Empty;
    }
    public class LocalizeManager : Singleton<LocalizeManager>
    {
        public void Init(LocalizeDataList datas)
        {
            dic = new Dictionary<string, string>();
            foreach (LocalizeData data in datas.list)
            {
                dic[data.id] = data.value;
            }
        }
        public string GetText(string id)
        {
            if(dic.ContainsKey(id))
            {
                return dic[id];
            }
            return String.Empty;
        }
        public Dictionary<string, string> dic;
    }
    public class LocalizeDataList
    {
        public static void SerializeData(LocalizeDataList data, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                new XmlSerializer(typeof(LocalizeDataList)).Serialize(streamWriter, data);
            }
        }
        public static LocalizeDataList LoadData(string path)
        {
            LocalizeDataList result;
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                result = (LocalizeDataList)new XmlSerializer(typeof(LocalizeDataList)).Deserialize(stringReader);
            }
            return result;
        }
        public List<LocalizeData> list = new List<LocalizeData>();
    }
    public class LocalizeData
    {
        public string id = String.Empty;
        public string value = String.Empty;
    }

    public class Singleton<T> where T : class, new()
    {
        public static T Instance
        {
            get
            {
                if (Singleton<T>._instance == null)
                {
                    Singleton<T>._instance = Activator.CreateInstance<T>();
                }
                return Singleton<T>._instance;
            }
        }

        protected Singleton()
        {
        }

        private static T _instance;
    }
}
