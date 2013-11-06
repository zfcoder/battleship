using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace BattleShip
{
    public interface IShip
    {
        bool GeneratePosition();  // функция задания кораблю нового местоположения
    }

    public class Ship : IShip  //базовый класс корабля
    {
        protected Random random;
        protected int size; //размер корабля
        bool[,] area; //поле 10х10

        public Ship(bool[,] area)
        {
            this.area = area;
            random = new Random();
        }

        public bool GeneratePosition()
        {
            ArrayList combination = new ArrayList();
            for (int j = 0; j < 10; j++) //вычисление всех возможных комбинаций положения нового корабля
            {
                for (int k = 0; k < 10; k++)
                {
                    int gor = 0; //положение (1 - горизантально, 0 - вертикально)
                    int x = j;
                    int y = k;

                    if (Check(area, gor, x, y)) // если комбинация возможна, 
                    {
                        int[] comb = new int[3];
                        comb[0] = x;
                        comb[1] = y;
                        comb[2] = gor;
                        combination.Add(comb); // вносим ее в список возможных комбинаций
                    }

                    gor = 1;

                    if (Check(area, gor, x, y)) //то же самое для вертикального расположения
                    {
                        int[] comb = new int[3];
                        comb[0] = x;
                        comb[1] = y;
                        comb[2] = gor;
                        combination.Add(comb);
                    }
                }
            }
            if (combination.Count == 0)
            {
                Console.WriteLine("Not is" + size);
                return false;
            }
            int rand_comb = random.Next(combination.Count);  //получаем случайное число

            IEnumerator inum = combination.GetEnumerator();
            for (int i = 0; i < rand_comb; i++)
            {
                inum.MoveNext();
            }

            int[] array = (int[])inum.Current; //по этому числу выбираем случайную комбинацию

            if (array[2] == 1) //устанавливаем горизонтальный корабль
            {
                for (int i = 0; i < size; i++)
                {
                    area[array[0] + i, array[1]] = true;
                }
                return true;
            }
            else //устанавливаем вертикальный корабль
            {
                for (int i = 0; i < size; i++)
                {
                    area[array[0], array[1] + i] = true;
                }
                return true;
            }
        }

        protected bool Check(bool[,] area, int gor, int x, int y)  //функция проверки возможности установки корабля(возвращает true, если возможно)
        {
            if (gor == 1) //если гориз-ый
            {
                bool flag = true; //флаг
                for (int i = 0; i < size; i++)
                {
                    if (x + i < 0 || x + i > 9) //проверяем границы
                    {
                        flag = false;
                        break;
                    }
                    if (Neighbor(area, x + i, y) || area[x + i, y]) //проверяем соседей
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    return true;
                }
            }
            else //то же самое для вертикаольного кор-я
            {
                bool flag = true;
                for (int i = 0; i < size; i++)
                {
                    if (y + i < 0 || y + i > 9)
                    {
                        flag = false;
                        break;
                    }
                    if (Neighbor(area, x, y + i) || area[x, y + i])
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool Neighbor(bool[,] area, int x, int y) //функция проверки на наличие соседей клетки(возвращает true если есть хотя бы один сосед)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i < 0 || x + i > 9 || y + j < 0 || y + j > 9) //проверяем границы поля
                    {
                        continue;
                    }
                    else
                        if (area[x + i, y + j]) //если хотя бы один сосед найден, возвращаем true
                            return true;
                }
            }
            return false;
        }
    }

    public class Ship1 : Ship //класс однопалубного корабля
    {
        public Ship1(bool[,] area)
            : base(area)
        {
            size = 1;
        }
    }

    public class Ship2 : Ship //класс двухпалубного корабля
    {
        public Ship2(bool[,] area)
            : base(area)
        {
            size = 2;
        }
    }

    public class Ship3 : Ship  //класс трехпалубного корабля
    {
        public Ship3(bool[,] area)
            : base(area)
        {
            size = 3;
        }
    }

    public class Ship4 : Ship //класс четырехпалубного корабля
    {
        public Ship4(bool[,] area)
            : base(area)
        {
            size = 4;
        }
    }

    public interface IShipFactory
    {
        void CreateShip1(int n);
        void CreateShip2(int n);
        void CreateShip3(int n);
        void CreateShip4(int n);
    }

    class ShipFactory : IShipFactory //класс, создающий новые корабли
    {
        ArrayList ships;
        bool[,] area;

        public ShipFactory(bool[,] area)
        {
            this.area = area;
            ships = new ArrayList();
        }

        public void CreateShip1(int n) //функция создания однопалубного корабля
        {
            for (int i = 0; i < n; i++)
            {
                IShip ship = new Ship1(area);
                ship.GeneratePosition();
                ships.Add(ship);
            }

        }

        public void CreateShip2(int n)  //функция создания двухпалубного корабля
        {
            for (int i = 0; i < n; i++)
            {
                IShip ship = new Ship2(area);
                ship.GeneratePosition();
                ships.Add(ship);
            }
        }

        public void CreateShip3(int n)  //функция создания трехпалубного корабля
        {
            for (int i = 0; i < n; i++)
            {
                IShip ship = new Ship3(area);
                ship.GeneratePosition();
                ships.Add(ship);
            }
        }

        public void CreateShip4(int n)  //функция создания четырехпалубного корабля
        {
            for (int i = 0; i < n; i++)
            {
                IShip ship = new Ship4(area);
                ship.GeneratePosition();
                ships.Add(ship);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool[,] area = new bool[10, 10]; //поле 10х10

            IShipFactory factory = new ShipFactory(area); //генератор кораблей
            factory.CreateShip4(1);  //создаем корабли
            factory.CreateShip3(2);
            factory.CreateShip2(3);
            factory.CreateShip1(4);
            string[] lines = new string[10]; //выводимы строки

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (area[i, j])
                    {
                        lines[i] += " 1";
                    }
                    else
                    {
                        lines[i] += " 0";
                    }
                }
            }
            System.IO.File.WriteAllLines("WriteLines.txt", lines); //запись в файл WriteLines.txt
        }
    }
}
