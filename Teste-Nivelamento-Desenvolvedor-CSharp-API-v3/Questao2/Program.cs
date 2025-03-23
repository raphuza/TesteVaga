using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {

        int totalGoals = 0;
        totalGoals += GetGoals(team, year, "team1").Result;
        totalGoals += GetGoals(team, year, "team2").Result;
        return totalGoals;
    }

    public static async System.Threading.Tasks.Task<int> GetGoals(string team, int year, string teamType)
    {
        int goals = 0;
        int page = 1;
        using (HttpClient client = new HttpClient())
        {
            while (true)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamType}={Uri.EscapeDataString(team)}&page={page}";
                HttpResponseMessage response = await client.GetAsync(url);
                string responseData = await response.Content.ReadAsStringAsync();

                JObject json = JObject.Parse(responseData);
                JArray data = (JArray)json["data"];

                foreach (var match in data)
                {
                    int goalsInMatch = int.Parse(match[$"{teamType}goals"].ToString());
                    goals += goalsInMatch;
                }

                int totalPages = (int)json["total_pages"];
                if (page >= totalPages) break;
                page++;
            }
        }
        return goals;
    }

}