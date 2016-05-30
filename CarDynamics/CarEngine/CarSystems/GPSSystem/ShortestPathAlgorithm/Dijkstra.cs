using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace CarDynamics
{
    public class Dijkstra
    {
        private int rank = 0;
        private int[,] L;
        private int[] C;
        public int[] D;
        public List<Icon>[] ArrOfBestRoads;
        private int IndexOfSource;

        public Dijkstra(int IndexOfSource, int paramRank, int[,] paramArray)
        {
            this.IndexOfSource = IndexOfSource;
            ArrOfBestRoads = new List<Icon>[paramRank];
            for (int i = 0; i < ArrOfBestRoads.Length; i++)
            {
                ArrOfBestRoads[i] = new List<Icon>();
            }
            L = new int[paramRank, paramRank];
            C = new int[paramRank];
            D = new int[paramRank];
            rank = paramRank;
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < rank; j++)
                {
                    L[i, j] = paramArray[i, j];
                }
            }
            for (int i = 0; i < rank; i++)
            {
                C[i] = i;
            }
            C[0] = IndexOfSource;
            for (int i = 0; i < rank; i++)
            {
                D[i] = L[IndexOfSource, i];
                if (D[i] > -1)
                {
                    ArrOfBestRoads[i].Add(new Icon(IndexOfSource, i));
                }
            }
        }

        public void SolveDijkstra()
        {
            int minValue = Int32.MaxValue;
            int minNode = 0;
            int FlaggedIndexInD = 0;
            for (int i = 0; i < rank; i++)
            {
                if (C[i] == -1)
                {
                    continue;
                }
                if (D[i] > 0 && D[i] < minValue)
                {
                    minValue = D[i];
                    minNode = i;
                    FlaggedIndexInD = i;
                }
            }

            C[minNode] = -1;
            for (int i = 0; i < rank; i++)
            {
                if (L[minNode, i] < 0)
                {
                    continue;
                }
                if (D[i] < 0)
                {
                    D[i] = minValue + L[minNode, i];
                    ArrOfBestRoads[i].Clear();
                    ArrOfBestRoads[i].Add(new Icon(IndexOfSource, FlaggedIndexInD));
                    ArrOfBestRoads[i].Add(new Icon(minNode, i));
                    continue;
                }
                if ((D[minNode] + L[minNode, i]) < D[i])
                {
                    D[i] = minValue + L[minNode, i];
                    ArrOfBestRoads[i].Clear();
                    ArrOfBestRoads[i].Add(new Icon(IndexOfSource, FlaggedIndexInD));
                    ArrOfBestRoads[i].Add(new Icon(minNode, i));
                }
            }
        }

        public bool AlreadyInList(int OuterLoop, int j, int Counter)
        {
            for (int FlagCounter = 0; FlagCounter < ArrOfBestRoads[OuterLoop].Count; FlagCounter++)
            {
                if ((ArrOfBestRoads[OuterLoop][FlagCounter].i == ArrOfBestRoads[j][Counter].i)
                    && (ArrOfBestRoads[OuterLoop][FlagCounter].j == ArrOfBestRoads[j][Counter].j))
                {
                    return true;
                }
            }
            return false;
        }

        int CompareIcons(Icon obj1, Icon obj2)
        {
            if (obj1.i == obj2.j)
                return +1;
            else if (obj1.i < obj2.i)
                return -1;
            else
                return 0;
        }

        public void GenerateRoads()
        {
            for (int OuterLoop = 0; OuterLoop < ArrOfBestRoads.Length; OuterLoop++)
            {
                for (int InnerLoop = 0; InnerLoop < ArrOfBestRoads[OuterLoop].Count; InnerLoop++)
                {
                    int i = ArrOfBestRoads[OuterLoop][InnerLoop].i;
                    int j = ArrOfBestRoads[OuterLoop][InnerLoop].j;
                    if (j != OuterLoop)
                    {
                        ArrOfBestRoads[OuterLoop].RemoveAt(InnerLoop);
                        for (int Counter = 0; Counter < ArrOfBestRoads[j].Count; Counter++)
                        {
                            if (!AlreadyInList(OuterLoop, j, Counter))
                            {
                                ArrOfBestRoads[OuterLoop].Add(ArrOfBestRoads[j][Counter]);
                            }
                        }
                    }
                    ArrOfBestRoads[OuterLoop].Sort(CompareIcons);
                }
            }
        }

        public void RunDijkstra()
        {
            for (int trank = 1; trank < rank; trank++)
            {
                SolveDijkstra();
            }
            GenerateRoads();
        }
    }
}
