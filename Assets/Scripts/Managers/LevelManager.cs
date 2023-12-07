using System;
using System.Collections;
using System.Collections.Generic;
using Models.Abstractions;
using Pooling;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public UIManager uiManager;
        public List<CharacterBase> enemyCharacters;
        public List<CharacterBase> playerCharacters;
        
        [SerializeField] private GameObject enemyMeleePrefab;
        [SerializeField] private GameObject enemyRangedPrefab;
        
        [SerializeField] private LevelData levelData;
        [SerializeField] private Transform spawnPoint;
        public Transform tower;
        [SerializeField] private float delayBetweenEnemies = 1f;

        private int currentWaveIndex = -1;
        private int currentLevelIndex;

        private void Start()
        {
            StartCoroutine(SpawnWaveAsync());
        }

        public void UpdateUI()
        {
            var killedEnemyCount = levelData.levels[currentLevelIndex].waves[currentWaveIndex].characters.Length - enemyCharacters.Count;
            uiManager.SetWaveProgress(killedEnemyCount, levelData.levels[currentLevelIndex].waves[currentWaveIndex].characters.Length);
            uiManager.SetWaveCount(levelData.levels[currentLevelIndex].waves.Length - currentWaveIndex);
        }
        
        public void SpawnWave()
        {
            if (currentWaveIndex >= levelData.levels[currentLevelIndex].waves.Length)
            {
                return;
            }
            
            StartCoroutine(SpawnWaveAsync());
        }

        private IEnumerator SpawnWaveAsync()
        {
            currentWaveIndex++;

            uiManager.SetWaveCount(levelData.levels[currentLevelIndex].waves.Length - currentWaveIndex);
            uiManager.SetWaveProgress(0, 
                levelData.levels[currentLevelIndex].waves[currentWaveIndex].characters.Length);
            
            WaveData currentWaveData = levelData.levels[currentLevelIndex].waves[currentWaveIndex];
            for (var i = 0; i < currentWaveData.characters.Length; i++)
            {
                SpawnEnemy(currentWaveData.characters[i]);
                yield return new WaitForSeconds(delayBetweenEnemies);
            }
            
            
            yield return null;
        }
        
        private void SpawnEnemy(CharacterBase.AttackType attackType)
        {
            CharacterBase characterBase = null;
            switch (attackType)
            {
                case CharacterBase.AttackType.MELEE:
                    characterBase = Instantiate(enemyMeleePrefab, spawnPoint.position, Quaternion.identity).GetComponent<CharacterBase>();
                    break;
                case CharacterBase.AttackType.RANGED:
                    characterBase = Instantiate(enemyRangedPrefab, spawnPoint.position, Quaternion.identity).GetComponent<CharacterBase>();
                    break;
            }

            characterBase.levelManager = this;
            enemyCharacters.Add(characterBase);
            characterBase.SetCharacterType(CharacterBase.CharacterType.ENEMY);
            characterBase.SetDestination(tower.position);
            
            characterBase.transform.position = spawnPoint.position;
        }
        
    }
}