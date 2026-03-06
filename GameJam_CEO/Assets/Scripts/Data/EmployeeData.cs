using UnityEngine;

namespace CEOGame.Data
{
    [CreateAssetMenu(fileName = "NewEmployee", menuName = "CEO Game/Employee Data")]
    public class EmployeeData : ScriptableObject
    {
        [Header("Identity")]
        public string employeeName;
        public Sprite portrait;
        [TextArea(2, 4)]
        public string personalityBio;

        [Header("Work")]
        public Position position;
        public Team team;
        public int salary;

        [Header("Stats")]
        [Range(0, 100)]
        public int happiness = 50;

        [Header("Relationships")]
        public Relationship[] relationships;

        [Header("Hidden")]
        public HiddenTrait[] hiddenTraits;
        [Tooltip("Set at runtime when HR reveals traits")]
        public bool traitsRevealed;
    }
}
