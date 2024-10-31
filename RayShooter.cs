using System.Collections;
using UnityEngine;

/// 
/// RayShooter
/// This script is attached to the Main Camera.
/// The Main Camera is a child of the Player GameObject.
/// 
/// It is responsible for shooting rays from the camera 
/// and detecting collisions with targets.
/// 
/// 
public class RayShooter : MonoBehaviour
{
    private Camera cam;
    private AudioSource pewSound;

    /// 
    /// Get the camera component and hide the cursor.
    /// 
    void Start()
    {
        cam = GetComponent<Camera>();
        pewSound = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// 
    /// Checks for mouse clicks, and emits a 
    /// ray from the center of the camera.
    /// 
    /// If the ray hits something, it calls the HandleHit method.
    /// 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pewSound.Play();
            Vector3 point = new(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                HandleHit(hit);
            }
        }
    }

    /// 
    /// HandleHit
    /// Takes a RaycastHit object as input
    /// and determines what to do with the hit object.
    /// 
    /// To determine whether the hit object is a target,
    /// it checks if the hit object has an FPSTarget component.
    /// 
    /// If it does, it calls the ReactToHit method on the target,
    /// adds points to the score, and applies a force to the target.
    /// 
    /// If the hit object is not a target, it creates a red sphere.
    /// 
    /// Note that the the FPSTarget component is a script that
    /// is attached to target GameObjects.
    /// 
    private void HandleHit(RaycastHit hit)
    {
        GameObject hitObject = hit.transform.gameObject;
        FPSTarget target = hitObject.GetComponent<FPSTarget>();
        if (target != null)
        {
            target.isHit = true;
            // It got shot!
            target.GetComponent<Rigidbody>().AddForceAtPosition(
                hit.normal * -10, hit.point, ForceMode.Impulse
            );
            target.ReactToHit();

            // Add points to the score
            // Note that the FPSGameController script 
            // is attached to the Controller GameObject.
            FPSGameController ctrl = GameObject.Find("Controller").GetComponent<FPSGameController>();
            ctrl.AddPoints(target.points);
        }
        else
        {
            StartCoroutine(SphereIndicator(hit.point));
        }
    }

    private IEnumerator SphereIndicator(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sphere.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }

    private void OnGUI()
    {
        int size = 20;
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;
        GUI.color = Color.red;
        GUI.Label(new Rect(posX, posY, size, size), "+");

    }




}
