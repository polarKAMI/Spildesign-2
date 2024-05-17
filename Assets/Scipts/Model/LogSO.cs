using UnityEngine;

[CreateAssetMenu]
public class LogSO : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    [field: TextArea]
    public string Description { get; set; }

    [field: SerializeField]
    [field: TextArea]
    public string Specs { get; set; }

    [field: SerializeField]
    public Sprite LogImage { get; set; }

    [field: SerializeField]
    public bool isNote;
    public bool isItem;
    public bool isEntity;
    public bool isFederation;
    public bool isMisc;

    [SerializeField]
    private bool collected = false; // Flag to indicate whether the log has been collected

    public bool Collected
    {
        get { return collected; }
        set { collected = value; }
    }
}
