using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Utilities
{
    public class LevelsManager : MonoBehaviour
    {
        public static LevelsManager Instance;
        [SerializeField] private List<LevelItem> levelItems;

        private void Start()
        {
            throw new NotImplementedException();
        }
    }
}