//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using UnityEngine;


public class AirwingHud : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AirwingController AirwingControl = null;

    [Header("HUD Elements")]
    [SerializeField] private RectTransform boresight = null;
    [SerializeField] private RectTransform mousePos = null;

    private Camera playerCam = null;

    private void Awake()
    {
        if (AirwingControl == null)
            Debug.LogError(name + ": AirwingHud - Mouse Flight Controller not assigned!");

        playerCam = AirwingControl.GetComponentInChildren<Camera>();

        if (playerCam == null)
            Debug.LogError(name + ": AirwingHud - No camera found on assigned Mouse Flight Controller!");
    }

    private void Update()
    {
        if (AirwingControl == null || playerCam == null)
            return;

        UpdateGraphics(AirwingControl);
    }

    private void UpdateGraphics(AirwingController controller)
    {
        if (boresight != null)
        {
            boresight.position = playerCam.WorldToScreenPoint(controller.BoresightPos);
            boresight.gameObject.SetActive(boresight.position.z > 1f);
        }

        if (mousePos != null)
        {
            mousePos.position = playerCam.WorldToScreenPoint(controller.MouseAimPos);
            mousePos.gameObject.SetActive(mousePos.position.z > 1f);
        }
    }

    public void SetReferenceAirwingControl(AirwingController controller)
    {
        AirwingControl = controller;
    }
}
