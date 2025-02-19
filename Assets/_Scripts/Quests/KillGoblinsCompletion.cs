using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillGoblinsCompletion : MonoBehaviour
{
    // QuestManager reference
    private QuestManager questManager;

    // name of the quest to complete
    public Quest questToComplete;

    public DialoguePrompt dialoguePrompt;

    // name of prerequisite quest that should be completed before
    public Quest prerequisiteQuest;

    // flag to determine if the player is in range
    public bool isPlayerInRange = false;

    private bool wasInCombatScene = false;

    // function to ensure the QuestManager is initialized
    private void Awake()
    {
        questManager = QuestManager.Instance;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "WorldScene" && wasInCombatScene)
        {
            if (questManager.IsQuestComplete(prerequisiteQuest))
            {
                if (!questManager.IsQuestComplete(questToComplete) && questManager.IsQuestActive(questToComplete)) // Check if this quest isn't already complete
                {
                    questManager.CompleteQuest(questToComplete); // Complete the quest
                    questManager.DeactivateQuest(questToComplete);
                    DialogueManager.Instance.TriggerDialogue(dialoguePrompt.finishLines);
                    Debug.Log($"Quest '{questToComplete.questName}' has been completed.");
                    this.enabled = false;
                }
            }
            else
            {
                Debug.Log($"Prerequisite quest '{prerequisiteQuest.questName}' is not completed.");
            }
            wasInCombatScene = false; // Reset the flag
        }
        else if (scene.name == "Combat")
        {
            wasInCombatScene = true; // Set the flag when entering the combat scene
        }
    }

    // THIS IS THE BACKUP METHOD TO COMPLETE THE QUEST
    //
    // private void Update()
    // {
    //     if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && questManager != null && questManager.quests.Contains(questToComplete))
    //     {
    //         if (questManager.IsQuestComplete(prerequisiteQuest))
    //         {
    //             if (!questManager.IsQuestComplete(questToComplete)) // Check if this quest isn't already complete
    //             {
    //                 questManager.CompleteQuest(questToComplete); // Complete the quest
    //                 questManager.DeactivateQuest(questToComplete);
    //                 DialogueManager.Instance.TriggerDialogue(dialoguePrompt.finishLines);
    //                 Debug.Log($"Quest '{questToComplete.questName}' has been completed.");
    //                 this.enabled = false;
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log($"Prerequisite quest '{prerequisiteQuest.questName}' is not completed.");
    //         }
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         isPlayerInRange = true;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         isPlayerInRange = false;
    //     }
    // }
}
