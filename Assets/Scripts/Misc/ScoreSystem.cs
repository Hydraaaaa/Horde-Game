using System.Collections;
using System.Collections.Generic;
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
    public ScriptScore(MonoBehaviour a_script = null)
    {
        script = a_script;
        score = 0;
    }

    public MonoBehaviour script;
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
        instance = this;

        tagScores = new List<TagScore>();

        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            TagScore deathEvent = new TagScore();
            deathEvent.tag = UnityEditorInternal.InternalEditorUtility.tags[i];
            tagScores.Add(deathEvent);
        }
    }

    public void Death(GameObject dead)
    {
        TagScore score = null;
        for (int i = 0; i < tagScores.Count; i++)
        {
            if (tagScores[i].tag == dead.tag)
            {
                score = tagScores[i];
                break;
            }
        }

        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
        {
            if (score != null)
                GameObjectManager.instance.players[i].score += score.score;


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
