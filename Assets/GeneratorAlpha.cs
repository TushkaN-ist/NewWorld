using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GenerateTest01
{
    public class Island : IComparable<Island>
    {
        public static bool SortWithName = false;
        string type = null;
        string name = null;
        List<Island> childrens;//6 output
        Island parent;
        float weight;
        bool destroy=false;
        public float x,y;
        
        public Island()
        {
            x = y = 0;
        }
        public List<Island> Childrens
        {
            get
            {
                return childrens;
            }
        }
        public bool Destroy
        {
            get
            {
                return destroy;
            }
        }
        public float Weight
        {
            get
            {
                return weight;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
        }
        public static Island Init(string Type, string Name="NULL")
        {
            Island isl = new Island();
            isl.name = Name;
            isl.type = Type;
            isl.childrens = new List<Island>();
            isl.weight = 2.5f + (float)GeneratorAlpha.rand.Next(750) / 100f;
            isl.x = isl.y = 0;
            return isl;
        }
        public static Island Init(Island from)
        {
            Island isl = new Island();
            isl.name = from.name;
            isl.type = from.type;
            isl.childrens = new List<Island>();
            isl.weight = 2.5f + (float)GeneratorAlpha.rand.Next(750) / 100f;
            isl.x = from.x; isl.y = from.y;
            return isl;
        }
        public void SetPosition(float X, float Y)
        {
            x = X; y = Y;
        }
        public bool Connect(Island isl)
        {
            if (childrens.Contains(isl)||this==isl)
            {
                return false;
            }
            else
            {
                childrens.Add(isl);
                isl.parent = this;
                return true;
            }
        }
        public void Connect(params Island[] isls)
        {
            foreach (Island isl in isls)
            {
                if (childrens.Contains(isl) || this == isl)
                {
                    Console.WriteLine("OPS!");
                    continue;
                }
                else
                {
                    childrens.Add(isl);
                    isl.parent = this;
                }
            }
        }
        public bool Devourment(Island isl,ref List<Island> isls)
        {
            if (!destroy && !isl.destroy && !this.Equals(isl) && type.Equals(isl.type))
            {
                if (GetDist(isl) < weight / 10)
                {
                    weight += isl.weight;
                    isl.destroy = true;
                    return true;
                }
                else
                {
                    float t = Math.Max(0, (weight.ToString("0.0").Equals(isl.weight.ToString("0.0")) ? weight*2 : 0) + isl.weight - GetDist(isl));
                    x += ((isl.x - x) / (weight/4)) * t;
                    y += ((isl.y - y) / (weight/4)) * t;
                    return false;
                }
            }
            else
            {
                float t = Math.Max(0, isl.weight - GetDist(isl))/10f;
                x -= ((isl.x - x) / (weight)) * t;
                y -= ((isl.y - y) / (weight)) * t;
                return false;
            }
        }
        public int CompareTo(Island B)
        {
            if (this == B)
                return 0;
            if (destroy)
                return 1;
            else
            {
                if (B.destroy)
                    return -1;
            }
            int R = SortWithName?B.type.CompareTo(type):0;
            return R == 0 ? B.weight.CompareTo(weight) : R;
        }

        float GetDist(Island B)
        {
            float X=B.x-x;
            float Y=B.y-y;
            return (float)Math.Sqrt(X * X + Y * Y);
        }
        public override string ToString()
        {
            return Type+":"+Name+":"+Weight;
        }
    }

    class GeneratorAlpha
    {
        public static Random rand = new Random((int)DateTime.Now.Ticks);
        List<Island> islandes;
        List<Island> islPrefubs;
        int sW=100, sH=100;
        float Scale=10;

        public int IslandsCount
        {
            get
            {
                return islandes.Count;
            }
        }

        public void Init()
        {
            islandes = new List<Island>();
            GeneratePref();
            //Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Инициализирован Генератор");
        }
        public void SetWorldSize(int W, int H)
        {
            sW = W; sH = H;
        }
        void GeneratePref()
        {
            islPrefubs = new List<Island>();
            islPrefubs.Add(Island.Init("Dust"));
            islPrefubs.Add(Island.Init("Tropics"));
            islPrefubs.Add(Island.Init("Cave"));
        }
        public void GenerateStuf(int rMin,int rMax)
        {
            int randGen = rMin + rand.Next(rMax - rMin);
            Island rIsland = null;
            for (int r = 0; r < randGen; ++r)
            {
                rIsland = Island.Init(islPrefubs[rand.Next(islPrefubs.Count)]);
                rIsland.SetPosition((float)rand.Next((int)(sW * Scale)) / Scale, (float)rand.Next((int)(sH * Scale)) / Scale);
                islandes.Add(rIsland);
            }
            //Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\rСоздано:{0}\t", randGen);
        }

        public void LifeCicle(uint steps)
        {
            for (int step = 0; step < steps; ++step)
            {
                for (int i = 0; i < islandes.Count; ++i)
                {
                    for (int i2 = 0; i2 < islandes.Count; ++i2)
                    {
                        islandes[i].Devourment(islandes[i2], ref islandes);
                    }
                }
            }
        }
        string[] Loading = { "\\", "|", "/", "-" };
        uint loadAnim = 0;
        public void DropTrash(float percentDrop)
        {
            islandes.Sort();

            int countRemove = 0;
            foreach (Island isl in islandes)
            {
                if (isl.Destroy)
                    break;
                ++countRemove;
            }
            islandes.RemoveRange(countRemove, islandes.Count-countRemove);

            countRemove = (int)(islandes.Count * (percentDrop / 100f));
            islandes.RemoveRange(islandes.Count - countRemove, countRemove);
            //Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Утилизировано:{0}\t", countRemove);
            //Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Loading[{0}]", Loading[loadAnim]);
            loadAnim=++loadAnim%4;
        }

        public void FinalStade()
        {
            Island.SortWithName = true;
            islandes.Sort();
            //Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nЗавершаем с числом миров: {0}", islandes.Count);
            //Console.ForegroundColor = ConsoleColor.Gray;
            string Types = "";
            int id = 0;
            foreach (Island isl in islandes)
            {
                if (!Types.Equals(isl.Type))
                {
                    Types = isl.Type; 
                    switch(Types){
                        case "Cave":
                            id = 0;
                            //Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case "Tropics":
                            id = 0;
                            //Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case "Dust":
                            id = 0;
                            //Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                    }
                }
                
                Console.WriteLine("{2}:Мир:{0}\tВес:{1}", isl.Type, isl.Weight.ToString("0.00"), ++id);
            }
        }

        public List<Island> LinkedStade(float sticksAround)
        {
            Dictionary<string, List<Island>> Links = new Dictionary<string, List<Island>>();
            List<Island> listLink = null;
            string typeLast = null;
            foreach(Island islnd in islandes){
                if (Links.ContainsKey(islnd.Type))
                {
                    listLink=Links[islnd.Type];
                    typeLast = islnd.Type;
                }else{
                    listLink=new List<Island>();
                    Links.Add(islnd.Type, listLink);
                    typeLast = islnd.Type;
                }
                //Console.WriteLine(typeLast + ":" + islnd.ToString());
                listLink.Add(islnd);
            }
            Island Next = null, Last = null, Temp=null;
            List<Island> Tails = new List<Island>();
            List<Island> Collector = new List<Island>();
            List<Island> FirstIsland = new List<Island>();
            foreach (KeyValuePair<string, List<Island>> lnk in Links)
            {
                listLink = lnk.Value;
                Console.WriteLine(lnk.Key);
                uint links = 0,Sticks=0;
                Last = null;
                Next = null;
                Tails.Clear();
                foreach (Island islnd in listLink)
                {
                    Next = islnd;
                    if (Last==null)
                    {
                        FirstIsland.Add(Next);
                        Tails.Add(Next);
                        ++links;
                    }
                    else
                    {
                        Collector.Add(Last);
                        if (!Last.Type.Contains(Next.Type))
                        {
                            Console.WriteLine("WARNING!");
                        }
                        if (Last.Weight >= Next.Weight - sticksAround && Last.Weight <= Next.Weight + sticksAround)
                        {
                            Last = Next;
                            continue;
                        }
                        Temp = Tails[rand.Next(Tails.Count)];
                        Temp.Connect(Collector.ToArray());
                        Tails.Remove(Temp);
                        Tails.AddRange(Collector);
                        if (Collector.Count > 1)
                            Sticks += (uint)Collector.Count - 1;
                        Collector.Clear();
                        ++links;
                    }
                    Last = Next;
                }
                Console.WriteLine("Links:{0} Sticks:{1}", links,Sticks);
            }
            return FirstIsland;
        }
    }
}
