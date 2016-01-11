using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldBoxExplorer.Lib.Plugins.DaxEcl.EclDump
{
	class MemType
	{
		public int startVal;
		public int endVal;
		public int loadOffset;
		public int type;
		public MemType(int s, int e, int l, int t)
		{
			startVal = s;
			endVal = e;
			loadOffset = l;
			type = t;
		}
	}


    public partial class EclDump
    {

		public static int MemStart;
      //  public static int MemStart = 0x8000;
        //public static int MemBase = 0x10000 - MemStart; // 0x6700 compared to 0x8000 used in Curese
        public static int MemBase;

        internal EclOpp[] LoadEclOpps(int numberOfSets)
        {
            int strIndex = 0;

            List<EclOpp> opps = new List<EclOpp>();

            for (int loop_var = 1; loop_var <= numberOfSets; loop_var++)
            {
				byte code = ecl_ptr[MemBase + ecl_offset + 1];
				byte low = ecl_ptr[MemBase + ecl_offset + 2];

                EclOpp curOpp = new EclOpp(code, low, this);

                ecl_offset += 2;

                if (code == 1 || code == 2 || code == 3)
                {
                    ecl_offset++;
					byte high = ecl_ptr[MemBase + ecl_offset];

                    curOpp.SetHigh(high);
                }
                else if (code == 0x80) // Load compressed string
                {
                    strIndex++;

                    short strLen = low;

                    if (strLen > 0)
                    {
                        LoadCompressedEclString(strIndex, strLen);
                    }
                    else
                    {
                        stringTable[strIndex] = string.Empty;
                    }
                }
                else if (code == 0x81)
                {
                    strIndex++;
                    ecl_offset++;
					byte high = ecl_ptr[MemBase + ecl_offset];

                    curOpp.SetHigh(high);

                    ushort loc = curOpp.Word;

                    stringTable[strIndex] = CopyStringFromMemory(loc);
                }
                else
                {
                }

                opps.Add(curOpp);
            }
            ecl_offset++;

            return opps.ToArray();
        }

        internal string CopyStringFromMemory(int loc)
        {
			if (loc >= 0x4900 && loc <= 0x4CFF)
            {
				return string.Format("<string starting at {0}>", Area1Name(0x6E00 + (loc * 2)));
            }
			else if (loc >= 0x6B00 && loc <= 0x6FFF)
            {
				if (loc == 0x6B00)
                {
                    return "<selected player Name>";
                }
                else
                {
					return string.Format("<string starting at {0}>", Area2Name(0x2A00 + (loc * 2)));
                }
            }
			else if (loc >= 0x9700 && loc <= 0x98FF)
            {
				return string.Format("<string starting at stru_1B2CA.word_{0:X4}>", (((loc * 2) + 0xD200) & 0xffff));
            }
			else if (loc >= 0x9900 && loc <= 0xB6FF)
            {
                var sb = new System.Text.StringBuilder();
                int offset = 0;
				while (ecl_ptr[offset + loc + MemBase] != 0)
                {
					sb.Append((char)ecl_ptr[offset + loc + MemBase]);
                    offset++;
                }

				return string.Format("<string starting at ecl byte 0x{0:X4}>{1}", (loc + MemBase) & 0xffff, sb.ToString());
            }
            else
            {
                return "<empty string>";
            }
        }

        internal string GetMemoryValue(int loc)
        {
			if (loc >= 0x4900 && loc <= 0x4CFF)
            {
				return Area1Name(0x6E00 + (loc * 2));
            }
			else if (loc >= 0x6B00 && loc <= 0x6FFF)
            {
                var playerString = get_player_values(loc);
                if (playerString != "")
                {
                    return playerString;
                }
                else
                {
					return Area2Name(0x2A00 + (loc * 2));
                }
            }
			else if (loc >= 0x9700 && loc <= 0x98FF)
            {
                return string.Format("<stru_1B2CA.word_{0:X4}>", (((loc * 2) + 0x0C00) & 0xffff));
            }
			else if (loc >= 0x9900 && loc <= 0xB6FF)
            {
				return string.Format("<ecl byte {0}>{1}", (loc + MemBase) & 0xffff, ecl_ptr[loc + MemBase]);
            }
            else
            {
                if (loc < 0xC04B)
                {
                    switch (loc)
                    {
                        case 0x00B1:
                            return "<word_1D918>";

                        case 0x00FB:
                            return "<word_1D914>";

                        case 0x00FC:
                            return "<word_1D916>";

                        case 0x033D:
                            return "<mapDirection>";

                        case 0x035F:
                            return "0";
                    }
                }
                else
                {
                    loc -= 0xC04B;

                    switch (loc)
                    {
                        case 0:
                            return "<mapPosX>";

                        case 0x01:
                            return "<mapPosY>";

                        case 0x02:
                            return "<gbl.mapDirection / 2>";

                        case 0x03:
                            return "<mapWallType>";

                        case 0x04:
                            return "<mapWallRoof>";

                        case 0x0E:
                            return "0";
                    }
                }
            }

            return string.Format("<BAD LOCATION 0x{0:X}>",loc);
        }


        internal string get_player_values(int loc)
        {
            loc -= 0x6B00;

			if (loc == 0x18)
			{
				return string.Format("<PLAYER.offset_0x14_b>");
			}
			else if (loc == 0x9B)
			{
				return string.Format("<PLAYER.offset_0x6E_b>");
			}
			else if (loc == 0xa0)
			{
				return string.Format("<PLAYER.offset_0x73_b>");
			}
			else if (loc >= 0xa5 && loc <= 0xac)
			{
				return string.Format("<PLAYER.offset_0x76[{0}]_b>",loc-0xa4);
			}
			else if (loc == 0xb8)
			{
				return string.Format("<PLAYER.offset_0x84_b>");
			}
			else if (loc == 0xbb)
			{
				return string.Format("<PLAYER.offset_0x88_w>");
			}
			else if (loc == 0xbd)
			{
				return string.Format("<PLAYER.offset_0x8A_w>");
			}
			else if (loc == 0xbf)
			{
				return string.Format("<PLAYER.offset_0x8C_w>");
			}
			else if (loc == 0xc1)
			{
				return string.Format("<PLAYER.offset_0x8E_w>");
			}
			else if (loc == 0xc3)
			{
				return string.Format("<PLAYER.offset_0x90_w>");
			}
			else if (loc == 0xd8)
			{
				return string.Format("<PLAYER.offset_0xA0_b>");
			}
			else if (loc == 0xf7)
			{
				return string.Format("<PLAYER.offset_0xB8_w>");
			}
			else if (loc == 0xf9)
			{
				return string.Format("<PLAYER.offset_0xBA_b>");
			}
			else if (loc == 0x100)
			{
                return "<selected player in_combat (true 1, false 0x80, player not found 0)>";
			}
			else if (loc == 0x10C)
			{
                return "<selected player (our team && quick fight 0x80, enemy team 0x81, else 0)>";
			}
			else if (loc == 0x10D)
			{
				return string.Format("<0>");
			}
			else if (loc == 0x11B)
			{
				return string.Format("<PLAYER.offset_0x11C_b>");
			}
			else if (loc == 0x2b1)
			{
				return string.Format("<PLAYER index?>");
			}
			else if (loc == 0x2b4)
			{
				return string.Format("<PLAYER index?>");
			}
			else if (loc == 0x2CF)
			{
                return "<selected player charisma (scaled 0-60)>";
			}
			else if (loc == 0x312)
			{
				return string.Format("<byte_1C6D4>");
			}
			else if (loc == 0x33E)
			{
				return string.Format("<dword_1BD37.field_0x67C>");
			}
			else
            {
                return string.Format("");
            }
        }


        string WriteStringToMemory(int loc)
        {
			if (loc >= 0x4900 && loc <= 0x4CFF)
            {
				return string.Format("<string starting at {0}>", Area1Name(0x6E00 + (loc * 2)));
            }
			else if (loc >= 0x6B00 && loc <= 0x6FFF)
            {
				if (loc == 0x6B00)
                {
                    return "<slected player Name>";
                }
                else
                {
					return string.Format("<string starting at {0}>", Area2Name(0x2A00 + (loc * 2)));
                }
            }
			else if (loc >= 0x9700 && loc <= 0x98FF)
            {
                return string.Format("<string starting at stru_1B2CA.word_{0:X4}>", (((loc * 2) + 0x0C00) & 0xffff));
            }
			else if (loc >= 0x9900 && loc <= 0xB6FF)
            {
				return string.Format("<string starting at ecl byte {0}>", (loc + 0x9900) & 0xffff);
            }
            else
            {
                return "</user/bin>";
            }
        }

		MemType[] curse = { new MemType(0x4B00, 0x4EFF, 0x6A00, 0), // Area1
		new MemType(0x7C00, 0x7FFF, 0x800, 1), // Area2
		new MemType(0x7A00, 0x7BFF, 0x0C00, 2), // 
		new MemType(0x8000, 0x9DFF, 0x0000, 3), // ECL string
		 };

		MemType[] pool = { new MemType(0x4900, 0x4CFF, 0x6E00, 0), // Area1
		new MemType(0x6B00, 0x6FFF, 0x2A00, 1), // Area2
		new MemType(0x9700, 0x98FF, 0xD200, 2), // 
		new MemType(0x9900, 0xB6FF, 0x6700, 3), // ECL string
		 };

        string SetMemoryValue(int loc, string value)
        {
			if (loc >= 0x4900 && loc <= 0x4CFF)
            {
                string extra = "";
				if ((loc - 0x4900) == 0x0FD || (loc - 0x4B00) == 0x0FE)
                {
                    extra = ", <byte_1EE94> = true";
                }

				return string.Format("<{0}> = {1}{2}", Area1Name(0x6E00 + (loc * 2)), value, extra);
            }
			else if (loc >= 0x6B00 && loc <= 0x6FFF)
            {
				string area2txt = string.Format("<{0}> = {1}", Area2Name(0x2A00 + (loc * 2)), value);
                string altertxt = alter_character(value, loc);
                return area2txt + ", " + altertxt;
            }
			else if (loc >= 0x9700 && loc <= 0x98FF)
            {
				return string.Format("<stru_1B2CA.word_{0:X4}> = {1}", (((loc * 2) + 0xD200) & 0xffff), value);
            }
			else if (loc >= 0x9900 && loc <= 0xB6FF)
            {
				return string.Format("<ecl byte {0}> = {1}", (loc + 0x9900) & 0xffff, value);    
            }
            else
            {
                if (loc < 0xBF68)
                {
                    switch (loc)
                    {
                        case 0xFB:
                            return "nop";

                        case 0xFC:
                            return "nop";

                        case 0xB1:
                            return "nop";

                        case 0x3DE:
                            return string.Format("<word_1EE76> = {0}", value);

                        case 0xB8:
                            return string.Format("<word_1EE78> = {0}", value);

                        case 0xB9:
                            return string.Format("<word_1EE7A> = {0}", value);
                    }
                }
                else
                {
                    loc -= 0xBF68;

                    switch (loc)
                    {
                        case 0xE3:
                            return string.Format("<mapPosX> = {0}", value);

                        case 0xE4:
                            return string.Format("<mapPosY> = {0}", value);

                        case 0xE5:
                            return string.Format("<mapDirection> = mapped({0}, [0,2,4,6,0,2,4,6])", value);

                        case 0xF1:
                        case 0xF7:
                            return string.Format("<byte_1EE91> = true");
                    }
                }
            }

            return "";

        }

        private static string alter_character(string value, int location)
        {
            int loc = location - 0x6B00;

            if (loc >= 0x20 && loc <= 0x70)
			{
				int spellSlot = loc - 0x1f;
                return string.Format("<selected player spell> = {0}", value);
			}
            else if (loc == 0xb8)
			{
                return string.Format("<selected player Morale> =  {0} [if value > 0xB2 then value -= 0x32]", value);
			}
            else if (loc == 0xbb)
			{
				return string.Format("<selected player field_88 (Copper?)> = {0}", value);
			}
            else if (loc == 0xbd)
			{
				return string.Format("<selected player field_8A (Electrum?)> = {0}", value);
			}
            else if (loc == 0xbf)
			{
				return string.Format("<selected player field_8C (Silver?)> = {0}", value);
			}
            else if (loc == 0xc1)
			{
				return string.Format("<selected player field_8E (Gold> = {0}", value);
			}
            else if (loc == 0xc3)
			{
				return string.Format("<selected player field_90 (Platinum?)> = {0}", value);
			}
            else if (loc == 0xf7)
			{
                return string.Format("<selected player field_B8> = {0}", value);
			}
            else if (loc == 0xf9)
			{
                return string.Format("<selected player field_13E> = {0}", value);
			}
            else if (loc == 0x100)
			{
                return string.Format("<selected player in_combat> = false [if value '{0}' >= 0x80, also if value == 0x87 the health_status = stoned]", value);
			}
            else if (loc == 0x10c)
			{
                return string.Format("<selected player CombatTeam  & QuickFight> = {0} [0 - Ours & Not Quick, 0x80 - Ours & Quick, 0x81 - Enemy & Quick]", value);
            }
			else if (loc == 0x80)
			{
				return string.Format("<selected player CombatTeam = 0 & QuickFight = 1>");
			} 
			else if (loc == 0x81)
			{
				return string.Format("<selected player CombatTeam = 1 & QuickFight = 1>");
			} 
			else if (loc == 0x312)
			{
                return string.Format("<game area> = {0}", value);
			}
			else if (loc == 0x322)
			{
                return string.Format("<if value > 0x80 set WALLDEF 1 value & 0x7F> value: {0}", value);
			}
            else if (loc == 0x324)
			{
                return string.Format("<if value > 0x80 set WALLDEF 2 value & 0x7F> value: {0}", value);
			}
            else if (loc == 0x326)
			{
                return string.Format("<if value > 0x80 set WALLDEF 3 value & 0x7F> value: {0}", value);
			}

            return "";
		}

        private static string Area2Name(int addr)
        {
            int loc = addr & 0xFFFF;

            return string.Format("area2.word_{0:X3}", loc);
        }

        private string Area1Name(int addr)
        {
            int loc = addr & 0xFFFF;

            return string.Format("area1.word_{0:X3}", addr & 0xffff);
        }



        internal void LoadCompressedEclString(int strIndex, int inputLength)
        {
            byte[] data = new byte[inputLength];

            for (int i = 0; i < inputLength; i++)
            {
				data[i] = ecl_ptr[ecl_offset + MemBase + 1 + i];
            }

            ecl_offset += (ushort)inputLength;

            stringTable[strIndex] = DecompressString(data);
        }

        internal string DecompressString(byte[] data)
        {
            var sb = new System.Text.StringBuilder();
            int state = 1;
            uint lastByte = 0;

            foreach (uint thisByte in data)
            {
                uint curr = 0;
                switch (state)
                {
                    case 1:
                        curr = (thisByte >> 2) & 0x3F;
                        if (curr != 0) sb.Append(inflateChar(curr));
                        state = 2;
                        break;

                    case 2:
                        curr = ((lastByte << 4) | (thisByte >> 4)) & 0x3F;
                        if (curr != 0) sb.Append(inflateChar(curr));
                        state = 3;
                        break;

                    case 3:
                        curr = ((lastByte << 2) | (thisByte >> 6)) & 0x3F;
                        if (curr != 0) sb.Append(inflateChar(curr));

                        curr = thisByte & 0x3F;
                        if (curr != 0) sb.Append(inflateChar(curr));
                        state = 1;
                        break;
                }
                lastByte = thisByte;
            }

            return sb.ToString();
        }


        internal char inflateChar(uint arg_0)
        {
            if (arg_0 <= 0x1f)
            {
                arg_0 += 0x40;
            }

            return (char)arg_0;
        }
    }


    public class EclOpp
    {
        bool wordSet;
        int word;
        int high;
        int low;
        int code;
        public EclDump ecldump;

        public EclOpp(byte _code, byte _low, EclDump ecl_dump)
        {
            code = _code;
            low = _low;
            ecldump = ecl_dump;
        }

        public void SetHigh(byte _high)
        {
            high = _high;
            word = (high << 8) | (low);
            wordSet = true;
        }

        public ushort Word
        {
            get
            {
                if (wordSet)
                {
                    return (ushort)word;
                }
                else
                {
                    //throw new Exception();
                    return (ushort)low;
                }
            }
        }

        public byte Code
        {
            get
            {
                return (byte)code;
            }
        }



        public string GetCmdValue()
        {
            switch (code)
            {
                case 0x00:
                    return low.ToString();

                case 0x01:
                case 0x03:
                case 0x80:
                    if (wordSet)
                        return ecldump.GetMemoryValue(word);
                    return "<opp not word loaded>";

                case 0x02:
                    if (wordSet)
                        return word.ToString();
                    return "<opp not word loaded>";

                default:
                    return "<invalid code " + code.ToString() + " >";
            }
        }
    }

    public class EclData
    {
        byte[] data;
        public EclData(byte[] _data)
        {
            data = new byte[_data.Length - 2];
            System.Array.Copy(_data, 2, data, 0, _data.Length - 2);
        }

        public byte this[int index]
        {
            get
            {
                // simulate the 16 bit memory space.
                int loc = index & 0xFFFF;
                if (loc < data.Length)

                    return data[loc];
                else
                    return 0xff;
            }
        }

        internal bool IsValidAddr(int addr)
        {
            int loc = addr & 0xFFFF;

            return loc < data.Length;
        }
    }

}
