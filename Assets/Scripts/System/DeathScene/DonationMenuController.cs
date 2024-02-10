using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DonationMenuController : MonoBehaviour
{
    // Private reference variables
    private RectTransform _transform;

    // Private value variables
    private string _UNDonationLink = "https://crisisrelief.un.org/t/somalia";


    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Change the position of the button to fit with the image
        transform.position = new Vector2(
            Screen.width * 4 / 8,
            Screen.height * 0.95f / 3
            );

        // Change the width of the button to fit with the background button
        _transform.sizeDelta = new Vector2(
            Screen.width * 4 / 8,
            Screen.height * 1 / 4
            );
    }


    public void openUNDonationSite()
    {
        Application.OpenURL(_UNDonationLink);
    }

    public void backToGameArea()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
