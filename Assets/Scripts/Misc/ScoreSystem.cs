using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreOnDeath
{
    public ScoreOnDeath(string a_tag = null)
    {
        tag = a_tag;
        score = 0;
    }

    public string tag;
    public int score;
}

[ExecuteInEditMode]
public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;

    public List<ScoreOnDeath> tagsScore;

    public int scoreOnPlayerEscape;
    public int scoreOnCivilianRescue;
    public int scoreOnScrapPickup;
    
	void Awake ()
    {
        instance = this;

        tagsScore = new List<ScoreOnDeath>();

        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            ScoreOnDeath deathEvent = new ScoreOnDeath();
            deathEvent.tag = UnityEditorInternal.InternalEditorUtility.tags[i];
            tagsScore.Add(deathEvent);
        }
    }

    public void Death(string tag)
    {
        ScoreOnDeath score = null;
        for (int i = 0; i < tagsScore.Count; i++)
        {
            if (tagsScore[i].tag == tag)
            {
                score = tagsScore[i];
                break;
            }
        }

        if (score == null)
        {
            Debug.Log("Score System: Received nonexistent tag");
            return;
        }

        for (int i = 0; i < GameObjectManager.instance.players.Count; i++)
            GameObjectManager.instance.players[i].score += score.score;
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
}
