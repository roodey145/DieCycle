using System;
using System.Collections.Generic;
public class Contractor
{
    public Guid ID { get; private set; }
    public string ContractorName;
    public List<Job> Jobs;

    public Contractor(Guid uuid, string contractorName)
    {
        ID = uuid;
        ContractorName = contractorName;
    }
    public Contractor(string contractorName)
    {
        ID = Guid.NewGuid();
        ContractorName = contractorName;
    }
}