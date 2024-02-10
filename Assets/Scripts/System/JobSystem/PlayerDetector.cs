using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private JobManager _jobManager;
    private GameObject _entity;


    private ContractorManager _contractorManager;

    private bool isInRange = false;
    private bool isPlayerOut = true;

    private void Start()
    {
        _entity = gameObject.transform.Find("Body").gameObject;
        _jobManager = GameObject.FindGameObjectWithTag("Player").GetComponent<JobManager>();
        _contractorManager = GetComponent<ContractorManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player") && isPlayerOut)
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.CompareTag("Player") && !isPlayerOut)
        {
            isInRange = false;
            isPlayerOut = true;
        }
    }
    private void Update()
    {
        if (isInRange && isPlayerOut)
        {
            //if (IsClickingOnNPC())
            //{
                // Check if the player has accepted a job
                if (_jobManager.AssingedJob != null)
                {// The player has an active job
                    
                    // Check if the player finished the job
                    if(_jobManager.AssingedJob.CollectedAmount >= _jobManager.AssingedJob.Amount)
                    {
                        // Pay the player 
                        _jobManager.receiveMoney();

                        // Play the money received audio
                    }
                }

                _contractorManager.OpenNPCMenu(_jobManager);

            // Indicate that the player is not outside the range after opening the menu
            isPlayerOut = false;
            // Restract the player movement
            PlayerInfo.isStopped = true;
            //}
        }
    }
    private bool IsClickingOnNPC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == _entity.transform)
                {
                    return true;
                }
            }

        }
        return false;
    }
}
