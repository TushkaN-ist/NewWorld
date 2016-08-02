using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateTest01
{
    class Program
    {
        GeneratorAlpha generator = new GeneratorAlpha();
        public float Progress = 0;
        public List<Island> islandsOut;
        void GenerateStade1(string worldsize, uint islandMin, uint lifeCicles, float dropOnTheTrash)
        {
            generator = new GeneratorAlpha();
            Island.SortWithName = false;
            generator.Init();
            //Console.ForegroundColor = ConsoleColor.Gray;
            string[] split = worldsize.Split('x');
            if (split.Length == 2)
            {
                int w = 100, h = 100;
                if (int.TryParse(split[0], out w) && int.TryParse(split[1], out h))
                {
                    generator.SetWorldSize(w, h);
                }
                else
                {
                    Console.WriteLine("Не корректные данные! использован стандартный размер 100x100");
                }
            }
            else
                Console.WriteLine("Не корректный формат! использован стандартный размер 100x100");
            Console.Write("Минимум островов:{0}",islandMin);
            Console.Write("Циклов сливание:{0}", lifeCicles);
            Console.Write("Процент мусора:{0}", dropOnTheTrash);
            int c, cM;
            DateTime dn = DateTime.Now;
            TimeSpan ts = TimeSpan.Zero;
            int islandNow = 0;
            for (c = 0, cM = 0; generator.IslandsCount < islandMin; ++c)
            {
                generator.GenerateStuf((int)(islandMin * (dropOnTheTrash / 100f)), (int)(islandMin * 0.75f));
                generator.LifeCicle(lifeCicles);
                generator.DropTrash(dropOnTheTrash);
                ts = DateTime.Now - dn;
                if (islandNow < generator.IslandsCount)
                {
                    islandNow = generator.IslandsCount;
                    cM = c;
                    Progress = ((float)islandNow / islandMin);
                    //form.SetProgress(Progress);
                    Console.Write(":{0}%\t", Progress * 100f);
                }
                if (c > cM + islandMin)
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nИтерация зависла и принудительно сброшена");
                    break;
                }
            }
            generator.FinalStade();
            //Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Прошло генераций:{0}\nПрошло времени:{1}\nСозданно островов:{2}", c, ts.ToString(), generator.IslandsCount);
            //Console.Read();
        }

        static Boolean _Run=true;

        public static void Quit()
        {
            _Run = false;
        }
        public void GenerateRun(string worldsize, int islandMin, int lifeCicles, float dropOnTheTrash,float arroundStick)
        {
            GenerateStade1(worldsize, (uint)islandMin, (uint)lifeCicles, dropOnTheTrash);
            islandsOut=generator.LinkedStade(arroundStick);
        }
    }
}
