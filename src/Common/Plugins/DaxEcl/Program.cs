using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using GoldBoxExplorer.Lib.Plugins;

namespace GoldBoxExplorer.Lib.Plugins.DaxEcl.EclDump
{
    partial class EclDump
    {
        public FileHelper.GameList game;
        public Dictionary<int, string> annotations = new Dictionary<int, string>();
        public Dictionary<int, string> decodedEcl = new Dictionary<int, string>();
        public int eventCount = 0;
        EclOpp[] _largestOnGotoOpps = null;
        public String _blockName;
        public int _blockId;
        private String _filename;
        public EclDump(byte[] data, int blockId, string filename)
        {
            _filename = filename;
            initGameSpecificParameters();
            _blockId = blockId;
            _blockName = blockId.ToString();
            SetupCommandTable();
            DumpEcl(data, _blockName);
            annnotateEventSubroutines();
        }

        private void annnotateEventSubroutines()
        {
            if (eventCount == 0) return;
            for (int i = 0; i < eventCount; i++)
            {

                AddAnnotation(_largestOnGotoOpps[i].Word, string.Format("event {0} begins ", i));
            }
        }
        private void initGameSpecificParameters()
        {
            detectGame();
            MemStart = 0x8000; // default ecl start location for most games
            if (game == FileHelper.GameList.PoolOfRadiance)
            {
                MemStart = 0x9900;
            }
            MemBase = 0x10000 - MemStart; 

        }

        private void detectGame()
        {
            game = FileHelper.DetermineGameFrom(_filename);
            if (game == FileHelper.GameList.Unknown)
            {
                System.Windows.Forms.MessageBox.Show("Error, unknown game - Gold Box Explorer determines the game by the presence of a .CFG file that should be present in the game's directory. If this file does not exist, try running the game (eg: start.exe) to generate it.");
                throw new FileNotFoundException("Error, unknown game - Gold Box Explorer determines the game by the presence of a .CFG file that should be present in the game's directory. If this file does not exist, try running the game (eg: start.exe) to generate it.");
            }
        }
        static void Main(string[] args)
        {
         /*  SetupCommandTable();
            
            string path = System.IO.Directory.GetCurrentDirectory();
            //string path = @"C:\games\DARKNESS";
            //string path = @"C:\games\TREASURE";
            //string path = @"C:\games\coab";
            //string path = @"C:\games\secret";
            //string path = @"C:\games\deathkrynn";
            //string path = @"C:\games\nwn";
            //string path = @"c:\games\buckmatrix";
			//string path = @"c:\games\buckcount";
			path = @"c:\games\pool";

			//TryDump(@"C:\Games\Pool\ecl1.dax");
            //FindBugTest();
            foreach (var filea in Directory.GetFiles(path, "ecl*.dax"))
            {
                TryDump(filea);
            }
            */
            //Console.ReadKey();
        }

        void FindBugTest()
        {
            var br = new BinaryReader(File.Open(@"c:\games\coab\ecl2_001.bin", FileMode.Open));
            byte[] data = br.ReadBytes((int)br.BaseStream.Length);
            DumpEcl(data, @"c:\games\coab\ecl2_001");
        }

        void TryDump(string file)
        {
            Console.WriteLine(file);
            foreach (var block in GetAllBlocks(file))
            {
                byte[] data = block.data;

				var ss = String.Format("File: {0} Block: {1} Size: {2}", block.file, block.id, data.Length);
                //Debug.WriteLine(ss);

                string block_name = string.Format("{0}_{1:000}", Path.Combine(Path.GetDirectoryName(block.file), Path.GetFileNameWithoutExtension(block.file)), block.id);

                DumpBin(data, block_name);
                DumpEcl(data, block_name);

            }
        }

        bool skipNext;
        bool stopVM;
        internal int ecl_offset;
        EclData ecl_ptr;
/*        
        string[] stringTable = new string[40] { 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
         };
        */
        string[] stringTable = new string[160] { 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
         };

        private void DumpEcl(byte[] data, string block_name)
        {
            addrDone = new Dictionary<int,string>();
            addrTodo = new Queue<int>();
            byteMap = new Dictionary<int, int>();

		//	Debug.WriteLine(block_name);

            ecl_offset = MemStart;
            ecl_ptr = new EclData(data);

            EclOpp[] opps;

            opps = LoadEclOpps(1);
            int vm_run_addr_1 = opps[0].Word;

            opps = LoadEclOpps(1);
            int SearchLocationAddr = opps[0].Word;
            opps = LoadEclOpps(1);
            int PreCampCheckAddr = opps[0].Word;
            opps = LoadEclOpps(1);
            int CampInterruptedAddr = opps[0].Word;
            opps = LoadEclOpps(1);
            int ecl_initial_entryPoint = opps[0].Word;


           // using (var sw = new StreamWriter(block_name + ".txt", false))
           // {
             //   sw.WriteLine("vm_run_1          0x{0:X4}", vm_run_addr_1);
             //   sw.WriteLine("SearchLocation    0x{0:X4}", SearchLocationAddr);
             //   sw.WriteLine("PreCampCheck      0x{0:X4}", PreCampCheckAddr);
             //   sw.WriteLine("CampInterrupted   0x{0:X4}", CampInterruptedAddr);
             //   sw.WriteLine("ecl_initial_entry 0x{0:X4}", ecl_initial_entryPoint);
            
                AddAddr(ecl_initial_entryPoint, "StartUp");
                AddAddr(CampInterruptedAddr, "StartUp");
                AddAddr(PreCampCheckAddr, "StartUp");
                AddAddr(SearchLocationAddr, "StartUp");
                AddAddr(vm_run_addr_1, "StartUp");
                DecodeBlock(decodedEcl);
         //   }
        }

        Dictionary<int, int> byteMap;
        Dictionary<int,string> addrDone;
        Queue<int> addrTodo;

       
        private void DecodeBlock(Dictionary<int, String> sw)
        {
            while (addrTodo.Count > 0)
            {
                int addr = addrTodo.Dequeue();
                if (ecl_ptr.IsValidAddr(addr + MemBase) &&
                    addrDone.ContainsKey(addr) == false)
                {
                   // Debug.WriteLine(String.Format("Pop: {0:x4}", addr));

                    if (byteMap.ContainsKey(addr) && byteMap[addr] != addr)
                    {
                        //crazy town!
                        //Debug.WriteLine(String.Format("addr {0:x4} is not aligned with precous instructions {1:x4}", addr, byteMap[addr]));
                    }
                    else
                    {
                        DecodeAddr(addr);
                    }
                }
            }

            int lastAddr = int.MaxValue;
            int lastCodeAddr = int.MaxValue;

            foreach (var a in byteMap.OrderBy(ob => ob.Key))
            {
                if (a.Key != lastAddr + 1) sw.Add(a.Key, "");

                if (a.Value != lastCodeAddr)
                {
               //     sw.Add(addrDone[a.Value]);
                    sw[a.Value] = addrDone[a.Value];
                    lastCodeAddr = a.Value;
                }
                lastAddr = a.Key;    
            }
        }
        private void AddAnnotation(int addr, string txt)
        {
            if (annotations.ContainsKey(addr))
                annotations[addr] += txt;
            else
                annotations[addr] = txt;
        }
        private void AddAddr(int addr, string txt)
        {
            if (addrDone.ContainsKey(addr) == false && addrTodo.Contains(addr) == false  )
            {
               // Debug.WriteLine(String.Format("Add: {0:x4} ecl_offset: {1:x4} {2}", addr, ecl_offset, txt));
                addrTodo.Enqueue(addr);
            }
        }

        private void AddLine(int addr, string txt, int len)
        {
            if (addrDone.ContainsKey(addr) == false)
            {
                addrDone.Add(addr, txt);
                for (int i = 0; i < len; i++)
                {
                    if (byteMap.ContainsKey(addr + i))
                    {
                       // Debug.WriteLine(String.Format("map byte {0:x4} points to {1:x4} not {2:x4}", addr + i, byteMap[addr + i], addr));
                    }
                    else
                    {
                        byteMap.Add(addr + i, addr);
                    }
                }
            }
        } 
        
        private void DecodeAddr(int entryPoint)
        {
            ecl_offset = entryPoint;
            stopVM = false;

            while (stopVM == false)
            {
                int command = ecl_ptr[ecl_offset + MemBase];

                int addr = ecl_offset;
                string txt = string.Format("0x{0:X4} 0x{1:X2} ", addr, command);

                CmdItem cmd;
                if (CommandTable.TryGetValue(command, out cmd))
                {
                    bool lastSkip = skipNext;

                    txt += string.Format("{0} {1}", cmd.Name(), cmd.Dump());

                    if (lastSkip)
                    {
                        skipNext = false;
                        lastSkip = false;
                    }
                }
                else
                {
                    txt += "Unknown command";
                    break;
                }

                if (stopVM) txt += "\n\r";

                AddLine(addr, txt, (ecl_offset - addr) & 0xFFFF);
            }
        }





        Dictionary<int, CmdItem> CommandTable = new Dictionary<int, CmdItem>();

        public void SetupCommandTable()
        {
            CommandTable.Add(0x00, new CmdItem(0, "EXIT", CMD_Exit, this)); //
            CommandTable.Add(0x01, new CmdItem(1, "GOTO", CMD_Goto, this)); //
            CommandTable.Add(0x02, new CmdItem(1, "GOSUB", CMD_Gosub, this)); //
            CommandTable.Add(0x03, new CmdItem(2, "COMPARE", CMD_Compare, this)); //
            CommandTable.Add(0x04, new CmdItem(3, "ADD", CMD_Add, this)); //
            CommandTable.Add(0x05, new CmdItem(3, "SUBTRACT", CMD_Sub, this)); //
            CommandTable.Add(0x06, new CmdItem(3, "DIVIDE", CMD_Div, this)); //
            CommandTable.Add(0x07, new CmdItem(3, "MULTIPLY", CMD_Multi, this)); //
            CommandTable.Add(0x08, new CmdItem(2, "RANDOM", CMD_Random, this)); //
            CommandTable.Add(0x09, new CmdItem(2, "SAVE", CMD_Save, this));
            CommandTable.Add(0x0A, new CmdItem(1, "LOAD CHARACTER", CMD_LoadCharacter, this));
            CommandTable.Add(0x0B, new CmdItem(3, "LOAD MONSTER", CMD_LoadMonster, this));
            CommandTable.Add(0x0C, new CmdItem(3, "SETUP MONSTER", CMD_SetupMonster, this)); //
            CommandTable.Add(0x0D, new CmdItem(0, "APPROACH", CMD_Approach, this)); // 
            CommandTable.Add(0x0E, new CmdItem(1, "PICTURE", CMD_Picture, this)); //
            CommandTable.Add(0x0F, new CmdItem(2, "INPUT NUMBER", CMD_InputNumber, this)); //
            CommandTable.Add(0x10, new CmdItem(2, "INPUT STRING", CMD_InputString, this)); //
            CommandTable.Add(0x11, new CmdItem(1, "PRINT", CMD_Print, this)); // 
            CommandTable.Add(0x12, new CmdItem(1, "PRINTCLEAR", CMD_Print, this)); //
            CommandTable.Add(0x13, new CmdItem(0, "RETURN", CMD_Return, this));
            CommandTable.Add(0x14, new CmdItem(4, "COMPARE AND", CMD_CompareAnd, this)); //
            CommandTable.Add(0x15, new CmdItem(0, "VERTICAL MENU", CMD_VertMenu, this));
            CommandTable.Add(0x16, new CmdItem(0, "IF =", CMD_If, this)); //
            CommandTable.Add(0x17, new CmdItem(0, "IF <>", CMD_If, this)); //
            CommandTable.Add(0x18, new CmdItem(0, "IF <", CMD_If, this)); //
            CommandTable.Add(0x19, new CmdItem(0, "IF >", CMD_If, this)); //
            CommandTable.Add(0x1A, new CmdItem(0, "IF <=", CMD_If, this)); //
            CommandTable.Add(0x1B, new CmdItem(0, "IF >=", CMD_If, this)); //
            CommandTable.Add(0x1C, new CmdItem(0, "CLEARMONSTERS", CMD_ClearMonsters, this));
            CommandTable.Add(0x1D, new CmdItem(1, "PARTYSTRENGTH", CMD_PartyStrength, this));
            CommandTable.Add(0x1E, new CmdItem(6, "CHECKPARTY", CMD_CheckParty, this));
            CommandTable.Add(0x1F, new CmdItem(2, "notsure 0x1f", null, this));
            CommandTable.Add(0x20, new CmdItem(1, "NEWECL", CMD_NewECL, this)); //
            CommandTable.Add(0x21, new CmdItem(3, "LOAD FILES", CMD_LoadFiles, this)); // 
            CommandTable.Add(0x22, new CmdItem(2, "PARTY SURPRISE", null, this)); //CMD_PartySurprise));
            CommandTable.Add(0x23, new CmdItem(4, "SURPRISE", CMD_Surprise, this));
            CommandTable.Add(0x24, new CmdItem(0, "COMBAT", CMD_Combat, this));
            CommandTable.Add(0x25, new CmdItem(0, "ON GOTO", CMD_OnGoto, this)); //
            CommandTable.Add(0x26, new CmdItem(0, "ON GOSUB", CMD_OnGoto, this)); //
            CommandTable.Add(0x27, new CmdItem(8, "TREASURE", CMD_Treasure, this)); //
            CommandTable.Add(0x28, new CmdItem(3, "ROB", CMD_Rob, this)); //
            CommandTable.Add(0x29, new CmdItem(14, "ENCOUNTER MENU", CMD_EncounterMenu, this)); //
            CommandTable.Add(0x2A, new CmdItem(3, "GETTABLE", CMD_GetTable, this)); //
            CommandTable.Add(0x2B, new CmdItem(0, "HORIZONTAL MENU", CMD_HorizontalMenu, this)); //
            CommandTable.Add(0x2C, new CmdItem(6, "PARLAY", CMD_Parlay, this)); //
            CommandTable.Add(0x2D, new CmdItem(1, "CALL", CMD_Call, this)); //
            CommandTable.Add(0x2E, new CmdItem(5, "DAMAGE", CMD_Damage, this)); //
            CommandTable.Add(0x2F, new CmdItem(3, "AND", CMD_And, this)); //
            CommandTable.Add(0x30, new CmdItem(3, "OR", CMD_Or, this)); //
            CommandTable.Add(0x31, new CmdItem(0, "SPRITE OFF", CMD_SpriteOff, this)); //
            CommandTable.Add(0x32, new CmdItem(1, "FIND ITEM", CMD_FindItem, this));
            CommandTable.Add(0x33, new CmdItem(0, "PRINT RETURN", CMD_Return, this)); //
			//CommandTable.Add(0x34, new CmdItem(1, "ECL CLOCK", CMD_EclClock)); //
			CommandTable.Add(0x34, new CmdItem(2, "ECL CLOCK", CMD_EclClock, this)); //
			CommandTable.Add(0x35, new CmdItem(3, "SAVE TABLE", CMD_SaveTable, this)); //
			//CommandTable.Add(0x36, new CmdItem(1, "ADD NPC", CMD_AddNPC));
			CommandTable.Add(0x36, new CmdItem(2, "ADD NPC", CMD_AddNPC, this)); //
            CommandTable.Add(0x37, new CmdItem(3, "LOAD PIECES", CMD_LoadPieces, this)); //
            CommandTable.Add(0x38, new CmdItem(1, "PROGRAM", CMD_Program, this)); //
            CommandTable.Add(0x39, new CmdItem(1, "WHO", CMD_Who, this)); //
            CommandTable.Add(0x3A, new CmdItem(0, "DELAY", CMD_Delay, this)); //
            CommandTable.Add(0x3B, new CmdItem(3, "SPELL", CMD_Spell, this)); //?
            CommandTable.Add(0x3C, new CmdItem(1, "PROTECTION", CMD_Protection, this)); //
            CommandTable.Add(0x3D, new CmdItem(0, "CLEAR BOX", CMD_ClearBox, this));//
            //CommandTable.Add(0x3E, new CmdItem(0, "DUMP", CMD_Dump));
            //CommandTable.Add(0x3F, new CmdItem(1, "FIND SPECIAL", CMD_FindSpecial));
            //CommandTable.Add(0x40, new CmdItem(1, "DESTROY ITEMS", CMD_DestroyItems));
        }


        private static void DumpBin(byte[] data, string block_name)
        {
            string bin_name = block_name + ".bin";

            using (BinaryWriter binWriter = new BinaryWriter(File.Open(bin_name, FileMode.Create)))
            {
                binWriter.Write(data);
            }
        }
    }



 
    internal class CmdItem
    {
        public delegate string CmdDelegate();

        int size;
        string name;
        CmdDelegate cmd;
        EclDump ecldump;

        public CmdItem(int Size, string Name, CmdDelegate Cmd, EclDump ecl_dump)
        {
            size = Size;
            name = Name;
            cmd = Cmd;
            ecldump = ecl_dump;
        }

        public string Dump()
        {
            if (cmd != null)
            {
                return cmd();
            }
            else
            {
                if (size == 0)
                {
                   // EclDump.ecl_offset += 1;
                    ecldump.ecl_offset += 1;
                }
                else
                {
                    //EclDump.LoadEclOpps(size);
                    ecldump.LoadEclOpps(size);
                }
                return "todo";
            }
        }

        public string Name()
        {
            return name;
        }
    }
}
