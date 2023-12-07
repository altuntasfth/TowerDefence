using Models;
using Models.Abstractions;
using UnityEngine;

namespace Components
{
    public class TurretArea : MonoBehaviour
    {
        public TurretBase selectedTurret;

        public TurretProjectile turretProjectile;
        public TurretRanged turretRanged;
        [SerializeField] private Material selectedTurretMaterial;
        [SerializeField] private Material defaultTurretMaterial;
        [SerializeField] private MeshRenderer turretAreaMeshRenderer;
        
        public void SetTurretAreaMaterial(bool isSelected)
        {
            turretAreaMeshRenderer.sharedMaterial = isSelected ? selectedTurretMaterial : defaultTurretMaterial;
        }
    }
}