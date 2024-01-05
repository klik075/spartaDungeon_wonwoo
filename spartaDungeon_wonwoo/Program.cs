using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
//using System.Threading;
namespace spartaDungeon_wonwoo
{

    public class Program
    {
        static readonly string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\gameData";
        static void Main(string[] args)
        {
            Player player = new Player();//플레이어 생성
            Shop shop = new Shop();//상점 생성
            Dungeon dungeon = new Dungeon();//던전 생성
            PlayerData pData = new PlayerData();//플레이어 데이터 객체
            ShopData sData = new ShopData();//상점 데이터 객체
            if (!(File.Exists(folderPath + @"\playerData.xml")))//파일 경로에 파일이 없다면
            {
                pData.SerializePlayer();//초기화된 플레이어 파일 생성
            }
            if (!(File.Exists(folderPath + @"\shopData.xml")))//파일 경로에 파일이 없다면
            {
                sData.SerializeShop(); //초기화된 상점 파일 생성
            }

            player = pData.DeserializePlayer(); //플레이어 파일 가져옴
            shop = sData.DeserializeShop(); //상점 파일 가져옴

            //player.AddItem(new Item());
            //player.items[0].SetItem("낡은 검", 500, 20, 0, ItemType.attack, "원우가 쓰다 버린 검");
            //player.AddItem(new Item());
            //player.items[1].SetItem("낡은 창", 600, 15, 5, ItemType.attack, "원우가 쓰다 버린 창");
            //player.AddItem(new Item());
            //player.items[2].SetItem("스파르타의 창", 700, 20, 0, ItemType.attack, "원우가 쓰다 버린 창");
            //player.AddItem(new Item());
            //player.items[3].SetItem("낡은 천", 1000, 0, 5, ItemType.defense, "원우가 쓰다 버린 천");
            //Console.SetWindowSize(200, 40); //창 크기 조절

            StartGame(ref player, ref shop, ref dungeon, ref pData, ref sData); //게임 시작
        }
        public class ShopData //게임 데이터
        {
            private string xmlFileName = folderPath + @"\shopData.xml";//xml 파일 경로
            public void SerializeShop()//게임 초기화 직렬화
            {
                Shop objShop = new Shop();
                objShop.SetStorage(); //상점 창고 설정
                objShop.SetItems(); //상점 판매목록 설정
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Shop));
                using (StreamWriter wr = new StreamWriter(xmlFileName))
                {
                    xmlSerializer.Serialize(wr, objShop);
                }
            }
            public Shop ResetShop(ref ShopData sData)//나갔다 들어오면 아이템 목록 초기화
            {
                Shop objShop = new Shop();
                objShop.SetStorage(); //상점 창고 설정
                objShop.SetItems(); //상점 판매목록 설정

                DestoryData();//기존 데이터 파일 삭제

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Shop));
                using (StreamWriter wr = new StreamWriter(xmlFileName))
                {
                    xmlSerializer.Serialize(wr, objShop);
                }
                return sData.DeserializeShop();
            }
            public void DestoryData()//데이터 파일 삭제
            {
                File.Delete(xmlFileName);
            }

            public void SaveShop(ref Shop shop) //현재 데이터 저장 직렬화 //아이템을 구매할 때 저장
            {
                DestoryData();//기존 데이터 파일 삭제

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Shop));
                using (StreamWriter wr = new StreamWriter(xmlFileName))
                {
                    xmlSerializer.Serialize(wr, shop);
                }
            }
            public Shop DeserializeShop() //역직렬화
            {
                Shop s;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Shop));
                using (StreamReader rdr = new StreamReader(xmlFileName))
                {
                    s = (Shop)xmlSerializer.Deserialize(rdr);
                }
                return s;
            }

        }
        public class PlayerData //게임 데이터
        {
            private string xmlFileName = folderPath + @"\playerData.xml";//xml 파일 경로
            public void SerializePlayer() //게임 초기화 직렬화
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                if (di.Exists == false)
                {
                    di.Create();
                }
                Player objPlayer = new Player();
                Console.WriteLine("어서오세요 플레이어의 이름을 입력해주세요");
                string name = Console.ReadLine();
                Console.Clear();
                objPlayer.Name = name;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Player));
                using (StreamWriter wr = new StreamWriter(xmlFileName))
                {
                    xmlSerializer.Serialize(wr, objPlayer);
                }
            }
            public void DestoryData()//데이터 파일 삭제
            {
                File.Delete(xmlFileName);
            }
            public void SavePlayer(ref Player player) //현재 데이터 저장 직렬화
            {
                //Player newPlayer = new Player();
                //newPlayer = player; //새로 만들 데이터에 현재 데이터 복사 ?? 복사가 아니라 참조라서 안 해도 되나? ( O )
                DestoryData();//기존 데이터 파일 삭제

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Player));
                using (StreamWriter wr = new StreamWriter(xmlFileName))
                {
                    xmlSerializer.Serialize(wr, player);
                }
            }
            public Player DeserializePlayer() //역직렬화
            {
                Player p;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Player));
                using (StreamReader rdr = new StreamReader(xmlFileName))
                {
                    p = (Player)xmlSerializer.Deserialize(rdr);
                }
                return p;
            }

        }
        static public void StartGame(ref Player player, ref Shop shop, ref Dungeon dungeon, ref PlayerData pData, ref ShopData sData) // 게임 시작
        {
            while (true)
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();

                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전입장");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine();

                GameState gameState = (GameState)InputKey(5);
                switch (gameState)
                {
                    case GameState.state:
                        player.PlayerState();//상태보기
                        break;
                    case GameState.inventory:
                        player.PlayerInventory(false,ref player);//인벤토리
                        break;
                    case GameState.shop:
                        shop.PlayerShop(ref player, ref pData, ref sData, ref shop, false);//상점
                        break;
                    case GameState.dungeon:
                        dungeon.StartDungeon(ref player);//던전 입장
                        break;
                    case GameState.rest:
                        player.PlayerRest(ref pData, ref player);//휴식하기
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                        break;
                }
                if (player.IsDead())//죽으면
                {
                    Console.Clear();
                    pData.DestoryData(); //데이터 삭제
                    sData.DestoryData(); //데이터 삭제
                    Console.WriteLine("You Died"); //유다희 출력
                    Console.WriteLine("다시 하려면 게임을 재실행 하세요..");
                    Console.ReadKey();//아무키 입력
                    break;
                }
                else
                {
                    pData.SavePlayer(ref player); // 데이터 저장
                }
            }
        }
        public class Dungeon
        {
            enum DungeonLevel //던전 난이도
            {
                easy = 1,
                normal = 2,
                hard = 3
            }
            public void StartDungeon(ref Player player) //던전 입장 멘트후 던전 선택
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("던전입장");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine($"현재 체력 : {player.Hp} , 현재 방어력 : {player.Defense + player.AddDefense} ");
                Console.WriteLine();

                Console.WriteLine("1. 쉬운 던전    | 방어력 5 이상 권장");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("2. 일반 던전    | 방어력 11 이상 권장");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("3. 어려운 던전  | 방어력 17 이상 권장");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                DungeonLevel Level = (DungeonLevel)InputKey(3);
                switch (Level)
                {
                    case DungeonLevel.easy:
                        EnterDungeon("쉬운 던전", 5, ref player);
                        break;
                    case DungeonLevel.normal:
                        EnterDungeon("일반 던전", 11, ref player);
                        break;
                    case DungeonLevel.hard:
                        EnterDungeon("어려운 던전", 17, ref player);
                        break;
                    default:
                        break;
                }
            }
            public void EnterDungeon(string level, int minDefense, ref Player player) //던전 입장
            {
                if ((player.Defense + player.AddDefense) < minDefense) //방어력이 권장보다 낮으면
                {
                    Random random = new Random();
                    int i = random.Next(1, 11); // 1 ~ 10
                    if (i <= 4)//40프로 이하이면 
                    {
                        DungeonFail(ref player);//던전 실패
                    }
                    else
                    {
                        DungeonClear(level, minDefense, ref player);//던전 성공
                    }
                }
                else
                {
                    DungeonClear(level, minDefense, ref player);
                }
            }
            public void DungeonClear(string level, int minDefense, ref Player player) //던전 성공
            {
                Random rand = new Random();
                int minusHp = rand.Next(20, 36);//감소 수치
                int diff = ((int)player.Defense + (int)player.AddDefense) - minDefense;
                minusHp -= diff;
                if (minusHp <= 0)
                    minusHp = 1;// 0보다 작아질수는 없다.

                int CurrentHp = player.Hp;
                player.Hp -= minusHp; //hp 감소
                if (player.IsDead()) //죽으면 나감
                {
                    return;
                }
                int award;
                if (level == "쉬운 던전")
                {
                    award = 1000;
                }
                else if (level == "일반 던전")
                {
                    award = 1700;
                }
                else
                {
                    award = 2500;
                }
                Random rand2 = new Random();
                int myPower = (int)(player.AddPower + player.Power);
                int addAward = rand2.Next(myPower, 2 * myPower);
                award += (int)(award * (float)addAward / 100);
                int CurrentGold = player.Gold;
                player.Gold += award;

                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!!");
                Console.WriteLine($"{level}을 클리어 하였습니다.");
                Console.WriteLine();

                Console.WriteLine("[탐험 결과]");
                player.LevelUp();
                Console.WriteLine($"체력 {CurrentHp} -> {player.Hp} (-{minusHp})");
                Console.WriteLine($"Gold {CurrentGold} G -> {player.Gold} G (+{award})");
                Console.WriteLine();

                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = InputKey(0);
                switch (input)
                {
                    case 0:
                        break;
                    default:
                        break;
                }
            }
            public void DungeonFail(ref Player player)//던전 실패
            {
                player.Hp /= 2; //hp절반 감소
                if (player.Hp <= 0)
                    return;
                Console.Clear();
                Console.WriteLine("던전 실패");
                Console.WriteLine($"체력이 절반으로 감소합니다. 현재 체력 : {player.Hp}");
                Console.WriteLine();
            }
        }
        public class Shop                                         //상점
        {
            public Item[] storage = new Item[15];                //구매할 수 있는 상점 아이템 창고 
            public Item[] items = new Item[10];                  //판매중인 아이템 배열
            public bool[] isSold = new bool[10];                 //아이템 팔렸는지 bool
            public int itemCount = 0;
            public int sItemCount = 0;

            public void AddItem(Item item)                        //상점에 아이템 추가
            {
                items[itemCount++] = item;
            }
            public void AddSItem(Item item)                       //창고에 아이템 추가
            {
                storage[sItemCount++] = item;
            }
            public void Purchase(ref Player player, ref PlayerData pdata, ref ShopData sData, ref Shop shop, int i)          //아이템 구매
            {
                if (shop.isSold[i] == true)
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else
                {
                    if (player.Gold >= shop.items[i].Price)
                    {
                        player.Gold -= shop.items[i].Price;              // 플레이어 골드 감소
                        Item newItem = new Item();
                        newItem.DuplicateItem(shop.items[i]);            //구매한 아이템 복제
                        player.AddItem(newItem);                   // 인벤토리에 아이템 추가
                        shop.isSold[i] = true;                           // 구매완료 표시
                        sData.SaveShop(ref shop); //상점 저장
                        pdata.SavePlayer(ref player); // 플레이어 저장
                        Console.WriteLine("구매를 완료했습니다.");
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                    }
                }
            }
            public void Sell(ref Player player, int input)                   //상점에 아이템 판매
            {
                if (player.items[input].IsEquip == true)                    //해당 아이템을 장착하고 있다면
                    player.EquipItem(input, ref player);                        //해제한다.
                player.Gold += (int)(player.items[input].Price * 0.85f);    //아이템 가격의 85%를 얻는다.
                player.DeleteItem(input);//플레이어의 해당 아이템 삭제

            }
            public void SetSold()                                           //팔렸는지 확인하는 bool값 초기화
            {
                for (int i = 0; i < itemCount; i++)
                {
                    isSold[i] = new bool();
                    isSold[i] = false;
                }
            }
            public void SetStorage() //창고에 아이템 저장
            {
                sItemCount = 0;
                AddSItem(new Item());
                storage[0].SetItem("수련자 갑옷", 1000, 0, 5, ItemType.defense, "수련에 도움을 주는 갑옷입니다.");
                AddSItem(new Item());
                storage[1].SetItem("무쇠갑옷", 1900, 0, 9, ItemType.defense, "무쇠로 만들어져 튼튼한 갑옷입니다.");
                AddSItem(new Item());
                storage[2].SetItem("스파르타의 갑옷", 3500, 0, 15, ItemType.defense, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.");
                AddSItem(new Item());
                storage[3].SetItem("낡은 검", 600, 2, 0, ItemType.attack, "쉽게 볼 수 있는 낡은 검 입니다.");
                AddSItem(new Item());
                storage[4].SetItem("청동 도끼", 1500, 5, 0, ItemType.attack, "어디선가 사용됐던거 같은 도끼입니다.");
                AddSItem(new Item());
                storage[5].SetItem("스파르타의 창", 2000, 7, 0, ItemType.attack, "스파르타의 전사들이 사용했다는 전설의 창입니다.");
                AddSItem(new Item());
                storage[6].SetItem("TIL", 4500, 15, 0, ItemType.attack, "누군가 작성한 TIL");
                AddSItem(new Item());
                storage[7].SetItem("브라움의 방패", 8000, 5, 30, ItemType.defense, "빡빡이 브라움이 사용하던 방패였습니다.");
                AddSItem(new Item());
                storage[8].SetItem("원우의 허름한 천", 20000, 20, 40, ItemType.defense, "전설의 용사가 입었던 옷의 일부입니다.");
                AddSItem(new Item());
                storage[9].SetItem("원우의 마우스", 300000, 100, 0, ItemType.attack, "전설의 용사가 사용했던 무기입니다.");
            }
            public void SetItems() //상점에 판매할 아이템 랜덤 설정
            {
                itemCount = 0;
                Random rnd = new Random();
                int rndIndex;
                for (int i = 0; i < 6; i++)
                {
                    rndIndex = rnd.Next(0, 51); // 0 ~ 50
                    if (rndIndex == 50)
                    {
                        AddItem(new Item()); //공간을 만들고
                        items[i].DuplicateItem(storage[9]);
                    }
                    else if (rndIndex >= 47)
                    {
                        AddItem(new Item()); //공간을 만들고
                        items[i].DuplicateItem(storage[8]);
                    }
                    else if (rndIndex >= 42)
                    {
                        AddItem(new Item()); //공간을 만들고
                        items[i].DuplicateItem(storage[7]);
                    }
                    else if (rndIndex >= 35)
                    {
                        AddItem(new Item()); //공간을 만들고
                        items[i].DuplicateItem(storage[6]);
                    }
                    else // 0 ~  34
                    {
                        rndIndex = rndIndex % 6; // 0 ~ 5
                        AddItem(new Item()); //공간을 만들고
                        items[i].DuplicateItem(storage[rndIndex]);
                    }
                    //Console.WriteLine($"{rndIndex}"); //몇번이 뽑혔는지 확인용
                }
                SetSold(); //팔렸는지 bool값 설정
            }
            public void ResetShop(ref Player player, ref PlayerData pData, ref ShopData sData, ref Shop shop) //상점 초기화
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("상점 ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("- 아이템 초기화");
                Console.WriteLine($"500 G 를 내면 상점을 초기화할 수 있습니다. (보유 골드 : {player.Gold} G)");
                Console.WriteLine();

                Console.WriteLine("1. 상점 리셋");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = InputKey(1);
                switch (input)
                {
                    case 1:
                        ResetPay(ref player, ref pData, ref sData, ref shop);
                        break;
                    case 0:
                        PlayerShop(ref player, ref pData, ref sData, ref shop, false);
                        break;
                    default:
                        break;
                }
            }
            public void ResetPay(ref Player player, ref PlayerData pData, ref ShopData sData, ref Shop shop) //상점 초기화 지불
            {
                int pay = 500;
                if (player.Gold >= pay)
                {
                    player.Gold -= pay;
                    shop = sData.ResetShop(ref sData); //상점 초기화

                    Console.WriteLine("상점을 초기화 했습니다.");
                    Console.WriteLine();
                    PlayerShop(ref player, ref pData, ref sData, ref shop, false);
                }
                else
                {
                    Console.WriteLine("Gold 가 부족합니다.");
                    Console.WriteLine();
                    PlayerShop(ref player, ref pData, ref sData, ref shop, false);
                }
            }

            public void SellShop(ref Player player, ref PlayerData pData, ref ShopData sData, ref Shop shop)//판매 문구
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("상점 ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("- 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();

                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();

                ShowPlayerItems(ref player); //플레이어 아이템 목록

                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = InputKey(player.itemCount);
                input--;
                if (0 <= input && input < player.itemCount)  //아이템을 선택하면
                {
                    Sell(ref player, input);
                    Console.Clear();
                    pData.SavePlayer(ref player);
                    sData.SaveShop(ref shop);
                    Console.WriteLine("판매를 완료했습니다.");
                    Console.WriteLine();

                    PlayerShop(ref player, ref pData, ref sData, ref shop, false);
                }
                else if (input == -1)   //나가기를 누르면
                {
                    Console.Clear();
                    PlayerShop(ref player, ref pData, ref sData, ref shop, false);
                }
                //else
                //{
                //    Console.Clear();
                //    Console.WriteLine("잘못된 입력입니다");
                //    Console.WriteLine();
                //    PlayerShop(ref player, false);
                //    return;
                //}
            }
            public void ShowPlayerItems(ref Player player)  //플레이어 아이템 목록
            {
                Console.WriteLine("[ 아이템 목록 ]");
                Console.WriteLine();

                if (player.itemCount == 0)
                {
                    Console.WriteLine("가지고 있는 장비가 없습니다.");
                    Console.WriteLine();
                    return;
                }
                for (int i = 0; i < player.itemCount; i++)
                {
                    Console.Write($"- {i + 1} {player.items[i].Name,-15}| "); //이름 출력
                    if (player.items[i].Power != 0 || player.items[i].Defense != 0)//공격력 또는 방어력이 있으면 출력
                    {
                        if (player.items[i].Power != 0)
                            Console.Write($"공격력 +{player.items[i].Power} ");
                        if (player.items[i].Defense != 0)
                            Console.Write($"방어력 +{player.items[i].Defense} ");
                        //if (player.items[i].Power == 0 || player.items[i].Defense == 0)
                        //    Console.Write($"{"",-10}");
                        Console.Write("| ");
                    }
                    else
                    {
                        Console.Write($"{"",-20}| ");
                    }
                    Console.Write($"{player.items[i].Explanation,-20}| ");//설명 출력
                    Console.WriteLine($"{(int)(player.items[i].Price * 0.85f)} G");
                }
                Console.WriteLine();
            }
            public void ShowItems(ref Player player, ref PlayerData pData, ref ShopData sData, ref Shop shop, bool isIn) // false면 인벤토리 화면 true 장착관리 화면
            {
                Console.WriteLine("[ 아이템 목록 ]");
                Console.WriteLine();

                if (shop.itemCount == 0)
                    return;

                for (int i = 0; i < shop.itemCount; i++)
                {
                    if (isIn == false)
                        Console.Write("- ");
                    else
                        Console.Write($"- {i + 1} ");
                    Console.Write($"{shop.items[i].Name,-15}| "); //이름 출력
                    if (shop.items[i].Power != 0 || shop.items[i].Defense != 0)//공격력 또는 방어력이 있으면 출력
                    {
                        if (shop.items[i].Power != 0)
                            Console.Write($"공격력 +{shop.items[i].Power} ");
                        if (shop.items[i].Defense != 0)
                            Console.Write($"방어력 +{shop.items[i].Defense} ");
                        //if (items[i].Power == 0 || items[i].Defense == 0)
                        //    Console.Write($"{"",-10}");
                        Console.Write("| ");
                    }
                    else
                    {
                        Console.Write($"{"",-20}| ");
                    }
                    Console.Write($"{shop.items[i].Explanation,-20}| ");//설명 출력
                    if (shop.isSold[i] == false)
                        Console.WriteLine($"{shop.items[i].Price} G");
                    else
                        Console.WriteLine("구매완료");
                }
                Console.WriteLine();
                int input;
                if (isIn == false) //상점 초기진입 화면
                {
                    Console.WriteLine("1. 아이템 구매");
                    Console.WriteLine("2. 아이템 판매");
                    Console.WriteLine("3. 상점 초기화");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    input = InputKey(3);
                }
                else //아이템 구매를 눌렀다면
                {
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    input = InputKey(shop.itemCount);
                }
                if (isIn == false) //초기화면 일 때
                {
                    switch (input)
                    {
                        case 1:
                            PlayerShop(ref player, ref pData, ref sData, ref shop, true); //아이템 구매
                            break;
                        case 2:
                            SellShop(ref player, ref pData, ref sData, ref shop); //아이템 판매
                            break;
                        case 3:
                            ResetShop(ref player, ref pData, ref sData, ref shop); //상점 초기화
                            break;
                        case 0:
                            break;
                        default:
                            break;
                    }
                }
                else //아이템 구매 창에 있을 때
                {
                    input--;
                    if (0 <= input && input < shop.itemCount)
                    {
                        Purchase(ref player, ref pData, ref sData, ref shop, input);
                        Console.WriteLine();
                        PlayerShop(ref player, ref pData, ref sData, ref shop, false);
                    }
                    else if (input == -1) // 나가기를 누르면
                    {
                        Console.Clear();
                        PlayerShop(ref player, ref pData, ref sData, ref shop, false);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다");
                        Console.WriteLine();
                        PlayerShop(ref player, ref pData, ref sData, ref shop, true);
                        return;
                    }
                }
            }
            public void PlayerShop(ref Player player, ref PlayerData pData, ref ShopData sData, ref Shop shop, bool isIn)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("상점");
                Console.ForegroundColor = ConsoleColor.White;
                if (isIn)
                    Console.Write(" - 아이템 구매");
                Console.WriteLine();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();

                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();

                ShowItems(ref player, ref pData, ref sData, ref shop, isIn); // 상점의 아이템 보여주기
            }
        }
        public class Item
        {
            string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            int price; //가격
            public ItemType itemType;
            public int Price
            {
                get { return price; }
                set { price = value; }
            }
            bool isEquip = false;
            public bool IsEquip
            {
                get { return isEquip; }
                set { isEquip = value; }
            }
            float power = 0f;
            public float Power
            {
                get { return power; }
                set { power = value; }
            }
            float defense = 0f;
            public float Defense
            {
                get { return defense; }
                set { defense = value; }
            }
            string explanation;
            public string Explanation
            {
                get { return explanation; }
                set { explanation = value; }
            }
            public Item()
            {
                name = "";
                price = 0;
                isEquip = false;
                power = 0f;
                defense = 0f;
                explanation = "";
            }
            public void DuplicateItem(Item item) //아이템 복제
            {
                this.SetItem(item.Name, item.Price, item.Power, item.Defense, item.itemType, item.Explanation);
            }
            public void EquipItem(ref Player player) //장비 장착 해제
            {
                if (isEquip == false) //현재 아이템이 장착이 안 됐으면
                {
                    isEquip = true;
                    string E = "[E]";
                    name = E + name;
                    player.AddItemPower(this.Power);
                    player.AddItemDefense(this.defense);
                }
                else if (isEquip == true)
                {
                    isEquip = false;
                    name = name.Replace("[E]", "");
                    player.SubItemPower(this.Power);
                    player.SubItemDefense(this.defense);
                }
            }
            public void SetItem(string name, int price, float power, float defense, ItemType type, string str) //아이템 속성을 설정한다.
            {
                this.name = name;
                this.price = price;
                this.power = power;
                this.defense = defense;
                itemType = type;
                explanation = str;
            }
        }
        [Serializable]
        public class Player
        {
            public Item[] items;// 아이템을 배열로 관리
            public int lv;
            public int clearCount; //던전 클리어 횟수
            string name;// //이름
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public string job; //직업

            float power; //공격력
            public float Power
            {
                get { return power; }
                set { power = value; }
            }
            float addPower;//추가 공격력
            public float AddPower
            {
                get { return addPower; }
                set { addPower = value; }
            }

            float defense; //방어력
            public float Defense
            {
                get { return defense; }
                set { defense = value; }
            }
            float addDefense; //추가 방어력
            public float AddDefense
            {
                get { return addDefense; }
                set { addDefense = value; }
            }

            int hp;//체력
            public int Hp
            {
                get { return hp; }
                set { hp = value; }
            }
            int gold;//골드
            public int equipAIndex; //착용한 공격 아이템의 인덱스
            public int equipDIndex; //착용한 방어 아이템의 인덱스
            public int Gold
            {
                get { return gold; }
                set { gold = value; }
            }
            public int itemCount; //소지한 아이템 개수
            public Player()
            {
                items = new Item[20];//공간을 만든다
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new Item(); //공간에 item을 초기화
                }
                lv = 1;
                name = "chad";
                job = "전사";
                power = 10f;
                defense = 5f;
                addDefense = 0f;
                addDefense = 0f;
                hp = 100;
                gold = 1500;
                equipAIndex = -1;
                equipDIndex = -1;
                clearCount = 0;
                itemCount = 0;
            }
            public bool IsDead()
            {
                if (Hp <= 0)
                {
                    return true;
                }
                return false;
            }
            public void LevelUp()  //클리어 횟수 확인후 레벨업
            {
                clearCount++;
                if (clearCount >= lv)
                {
                    lv++;
                    clearCount = 0;
                    power += 0.5f;
                    defense += 1f;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"레벨 업 했습니다. 현재 레벨{lv}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"레벨 업까지 앞으로 ({clearCount}/{lv})");
                    Console.WriteLine();
                }
            }
            public void Gambling(ref PlayerData pData, ref Player player)//도박장
            {
                Console.WriteLine("은밀한 도박장에 어서오세요.");
                Console.WriteLine("참가한 돈 * 확률로 돈을 얻습니다.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[ 주의 : 돈이 충분하지 않으면 맞을 수도 있습니다. ]");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

                Console.WriteLine("최소 500골드부터 이용 가능합니다.");
                Console.WriteLine();

                Console.WriteLine($"보유 골드 : {player.Gold} G ");
                Console.WriteLine();

                Console.WriteLine("1. 참가한다.");
                Console.WriteLine("0. 나간다.");

                int input = InputKey(1);
                if (input == 1)
                {
                    GamblingIn(ref pData, ref player);//도박 실행
                }
                else
                {
                    PlayerRest(ref pData, ref player);
                }
            }
            public void GamblingIn(ref PlayerData pData, ref Player player)
            {
                if (player.Gold <= 500)
                {
                    Console.WriteLine("그지는 가라");
                    Console.WriteLine("당신은 도박장에서 맞았습니다.");
                    player.Hp -= 10;
                    if (player.Hp <= 0)
                        player.Hp = 0;
                    Console.WriteLine($"현재 체력 : {player.Hp} (-10)");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                Console.WriteLine($"보유 골드 : {player.Gold} G ");
                Console.WriteLine("최소 500 G 이상 가능");
                Console.WriteLine();

                Console.WriteLine("참가비를 입력하세요 :");
                Console.WriteLine();

                int input = InputKey(player.Gold);
                if (input < 500)
                {
                    Console.WriteLine("장난하냐?");
                    Console.WriteLine("당신은 쫒겨났습니다.");
                    Console.WriteLine();
                    return;
                }
                Random rnd = new Random();
                float ratio = (float)rnd.Next(1, 10);// 1 ~ 9
                int pM = rnd.Next(10);
                if (pM >= 5)
                {
                    ratio /= 10; //50퍼로 10분의 1토막
                }
                pM = rnd.Next(10); // 0 ~ 9


                Console.Clear();
                //Console.WriteLine($"배수는 {ratio}"); //퍼센트 출력해보기
                Thread.Sleep(1000);
                Console.WriteLine(".");
                Thread.Sleep(1000);
                Console.WriteLine("..");
                Thread.Sleep(1000);
                Console.WriteLine("...");
                Thread.Sleep(1000);
                Console.WriteLine("......!");
                int result;//당첨 금액
                if (pM >= 4) // 60퍼 확률 -  // + , - 를 결정
                {
                    Console.WriteLine("[ 실패 ]");
                    Console.WriteLine();
                    result = -(int)(input * ratio);
                }
                else
                {
                    Console.WriteLine("[ 성공 ]");
                    Console.WriteLine();
                    result = (int)(input * ratio);
                }
                int total = result + input;
                if (total <= 0)// 참가비를 넘는 차감 금액이면
                {
                    player.Gold -= input;// 참가비만큼 뺌
                    Console.WriteLine("참가비를 모두 잃었습니다.");
                }
                else
                {
                    player.Gold += result;
                    if (result <= 0)
                    {
                        Console.WriteLine($"감소한 금액 : {result}");
                    }
                    else
                    {
                        Console.WriteLine($"얻은 금액 : {result}");
                    }
                }
                pData.SavePlayer(ref player);//데이터 저장
                Console.ReadKey();
                Console.Clear();
                Gambling(ref pData, ref player);
            }
            public void RestPay(ref Player player) //휴식하기 돈 지불
            {
                int pay = 500;
                if (player.Gold >= pay)
                {
                    player.Gold -= pay;
                    player.Hp = 100;
                    Console.Clear();
                    Console.WriteLine("휴식을 완료했습니다.");
                    Console.WriteLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Gold 가 부족합니다.");
                    Console.WriteLine();
                }
            }
            public void PlayerRest(ref PlayerData pData, ref Player player)// 휴식하기 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("휴식하기");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {Gold} G)");
                Console.WriteLine();

                Console.WriteLine("1. 휴식하기");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("2. 은밀한 휴식 ㅎㅎ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = InputKey(2);
                switch (input)
                {
                    case 1:
                        RestPay(ref player);
                        break;
                    case 2:
                        Gambling(ref pData, ref player);
                        break;
                    case 0:
                        break;
                    default:
                        break;
                }
            }
            public void EquipItem(int index, ref Player player) //플레이어의 아이템 장착 메소드
            {
                if (items[index].itemType == ItemType.attack)//아이템의 타입으로 방어구면 방어구 메서드 공격이면 공격 메서드
                {
                    EquipA(index, ref player); //공격 관련 장착 해제
                }
                else
                {
                    EquipD(index, ref player); //방어 관련 장착 해제
                }
            }
            public void EquipA(int index, ref Player player)
            {
                if (equipAIndex == -1) // 장착한것이 없다면
                {
                    items[index].EquipItem(ref player);
                    equipAIndex = index;
                }
                else //장착한 것이 있다면 자기 자신을 눌렀을 때와 다른 아이템을 눌렀을 때 (같은 공격아이템)
                {
                    if (equipAIndex == index)//현재 장착하고 있는 것을 선택하면
                    {
                        items[index].EquipItem(ref player); //장비 해제
                        equipAIndex = -1; //장비를 장착하지 않은 상태를 -1 로 표현
                    }
                    else
                    {
                        items[equipAIndex].EquipItem(ref player); //기존 장비 해제
                        items[index].EquipItem(ref player); //새로운 장비 장착
                        equipAIndex = index;
                    }
                }

            }
            public void EquipD(int index, ref Player player)
            {
                if (equipDIndex == -1) // 장착한것이 없다면
                {
                    items[index].EquipItem(ref player);
                    equipDIndex = index;
                }
                else //장착한 것이 있다면
                {
                    if (equipDIndex == index) //자기 자신이라면
                    {
                        items[index].EquipItem(ref player);
                        equipDIndex = -1;
                    }
                    else // 다른 아이템을 고르면
                    {
                        items[equipDIndex].EquipItem(ref player);//기존 아이템 해제
                        items[index].EquipItem(ref player); // 새로운 아이템 장착
                        equipDIndex = index;           //현재 장착 아이템 인덱스 저장
                    }
                }
            }
            public void DeleteItem(int index) //아이템 삭제
            {
                for (int i = index; i < itemCount; i++)
                {
                    items[i] = items[i + 1]; //한 칸씩 당기기
                }
                items[itemCount] = new Item(); //마지막 장비에 새로운 초기화 아이템 생성
                itemCount--; //현재 소지하고 있는 아이템 개수 하나 감소
            }
            public void AddItemPower(float power)
            {
                addPower += power;
            }
            public void SubItemPower(float power)
            {
                addPower -= power;
            }
            public void AddItemDefense(float defense)
            {
                addDefense += defense;
            }
            public void SubItemDefense(float defense)
            {
                addDefense -= defense;
            }
            public void PlayerState() //플레이어 상태창 관리
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("상태 보기");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine();

                Console.WriteLine($"Lv. {lv:D2}");  //D2로 자릿수 지정자 
                Console.WriteLine($"{name} ( {job} )");

                Console.Write($"공격력 : {power + addPower} ");
                if (addPower != 0)
                {
                    Console.WriteLine($"(+{addPower})");
                }
                else
                {
                    Console.WriteLine();
                }

                Console.Write($"방어력 : {defense + addDefense} ");
                if (addDefense != 0)
                {
                    Console.WriteLine($"(+{addDefense})");
                }
                else
                {
                    Console.WriteLine();
                }
                Console.WriteLine($"체 력 : {hp}");
                Console.WriteLine($"Gold : {gold} G");
                Console.WriteLine();

                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                int input = InputKey(0);
            }
            public void AddItem(Item item) //플레이어의 아이템 추가
            {
                items[itemCount++] = item;
            }
            public void ShowItems(bool isIn, ref Player player) // false면 인벤토리 화면 true 장착관리 화면
            {
                Console.WriteLine("[ 아이템 목록 ]");
                Console.WriteLine();

                if (itemCount == 0)
                {
                    Console.Clear();
                    Console.WriteLine("장착할 수 있는 아이템이 없습니다.");
                    Console.WriteLine();
                    return;
                }
                else
                {
                    for (int i = 0; i < itemCount; i++)
                    {
                        if (isIn == false)
                            Console.Write("- ");
                        else
                            Console.Write($"- {i + 1} ");
                        Console.Write($"{items[i].Name,-15}| "); //이름 출력
                        if (items[i].Power != 0 || items[i].Defense != 0)//공격력 또는 방어력이 있으면 출력
                        {
                            if (items[i].Power != 0)
                                Console.Write($"공격력 +{items[i].Power} ");
                            if (items[i].Defense != 0)
                                Console.Write($"방어력 +{items[i].Defense} ");
                            //if (items[i].Power == 0 || items[i].Defense == 0)
                            //    Console.Write($"{"",-10}");
                            Console.Write("| ");
                        }
                        else
                        {
                            Console.Write($"{"",-20}| ");
                        }
                        Console.WriteLine($"{items[i].Explanation,-20} ");//설명 출력
                    }
                }
                Console.WriteLine();
                int input;
                if (isIn == false)
                {
                    Console.WriteLine("1. 장착 관리");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    input = InputKey(1);
                }
                else
                {
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    input = InputKey(player.itemCount);
                }

                if (isIn == false)
                {
                    switch (input)
                    {
                        case 1:
                            PlayerInventory(true, ref player);
                            break;
                        case 0:
                            break;
                        default:
                            break;
                    }
                }
                else //장착 관리에 들어왔을 때
                {
                    input--;
                    if (0 <= input && input < itemCount) //아이템을 선택했을 때
                    {
                        EquipItem(input, ref player);
                        Console.Clear();
                        //ShowItems(false, ref player);
                        PlayerInventory(false, ref player);

                    }
                    else if (input == -1) //나가기 눌렀을 때
                    {
                        Console.Clear();
                        //ShowItems(false, ref player);
                        PlayerInventory(false, ref player);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다");
                        Console.WriteLine();
                        //ShowItems(true, ref player);
                        PlayerInventory(true, ref player);
                        return;
                    }
                }
            }
            public void PlayerInventory(bool isIn ,ref Player player) //플레이어 인벤토리 관리
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("인벤토리");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();

                ShowItems(isIn, ref player);// 아이템 목록을 보여줌 
            }
        }
        public enum GameState
        {
            state = 1,
            inventory = 2,
            shop = 3,
            dungeon = 4,
            rest = 5
        }
        public enum ItemType
        {
            attack = 0,
            defense = 1
        }
        static public int InputKey(int range) //행동 입력 멘트후 입력과 int 형 반환
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">>");
            Console.ForegroundColor = ConsoleColor.White;
            int value;
            string str = Console.ReadLine();
            while ((!int.TryParse(str, out value)) || (value < 0 || value > range))
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.WriteLine("올바른 행동를 입력하세요.");
                Console.Write(">>");
                str = Console.ReadLine();
            }
            int input = int.Parse(str);
            Console.Clear();
            return input;
        }

    }
}
