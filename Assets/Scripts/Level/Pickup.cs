using UnityEngine;
using Utilities;

namespace Level
{
    public class Pickup : MonoBehaviour
    {
        public Type type;

        private void Start()
        {
            if (type == Type.Card) GameManager.Instance.IncreaseTotalCards();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                switch (type)
                {
                    case Type.Card:
                        AudioManager.Instance.PlayOneShot("cardPickup");
                        break;
                    case Type.Money:
                        AudioManager.Instance.PlayOneShot("cashPickup");
                        break;
                }

                GameEvents.Instance.PickupCollected(type);
                Destroy(gameObject);
            }
        }

        public enum Type
        {
            Card,
            Money
        }
    }
}