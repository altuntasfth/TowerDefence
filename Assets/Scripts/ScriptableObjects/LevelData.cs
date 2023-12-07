using System;
using Models.Abstractions;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create Level Data", fileName = "LevelData")]
    public class LevelData : ScriptableObject
    {
        public Level[] levels;
    }
    
    [Serializable]
    public struct Level
    {
        public int levelNumber;
        public WaveData[] waves;
    }

    [Serializable]
    public struct WaveData
    {
        public CharacterBase.AttackType[] characters;
    }
}