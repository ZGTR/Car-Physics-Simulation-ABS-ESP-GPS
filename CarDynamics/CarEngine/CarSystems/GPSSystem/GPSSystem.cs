using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using CameraViewer;

namespace CarDynamics
{
    public class GPSSystem
    {
        # region GPSSystem DataMembers
        //private readonly int DistanceAmount = 600;
        private readonly int CloseEnoughToGPSPointAmount = 600;
        private readonly int CloseEnoughToClosestPointAmount = 600; // Same as CloseEnoughToGPSPointAmount
        private readonly int OptimizedVerticesMarchal = 1;
        private readonly float RoadScaleAmount = 50;
        private readonly int ConstRadius = 50;
        private GraphicsDevice graphicDevice;
        private ContentManager content;
        private BasicCamera camera;
        private Car car;
        private int GPSIndex;
        private System.Collections.Generic.List<RoadIcon> Road0Phase;
        private System.Collections.Generic.List<RoadIcon> Road1stPhase;
        private System.Collections.Generic.List<RoadIcon> Road2ndPhase;
        private System.Collections.Generic.List<RoadIcon> UniqueNodesList;
        private System.Collections.Generic.List<List<List<RoadIcon>>> RoadLayersList;
        public List<Icon>[] ListOfBestRoads;
        private List<List<RoadIcon>> LayersList;
        private List<RoadIcon> IntersectionList;
        private int NextTrackPointAngle;
        public bool GPSPointApproached;
        public bool ClosestTrackPointApproached;
        private bool FirstTimeInspecting;
        private GameTime gameTime;
        private bool LockedInTrack;
        public int LastGPSPointIndex = 0;
        public int LastClosestTrackPointIndex = 0;
        public Vector3 GPSPoint;
        public Vector3 ClosestTrackPoint;
        public int PreciseCircleRadius;
        private int IndexOfClosestPoint;
        private int RoadLayersCount = 1;
        private GPSRoad road;
        public int[,] GraphArr;
        public int MaxNodesNumber = 0;
        private int FirstIntersectionIndexIn1stLayer = -1;
        private int FirstIntersectionIndexIn2ndLayer = -1;
        private int LastIntersectionIndexIn1stLayer = -1;
        private int LastIntersectionIndexIn2ndLayer = -1;
        private Vector3 SourcePoint;
        public Vector3 DestinationPoint;
        private List<int> ListOfSequencedNodes;

        // static data members
        public static System.Collections.Generic.List<Object3D> Object3DList;
        public static System.Collections.Generic.List<Object2D> Object2DList;
        #endregion

        public GPSSystem(Game1 game1, Car car, GPSRoad road)
        {
            this.graphicDevice = game1.GraphicsDevice;
            this.content = game1.Content;
            this.camera = game1.GetCurrentCamera;
            this.car = car;
            this.road = road;
            LayersList = ConvertToRoadIconList(road.LayersList);
            RoadLayersCount = road.LayersList.Count;

            // Initialize data members
            GPSPoint = Vector3.Zero;
            ClosestTrackPoint = Vector3.Zero;
            SourcePoint = Vector3.Zero;
            GPSIndex = 0;


            GPSPointApproached = false;
            ClosestTrackPointApproached = false;
            PreciseCircleRadius = 0;
            IndexOfClosestPoint = 0;
            FirstTimeInspecting = true;
            LockedInTrack = false;
            Object3DList = new List<Object3D>();
            Object2DList = new List<Object2D>();
            IntersectionList = new List<RoadIcon>();
            Road0Phase = new System.Collections.Generic.List<RoadIcon>();
            Road1stPhase = new System.Collections.Generic.List<RoadIcon>();
            Road2ndPhase = new System.Collections.Generic.List<RoadIcon>();

            UniqueNodesList = new List<RoadIcon>();
            InitializeRoadLayersList();
            InitializeRoadPhases();

            //StructureOptimizeRoad0Phase1stdPhase();
            SetGraph();
            ListOfBestRoads = new List<Icon>[UniqueNodesList.Count];
        }

        private List<RoadIcon> ConvertToRoadIconList(List<Vector3> InComingList)
        {
            List<RoadIcon> ListToReturn = new List<RoadIcon>();
            for (int i = 0; i < InComingList.Count; i++)
            {
                ListToReturn.Add(new RoadIcon(InComingList[i]));
            }
            return ListToReturn;
        }

        private List<List<RoadIcon>> ConvertToRoadIconList(List<List<Vector3>> InComingList)
        {
            List<List<RoadIcon>> ListToReturn = new List<List<RoadIcon>>();
            //for (int i = 0; i < InComingList.Count; i++)
            //{
            //    List<RoadIcon> dummyList = new List<RoadIcon>();
            //    for (int j = 0; j < InComingList[i].Count; j++)
            //    {
            //        dummyList.Add(new RoadIcon(ConvertToReScaledPosition(InComingList[i][j])));
            //    }
            //    ListToReturn.Add(dummyList);
            //}

            List<RoadIcon> dummyList1 = new List<RoadIcon>();
            dummyList1.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(0, 0, 0))));
            dummyList1.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(0, 0, -60))));
            dummyList1.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(-50, 0, -60))));
            dummyList1.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(-50, 0, 0))));


            List<RoadIcon> dummyList2 = new List<RoadIcon>();
            dummyList2.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(-50, 0, 0))));
            dummyList2.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(-50, 0, -60))));
            dummyList2.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(-80, 0, -40))));
            dummyList2.Add(new RoadIcon(ConvertToReScaledPosition(new Vector3(-80, 0, -20))));



            //List<RoadIcon> dummyList1 = new List<RoadIcon>();
            //dummyList1.Add(new RoadIcon((new Vector3(0, 0, 0))));
            //dummyList1.Add(new RoadIcon((new Vector3(0, 0, -60))));
            //dummyList1.Add(new RoadIcon((new Vector3(-50, 0, -60))));
            //dummyList1.Add(new RoadIcon((new Vector3(-50, 0, 0))));


            //List<RoadIcon> dummyList2 = new List<RoadIcon>();
            //dummyList2.Add(new RoadIcon((new Vector3(-50, 0, 0))));
            //dummyList2.Add(new RoadIcon((new Vector3(-50, 0, -60))));
            //dummyList2.Add(new RoadIcon((new Vector3(-80, 0, -40))));
            //dummyList2.Add(new RoadIcon((new Vector3(-80, 0, -20))));

            ListToReturn.Add(dummyList1);
            ListToReturn.Add(dummyList2);

            return ListToReturn;
        }

        private List<Vector3> ConvertToVertex3List(List<RoadIcon> InComingList)
        {
            List<Vector3> ListToReturn = new List<Vector3>();
            for (int i = 0; i < InComingList.Count; i++)
            {
                ListToReturn.Add(InComingList[i].position);
            }
            return ListToReturn;
        }

        private void InitializeRoadLayersList()
        {
            RoadLayersList = new List<List<List<RoadIcon>>>(RoadLayersCount);
            for (int i = 0; i < RoadLayersCount; i++)
            {
                RoadLayersList.Add(new List<List<RoadIcon>>(3)); // "3" cause of 3 phases
                RoadLayersList[i].Add(LayersList[i]);
            }
            IntersectionList = SetLayersIntersectionList();
            for (int i = 0; i < RoadLayersCount; i++)
            {
                for (int j = 1; j < 3; j++)                      // "3" cause of 3 phases
                {
                    if (j == 1)
                    {
                        RoadLayersList[i].Add(
                            ConvertToRoadIconList(
                            OptimizeRoad1stPhase(
                            ConvertToVertex3List(
                            RoadLayersList[i][0]))));
                    }
                    else
                    {
                        if (j == 2)
                        {
                            RoadLayersList[i].Add(ConvertToRoadIconList
                                (OptimizeRoad2ndPhase(ConvertToVertex3List(RoadLayersList[i][1]))));
                        }
                    }
                }
            }
        }

        private int FindPointNodeNumberInUniqueList(Vector3 position)
        {
            for (int i = 0; i < UniqueNodesList.Count; i++)
            {
                if (UniqueNodesList[i].position == position)
                {
                    return UniqueNodesList[i].NodeNumber;
                }
            }
            return 0;
        }

        private void FindShortestPath()
        {
            Dijkstra myDijkstra = new Dijkstra
                (FindPointNodeNumberInUniqueList(SourcePoint)
                , UniqueNodesList.Count
                , GraphArr);
            myDijkstra.RunDijkstra();

            // Define Node Number Of Destination
            int DestinationPointNodeNumber = FindPointNodeNumberInUniqueList
                (GetNearestPointOnTrackIn1stPhase(DestinationPoint));
            // NOTE ZGTR ! DestinationPointNodeNumber = DestinationPointIndex

            // Get List Of Best Roads
            this.ListOfSequencedNodes = ConvertToSequencedNodes(DestinationPointNodeNumber, myDijkstra.ArrOfBestRoads);
        }

        private bool AlreadyInList(int k, List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == k)
                {
                    return true;
                }
            }
            return false;
        }

        private List<int> ConvertToSequencedNodes(int DestinationPointIndex, List<Icon>[] ArrOfBestRoads)
        {
            List<int> listToReturn = new List<int>();
            for (int j = 0; j < ArrOfBestRoads[DestinationPointIndex].Count; j++)
            {
                if (!AlreadyInList(ArrOfBestRoads[DestinationPointIndex][j].i, listToReturn))
                {
                    listToReturn.Add(ArrOfBestRoads[DestinationPointIndex][j].i);
                }
                if (!AlreadyInList(ArrOfBestRoads[DestinationPointIndex][j].j, listToReturn))
                {
                    listToReturn.Add(ArrOfBestRoads[DestinationPointIndex][j].j);
                }
            }
            return listToReturn;
        }

        private bool AlreadyNumbered(Vector3 position, int iIndex, int jIndex)
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int j = 0; j < RoadLayersList[i][1].Count; j++)
                {
                    if (RoadLayersList[i][1][j].position == position)
                    {
                        if (!((i == iIndex) && (j == jIndex)))
                        {
                            if (RoadLayersList[i][1][j].NodeNumber != -1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool AlreadyInList(Vector3 position, List<Vector3> myList)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i] == position)
                    return true;
            }
            return false;
        }

        private bool AlreadyInList(Vector3 position, List<RoadIcon> myList)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i].position == position)
                    return true;
            }
            return false;
        }

        private bool AlreadyInList(int NodeNumber, List<RoadIcon> myList, ref int indexOfFound)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i].NodeNumber == NodeNumber)
                {
                    indexOfFound = i;
                    return true;
                }
            }
            return false;
        }

        private bool AlreadyInList(Vector3 position, List<RoadIcon> myList, ref int indexOfFound)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i].position == position)
                {
                    indexOfFound = i;
                    return true;
                }
            }
            return false;
        }

        private int GetNodeNumberOfDoubledVertex(Vector3 position)
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int j = 0; j < RoadLayersList[i][1].Count; j++)
                {
                    if (RoadLayersList[i][1][j].position == position)
                        return RoadLayersList[i][1][j].NodeNumber;
                }
            }
            return 0;
        }

        private void NumberRoadNodes()
        {
            int Counter = 0;
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int j = 0; j < RoadLayersList[i][1].Count; j++)
                {
                    if (!AlreadyNumbered(RoadLayersList[i][1][j].position, i, j))
                    {
                        // Number the nodes in second Phase Only , i.e : "1"
                        RoadLayersList[i][1][j].NodeNumber = Counter;
                        Counter++;
                    }
                    else
                    {
                        RoadLayersList[i][1][j].NodeNumber =
                            GetNodeNumberOfDoubledVertex(RoadLayersList[i][1][j].position);
                    }
                }
            }
            MaxNodesNumber = Counter;
        }

        private void LinkRoadNodes()
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int j = 0; j < RoadLayersList[i][1].Count; j++)
                {
                    if (j < RoadLayersList[i][1].Count - 1)
                    {
                        RoadLayersList[i][1][j].LinkedNodes.Add(RoadLayersList[i][1][j + 1]);
                    }
                    else
                    {
                        RoadLayersList[i][1][j].LinkedNodes.Add(RoadLayersList[i][1][0]);
                    }
                    if (j > 0)
                    {
                        RoadLayersList[i][1][j].LinkedNodes.Add(RoadLayersList[i][1][j - 1]);

                    }
                    else
                    {
                        RoadLayersList[i][1][j].LinkedNodes.Add(RoadLayersList[i][1][RoadLayersList[i][1].Count - 1]);
                    }
                }
            }
        }

        private void MakeListOfUniqueNodes()
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int j = 0; j < RoadLayersList[i][1].Count; j++)
                {
                    int indexOfFound = -1;
                    if (AlreadyInList(RoadLayersList[i][1][j].NodeNumber, UniqueNodesList, ref indexOfFound))
                    {
                        for (int k = 0; k < RoadLayersList[i][1][j].LinkedNodes.Count; k++)
                        {
                            if (!AlreadyInList(RoadLayersList[i][1][j].LinkedNodes[k].position, UniqueNodesList[indexOfFound].LinkedNodes))
                            {
                                UniqueNodesList[indexOfFound].LinkedNodes.Add(RoadLayersList[i][1][j].LinkedNodes[k]);
                            }
                        }
                    }
                    else
                    {
                        UniqueNodesList.Add(new RoadIcon(RoadLayersList[i][1][j]));
                    }
                }
            }
        }

        private void InitializeGraphFirstTime()
        {
            for (int i = 0; i < UniqueNodesList.Count; i++)
            {
                for (int j = 0; j < UniqueNodesList.Count; j++)
                {
                    GraphArr[i, j] = -1;
                }
            }
        }

        private void FillGraphWithNewWeights()
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < UniqueNodesList.Count; i++)
            {
                for (int j = 0; j < UniqueNodesList[i].LinkedNodes.Count; j++)
                {
                    GraphArr[UniqueNodesList[i].NodeNumber, UniqueNodesList[i].LinkedNodes[j].NodeNumber]
                            = rd.Next(1, 10);
                }
            }
            FixGraph();
        }

        private void FixGraph()
        {
            for (int i = 0; i < UniqueNodesList.Count; i++)
            {
                for (int j = i + 1; j < UniqueNodesList.Count; j++)
                {
                    GraphArr[j, i] = GraphArr[i, j];
                }
            }
        }

        private void StructureGraph()
        {
            GraphArr = new int[UniqueNodesList.Count, UniqueNodesList.Count];
            // Initialize All vertices to "-1"
            InitializeGraphFirstTime();
            FillGraphWithNewWeights();
        }

        private void SetGraph()
        {
            // Number vector3 vertices
            NumberRoadNodes();
            // Link Vertices with each others
            LinkRoadNodes();
            // Assemble Unique Nodes
            MakeListOfUniqueNodes();
            // Structure Graph
            StructureGraph();
        }

        private List<RoadIcon> SetLayersIntersectionList()
        {
            bool LockedYet = false;
            List<RoadIcon> dummyList = new List<RoadIcon>();
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int j = i + 1; j < RoadLayersList.Count; j++)
                {
                    for (int s = 0; s < RoadLayersList[j][0].Count; s++)
                    {
                        for (int k = 0; k < RoadLayersList[i][0].Count; k++)
                        {
                            if (RoadLayersList[i][0][k].position == RoadLayersList[j][0][s].position)
                            {
                                if (!LockedYet)
                                {
                                    FirstIntersectionIndexIn1stLayer = k;
                                    FirstIntersectionIndexIn2ndLayer = s;
                                    LockedYet = true;
                                }
                                dummyList.Add(RoadLayersList[j][0][s]);
                                LastIntersectionIndexIn1stLayer = k;
                                LastIntersectionIndexIn2ndLayer = s;
                            }
                        }
                    }
                }
            }
            FixFirstLastIndex();
            return dummyList;
        }

        private void FixFirstLastIndex()
        {
            if (FirstIntersectionIndexIn1stLayer > LastIntersectionIndexIn1stLayer)
            {
                int temp = FirstIntersectionIndexIn1stLayer;
                FirstIntersectionIndexIn1stLayer = LastIntersectionIndexIn1stLayer;
                LastIntersectionIndexIn1stLayer = temp;
            }

            if (FirstIntersectionIndexIn2ndLayer > LastIntersectionIndexIn2ndLayer)
            {
                int temp = FirstIntersectionIndexIn2ndLayer;
                FirstIntersectionIndexIn2ndLayer = LastIntersectionIndexIn2ndLayer;
                LastIntersectionIndexIn2ndLayer = temp;
            }
        }

        private void StructureOptimizeRoad0Phase1stdPhase()
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                RoadLayersList[i][1].AddRange(IntersectionList);
            }
        }

        private void InitializeRoadPhases()
        {
            IntializeRoad0Phase();
            IntializeRoad1stPhase();
            IntializeRoad2ndPhase();
        }

        private void IntializeRoad0Phase()
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int k = 0; k < RoadLayersList[i][0].Count; k++)
                {
                    if (!AlreadyInList(RoadLayersList[i][0][k].position, Road0Phase))
                    {
                        Road0Phase.Add(RoadLayersList[i][0][k]);
                    }
                }
            }
        }

        private void IntializeRoad1stPhase()
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int k = 0; k < RoadLayersList[i][1].Count; k++)
                {
                    if (!AlreadyInList(RoadLayersList[i][1][k].position, Road1stPhase))
                    {
                        Road1stPhase.Add(RoadLayersList[i][1][k]);
                    }
                }
            }
        }

        private void IntializeRoad2ndPhase()
        {
            for (int i = 0; i < RoadLayersList.Count; i++)
            {
                for (int k = 0; k < RoadLayersList[i][2].Count; k++)
                {
                    if (!AlreadyInList(RoadLayersList[i][2][k].position, Road2ndPhase))
                    {
                        Road2ndPhase.Add(RoadLayersList[i][2][k]);
                    }
                }
            }
        }

        private List<Vector3> OptimizeRoad1stPhase(List<Vector3> InComing0PhaseList)
        {
            int Counter = 0;
            bool FlagToContinue = false;
            List<Vector3> OutComing1stPhaseList = new List<Vector3>();
            for (int i = 0; i < InComing0PhaseList.Count; i++)
            {
                int Decider = Counter % OptimizedVerticesMarchal;
                if (!(Decider > FirstIntersectionIndexIn2ndLayer) || (FlagToContinue == true))
                {
                    if (Decider == 0)
                    {
                        OutComing1stPhaseList.Add(InComing0PhaseList[i]);
                        Counter = 0;
                    }
                    Counter++;
                }
                else
                {
                    for (int j = FirstIntersectionIndexIn2ndLayer; j <= LastIntersectionIndexIn1stLayer; j++)
                    {
                        if (!AlreadyInList(InComing0PhaseList[j], OutComing1stPhaseList))
                            OutComing1stPhaseList.Add(InComing0PhaseList[j]);
                    }
                    FlagToContinue = true;
                }
            }
            return OutComing1stPhaseList;
        }

        private List<Vector3> OptimizeRoad2ndPhase(List<Vector3> InComing1stPhaseList)
        {
            int Counter = 0;
            List<Vector3> OutComing2ndPhaseList = new List<Vector3>();
            for (int i = 0; i < InComing1stPhaseList.Count; i++)
            {
                if (Counter % OptimizedVerticesMarchal == 0)
                {
                    OutComing2ndPhaseList.Add(InComing1stPhaseList[i]);
                    Counter = 0;
                }
                Counter++;
            }
            return OutComing2ndPhaseList;
        }

        private bool IsCloseEnough(Vector3 RoadPoint)
        {
            bool GotX = false;
            bool GotZ = false;
            float XPointToCompare = (float)Math.Abs(car.position.X) + PreciseCircleRadius;
            float ZPointToCompare = (float)Math.Abs(car.position.Z) + PreciseCircleRadius;
            if (Math.Abs(XPointToCompare - (float)Math.Abs(RoadPoint.X)) < PreciseCircleRadius)
            {
                GotX = true;
                if (Math.Abs(ZPointToCompare - (float)Math.Abs(RoadPoint.Z)) < PreciseCircleRadius)
                {
                    GotZ = true;
                }
            }
            if ((GotX) && (GotZ))
            {
                return true;
            }
            return false;
        }

        private bool GetPointsForword()
        {
            bool ClosestPointDefined = false;
            int RangeSize = LastClosestTrackPointIndex + PreciseCircleRadius;
            if (RangeSize > 0)
            {
                if ((RangeSize < Road0Phase.Count) && (RangeSize > -1))
                {
                    for (int i = LastClosestTrackPointIndex; i < RangeSize; i++)
                    {
                        if (IsCloseEnough(Road0Phase[i].position))
                        {
                            ClosestPointDefined = true;
                            ClosestTrackPoint = Road0Phase[i].position;
                            LastClosestTrackPointIndex = i;
                            break;
                        }
                    }
                }
            }
            return ClosestPointDefined;
        }

        private bool GetPointsBackword()
        {
            bool ClosestPointDefined = false;
            int RangeSize = LastClosestTrackPointIndex - PreciseCircleRadius;
            if (RangeSize > 0)
            {
                if ((RangeSize < Road0Phase.Count) && (RangeSize > -1))
                {
                    for (int i = LastClosestTrackPointIndex; i > 0; i--)
                    {
                        if (IsCloseEnough(Road0Phase[i].position))
                        {
                            ClosestPointDefined = true;
                            ClosestTrackPoint = Road0Phase[i].position;
                            LastClosestTrackPointIndex = i;
                            break;
                        }
                    }
                }
            }
            return ClosestPointDefined;
        }

        private void GetClosestTrackPoint()
        {
            bool ClosestPointDefined = false;
            while (!ClosestPointDefined)
            {
                if (GetPointsForword())
                {
                    ClosestPointDefined = true;
                    break;
                }
                else
                {
                    if (GetPointsBackword())
                    {
                        ClosestPointDefined = true;
                        break;
                    }
                }
                PreciseCircleRadius += ConstRadius;
            }
        }

        static float GetLineAngle(Vector3 A, Vector3 B)
        {
            float XComponent = B.X - A.X;
            float ZComponent = B.Z - A.Z;
            float Value = (float)Math.Sqrt(Math.Pow(XComponent, 2) + Math.Pow(ZComponent, 2));

            double Sin = Math.Abs(XComponent / Value);

            float Angle = Math.Abs(MathHelper.ToDegrees((float)Math.Abs(Math.Asin(Sin)))) % 180;

            if (XComponent < 0 && ZComponent < 0)
                ;
            else if (XComponent > 0 && ZComponent < 0)
                Angle *= -1;
            else if (XComponent < 0 && ZComponent > 0)
                Angle = 180 - Angle;
            else if (XComponent > 0 && ZComponent > 0)
                Angle = -180 + Angle;

            return Angle;
        }

        public float GetGPSPointAngle()
        {
            return MathHelper.ToRadians(GetLineAngle(car.position, GPSPoint));
        }

        public bool GPSPointIsApproached()
        {
            if ((car.position - GPSPoint).Length() > CloseEnoughToGPSPointAmount)
            {
                GPSPointApproached = false;
            }
            else
            {
                GPSPointApproached = true;
            }
            return GPSPointApproached;
        }

        public bool ClosestTrackPointIsApproached()
        {
            if ((car.position - ClosestTrackPoint).Length() > CloseEnoughToClosestPointAmount)
            {
                ClosestTrackPointApproached = false;
            }
            else
            {
                ClosestTrackPointApproached = true;
            }
            return ClosestTrackPointApproached;
        }

        private bool UserIsInspecting()
        {
            KeyboardState KS = Keyboard.GetState();
            if (KS.IsKeyDown(Keys.I))
            {
                return true;
            }
            return false;
        }

        private int ManipulateInitialIndex(int LastReferencePointIndex, List<RoadIcon> list)
        {
            if (LastReferencePointIndex * OptimizedVerticesMarchal
                - list.Count * 0.1 > 0)
            {
                if (LastReferencePointIndex * OptimizedVerticesMarchal
                - list.Count * 0.1 < Road1stPhase.Count)
                {
                    return (LastReferencePointIndex * OptimizedVerticesMarchal)
                        - (int)(list.Count * 0.1);
                }
            }
            return LastReferencePointIndex;
        }

        private int ManipulateLastIndex(int LastReferencePointIndex, List<RoadIcon> list)
        {
            if (LastReferencePointIndex * OptimizedVerticesMarchal
                + Road1stPhase.Count * 0.1 > 0)
            {
                if (LastReferencePointIndex * OptimizedVerticesMarchal
                + Road1stPhase.Count * 0.1 < Road1stPhase.Count)
                {
                    return LastReferencePointIndex * OptimizedVerticesMarchal
                        + (int)(Road1stPhase.Count * 0.1);
                }
            }
            return Road1stPhase.Count - 1;
        }

        private int Road2ndPhaseManipulating(Vector3 position)
        {
            float minDistance = (Road2ndPhase[0].position - car.position).Length();
            int ReferencePointIndex = 0;
            for (int i = 0; i < Road2ndPhase.Count; i++)
            {
                if ((Road2ndPhase[i].position - car.position).Length() < minDistance)
                {
                    minDistance = (Road2ndPhase[i].position - car.position).Length();
                    ReferencePointIndex = i;
                }
            }
            return ReferencePointIndex;
        }

        private int Road1stPhaseManipulating(int LastReferencePointIndex, Vector3 position)
        {
            int ReferencePointIndex = LastReferencePointIndex;
            int InitialIndex = ManipulateInitialIndex(LastReferencePointIndex, Road1stPhase);
            int LastIndex = ManipulateLastIndex(LastReferencePointIndex, Road1stPhase);
            ReferencePointIndex = (int)((InitialIndex + LastIndex) / 2);
            float minDistance = 0;
            if (LastReferencePointIndex * OptimizedVerticesMarchal < Road1stPhase.Count)
            {
                minDistance =
                    (
                    Road1stPhase[LastReferencePointIndex * OptimizedVerticesMarchal].position
                    - car.position
                    ).Length();
            }
            else
            {
                minDistance =
                    (
                    Road1stPhase[Road1stPhase.Count - 1].position
                    - car.position
                    ).Length();
            }
            for (int i = InitialIndex; i < LastIndex; i++)
            {
                if ((Road1stPhase[i].position - position).Length() < minDistance)
                {
                    minDistance = (Road1stPhase[i].position - car.position).Length();
                    ReferencePointIndex = i;
                }
            }
            return ReferencePointIndex;
        }

        private Vector3 Road0PhaseManipulating(int LastReferencePointIndex, Vector3 position)
        {
            int ReferencePointIndex = LastReferencePointIndex;
            int InitialIndex = ManipulateInitialIndex(LastReferencePointIndex, Road0Phase);
            int LastIndex = ManipulateLastIndex(LastReferencePointIndex, Road0Phase);
            ReferencePointIndex = (int)((InitialIndex + LastIndex) / 2);
            float minDistance = 0;
            if (LastReferencePointIndex * OptimizedVerticesMarchal < Road0Phase.Count)
            {
                minDistance =
                    (
                    Road0Phase[LastReferencePointIndex * OptimizedVerticesMarchal].position
                    - car.position
                    ).Length();
            }
            else
            {
                minDistance =
                    (
                    Road0Phase[Road0Phase.Count - 1].position
                    - car.position
                    ).Length();
            }
            for (int i = InitialIndex; i < LastIndex; i++)
            {
                if ((Road0Phase[i].position - position).Length() < minDistance)
                {
                    minDistance = (Road0Phase[i].position - car.position).Length();
                    ReferencePointIndex = i;
                }
            }
            IndexOfClosestPoint = ReferencePointIndex;
            if (ReferencePointIndex < Road0Phase.Count)
                return Road0Phase[ReferencePointIndex].position;
            else
                return Road0Phase[Road0Phase.Count - 1].position;
        }

        private Vector3 GetClosestTrackPoint_OffRoadMode(Vector3 position)
        {
            ClosestTrackPoint = Road0PhaseManipulating(
                Road1stPhaseManipulating(
                Road2ndPhaseManipulating
                (position)
                , position)
                , position);
            return ClosestTrackPoint;
        }

        private Vector3 GetNearestPointOnTrackIn1stPhase(Vector3 position)
        {
            return Road1stPhaseManipulating(position);
        }

        public void DefineShortestPath(Vector3 TargetedPositionToGoOn)
        {
            int IndexOfClosestTrackPointToCar = IndexOfClosestPoint;
            Vector3 ClosestTrackPointToTargetedPositionToGoOn =
                GetClosestTrackPoint_OffRoadMode(TargetedPositionToGoOn);
            int IndexOfTargetedPositionToGoOn = IndexOfClosestPoint;
        }

        private void GetNewGPSPoint()
        {
            if (UniqueNodesList[ListOfSequencedNodes[0]].position != car.position)
            {
                GPSPoint = UniqueNodesList[ListOfSequencedNodes[0]].position;
            }
            else
            {
                if (ListOfSequencedNodes[1] < UniqueNodesList.Count)
                {
                    GPSPoint = UniqueNodesList[ListOfSequencedNodes[1]].position;
                }
                else
                {
                    GPSPoint = UniqueNodesList[UniqueNodesList.Count - 1].position;
                }
            }
        }

        private void GetNewSequentialGPSPoint()
        {
            GPSIndex += 1;
            if (GPSIndex < ListOfSequencedNodes.Count)
            {
                GPSPoint = UniqueNodesList[ListOfSequencedNodes[GPSIndex]].position;
            }
        }

        private Vector3 ConvertToReScaledPosition(Vector3 position)
        {
            Vector3 dummyPosition = position;
            Matrix tempMatrix = Matrix.CreateTranslation(dummyPosition);
            Matrix worldMatrix = GetWorldMatrix(RoadScaleAmount, 0, dummyPosition);
            tempMatrix *= worldMatrix;
            dummyPosition.X = tempMatrix.Translation.X;
            dummyPosition.Z = tempMatrix.Translation.Z;
            dummyPosition.Y = tempMatrix.Translation.Y;
            return dummyPosition;
        }

        public Matrix GetWorldMatrix(float scaleAmount, float Angle, Vector3 position)
        {
            // Identity
            Matrix myMatrix = Matrix.Identity;
            // Scale
            myMatrix *= Matrix.CreateScale(scaleAmount);
            // Rotate
            myMatrix *= Matrix.CreateRotationY(Angle);
            // Translation
            myMatrix *= Matrix.CreateTranslation(position);

            return myMatrix;
        }

        private void Assemble3DObjectsToDraw()
        {
            //Vector3 dummyPosition = ConvertToReScaledPosition(ClosestTrackPoint);
            if (!LockedInTrack)
            {
                Object3D ClosestTrackPointObject = new Object3D(
                    this.graphicDevice
                    , @"Models\CarModel\MyBMWBody1"
                    , this.content
                    , this.camera
                    , 0.1f
                    , ClosestTrackPoint);
                GPSSystem.Object3DList.Add(ClosestTrackPointObject);
            }
            else
            {
                Object3D ClosestTrackPointObject = new Object3D(
                this.graphicDevice
                , @"Models\CarModel\MyBMWBody1"
                , this.content
                , this.camera
                , 0.1f
                , GPSPoint);
                GPSSystem.Object3DList.Add(ClosestTrackPointObject);

            }
        }

        private void Assemble2DObjectsToDraw()
        {
            Object2D GPSArrow = new Object2D(
                graphicDevice
                , @"Images\Arrow"
                , content
                , camera
                , new Vector2(30, 30)
                , "Ground"
                , new Vector3(0, 50, -1000)
                );
            GPSSystem.Object2DList.Add(GPSArrow);
        }

        private void FreeLists()
        {
            GPSSystem.Object2DList.Clear();
            GPSSystem.Object3DList.Clear();
        }

        public void GetObjectsToDraw()
        {
            FreeLists();
            Assemble3DObjectsToDraw();
            Assemble2DObjectsToDraw();
        }

        private Vector3 Road1stPhaseManipulating(Vector3 carPosition)
        {
            Vector3 ClosestPointToTrack = Vector3.Zero;
            float minDistance = (Road1stPhase[0].position - carPosition).Length();
            for (int i = 0; i < Road1stPhase.Count; i++)
            {
                if ((Road1stPhase[i].position - carPosition).Length() < minDistance)
                {
                    if ((Road1stPhase[i].position - carPosition).Length() != 0)
                    {
                        minDistance = (Road1stPhase[i].position - carPosition).Length();
                        ClosestPointToTrack = Road1stPhase[i].position;
                    }
                }
            }
            return ClosestPointToTrack;
        }

        public void ManipulateGraphUserNewWeights(Vector3 position)
        {
            Vector3 PointToTrackNr1 = Vector3.Zero;
            Vector3 PointToTrackNr2 = Vector3.Zero;
            PointToTrackNr1 = GetNearestPointOnTrackIn1stPhase(position);
            PointToTrackNr2 = GetNearestPointOnTrackIn1stPhase(PointToTrackNr1);
            int IndexOfPointToTrackNr1 = FindPointNodeNumberInUniqueList(PointToTrackNr1);
            int IndexOfPointToTrackNr2 = FindPointNodeNumberInUniqueList(PointToTrackNr2);
            //GraphArr[IndexOfPointToTrackNr1, IndexOfPointToTrackNr2] = UserDefinedWeight;
            //GraphArr[IndexOfPointToTrackNr2, IndexOfPointToTrackNr1] = UserDefinedWeight;
        }

        private void GetTwoClosestPoint(Vector3 position
            , ref Vector3 ClosestPointToTrackNr1
            , ref Vector3 ClosestPointToTrackNr2)
        {
            float minDistance = (Road1stPhase[0].position - position).Length();
            for (int i = 0; i < Road1stPhase.Count; i++)
            {
                if ((Road1stPhase[i].position - position).Length() < minDistance)
                {
                    minDistance = (Road1stPhase[i].position - position).Length();
                    ClosestPointToTrackNr2 = ClosestPointToTrackNr1;
                    ClosestPointToTrackNr1 = Road1stPhase[i].position;
                }
            }
        }

        public void UpdateGPSSystem(GameTime gameTime)
        {
            this.gameTime = gameTime;
            ManipulateGraphUserNewWeights((new Vector3(-50, 0, -40)));
            if (FirstTimeInspecting)
            {
                GPSPoint = Vector3.Zero;
                ClosestTrackPoint = Vector3.Zero;
                GPSPointApproached = false;
                ClosestTrackPointApproached = false;
                PreciseCircleRadius = 0;

                // Getting best point nearby the car
                GetClosestTrackPoint_OffRoadMode(car.position);
                FirstTimeInspecting = false;
            }
            else
            {
                if (UserIsInspecting())
                {
                    if (!LockedInTrack)
                    {
                        GPSPoint = Vector3.Zero;
                        ClosestTrackPoint = Vector3.Zero;
                        GPSPointApproached = false;
                        ClosestTrackPointApproached = false;
                        PreciseCircleRadius = 0;

                        // Getting best point nearby the car
                        GetClosestTrackPoint_OffRoadMode(car.position);

                    }
                }
                else
                {
                    if (LockedInTrack)
                    {
                        if (GPSPointIsApproached())
                        {
                            if (UserIsInspecting())
                            {
                                // Re-Discover New Route
                                FillGraphWithNewWeights();
                                // Manipulate Shortest Path  
                                FindShortestPath();
                                // Start From New Position (Closest point of road to Car Position)
                                SourcePoint = GetNearestPointOnTrackIn1stPhase(car.position);

                                // Define next GPS point
                                GetNewGPSPoint();
                            }
                            else
                            {
                                // Define next GPS point
                                GetNewSequentialGPSPoint();
                            }
                        }
                    }
                    else
                    {
                        // else placed here to increase Performance
                        if (ClosestTrackPointIsApproached())
                        {
                            LockedInTrack = true;

                            // Start From New Position (Closest point of road to Car Position)
                            SourcePoint = GetNearestPointOnTrackIn1stPhase(car.position);

                            // Discover New Route
                            FillGraphWithNewWeights();
                            // Manipulate Shortest Path
                            FindShortestPath();

                            // Define next GPS point
                            GetNewGPSPoint();
                        }
                    }
                }
            }
            GetObjectsToDraw();
        }
    }
}