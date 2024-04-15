using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace KP_new
{
    class Program
    {

        static void Main(string[] args)
        {
            Random rnd = new Random();
            int[] numbers = { 0, 0, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6 };
            int[] Player = new int[7];
            int[] PlayerAI = new int[7];
            int[] Board = new int[0];
            int[] Bazar = new int[21];
            int[] playerNOT = new int[0];

            for (int i = 0; i < 7; i++)
            {
                int ind = rnd.Next(numbers.Length);
                Player[i] = numbers[ind];
                numbers = numbers.Where((val, idx) => idx != ind).ToArray();

                ind = rnd.Next(numbers.Length);
                PlayerAI[i] = numbers[ind];
                numbers = numbers.Where((val, idx) => idx != ind).ToArray();
            }

            Array.Copy(numbers, Bazar, numbers.Length);

            bool playerTurn = true;

            int passPL = 0, passAI = 0;

            while (Player.Count() > 0 && PlayerAI.Count() > 0)
            {
                if (playerTurn)   //People
                {
                    if (passAI == 6)
                    {
                        break;
                    }
                    Console.WriteLine(string.Join(",", playerNOT));
                    Console.WriteLine("Доска:");
                    Console.WriteLine(string.Join(" -> ", Board));
                    Console.WriteLine("Ваша рука:");
                    for (int i = 0; i < Player.Count(); i++)
                    {
                        Console.WriteLine(((i + 1) + ") " + Player[i]));
                    }
                    Console.WriteLine("Введите 0-для взятия из базара или индекс элемента для игры");
                    string input = Console.ReadLine();
                    int index;

                    while (!int.TryParse(input, out index) || (index != 0 && (index < 1 || index > Player.Count())))
                    {
                        Console.WriteLine("Введите корректное значение!");
                        input = Console.ReadLine();
                    }
                    if (index == 0)
                    {
                        if (Bazar.Length == 0)
                        {
                            Console.WriteLine("Базар пуст! Пропуск хода.");
                            int lastBone = Board[Board.Length - 1];
                            Array.Resize(ref playerNOT, playerNOT.Length + 3);
                            playerNOT[playerNOT.Length - 1] = lastBone;
                            playerNOT[playerNOT.Length - 2] = lastBone + 1;
                            playerNOT[playerNOT.Length - 3] = lastBone - 1;
                            passPL++;
                            playerTurn = !playerTurn;
                        }
                        else
                        {
                            int randomIndex = rnd.Next(Bazar.Length);
                            int newBone = Bazar[randomIndex];
                            int lastBone;
                            if (Board.Count() == 0)
                            {
                                lastBone = newBone;
                            }
                            else
                            {
                                lastBone = Board[Board.Length - 1];
                            }
                            for (int i = randomIndex; i < Bazar.Length - 1; i++)
                            {
                                Bazar[i] = Bazar[i + 1];

                            }
                            Array.Resize(ref Bazar, Bazar.Length - 1);
                            if (newBone == lastBone || newBone == lastBone - 1 || newBone == lastBone + 1)
                            {
                                Array.Resize(ref Board, Board.Length + 1);
                                Board[Board.Length - 1] = newBone;
                                Console.WriteLine("Костяшка из базара была выложена на доску игроком");
                                if (passPL > 0)
                                {
                                    passPL--;
                                }
                                playerTurn = !playerTurn;
                            }
                            else
                            {
                                Array.Resize(ref Player, Player.Length + 1);
                                Player[Player.Length - 1] = newBone;
                                Console.WriteLine("Пропуск хода игроком");
                                passPL++;
                                playerTurn = !playerTurn;
                            }
                            Array.Resize(ref playerNOT, playerNOT.Length + 3);
                            playerNOT[playerNOT.Length - 1] = lastBone;
                            playerNOT[playerNOT.Length - 2] = lastBone + 1;
                            playerNOT[playerNOT.Length - 3] = lastBone - 1;
                        }
                    }
                    else
                    {
                        int BoneToPlay = Player[index - 1];
                        if (Board.Count() == 0)
                        {
                            Array.Resize(ref Board, Board.Length + 1);
                            Board[Board.Length - 1] = BoneToPlay;
                            int[] newPlayer = new int[Player.Length - 1];
                            index--;
                            Array.Copy(Player, 0, newPlayer, 0, index);
                            Array.Copy(Player, index + 1, newPlayer, index, Player.Length - index - 1);
                            index++;
                            Player = newPlayer;
                            if (passPL > 0)
                            {
                                passPL--;
                            }
                            playerTurn = !playerTurn;
                        }
                        else
                        {
                            int lastBone = Board[Board.Length - 1];
                            if (BoneToPlay == lastBone || BoneToPlay == lastBone + 1 || BoneToPlay == lastBone - 1)
                            {
                                Array.Resize(ref Board, Board.Length + 1);
                                Board[Board.Length - 1] = BoneToPlay;
                                int[] newPlayer = new int[Player.Length - 1];
                                index--;
                                Array.Copy(Player, 0, newPlayer, 0, index);
                                Array.Copy(Player, index + 1, newPlayer, index, Player.Length - index - 1);
                                index++;
                                Player = newPlayer;
                                if (passPL > 0)
                                {
                                    passPL--;
                                }
                                playerTurn = !playerTurn;
                            }
                            else
                            {
                                Console.WriteLine("Не верный ход!");
                            }
                        }
                    }
                }
                else //AI 
                {
                    if (passPL == 6)
                    {
                        break;
                    }
                    Console.WriteLine("Доска:");
                    Console.WriteLine(string.Join(" -> ", Board));
                    Console.WriteLine("Ход Компьютера");
                    Thread.Sleep(1000);//задержка
                    if (Board.Count() == 0) //Если на доске ничего нету, то кидаем рандомную с руки
                    {
                        int randomIndex = rnd.Next(PlayerAI.Length);
                        int newBoneAI = PlayerAI[randomIndex];
                        Array.Resize(ref Board, Board.Length + 1);
                        Board[Board.Length - 1] = newBoneAI;
                        int[] newPlayerAI = new int[PlayerAI.Length - 1];
                        randomIndex--;
                        Array.Copy(PlayerAI, 0, newPlayerAI, 0, randomIndex);
                        Array.Copy(PlayerAI, randomIndex + 1, newPlayerAI, randomIndex, PlayerAI.Length - randomIndex - 1);
                        randomIndex++;
                        PlayerAI = newPlayerAI;
                        if (passAI > 0)
                        {
                            passAI--;
                        }
                        playerTurn = !playerTurn;
                    }
                    else //INTEL
                    {
                        int lastBONE = Board[Board.Length - 1];

                        int notBONE = -1;

                        for (int i = 0; i < PlayerAI.Length; i++)//сравнение чисел как с плеернот, если совпадают, то кидаем совпавшее, иначе выбираем по частоте
                        {
                            int num = PlayerAI[i];
                            if (Array.IndexOf(playerNOT, num) > -1)
                            {
                                notBONE = num;
                                break;
                            }
                        }

                        Dictionary<int, int> numCounts = new Dictionary<int, int>();//выбор числа по частоте

                        for (int i = 0; i < PlayerAI.Length; i++)
                        {
                            int num = PlayerAI[i];

                            if (numCounts.ContainsKey(num))
                            {
                                numCounts[num]++;
                            }
                            else
                            {
                                numCounts[num] = 0;
                            }
                        }

                        int maxNumCount = 0;
                        int maxNum = 0;

                        for (int i = 0; i < numCounts.Count; i++)
                        {
                            int num = numCounts.Keys.ElementAt(i);
                            int count = numCounts.Values.ElementAt(i);

                            if (num == lastBONE - 1 || num == lastBONE || num == lastBONE + 1)
                            {
                                if (count > maxNumCount)
                                {
                                    maxNumCount = count;
                                    maxNum = num;
                                }
                            }
                        }
                        if (maxNumCount > 0)
                        {
                            int index = 0;
                            Array.Resize(ref Board, Board.Length + 1);
                            if (notBONE != -1 && (notBONE == lastBONE || notBONE == lastBONE - 1 || notBONE == lastBONE + 1))//если подходит по условиям, то кидаем то число, которое есть в плеернот, иначе кидаем число выбранное по макс. встречаемости
                            {
                                Board[Board.Length - 1] = notBONE;

                                for (int i = 0; i < PlayerAI.Length - 1; i++)
                                {
                                    if (PlayerAI[i] == notBONE)
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Board[Board.Length - 1] = maxNum;

                                for (int i = 0; i < PlayerAI.Length - 1; i++)
                                {
                                    if (PlayerAI[i] == maxNum)
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                            }

                            int[] newPlayerAI = new int[PlayerAI.Length - 1];
                            Array.Copy(PlayerAI, 0, newPlayerAI, 0, index);
                            Array.Copy(PlayerAI, index + 1, newPlayerAI, index, PlayerAI.Length - index - 1);
                            PlayerAI = newPlayerAI;
                            if (passAI > 0)
                            {
                                passAI--;
                            }
                            playerTurn = !playerTurn;
                        }
                        else
                        {
                            if (Bazar.Length == 0)
                            {
                                Console.WriteLine("Базар пуст! Пропуск хода.");
                                passAI++;
                                playerTurn = !playerTurn;
                            }
                            else
                            {
                                int randomIndexAI = rnd.Next(Bazar.Length);
                                int newBONE = Bazar[randomIndexAI];
                                if (Board.Count() == 0)
                                {
                                    lastBONE = newBONE;
                                }
                                else
                                {
                                    lastBONE = Board[Board.Length - 1];
                                }
                                for (int k = randomIndexAI; k < Bazar.Length - 1; k++)
                                {
                                    Bazar[k] = Bazar[k + 1];
                                }
                                Array.Resize(ref Bazar, Bazar.Length - 1);
                                if (newBONE == lastBONE || newBONE == lastBONE - 1 || newBONE == lastBONE + 1)
                                {
                                    Array.Resize(ref Board, Board.Length + 1);
                                    Board[Board.Length - 1] = newBONE;
                                    Console.WriteLine("Костяшка из базара была выложена на доску компьютером");
                                    if (passAI > 0)
                                    {
                                        passAI--;
                                    }
                                    playerTurn = !playerTurn;
                                }
                                else
                                {
                                    Array.Resize(ref PlayerAI, PlayerAI.Length + 1);
                                    PlayerAI[PlayerAI.Length - 1] = newBONE;
                                    Console.WriteLine("Пропуск хода компьютером");
                                    passAI++;
                                    playerTurn = !playerTurn;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Игра окончена!");
            if (PlayerAI.Length == 0)
            {
                Console.WriteLine("Победил компьютер!");
                Console.ReadLine();
            }
            else if (Player.Length == 0)
            {
                Console.WriteLine("Вы победили!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Ничья!");
                Console.ReadLine();
            }

        }
    }
}
