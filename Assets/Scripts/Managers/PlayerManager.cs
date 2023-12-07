using System;
using Components;
using Models.Abstractions;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public UIManager uiManager;
        public LayerMask turretAreaLayer; // Kule layer'ı
        private Camera mainCamera;
        public int currency;
        public TurretArea selectedTurretArea;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (selectedTurretArea == null && Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit, 100f))
                {
                    selectedTurretArea = hit.collider.GetComponent<TurretArea>();
                    if (selectedTurretArea != null)
                    {
                        
                        uiManager.OpenTurretUI(selectedTurretArea);
                    }
                }
            }
        }
        
        
    }
}