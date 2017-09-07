using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TagScore
{
    public TagScore(string a_tag = null)
    {
        tag = a_tag;
        score = 0;
    }

    public string tag;
    public int score;
}

[System.Serializable]
public class ScriptScore
{
    public ScriptScore(MonoScript a_script = null)
    {
        script = a_script;
        score = 0;
    }

    public MonoScript script;
    public int score;
}

[ExecuteInEditMode]
public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;

    public List<TagScore> tagScores;
    public List<ScriptScore> scriptScores;

    public int scoreOnPlayerEscape;
    public int scoreOnCivilianRescue;
    public int scoreOnScrapPickup;
    public int scoreOnPlayerRevive;

    void Awake ()
    {
        if (scriptScores == null)
            scriptScores = new List<ScriptScore>();

        instance = this;

        if (tagScores == null)
        {
            Debug.Log("TagScores == null");
            tagScores = new List<TagScore>();
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
            {
                TagScore tagScore = new TagScore();
                tagScore.tag = UnityEditorInternal.InternalEditorUtility.tags[i];
                tagScores.Add(tagScore);
            }
        }
    }

    public void Death(GameObject dead)
    {
        for (int i = 0; i < tagScores.Count; i++)
        {
            if (tagScores[i].tag == dead.tag)
            {
                for (int j = 0; j < GameObjectManager.instance.players.Count; j++)
                {
                    GameObjectManager.instance.players[j].score += tagScores[i].score;
                }
                break;
            }
        }

        MonoBehaviour[] components = dead.GetComponents<MonoBehaviour>();
        for (int i = 0; i < scriptScores.Count; i++)
        {
            for (int j = 0; j < components.Length; j++)
            {
                if (scriptScores[i].script == MonoScript.FromMonoBehaviour(components[j]))
                {
                    for (int k = 0; k < GameObjectManager.instance.players.Count; k++)
                    {
                        GameObjectManager.instance.players[k].score += scriptScores[i].score;
                    }
                    break;
                }
            }
        }
    }

    public void PlayerEscape(GameObject player)
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            if (GameObjectManager.instance.players[i].gameObject == player)
            {
                GameObjectManager.instance.players[i].score += scoreOnPlayerEscape;
                return;
            }
        }
    }

    public void CivilianRescue()
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            GameObjectManager.instance.players[i].score += scoreOnCivilianRescue;
    }

    public void ScrapPickup(GameObject player)
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            if (GameObjectManager.instance.players[i].gameObject == player)
            {
                GameObjectManager.instance.players[i].score += scoreOnScrapPickup;
                return;
            }
        }
    }

    public void PlayerRevived(GameObject revivingPlayer)
    {
        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            if (GameObjectManager.instance.players[i].gameObject == revivingPlayer)
            {
                GameObjectManager.instance.players[i].score += scoreOnPlayerRevive;
                return;
            }
        }
    }
}
