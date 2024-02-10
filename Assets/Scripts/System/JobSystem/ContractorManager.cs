using UnityEngine;
using TMPro;
public class ContractorManager : MonoBehaviour
{
    public Contractor contractor { get; private set; }
    public int maxJobs;
    private JobManager _player;
    [SerializeField]
    private GameObject _jobMenu;
    private ContractorMenuManager _contractorMenuManager;
    private bool isMenuOpen;
    private void Awake()
    {
        contractor = new Contractor("yes"); // Get random name from a file
        contractor.Jobs = JobGenerator.GenerateRandomJobs(contractor.ID, maxJobs);
    }
    private void Start()
    {
        if (isMenuOpen) OpenNPCMenu(_player);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<JobManager>();
        _contractorMenuManager = _jobMenu.GetComponent<ContractorMenuManager>();
        isMenuOpen = _jobMenu.activeInHierarchy;
    }

    /// <summary>Opens a menu for a specific contractor</summary>
    public void OpenNPCMenu(JobManager player)
    {
        _jobMenu.SetActive(true);
        _contractorMenuManager.contractorManager = this;
        isMenuOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
