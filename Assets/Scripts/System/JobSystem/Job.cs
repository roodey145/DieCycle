using System;

public class Job
{
    public string JobTitle;
    public float Paycheck;
    public DateTime Deadline;
    public int CollectedAmount;


    public Guid ID { get; private set; }
    public Guid ContracterID { get; private set; }
    public int Amount { get; private set; }
    public Job(Guid ID, string jobTitle, Guid contractorID, int amount, float paycheck, DateTime deadline)
    {
        this.ID = ID;
        this.JobTitle = jobTitle;
        this.ContracterID = contractorID;
        this.Amount = amount;
        this.Paycheck = paycheck;
        this.Deadline = deadline;
    }

    public Job(string jobTitle, Guid contractorID, int amount, float paycheck, DateTime deadline)
    {
        this.ID = Guid.NewGuid();
        this.JobTitle = jobTitle;
        this.ContracterID = contractorID;
        this.Amount = amount;
        this.Paycheck = paycheck;
        this.Deadline = deadline;
    }
}