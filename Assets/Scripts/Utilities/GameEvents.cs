using System;
using Level;
using UnityEngine;

namespace Utilities
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance;

        private void Awake()
        {
            Instance = this;
        }


        public event Action OnPlayerInput;

        public void PlayerInput()
        {
            OnPlayerInput?.Invoke();
        }

        public event Action<Pickup.Type> OnPickupCollected;

        public void PickupCollected(Pickup.Type type)
        {
            OnPickupCollected?.Invoke(type);
        }
    }
}