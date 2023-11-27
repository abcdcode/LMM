using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.IO.Compression;
using static System.Net.WebRequestMethods;
using System.Threading;
using System.Runtime.InteropServices.ComTypes;
using System.Net.NetworkInformation;
using Steamworks;
using Steamworks.Data;
using System.Configuration;
using static LobotomyBaseGUI.Program;

namespace LobotomyBaseGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            __instance = this;
        }
        public void Init()
        {
            try
            {
                SteamClient.Init(Program.appid, true);
                SteamId steamId = SteamClient.SteamId;
                SteamInit = true;
            }
            catch (Exception e)
            {
                SteamInit = false;
            }
            InitializeComponent();
            CheckConfig();
            LocalizeComp();
        }
        public static bool SteamInit = false;
        public string CurVersion = "0.0";
        public static string LobotomyPatchInfo = "15PdeZ-CfVxM_4l5__drlDHkxvA-PoaV2";
        public void LocalizeComp()
        {
            ApplyBtn.Text = LocalizeManager.Instance.GetText("AddMod");
            UpBtn.Text = LocalizeManager.Instance.GetText("Up");
            DownBtn.Text = LocalizeManager.Instance.GetText("Down");
            ApplyAndRunBtn.Text = LocalizeManager.Instance.GetText("StartGame");
            PathFindBtn.Text = LocalizeManager.Instance.GetText("GetGamePath");
            RemoveBtn.Text = LocalizeManager.Instance.GetText("RemoveMod");
            Text = LocalizeManager.Instance.GetText("ProgramName"); 
        }
        public void CheckConfig()
        {

            
            string configpath = AppDataPath + "/Config.xml";
            if (System.IO.File.Exists(configpath))
            {
                DescWriteLine("이전 환경 파일 확인됨");
               
                ProgramConfig config = ProgramConfig.LoadData(configpath);
                LoadProgramWithPath(config.LobotomyPath);
                CurVersion = config.CurVersion;
                PatchProgram.Managed = new DirectoryInfo(new FileInfo(config.LobotomyPath).DirectoryName + "/LobotomyCorp_Data/Managed");
                new Folder();
            } else
            {
                DescWriteLine("이전 환경 파일 미확인");
                if (SteamInit)
                {
                    DescWriteLine("로보토미 실행 파일 검색");
                    try
                    {
                        string path = SteamApps.AppInstallDir(Program.Lobotomyappid);
                        LoadProgramWithPath(path + "/LobotomyCorp.exe");
                    }
                    catch (Exception e)
                    {
                        DescWriteLine("검색 실패. 수동으로 로보토미 실행 파일 지정 필요");

                    }
                }

                
			}
		}
        public void LoadProgramWithPath(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                DescText.Text += "해당 파일은 존재하지 않습니다.";
                return;
            }
            PathText.Text = path;
            FileInfo info = new FileInfo(PathText.Text);
            checkedListBox1.Items.Clear();
            modlist = new List<ModInfo>();
            DescText.Text += "연동모드 폴더 경로 - " + info.Directory.FullName + "\\LobotomyCorp_Data\\BaseMods" + Environment.NewLine;
            if (Directory.Exists(info.Directory.FullName + "\\LobotomyCorp_Data\\BaseMods"))
            {
                DescText.Text += "연동모드 폴더 경로 확인" + Environment.NewLine;
                LoadProgramWithPath_Config(info.Directory.FullName + "\\LobotomyCorp_Data\\BaseMods");
            }
            else
            {
                DescText.Text += "연동모드 폴더 경로 미확인. 임의 생성함" + Environment.NewLine;
                Directory.CreateDirectory(info.Directory.FullName + "/LobotomyCorp_Data/BaseMods");
                LoadProgramWithPath_Config(info.Directory.FullName + "\\LobotomyCorp_Data\\BaseMods");
            }

        }
        public void LoadProgramWithPath_Config(string path)
        {
            modfolder = new DirectoryInfo(path);
            modlist = new List<ModInfo>();
            List<DirectoryInfo> dirlist = modfolder.GetDirectories().ToList();
            List<DirectoryInfo> dirlist_workshop = null;
            if (SteamInit)
            {
                dirlist_workshop = new DirectoryInfo(GetWorkshopDirPath()).GetDirectories().ToList();
            }
            if (System.IO.File.Exists(modfolder.FullName + "/BaseModList.xml") && !System.IO.File.Exists(modfolder.FullName + "/BaseModList_v2.xml"))
            {
                ModListXml xmldata = ModListXml.LoadData(modfolder.FullName + "/BaseModList.xml");
                ModListXml.SerializeData(xmldata, modfolder.FullName + "/BaseModList_v2.xml");
            }
            if (System.IO.File.Exists(modfolder.FullName + "/BaseModList_v2.xml"))
            {
                ModListXml xmldata = ModListXml.LoadData(modfolder.FullName + "/BaseModList_v2.xml");
                foreach (ModInfoXml moinfo in xmldata.list)
                {
                    DirectoryInfo dir = null;
                    if (moinfo.IsWorkShop && SteamInit)
                    {
                        dir = dirlist_workshop.Find(x => x.Name == moinfo.modfoldername);
                        if(dir!=null) dirlist_workshop.Remove(dir);

                    } else
                    {
                        dir = dirlist.Find(x => x.Name == moinfo.modfoldername);
                        if (dir != null) dirlist.Remove(dir);
                    }
                    if (dir != null)
                    {
                        ModInfo modinfo = new ModInfo();
                        modinfo.Init(dir, moinfo.IsWorkShop);
                        modlist.Add(modinfo);
                        checkedListBox1.Items.Add(new ModStr(modinfo.modname, checkedListBox1.Items.Count, moinfo.IsWorkShop), moinfo.Useit);
                    }
                }
                DescText.Text += "이전 우선도 설정 파일 확인" + Environment.NewLine;
            }
            else
            {
                DescText.Text += "이전 우선도 설정 파일 미확인" + Environment.NewLine;
            }
            foreach (DirectoryInfo dir in dirlist)
            {
                ModInfo modinfo = new ModInfo();
                modinfo.Init(dir,false);
                modlist.Add(modinfo);
                checkedListBox1.Items.Add(new ModStr(modinfo.modname, checkedListBox1.Items.Count,false), true);
            }
            if (SteamInit)
            {
                foreach (DirectoryInfo dir in dirlist_workshop)
                {
                    ModInfo modinfo = new ModInfo();
                    modinfo.Init(dir, true);
                    modlist.Add(modinfo);
                    checkedListBox1.Items.Add(new ModStr(modinfo.modname, checkedListBox1.Items.Count, true), true);
                }
            }

            ProgramConfig config = new ProgramConfig();
            DirectoryInfo d = new DirectoryInfo(path).Parent.Parent;
            config.LobotomyPath = d.FullName+ "/LobotomyCorp.exe";
            config.CurVersion = CurVersion;
            ProgramConfig.SerializeData(config, AppDataPath + "/Config.xml");
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RemoveBtn.Text = LocalizeManager.Instance.GetText("RemoveMod");
            RemoveBtn.Enabled = true;
            int index = checkedListBox1.SelectedIndex;
            if(index != -1)
            {
                DescText.Text = modlist[index].modinfo;

                if (modlist[index].IsWorkshop)
                {
                    //RemoveBtn.Text = LocalizeManager.Instance.GetText("RemoveMod_Steam");
                    RemoveBtn.Enabled = false;
                }
            }
            Refresh();
        }
        public void RefreshListIndex()
        {
            int i = 0;
            foreach(object obj in checkedListBox1.Items)
            {
                ModStr str = (ModStr)obj;
                str.index = i;
                i++;
            }
            Refresh();
        }
        private void UpBtnClick(object sender, EventArgs e)
        {
            int index = checkedListBox1.SelectedIndex;
            if (index == -1) return;
            if(index <= 0)
            {
                return;
            }
            ModInfo info = modlist[index];
            modlist[index] = modlist[index-1];
            modlist[index - 1] = info;

            object item = checkedListBox1.SelectedItem;
            checkedListBox1.Items[index] = checkedListBox1.Items[index - 1];
            checkedListBox1.Items[index - 1] = item;
            CheckState checkState = checkedListBox1.GetItemCheckState(index - 1);
            checkedListBox1.SetItemCheckState(index - 1, checkedListBox1.GetItemCheckState(index));
            checkedListBox1.SetItemCheckState(index, checkState);
            checkedListBox1.SelectedIndex = index - 1;

            RefreshListIndex();
        }

        private void DownBtnClick(object sender, EventArgs e)
        {
            int index = checkedListBox1.SelectedIndex;
            if (index == -1) return;
            if (index == checkedListBox1.Items.Count-1)
            {
                return;
            }
            ModInfo info = modlist[index];
            modlist[index] = modlist[index + 1];
            modlist[index + 1] = info;

            object item = checkedListBox1.SelectedItem;
            checkedListBox1.Items[index] = checkedListBox1.Items[index + 1];
            checkedListBox1.Items[index + 1] = item;
            CheckState checkState = checkedListBox1.GetItemCheckState(index + 1);
            checkedListBox1.SetItemCheckState(index + 1, checkedListBox1.GetItemCheckState(index));
            checkedListBox1.SetItemCheckState(index, checkState);
            checkedListBox1.SelectedIndex = index + 1;

            RefreshListIndex();
        }
        public void PathFindBtnClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = LocalizeManager.Instance.GetText("Popup_LobotomyEXE");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DescText.Text = "";
                LoadProgramWithPath(openFileDialog.FileName);
            }
        }
        public string GetWorkshopDirPath()
        {
            string text = SteamApps.AppInstallDir(default(AppId));
            DirectoryInfo dir = new DirectoryInfo(string.Format(text.Remove(text.Length - "common\\Lobotomy Mod Manager".Length) + "workshop\\content\\{0}\\", SteamClient.AppId));
            if (!dir.Exists)
            {
                dir.Create();
            }
            return dir.FullName;
        }
        public void ApplyAndRun(object sender, EventArgs e)
        {
            Apply(sender, e);
            if(System.IO.File.Exists(PathText.Text))
            {
                DescText.Text = "";
                FileInfo info = new FileInfo(PathText.Text);
               bool result = PatchProgram.Patch(info.DirectoryName + "/LobotomyCorp_Data/Managed");
                if (result)
                {
                    if (info.Extension == ".exe" && info.Name.Contains("LobotomyCorp"))
                    {
                        string steamAppUrl = "steam://run/" + Program.Lobotomyappid;
                        Process.Start(steamAppUrl);
                        //Process.Start(info.FullName);
                        Application.Exit();
                    }
                } else
                {
                    DescWriteLine("게임 패치 중 문제가 발생했습니다. 게임을 실행하지 않습니다.");
                }
            }
        }
        public void Apply(object sender, EventArgs e)
        {
            if(modlist != null)
            {
                ModListXml xml = new ModListXml();
                for(int i = 0; i < modlist.Count; i++)
                {
                    ModInfo info = modlist[i];
                    ModInfoXml modinfo = new ModInfoXml();
                    modinfo.modfoldername = info.modpath.Name;
                    modinfo.Useit = checkedListBox1.GetItemCheckState(i) == CheckState.Checked;
                    modinfo.IsWorkShop = info.IsWorkshop;
                    xml.list.Add(modinfo);
                }
                ModListXml.SerializeData(xml, modfolder + "\\BaseModList_v2.xml");
            }
            ProgramConfig config = new ProgramConfig();
            config.LobotomyPath = PathText.Text;
            config.CurVersion = CurVersion;
            ProgramConfig.SerializeData(config, AppDataPath + "/Config.xml");
        }
        public void AddMod(object sender, EventArgs e)
        {
            if(PathText.Text ==String.Empty)
            {
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = LocalizeManager.Instance.GetText("Popup_Zip");
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInfo file = new FileInfo(openFileDialog.FileName);
                    ZipArchive zipfile = ZipFile.OpenRead(openFileDialog.FileName);
                    DirectoryInfo dir = Directory.CreateDirectory(new FileInfo(PathText.Text).DirectoryName + "/LobotomyCorp_Data/BaseMods/" + Path.GetFileNameWithoutExtension(openFileDialog.FileName));
                    zipfile.ExtractToDirectory(dir.FullName);

                    ModInfo info = new ModInfo();
                    info.Init(dir,false);
                    modlist.Add(info);
                    checkedListBox1.Items.Add(new ModStr(info.modname, checkedListBox1.Items.Count,false), true);
                }
            } catch(Exception ex)
            {
                DescText.Text = "모드 로드 에러 - " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }
        public List<ModInfo> modlist;

        public DirectoryInfo modfolder;

        public static Form1 __instance;

        public void DescReset()
        {
            DescText.Text = "";
        }

        public void DescWriteLine(string msg)
        {
            DescText.Text += msg + Environment.NewLine;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = new Icon(Application.StartupPath + "/Icon.ico");
        }

        private void RemoveMod(object sender, EventArgs e)
        {
            int index = checkedListBox1.SelectedIndex;
            if (index != -1)
            {
                checkedListBox1.Items.RemoveAt(index);
                Directory.Delete(modlist[index].modpath.FullName, true);
                modlist.RemoveAt(index);
                DescReset();
                RefreshListIndex();
            }
        }
    }
    public class ModStr
    {
        public ModStr(string modname, int index, bool IsWorkshop)
        {
            this.index = index;
            this.modname = modname;
            this.IsWorkshop = IsWorkshop;
        }
        public int index;
        public string modname;
        public bool IsWorkshop = false;
        public override string ToString()
        {
            string result = string.Empty;
            ModInfo mod = Form1.__instance.modlist[index];
            if(mod.requiremods.Count > 0)
            {
                List<string> requiremod = new List<string>(mod.requiremods);
                for (int i = 0; i < index; i++)
                {
                    requiremod.Remove(Form1.__instance.modlist[i].modid);
                }
                if (requiremod.Count > 0) { 
                    result = modname + " (Need Other Mod !)";
                    goto Skip;
                }

            }
            result = modname;

        Skip:
            object obj = Form1.__instance.checkedListBox1.SelectedItem;
            if (obj != null)
            {
                ModStr str = (ModStr)obj;
                ModInfo selmod = Form1.__instance.modlist[str.index];
                if (selmod.requiremods.Contains(mod.modid)) result += " ◁";
            }

            if (IsWorkshop) result += " // " + LocalizeManager.Instance.GetText("SteamMod");
            return result;
        }
    }
}
