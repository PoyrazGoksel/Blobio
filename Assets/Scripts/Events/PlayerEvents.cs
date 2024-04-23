using UnityEngine.Events;

namespace Events
{
    public static class PlayerEvents
    {
        public static UnityAction<int> PlayerBaitConsume;
        public static UnityAction<int> SizeIncreased;
    }
}