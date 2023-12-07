using System;
using Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private GameObject turretUpgradeUI;
        [SerializeField] private GameObject turretBuyUI;
        [SerializeField] private GameObject closeButton;
        
        [SerializeField] private Slider progressBar;
        [SerializeField] private TMP_Text progressCountTMP;
        [SerializeField] private TMP_Text waveCountTMP;
        [SerializeField] private TMP_Text currencyTMP;
        [SerializeField] private TMP_Text turretDamageLevelTMP;
        [SerializeField] private TMP_Text turretSpeedLevelTMP;
        [SerializeField] private TMP_Text turretDamageCostTMP;
        [SerializeField] private TMP_Text turretSpeedCostTMP;
        [SerializeField] private Button turretDamageUpgradeButton;
        [SerializeField] private Button turretSpeedUpgradeButton;

        private TurretArea _turretArea;

        private void Awake()
        {
            SetCurrency(playerManager.currency);
        }

        public void SetWaveProgress(float killedEnemyCount, float enemyCountMax)
        {
            float progress = ((enemyCountMax - killedEnemyCount) / enemyCountMax);
            progressBar.value = progress;
            progressCountTMP.text = $"%{progress * 100f}";

            if (progress <= 0.01f)
            {
                levelManager.SpawnWave();
            }
        }
        
        public void SetWaveCount(int waveCount)
        {
            waveCountTMP.text = $"{waveCount}";
        }
        
        private void SetCurrency(int currency)
        {
            currencyTMP.text = $"{currency}";
        }

        public void OpenTurretUI(TurretArea turretArea)
        {
            _turretArea = turretArea;
            _turretArea.SetTurretAreaMaterial(true);
            closeButton.SetActive(true);

            turretUpgradeUI.SetActive(_turretArea.selectedTurret != null);
            turretBuyUI.SetActive(_turretArea.selectedTurret == null);

            if (turretArea.selectedTurret != null)
            {
                turretDamageUpgradeButton.interactable = playerManager.currency >= _turretArea.selectedTurret.currentDamageCost;
                turretSpeedUpgradeButton.interactable = playerManager.currency >= _turretArea.selectedTurret.currentSpeedCost;
            
                turretDamageLevelTMP.text = $"Level: {_turretArea.selectedTurret.damageLevel}";
                turretSpeedLevelTMP.text = $"Level: {_turretArea.selectedTurret.speedLevel}";
            
                turretDamageCostTMP.text = $"Cost: {_turretArea.selectedTurret.currentDamageCost}";
                turretSpeedCostTMP.text = $"Cost: {_turretArea.selectedTurret.currentSpeedCost}";
            }
        }
        
        public void BuyNewTurret(bool isRanged)
        {
            _turretArea.turretRanged.gameObject.SetActive(isRanged);
            _turretArea.turretProjectile.gameObject.SetActive(!isRanged);
            _turretArea.selectedTurret = isRanged ? _turretArea.turretRanged : _turretArea.turretProjectile;
            _turretArea.SetTurretAreaMaterial(false);
            
            turretUpgradeUI.SetActive(false);
            turretBuyUI.SetActive(false);
            closeButton.SetActive(false);
            
            playerManager.selectedTurretArea = null;
        }
        
        public void UpgradeTurret(bool isDamageUpgrade)
        {
            if (isDamageUpgrade)
            {
                if (playerManager.currency >= _turretArea.selectedTurret.currentDamageCost)
                {
                    _turretArea.selectedTurret.damageMultiplier += 0.5f;
                    _turretArea.selectedTurret.damageLevel += 1;
                    
                    playerManager.currency -= _turretArea.selectedTurret.currentDamageCost;
                    SetCurrency(playerManager.currency);
                    
                    _turretArea.selectedTurret.currentSpeedCost *= _turretArea.selectedTurret.damageLevel;
                }
            }
            else
            {
                if (playerManager.currency >= _turretArea.selectedTurret.currentSpeedCost)
                {
                    _turretArea.selectedTurret.speedMultiplier += 0.5f ;
                    _turretArea.selectedTurret.speedLevel += 1;
                    
                    playerManager.currency -= _turretArea.selectedTurret.currentSpeedCost;
                    SetCurrency(playerManager.currency);

                    _turretArea.selectedTurret.currentSpeedCost *= _turretArea.selectedTurret.speedLevel;
                }
            }
            

            _turretArea.SetTurretAreaMaterial(false);
            
            turretUpgradeUI.SetActive(false);
            turretBuyUI.SetActive(false);
            closeButton.SetActive(false);
            
            playerManager.selectedTurretArea = null;
        }
        
        public void OnCloseButtonClicked()
        {
            _turretArea.SetTurretAreaMaterial(false);
            turretUpgradeUI.SetActive(false);
            turretBuyUI.SetActive(false);
            closeButton.SetActive(false);
            
            playerManager.selectedTurretArea = null;
        }
    }
}