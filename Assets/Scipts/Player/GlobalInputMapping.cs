using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GlobalInputMapping
{
    // Define dictionaries to store input mappings for different control schemes
    public static Dictionary<string, KeyCode> inGameInputMapping = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> inventoryInputMapping = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> optionsInputMapping = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> menuInputMapping = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> logInputMapping = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> climbInputMapping = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> GameOverInputMapping = new Dictionary<string, KeyCode>();

    // Reference to the currently active dictionary
    public static Dictionary<string, KeyCode> activeInputMappings;
    private static bool isInitialized = false;

    static GlobalInputMapping()
    {
        Debug.Log("inputs done " + isInitialized);
        if(!isInitialized){
            inGameInputMapping["MoveLeft"] = KeyCode.A;
            inGameInputMapping["MoveRight"] = KeyCode.D;
            inGameInputMapping["Sprint"] = KeyCode.LeftShift;
            inGameInputMapping["Jump"] = KeyCode.Space;
            inGameInputMapping["Shoot"] = KeyCode.Q;
            inGameInputMapping["Interact"] = KeyCode.E;
            inGameInputMapping["Menu"] = KeyCode.Escape;
            inGameInputMapping["Inventory"] = KeyCode.I;

            inventoryInputMapping["ScrollLeft"] = KeyCode.A;
            inventoryInputMapping["ScrollRight"] = KeyCode.D;
            inventoryInputMapping["Select"] = KeyCode.E;
            inventoryInputMapping["Inventory"] = KeyCode.I;
            inventoryInputMapping["Menu"] = KeyCode.Escape;

            optionsInputMapping["ScrollUp"] = KeyCode.W;
            optionsInputMapping["ScrollDown"] = KeyCode.S;
            optionsInputMapping["Select"] = KeyCode.E;
            optionsInputMapping["Deselect"] = KeyCode.Q;
            optionsInputMapping["Menu"] = KeyCode.Escape;
            optionsInputMapping["Inventory"] = KeyCode.I;

            menuInputMapping["ScrollUp"] = KeyCode.W;
            menuInputMapping["ScrollDown"] = KeyCode.S;
            menuInputMapping["Select"] = KeyCode.E;
            menuInputMapping["Menu"] = KeyCode.Escape;

            logInputMapping["Menu"] = KeyCode.Escape;
            logInputMapping["Select+"] = KeyCode.D;
            logInputMapping["Select-"] = KeyCode.A;
            logInputMapping["Select"] = KeyCode.E;
            logInputMapping["ScrollUp"] = KeyCode.W;
            logInputMapping["ScrollDown"] = KeyCode.S;
            logInputMapping["Deselect"] = KeyCode.Q;

            climbInputMapping["MoveUp"] = KeyCode.W;
            climbInputMapping["MoveDown"] = KeyCode.S;
            climbInputMapping["Off-"] = KeyCode.A;
            climbInputMapping["Off+"] = KeyCode.D;
            climbInputMapping["Off"] = KeyCode.E;
            climbInputMapping["Menu"] = KeyCode.Escape;

            GameOverInputMapping["MoveUp"] = KeyCode.W;
            GameOverInputMapping["MoveDown"] = KeyCode.S;
            GameOverInputMapping["Select"] = KeyCode.E;

            SetActiveInputMappings(inGameInputMapping);
            isInitialized = true;
        }     
    }
    public static void SetActiveInputMappings(Dictionary<string, KeyCode> inputMappings)
    {
        activeInputMappings = inputMappings;
        Debug.Log("Active input mappings changed to: " + inputMappings.Count);
    }
}