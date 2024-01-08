using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace spartagame
{
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Gold { get; }

        public Character(string name, string job, int level, int atk, int def,int hp,int gold)
        {
            Name = name;    
            Job = job;  
            Level = level;  
            Atk = atk;  
            Def = def;  
            Hp = hp;
            Gold = gold;

        }
         
    }

    public class item
    {
        public string Name { get; }     
        public string Description { get; }  
        public int Type { get; }
        public int Atk { get; } 
        public int Def { get; } 
        public int Hp { get; } 
        
        public bool IsEquipped { get; set; }
        public static int ItemCnt = 0;

        public item (string name,string description, int type, int atk,int def,int hp, bool isEquipped = false) 
        {
            Name = name;    
            Description = description;  
            Type = type;    
            Atk = atk; Def = def;
            Hp = hp;
            isEquipped = isEquipped;
           }

        public void PrintItemStatDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0}", idx);
                Console.ResetColor();
            }


            if (IsEquipped)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("E");
                Console.ResetColor();
                Console.Write("]");
            }
            Console.Write(Name);
            Console.Write(" | ");

            if (Atk != 0) Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk} ");
            if (Def != 0) Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def} ");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp} ");

            Console.Write(" | ");

            Console.WriteLine(Description); 
        }
    }
    internal class Program
    {
        static Character _player;
        static item[] _items;

        static void Main(string[] args)
        {
            GameDataSetting();
            PrintStartLogo();
            StartMenu();

        }

        static void StartMenu()
        {
            Console.Clear();
            Console.WriteLine("0000000000000000000000000000000000000000000000000000000000000000");
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine("0000000000000000000000000000000000000000000000000000000000000000");
            Console.WriteLine("");
            Console.WriteLine("1.상태보기");
            Console.WriteLine("2.인벤토리");
            Console.WriteLine("");

            switch (CheckVaildinput(1, 2))
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
            }
        }

        private static void InventoryMenu()
        {
            Console.Clear ();

            ShowHighlightedText("@인벤토리@");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템목록]");

            for(int i = 0; i < item.ItemCnt; i++) {
                _items[i].PrintItemStatDescription();
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("");
            switch (CheckVaildinput(1, 2))
            {
                case 1:
                    StartMenu();
                    break;
                case 2:
                    EquipMenu();
                    break;
            }

        }

        private static void EquipMenu()
        {
            Console.Clear();

            ShowHighlightedText("@인벤토리 - 장착관리@");
            Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0;i < item.ItemCnt;i++)
            {
                _items[i].PrintItemStatDescription(true, i+1);
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");

            int keyinput = CheckVaildinput(0, item.ItemCnt);

            switch (keyinput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    ToggleEquipStatus(keyinput - 1);
                    EquipMenu();
                    break;
            }


        }

        private static void ToggleEquipStatus(int idx)
        {
            _items[idx].IsEquipped = !_items[idx].IsEquipped;
        }

        private static void StatusMenu()
        {
            Console.Clear ();

            ShowHighlightedText("@상태보기@");
            Console.WriteLine("캐릭터의 정보가 표기됩니다.");

            PrintTextWithHighlights("Lv.", _player.Level.ToString("00"));
            Console.WriteLine("");
            Console.WriteLine("{0} (  {1}  )", _player.Name, _player.Job);

            int bonusAtk = getSumBonusAtk();

            PrintTextWithHighlights("공격력 : ",( _player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? string.Format("(+{0})", bonusAtk) : "");
            int bonusDef = getSumBonusDef();
            PrintTextWithHighlights("방어력 : ", (_player.Def + bonusDef).ToString(), bonusDef > 0 ? string.Format("(+{0})", bonusDef) : "");
            int bonusHp = getSumBonusHp();
            PrintTextWithHighlights("체력 : ", (_player.Hp + bonusHp).ToString(), bonusHp > 0 ? string.Format("(+{0})", bonusHp) : "");
            PrintTextWithHighlights("골드 : ", _player.Gold.ToString());
            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");
            switch (CheckVaildinput(0, 0))
            {
                case 0:
                    StartMenu();
                    break;   
            }

        }

        private static int getSumBonusAtk()
        {
            int sum = 0;
            for(int i= 0; i < item.ItemCnt; i++)
            {
                if (_items[i].IsEquipped) sum += _items[i].Atk;
            }
            return sum;
        }

        private static int getSumBonusDef()
        {
            int sum = 0;
            for (int i = 0; i < item.ItemCnt; i++)
            {
                if (_items[i].IsEquipped) sum += _items[i].Def;
            }
            return sum; 
        }

        private static int getSumBonusHp()
        {
            int sum = 0;
            for (int i = 0; i < item.ItemCnt; i++)
            {
                if (_items[i].IsEquipped) sum += _items[i].Hp;
            }
            return sum;
        }

        private static void ShowHighlightedText(String text) {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(text);    
            Console.ResetColor();

        }

        private static void PrintTextWithHighlights(string s1, string s2, string s3 = "")
        {
            Console.Write(s1);
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.Write(s2);
            Console.ResetColor ();
            Console.WriteLine(s3);
        }

        private static int CheckVaildinput(int min, int max)
        {
            int keyinput;
            bool result;

            do
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                result = int.TryParse(Console.ReadLine(), out keyinput);
            } while (result == false || CheckIfVaild(keyinput, min, max) == false);

            return keyinput;
          


        }

        private static bool CheckIfVaild(int keyinput, int min, int max)
        {
            if (min <= keyinput && keyinput <= max) return true;
            return false;
        }

        private static void PrintStartLogo()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("_________ __                    __");
            Console.WriteLine("/ _____ / _____     _____ ____       / _____ / _ /  | _ _____ _______ _ /  | _");
            Console.WriteLine("/   ___ __     /     _ / __      _____     __\\__  _ __ \\   __ ");
            Console.WriteLine("    _ / __ _ | Y Y  \\  ___ /      /         |  |   / __ _ |  | / |  |");
            Console.WriteLine("______ / (____ /| __ | _ |  / __ >    / _______ / | __ | (____ /| __ |    | __ |");
            Console.WriteLine("/      /       /      /             /             /");
            Console.WriteLine("                             PRESS ANYKEY TO START                                ");
        }

        private static void GameDataSetting()
        {
            _player = new Character("chad", "전사", 1, 10, 5, 100, 1500);
            _items = new item[10];
            Additem(new item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 5, 0));
            Additem(new item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 1, 2, 0, 0));

        }
        static void Additem(item item)
        {
            if (item.ItemCnt == 10) return;
            _items[item.ItemCnt] = item;
            item.ItemCnt++;

        }
    }
}
