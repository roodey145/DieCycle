using System.Collections.Generic;
using UnityEngine;
using System;

public class JobGenerator
{
    private static System.Random random = new System.Random();
    private static int _shillingsPerPiece = 150;
    private static int _MIN_REQUIRED_PIECES = 10;
    private static int _MAX_REQUIRED_PIECES = 30;
    public static List<Job> GenerateRandomJobs(Guid contractorId, int amountOfJobs)
    {
        List<Job> jobs = new List<Job>();
        for (int i = 0; i < amountOfJobs; i++)
        {
            int jobAmount = random.Next(_MIN_REQUIRED_PIECES, _MAX_REQUIRED_PIECES + 1);
            string jobTitle = string.Format("Collect {0} {1}", jobAmount, "garbage");
            jobs.Add(new Job(jobTitle, contractorId, jobAmount, _shillingsPerPiece * jobAmount, DateTime.Now));
        }
        return jobs;
    }
}
