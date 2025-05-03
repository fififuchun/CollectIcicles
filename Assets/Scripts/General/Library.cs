using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// 便利な関数集
namespace FuchunLibrary
{
    public static class Library
    {
        /// <summary>
        /// int配列の中で一番初めのnumberのindexを返します
        /// </summary>
        /// <param name="ints"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int SearchNumberIndex(int[] ints, int number)
        {
            for (int i = 0; i < ints.Length; i++) if (ints[i] == number) return i;
            return -1;
        }

        /// <summary>
        /// bool配列の一番最初に現れたfalseの位置を返します、全部trueだったら-1を返します
        /// </summary>
        /// <param name="bools"></param>
        /// <returns></returns>
        public static int FirstFalseIndex(bool[] bools)
        {
            for (int i = 0; i < bools.Length; i++) if (!bools[i]) return i;
            return -1;
        }

        /// <summary>
        /// 特性関数、bool配列に対してTrueの数を返します
        /// </summary>
        /// <param name="bools"></param>
        /// <returns></returns>
        public static int CharacteristicFanction(bool[] bools)
        {
            int count = 0;
            for (int i = 0; i < bools.Length; i++) if (bools[i]) count++;
            return count;
        }

        /// <summary>
        /// 可変長配列の最小値を返す
        /// </summary>
        /// <param name="numbers">int配列</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int Min(params int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                throw new ArgumentException("Require at least one argument");

            int min = numbers[0];
            foreach (int num in numbers)
            {
                if (num < min)
                {
                    min = num;
                }
            }
            return min;
        }

        /// <summary>
        /// 整数の下二桁を二文字のstring型で返す
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string LastTwoDigits(int a)
        {
            int reminder = a % 100;

            if (a > 9) return reminder.ToString();
            else if (a >= 0) return "0" + reminder.ToString();
            else return "";
        }

        /// <summary>
        /// プリミティブな二次元配列を一次元に変換する
        /// </summary>
        /// <param name="grid">プリミティブな二次元配列</param>
        /// <returns>一次元</returns>
        public static T[] ConvertPrimDimTwoToOne<T>(T[,] grid)
        {
            int ymax = grid.GetLength(0);
            int xmax = grid.GetLength(1);
            int len = xmax * ymax;
            T[] array = new T[len];

            int size = Marshal.SizeOf(typeof(T));
            int gridOffset = 0;
            int arrayOffset = 0;

            Buffer.BlockCopy(grid, gridOffset, array, arrayOffset, len * size);
            return array;
        }

        /// <summary>
        /// 二次元配列を一次元に変換する
        /// </summary>
        /// <param name="grid">二次元配列</param>
        /// <returns>一次元</returns>
        public static T[] ConvertDimTwoToOne<T>(T[,] grid)
        {
            int ymax = grid.GetLength(0);
            int xmax = grid.GetLength(1);
            int len = xmax * ymax;
            T[] array = new T[len];

            for (int y = 0, i = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++, i++)
                {
                    array[i] = grid[y, x];
                }
            }
            return array;
        }

        /// <summary>
        /// プリミティブな一次元配列を二次元に変換する、返す配列の要素が多い場合は初期値を、足りない場合はgridのラストから切り捨てる
        /// </summary>
        /// <typeparam name="T">好きな型</typeparam>
        /// <param name="array">変換したい一次元配列</param>
        /// <param name="height">縦</param>
        /// <param name="width">横</param>
        /// <returns>二次元配列</returns>
        public static T[,] ConvertPrimDimOneToTwo<T>(T[] array, int height, int width)
        {
            var grid = new T[height, width];
            int len = width * height;
            // 引数の一次元配列の要素数が、変換したい要素数に満たない場合、少ない方に合わせる
            len = array.Length < len ? array.Length : len;
            int gridOffset = 0;
            int arrayOffset = 0;

            int size = Marshal.SizeOf(typeof(T));
            Buffer.BlockCopy(array, arrayOffset, grid, gridOffset, len * size);
            return grid;
        }

        /// <summary>
        /// 一次元配列を二次元に変換する
        /// </summary>
        /// <typeparam name="T">好きな型</typeparam>
        /// <param name="array">変換したい一次元配列</param>
        /// <param name="height">縦</param>
        /// <param name="width">横</param>
        /// <returns>二次元配列</returns>
        public static T[,] ConvertDimOneToTwo<T>(T[] array, int height, int width)
        {
            var grid = new T[height, width];
            int len = width * height;

            // lenが大きい場合には、初期値を代入
            // lenが小さい場合には、

            if (array.Length > len)
                throw new ArgumentException("変換したい一次元配列の要素が多すぎます");

            for (int i = 0; i < array.Length; i++)
            {
                int y = i / width;
                int x = i % width;
                grid[y, x] = array[i];
            }
            return grid;
        }

        // 上の関数の使用例
        // var one = Library.ConvertDimTwoToOne<int>(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });
        // Debug.Log(String.Join(",", one));
        // var two = Library.ConvertDimOneToTwo<int>(one, 2, 3);
        // Library.Print2DArray(two);

        /// <summary>
        /// 二次元配列をログにタブ区切りで表示する
        /// </summary>
        /// <param name="array">表示したい二次元配列</param>
        public static void Print2DArray<T>(T[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            string outputText = "";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    outputText += array[i, j] + "\t";
                }
                outputText += "\n";
            }

            Debug.Log(outputText);
        }

        /// <summary>
        /// boolの二次元配列をログにタブ区切りで表示する
        /// </summary>
        /// <param name="array">表示したい二次元配列</param>
        public static void Print2DBoolArray(bool[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            string outputText = "";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (array[i, j]) outputText += "T ";
                    else outputText += "F ";
                }
                outputText += "\n";
            }

            Debug.Log(outputText);
        }
    }
}

