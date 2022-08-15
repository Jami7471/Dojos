using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{ 
    public sealed class Task09 : BaseTask
    { 
        /*
     
        --- Day 9: All in a Single Night ---
        Every year, Santa manages to deliver all of his presents in a single night.

        This year, however, he has some new locations to visit; his elves have provided him the distances between every pair of locations. 
        He can start and end at any two (different) locations he wants, but he must visit each location exactly once. What is the shortest distance he can travel to achieve this?

        For example, given the following distances:

        London to Dublin = 464
        London to Belfast = 518
        Dublin to Belfast = 141
        The possible routes are therefore:

        Dublin -> London -> Belfast = 982
        London -> Dublin -> Belfast = 605
        London -> Belfast -> Dublin = 659
        Dublin -> Belfast -> London = 659
        Belfast -> Dublin -> London = 605
        Belfast -> London -> Dublin = 982

        The shortest of these is London -> Dublin -> Belfast = 605, and so the answer is 605 in this example.

        Task: What is the distance of the shortest route?
        Solution: 141

        --- Part Two ---
        The next year, just to show off, Santa decides to take the route with the longest distance instead.

        He can still start and end at any two (different) locations he wants, and he still must visit each location exactly once.

        For example, given the distances above, the longest route would be 982 via (for example) Dublin -> London -> Belfast.

        Task: What is the distance of the longest route?
        Solution: 736

        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input09.txt");
            List<RouteNode> convertedRouteList = ConvertToRouteList(input);
            string[] allLocation = GetAllLocation(convertedRouteList);
            List<RouteNode> lastRouteNodeList = new List<RouteNode>();

            List<RouteNode> allRoutes = CreateAllRoutes(allLocation, convertedRouteList);

            foreach (RouteNode routeNode in allRoutes)
            {
                FillLastRouteNodeList(lastRouteNodeList, routeNode);
            }

            return lastRouteNodeList.Where(dl => dl.ItemOrder >= allLocation.Length - 1).Min(dl => dl.DistanceBetweenStartAndDestination).ToString();
        }    

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input09.txt");
            List<RouteNode> convertedRouteList = ConvertToRouteList(input);
            string[] allLocation = GetAllLocation(convertedRouteList);
            List<RouteNode> lastRouteNodeList = new List<RouteNode>();

            List<RouteNode> allRoutes = CreateAllRoutes(allLocation, convertedRouteList);

            foreach (RouteNode routeNode in allRoutes)
            {
                FillLastRouteNodeList(lastRouteNodeList, routeNode);
            }

            return lastRouteNodeList.Where(dl => dl.ItemOrder >= allLocation.Length - 1).Max(dl => dl.DistanceBetweenStartAndDestination).ToString();

        }

        private List<RouteNode> CreateAllRoutes(string[] allLocation, List<RouteNode> convertedRouteList)
        {
            List<RouteNode> allRoutes = new List<RouteNode>();

            foreach (string location in allLocation)
            {                
                RouteNode[] startRoutes = convertedRouteList.Where(r => r.StartLocation == location).ToArray();

                foreach (RouteNode startRoute in startRoutes)
                {
                    AddNextRoutes(convertedRouteList, startRoute, new List<string>() { location });
                }

                allRoutes.AddRange(startRoutes);
            }

            return allRoutes;
        }
               
        private void FillLastRouteNodeList(List<RouteNode> lastRouteNodeList, RouteNode route)
        {
            if (route.NextRoutes.Count > 0)
            {
                foreach (RouteNode nextRoute in route.NextRoutes)
                {
                    FillLastRouteNodeList(lastRouteNodeList, nextRoute);
                }
            }
            else
            {
                lastRouteNodeList.Add(route);
            }
        }

        private void AddNextRoutes(List<RouteNode> convertedRouteList, RouteNode parent, List<string> visitedLocations)
        {
            RouteNode[] nextRoutes = convertedRouteList.Where(r => r.StartLocation == parent.DestinationLocation 
                && visitedLocations.Contains(r.DestinationLocation) == false)
                .ToArray();

            foreach (RouteNode nextRoute in nextRoutes)
            {
                RouteNode next = GetNextRouteNote(parent, nextRoute);

                List<string> newVisitedLocations = new List<string>();

                if (visitedLocations != null)
                {
                    newVisitedLocations.AddRange(visitedLocations);
                }

                newVisitedLocations.Add(nextRoute.StartLocation);
                AddNextRoutes(convertedRouteList, next, newVisitedLocations);
            }
        }

        private RouteNode GetNextRouteNote(RouteNode parent, RouteNode nextRoute)
        {
            RouteNode next = new RouteNode(nextRoute.StartLocation, nextRoute.DestinationLocation, nextRoute.DistanceBetweenRoutes);
            next.PreviousRoute = parent;
            next.DistanceBetweenStartAndDestination += parent.DistanceBetweenStartAndDestination;
            next.ItemOrder = parent.ItemOrder + 1;
            parent.NextRoutes.Add(next);
            return next;
        }

        private List<RouteNode> ConvertToRouteList(List<string> routeInformations)
        {
            List<RouteNode> routeList = new List<RouteNode>();

            foreach (string routeInformation in routeInformations)
            {
                string[] routeParts = routeInformation.Replace(" to ", ";").Replace(" = ", ";").Split(';');
                routeList.Add(new RouteNode(routeParts[0], routeParts[1], Convert.ToInt32(routeParts[2])));
                routeList.Add(new RouteNode(routeParts[1], routeParts[0], Convert.ToInt32(routeParts[2])));
            }

            return routeList;
        }

        private string[] GetAllLocation(List<RouteNode> routeList)
        {
            return routeList.DistinctBy(r => r.StartLocation).Select(r => r.StartLocation).ToArray();
        }

        private sealed class RouteNode
        {
            public RouteNode(string startLocation, string destinationLocation, int distanceBetweenRoutes)
            {
                StartLocation = startLocation;
                DestinationLocation = destinationLocation;
                DistanceBetweenRoutes = distanceBetweenRoutes;
                DistanceBetweenStartAndDestination = distanceBetweenRoutes;
            }

            public string StartLocation { get; }

            public string DestinationLocation { get; }

            public int DistanceBetweenStartAndDestination { get; set; }

            public int DistanceBetweenRoutes { get; }

            public RouteNode? PreviousRoute { get; set; }

            public List<RouteNode> NextRoutes { get; set; } = new List<RouteNode>();

            public int ItemOrder { get; set; } = 1;
        }        
    }
}
